using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using Newtonsoft.Json;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;

namespace OrderingSystem.Repo.CashierMenuRepository
{
    public class MenuRepository : IMenuRepository
    {
        public bool isMenuNameExist(string name)
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT 1 FROM menu WHERE menu_name = @menu_name", con))
                {
                    cmd.Parameters.AddWithValue("@menu_name", name);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "MenuRepository isMenuNameExist");
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
            return false;
        }
        public List<MenuDetailModel> getMenu()
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM view_menu", con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            MenuDetailModel md = MenuDetailModel.Builder()
                                     .WithCategoryName(reader.GetString("category_Name"))
                                     .isAvailable(reader.GetString("isAvailable").ToLower() == "yes")
                                     .WithMenuId(reader.GetInt32("menu_id"))
                                     .WithMenuDescription(reader.GetString("menu_description"))
                                     .WithMenuName(reader.GetString("menu_name"))
                                     .WithCategoryId(reader.GetInt32("category_id"))
                                     .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                     .Build();
                            list.Add(md);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + "MenuRepository getMenu");
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public bool createServing(int id, ServingsModel s)
        {
            try
            {
                var db = DatabaseHandler.getInstance();
                var cmd = new MySqlCommand("p_createServing", db.getConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                string json = JsonConvert.SerializeObject(s.ingList);
                cmd.Parameters.AddWithValue("@p_menu_id", id);
                cmd.Parameters.AddWithValue("@p_price", s.Price);
                cmd.Parameters.AddWithValue("@p_qty", s.Quantity);
                cmd.Parameters.AddWithValue("@p_prep_time", s.PrepTime);
                cmd.Parameters.AddWithValue("@p_serving_date", s.date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@p_ingredient", json);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool isServingDateExistsing(int id, DateTime date)
        {
            try
            {
                var db = DatabaseHandler.getInstance();
                var cmd = new MySqlCommand("SELECT 1 FROM menu_serving WHERE serving_date = @date AND menu_id = @id AND status = 'Ongoing'", db.getConnection());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.Add("@date", MySqlDbType.Date).Value = date.Date;
                var reader = cmd.ExecuteReader();
                return reader.Read();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool saveMenu(MenuDetailModel md)
        {
            try
            {
                var db = DatabaseHandler.getInstance();
                var cmd = new MySqlCommand("p_createMenu", db.getConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                string json = JsonConvert.SerializeObject(md.servingMenu.ingList);
                cmd.Parameters.AddWithValue("@p_menu_name", md.MenuName);
                cmd.Parameters.AddWithValue("@p_menu_desc", md.MenuDescription);
                cmd.Parameters.AddWithValue("@p_image", md.MenuImageByte);
                cmd.Parameters.AddWithValue("@p_cat_name", md.CategoryName);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool saveMenuWithServing(MenuDetailModel md)
        {
            try
            {
                var db = DatabaseHandler.getInstance();
                var cmd = new MySqlCommand("p_createMenuWithServing", db.getConnection());
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                string json = JsonConvert.SerializeObject(md.servingMenu.ingList);
                cmd.Parameters.AddWithValue("@p_menu_name", md.MenuName);
                cmd.Parameters.AddWithValue("@p_menu_description", md.MenuDescription);
                cmd.Parameters.AddWithValue("@p_image", md.MenuImageByte);
                cmd.Parameters.AddWithValue("@p_category_name", md.CategoryName);
                cmd.Parameters.AddWithValue("@p_price", md.servingMenu.Price);
                cmd.Parameters.AddWithValue("@p_qty", md.servingMenu.Quantity);
                cmd.Parameters.AddWithValue("@p_prep_time", md.servingMenu.PrepTime);
                cmd.Parameters.AddWithValue("@p_serving_date", md.servingMenu.date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@p_ingredient", json);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        public bool cancelServing(int id)
        {
            try
            {
                List<ServingsModel> sm = new List<ServingsModel>();
                var db = DatabaseHandler.getInstance();
                var cmd = new MySqlCommand("p_cancelServing", db.getConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_serving_id", id);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ServingsModel> getServings(int id)
        {
            try
            {
                List<ServingsModel> sm = new List<ServingsModel>();
                string query = @"
                        SELECT 
                             FLOOR(ms.quantity - COALESCE(SUM(
                           CASE 
                                WHEN o.status = 'Pending' AND oi.type = 'Ordered' THEN oi.quantity
                                ELSE 0
                           END
                         ),0))as serving_left,
                            ms.serving_id,
                            ms.price, 
                            ms.quantity,
                            ms.serving_date,
                            ms.estimated_time,
                            ms.menu_id
                        FROM menu_serving ms
                        LEFT JOIN order_item oi ON oi.serving_id = ms.serving_id
                        LEFT JOIN orders o ON o.order_id = oi.order_id
                        WHERE ms.menu_id = @menu_id AND ms.status = 'Ongoing' AND ms.serving_date >= CURDATE()
                        GROUP BY ms.serving_id
                        ORDER by ms.serving_date
                        ";
                var db = DatabaseHandler.getInstance();
                var cmd = new MySqlCommand(query, db.getConnection());
                cmd.Parameters.AddWithValue("@menu_id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ServingsModel m = ServingsModel.Build()
                           .withServingId(reader.GetInt32("serving_id"))
                           .withPrice(reader.GetDouble("price"))
                           .withQuantity(reader.GetInt32("quantity"))
                           .withLeftQuantity(reader.GetInt32("serving_left"))
                           .withServingDate(reader.GetDateTime("serving_date"))
                           .withPrepTime(reader.GetTimeSpan("estimated_time"))
                           .Build();
                    sm.Add(m);
                }

                return sm;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
