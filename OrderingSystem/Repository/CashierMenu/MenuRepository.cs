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
        public bool createRegularMenu(MenuDetailModel md)
        {

            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("p_createRegularMenu", con))
                {
                    string json = JsonConvert.SerializeObject(md.MenuVariant);
                    Console.WriteLine(json);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_menu_name", md.MenuName);
                    cmd.Parameters.AddWithValue("@p_menu_desc", md.MenuDescription);
                    cmd.Parameters.AddWithValue("@p_cat_name", md.CategoryName);
                    cmd.Parameters.AddWithValue("@p_image", md.MenuImageByte);
                    cmd.Parameters.AddWithValue("@p_variant_detail", json);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }

        }
        public bool createBundleMenu(MenuPackageModel md)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("p_createBundleMenu", con))
                {
                    string jsoning = JsonConvert.SerializeObject(md.MenuIngredients);
                    string jsonmenu = JsonConvert.SerializeObject(md.MenuIncluded);
                    Console.WriteLine(jsoning);
                    Console.WriteLine(jsonmenu);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_menu_name", md.MenuName);
                    cmd.Parameters.AddWithValue("@p_menu_desc", md.MenuDescription);
                    cmd.Parameters.AddWithValue("@p_cat_name", md.CategoryName);
                    cmd.Parameters.AddWithValue("@p_estimated_time", md.EstimatedTime);
                    cmd.Parameters.AddWithValue("@p_image", md.MenuImageByte);
                    cmd.Parameters.AddWithValue("@p_ingredient_menu", jsoning);
                    cmd.Parameters.AddWithValue("@p_included_menu", jsonmenu);
                    cmd.Parameters.AddWithValue("@p_menu_price", md.MenuPrice);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "MenuRepository createBundleMenu");
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }

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
                            DiscountModel d = new DiscountModel(reader.GetInt32("discount_id"), reader.GetDouble("rate"), reader.GetDateTime("until_date"));

                            MenuDetailModel md = MenuDetailModel.Builder()
                                     .WithCategoryName(reader.GetString("category_Name"))
                                     .isAvailable(reader.GetString("isAvailable").ToLower() == "yes")
                                     .WithMenuId(reader.GetInt32("menu_id"))
                                     .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                     .WithMenuDescription(reader.GetString("menu_description"))
                                     .WithMenuName(reader.GetString("menu_name"))
                                     .WithCategoryId(reader.GetInt32("category_id"))
                                     .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                     .WithDiscount(d)
                                     .Build();
                            list.Add(md);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "MenuRepository getMenu");
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public List<MenuDetailModel> getMenuDetail()
        {
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM view_menu_details", con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DiscountModel d = new DiscountModel(reader.GetInt32("discount_id"), reader.GetDouble("rate"), reader.GetDateTime("until_date"));
                            MenuPackageModel md = MenuPackageModel.Builder()
                                .WithMenuId(reader.GetInt32("menu_id"))
                                .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithFlavorName(reader.GetString("flavor_name"))
                                .WithEstimatedTime(reader.GetTimeSpan("estimated_time"))
                                .WithSizeName(reader.GetString("size_name"))
                                .WithPrice(reader.GetDouble("price"))
                                .WithDiscount(d)
                                .Build();
                            list.Add(md);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "MenuRepository getMenu");
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public List<string> getFlavor()
        {
            List<string> list = new List<string>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT DISTINCT flavor_name FROM menu_detail", con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString("flavor_name"));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally

            {
                db.closeConnection();
            }
            return list;
        }
        public List<string> getSize()
        {
            List<string> list = new List<string>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT DISTINCT size_name FROM menu_detail", con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader.GetString("size_name"));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public bool newMenuVariant(int menuId, List<MenuDetailModel> m)
        {

            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("p_newMenuDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(m);
                    Console.WriteLine(json);
                    cmd.Parameters.AddWithValue("@p_menu_id", menuId);
                    cmd.Parameters.AddWithValue("@p_variant_detail", json);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException)
            {
                throw;
            }
            finally
            {
                db.closeConnection();
            }

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
        public List<MenuDetailModel> getBundled(MenuDetailModel menu)
        {

            string query = @"SElECT m.menu_id, m.menu_name, md.menu_detail_id,md.price,md.estimated_time,md.size_name,md.flavor_name,mp.quantity,mp.package_type,mp.package_id FROM menu m 
                                INNER JOIN menu_detail md ON md.menu_id = m.menu_id 
                                INNER JOIN menu_package mp ON md.menu_detail_id = mp.included_menu_detail_id
                                WHERE mp.from_menu_detail_id = (	
	                                SELECT menu_detail_id FROM menu_detail 
	                                WHERE menu_id = @menu_id
                                )
                              ";
            List<MenuDetailModel> list = new List<MenuDetailModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@menu_id", menu.MenuId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            MenuDetailModel md = MenuPackageModel.Builder()
                                .WithMenuId(reader.GetInt32("menu_id"))
                                .WithPackageId(reader.GetInt32("package_id"))
                                .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithQuantity(reader.GetInt32("quantity"))
                                .isFixed(reader.GetString("package_type").ToLower() == "fixed")
                                .WithFlavorName(reader.GetString("flavor_name"))
                                .WithEstimatedTime(reader.GetTimeSpan("estimated_time"))
                                .WithSizeName(reader.GetString("size_name"))
                                .WithPrice(reader.GetDouble("price"))
                                .Build();
                            list.Add(md);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "MenuRepository getBundled");
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }
        public double getBundlePrice(MenuDetailModel menu)
        {
            var db = DatabaseHandler.getInstance();
            try
            {

                var conn = db.getConnection();

                using (var cmd = new MySqlCommand("SELECT price FROM menu_detail WHERE menu_id = @menu_id LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("@menu_id", menu.MenuId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("price");
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
            return 0;
        }
        public bool updateRegularMenu(MenuDetailModel m)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("p_updateMenu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string ijson = JsonConvert.SerializeObject(m.MenuIngredients);
                    cmd.Parameters.AddWithValue("@p_menu_id", m.MenuId);
                    cmd.Parameters.AddWithValue("@p_menu_name", m.MenuName);
                    cmd.Parameters.AddWithValue("@p_category_name", m.CategoryName);
                    cmd.Parameters.AddWithValue("@p_description", m.MenuDescription);
                    cmd.Parameters.AddWithValue("@p_isAvailable", m.isAvailable);
                    if (m.MenuImageByte == null) cmd.Parameters.AddWithValue("@p_image", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@p_image", m.MenuImageByte);
                    string json = JsonConvert.SerializeObject(m.MenuVariant);
                    cmd.Parameters.AddWithValue("@p_menu_detail", json);
                    if (m.Discount != null && m.Discount.DiscountId != 0)
                        cmd.Parameters.AddWithValue("@p_discount_id", m.Discount.DiscountId);
                    else
                        cmd.Parameters.AddWithValue("@p_discount_id", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return true;
                }

            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }

        }
        public bool updatePackageMenu(MenuPackageModel m)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("p_updateBundle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string ijson = JsonConvert.SerializeObject(m.MenuIngredients);
                    cmd.Parameters.AddWithValue("@p_menu_id", m.MenuId);
                    cmd.Parameters.AddWithValue("@p_menu_name", m.MenuName);
                    cmd.Parameters.AddWithValue("@p_category_name", m.CategoryName);
                    cmd.Parameters.AddWithValue("@p_description", m.MenuDescription);
                    cmd.Parameters.AddWithValue("@p_isAvailable", m.isAvailable);
                    if (m.MenuImageByte == null) cmd.Parameters.AddWithValue("@p_image", DBNull.Value);
                    else cmd.Parameters.AddWithValue("@p_image", m.MenuImageByte);
                    string json = JsonConvert.SerializeObject(m.MenuIncluded);
                    Console.WriteLine(json);
                    cmd.Parameters.AddWithValue("@p_price", m.MenuPrice);
                    cmd.Parameters.AddWithValue("@p_package_included", json);
                    if (m.Discount != null && m.Discount.DiscountId != 0)
                        cmd.Parameters.AddWithValue("@p_discount_id", m.Discount.DiscountId);
                    else
                        cmd.Parameters.AddWithValue("@p_discount_id", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    return true;
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "MenuRepository updatePackageMenu");

                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
        }
        public bool updateBundle(int id, List<MenuDetailModel> included)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var con = db.getConnection();
                using (var cmd = new MySqlCommand("p_updateBundle2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(included);
                    Console.WriteLine(json);
                    cmd.Parameters.AddWithValue("@p_package_included", json);
                    cmd.Parameters.AddWithValue("@p_menu_detail_id", id);
                    cmd.ExecuteNonQuery();
                    return true;
                }

            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                db.closeConnection();
            }
        }
    }
}
