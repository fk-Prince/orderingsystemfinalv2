using System;
using System.Collections.Generic;
using MySqlConnector;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;

namespace OrderingSystem.Repository
{
    public class KioskMenuRepository : IKioskMenuRepository
    {
        private List<OrderItemModel> orderList;
        public KioskMenuRepository(List<OrderItemModel> orderList)
        {
            this.orderList = orderList;
        }
        public List<MenuDetailModel> getMenu()
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                string query = @"
                     SELECT
                       m.menu_id,
                       c.category_name,
                       c.category_id,
                       m.menu_name,
                       m.menu_description,
                       m.image AS Image,
                       m.isAvailable,
                       ms.serving_id,
                       ms.price,
                       FLOOR(ms.quantity - COALESCE(SUM(
                           CASE 
                                WHEN o.status = 'Pending' AND oi.type = 'Ordered' THEN oi.quantity
                                ELSE 0
                           END
                       ),0)) AS quantity,
                       ms.estimated_time
                    FROM view_menu m
                    INNER JOIN menu_serving ms ON ms.menu_id = m.menu_id 
                    INNER JOIN category c ON c.category_id = m.category_id
                    LEFT JOIN order_item oi ON oi.serving_id = ms.serving_id
                    LEFT JOIN orders o ON o.order_id = oi.order_id
                    WHERE m.isAvailable = 'Yes'
                      AND DATE(ms.serving_date) = CURDATE()
                      AND ms.status = 'Ongoing'
                    GROUP BY ms.serving_id;

                        ";
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServingsModel s = ServingsModel.Build()
                                .withServingId(reader.GetInt32("serving_id"))
                                .withPrice(reader.GetDouble("price"))
                                .withQuantity(reader.GetInt32("quantity"))
                                .withLeftQuantity(reader.GetInt32("quantity"))
                                .withPrepTime(reader.GetTimeSpan("estimated_time"))
                                .Build();
                            var menu = MenuDetailModel.Builder()
                                .WithMenuId(reader.GetInt32("menu_id"))
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithCategoryName(reader.GetString("category_Name"))
                                .WithMenuDescription(reader.GetString("menu_description"))
                                .WithCategoryId(reader.GetInt32("category_id"))
                                .withServing(s)
                                .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                .Build();
                            list.Add(menu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
    }
}
