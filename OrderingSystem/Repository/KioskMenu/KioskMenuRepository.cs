using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using Newtonsoft.Json;
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
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM view_menu WHERE isAvailable = 'Yes'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DiscountModel d = new DiscountModel(reader.GetInt32("discount_id"), reader.GetDouble("rate"));
                            var menu = MenuDetailModel.Builder()
                                .WithMenuId(reader.GetInt32("menu_id"))
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithMenuDescription(reader.GetString("menu_description"))
                                .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                .WithPrice(reader.GetDouble("min_price"))
                                .WithEstimatedTime(reader.GetTimeSpan("estimated_time"))
                                .WithCategoryId(reader.GetInt32("category_id"))
                                .WithMaxOrder(getMaxOrderRealTime(reader.GetInt32("menu_id"), orderList))
                                .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                .WithDiscount(d)
                                .Build();
                            list.Add(menu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "KioskMenuRepository getMenu");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public List<MenuDetailModel> getDetails(MenuDetailModel menu)
        {
            var db = DatabaseHandler.getInstance();
            List<MenuDetailModel> mds = new List<MenuDetailModel>();
            try
            {
                var conn = db.getConnection();
                string query1 = @"
                                    SELECT * FROM view_menu_details m 
                                    WHERE m.isAvailable = 'Yes' 
                                    AND m.menu_id = @menu_id
                                    AND m.menu_detail_id NOT IN (SELECT from_menu_detail_id FROM menu_package)
                                    ORDER BY m.price ASC
                                    ";
                using (var cmd = new MySqlCommand(query1, conn))
                {
                    cmd.Parameters.AddWithValue("@menu_id", menu.MenuId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DiscountModel d = new DiscountModel(reader.GetInt32("discount_id"), reader.GetDouble("rate"));
                            int maxOrder = getMaxOrderRealTime(reader.GetInt32("menu_detail_id"), orderList);
                            mds.Add(MenuDetailModel.Builder()
                                .WithMenuId(reader.GetInt32("menu_id"))
                                .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                .WithPrice(reader.GetDouble("price"))
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                .WithEstimatedTime(reader.GetTimeSpan("estimated_time"))
                                .WithMaxOrder(maxOrder)
                                .WithFlavorName(reader.GetString("flavor_name"))
                                .WithSizeName(reader.GetString("size_name"))
                                .WithDiscount(d)
                                .Build());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine("error on getMenuDetailFlavor");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return mds;
        }
        public List<MenuDetailModel> getFrequentlyOrderedTogether(MenuDetailModel menus)
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_retrieve_FrequentlyOrderedTogether", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_menu_id", menus.MenuId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        do
                        {
                            while (reader.Read())
                            {
                                int i = getMaxOrderRealTime(reader.GetInt32("menu_detail_id"), orderList);
                                if (i == 0)
                                    continue;

                                var menu = MenuDetailModel.Builder()
                                    .WithMenuId(reader.GetInt32("menu_id"))
                                    .WithMenuName(reader.GetString("menu_name"))
                                    .WithMenuDescription(reader.GetString("menu_description"))
                                    .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                    .WithFlavorName(reader.GetString("flavor_name"))
                                    .WithSizeName(reader.GetString("size_name"))
                                    .WithPrice(reader.GetDouble("price"))
                                    .WithMaxOrder(i)
                                    .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                    .Build();

                                list.Add(menu);
                            }
                        } while (reader.NextResult());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.WriteLine(ex.Message + "KioskMenuRepository getFrequentlyOrderedTogether");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public List<MenuDetailModel> getIncludedMenu(MenuDetailModel menu)
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                string query = @"
                             WITH RECURSIVE package_items AS (
                                SELECT
                                    mp.included_menu_detail_id,
                                    mp.quantity,
                                    mp.package_type,
                                    1 AS item_number
                                FROM menu_package mp
                                JOIN menu_detail md_package ON mp.from_menu_detail_id = md_package.menu_detail_id
                                WHERE md_package.menu_id = @menu_id 
                                UNION ALL
                                SELECT
                                    p.included_menu_detail_id,
                                    p.quantity,
                                    p.package_type,    
                                    p.item_number + 1
                                FROM package_items p
                                WHERE p.item_number < p.quantity
                                      
                            )
                            SELECT
                                md.menu_id,
                                md.menu_detail_id,
                                m.menu_name,
                                m.menu_description,
                                md.flavor_name,
                                md.size_name,
                                md.price,
                                pi.quantity,
                                pi.package_type
                            FROM package_items pi
                            JOIN menu_detail md ON pi.included_menu_detail_id = md.menu_detail_id
                            LEFT JOIN menu m ON md.menu_id = m.menu_id
                            ORDER BY
                                md.menu_id, 
                                md.flavor_name;";
                var conn = db.getConnection();

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@menu_id", menu.MenuId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int max = getMaxOrderRealTime(reader.GetInt32("menu_detail_id"), orderList);
                            var x = MenuPackageModel.Builder()
                               .WithMenuId(reader.GetInt32("menu_id"))
                               .isFixed(reader.GetString("package_type") == "Fixed" ? true : false)
                               .WithMenuName(reader.GetString("menu_name"))
                               .WithDiscount(menu.Discount)
                               .WithMenuDescription(reader.GetString("menu_description"))
                               .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                               .WithPrice(reader.GetDouble("price"))
                               .WithSizeName(reader.GetString("size_name"))
                               .WithFlavorName(reader.GetString("flavor_name"))
                               .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                               .Build();
                            list.Add(x);
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                Console.WriteLine("error on getMenuId");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public List<MenuDetailModel> getDetailsByPackage(MenuDetailModel menu)
        {
            var db = DatabaseHandler.getInstance();
            List<MenuDetailModel> mds = new List<MenuDetailModel>();
            try
            {
                var conn = db.getConnection();
                string query = @"                                                 
                            SELECT 
                                md.menu_id,
                                md.menu_detail_id,
                                md.price,
                                md.flavor_name,
                                md.size_name,
                                COALESCE(mp.package_type, 'Not-Fixed') AS package_type
                            FROM menu m        
                            LEFT JOIN menu_detail md ON m.menu_id = md.menu_id  
                            LEFT JOIN (SELECT from_menu_detail_id, package_type FROM menu_package) mp ON mp.from_menu_detail_id = md.menu_detail_id
                            WHERE m.menu_id = @menu_id
                             AND (
                                  mp.package_type IS NULL  
                                  OR
                                  (mp.package_type <> 'fixed')  
                                  OR
                                  (mp.package_type = 'fixed' AND md.menu_detail_id = @menu_detail_id)  
                              )
                            ORDER BY 
                              CASE WHEN md.menu_detail_id = @menu_detail_id THEN 0 ELSE 1 END,
                              md.menu_detail_id; 
                                        ";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@menu_id", menu.MenuId);
                    cmd.Parameters.AddWithValue("@menu_detail_id", menu.MenuDetailId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int maxOrder = getMaxOrderRealTime(reader.GetInt32("menu_detail_id"), orderList);
                            mds.Add(MenuPackageModel.Builder()
                               .WithMenuId(reader.GetInt32("menu_id"))
                               .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                               .WithPrice(reader.GetDouble("price"))
                               .isFixed(reader.GetString("package_type") == "Fixed" ? true : false)
                               .WithMaxOrder(maxOrder)
                               .WithFlavorName(reader.GetString("flavor_name"))
                               .WithSizeName(reader.GetString("size_name"))
                               .Build());

                        }
                    }
                }
            }
            catch (MySqlException)
            {
                Console.WriteLine("error on getMenuDetailFlavorPackage");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return mds;
        }
        public bool isMenuPackage(MenuDetailModel menu)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                string query = @"
                                SELECT COUNT(*) c FROM menu m
                                INNER JOIN menu_detail md ON m.menu_id = md.menu_id
                                WHERE md.menu_detail_id IN (
	                                SELECT pg.from_menu_detail_id FROM menu_package pg 
                                ) AND m.menu_id = @menu_id;
                                ";
                var conn = db.getConnection();

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@menu_id", menu.MenuId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("c") >= 1;
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                Console.WriteLine("error on isMenuPackage");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return false;
        }
        public double getNewPackagePrice(int menuid, List<MenuDetailModel> selectedMenus)
        {
            var db = DatabaseHandler.getInstance();

            try
            {
                var conn = db.getConnection();

                using (var cmd = new MySqlCommand("p_menu_package_price", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(selectedMenus);
                    Console.WriteLine(json);
                    cmd.Parameters.AddWithValue("@p_package_id", menuid);
                    cmd.Parameters.AddWithValue("@p_included", json);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal d = reader.GetDecimal(0);
                            return (double)d;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + "123");
                Console.WriteLine("error on getNewPackagePrice");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return 0;
        }
        public int getMaxOrderRealTime2(int menu_id, List<OrderItemModel> orderList)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();

                using (var cmd = new MySqlCommand("p_menu_max_order2x", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(orderList);
                    cmd.Parameters.AddWithValue("@p_menu_id", menu_id);
                    cmd.Parameters.AddWithValue("@p_json", json);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("max_order");
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("error on getMaxOrderRealTime");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return 0;
        }
        public int getMaxOrderRealTime(int menu_id, List<OrderItemModel> orderList)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();

                using (var cmd = new MySqlCommand("p_menu_max_order", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(orderList);
                    cmd.Parameters.AddWithValue("@p_menu_detail_id", menu_id);
                    cmd.Parameters.AddWithValue("@p_json", json);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("max_order");
                        }
                    }
                }
            }
            catch (MySqlException)
            {

                Console.WriteLine("error on getMaxOrderRealTime");
                throw;
            }
            finally
            {
                db.closeConnection();
            }
            return 0;
        }
    }
}
