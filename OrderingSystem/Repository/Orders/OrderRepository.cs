using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using Newtonsoft.Json;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Order
{
    public class OrderRepository : IOrderRepository
    {
        public bool getOrderAvailable(string order_id)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT COUNT(*) as c FROM orders WHERE order_id = @order_id AND available_until > NOW()", conn))
                {
                    cmd.Parameters.AddWithValue("@order_id", order_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("c") > 0;
                        }
                    }
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
            return false;
        }
        public bool getOrderExists(string order_id)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT COUNT(*) as c FROM orders WHERE order_id = @order_id", conn))
                {
                    cmd.Parameters.AddWithValue("@order_id", order_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetInt32("c") > 0;
                        }
                    }
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
            return false;
        }
        public OrderModel getOrders(string order_id)
        {
            List<OrderItemModel> oim = new List<OrderItemModel>();
            var db = DatabaseHandler.getInstance();
            double couponRate = 0;
            string orderId = "";

            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_retrieve_order", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_order_id", order_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DiscountModel d = DiscountModel.Builder()
                                .WithDiscountId(reader.GetInt32("discount_id"))
                                .WithRate(reader.GetDouble("rate"))
                                .Build();

                            MenuModel m = MenuModel.Builder()
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithFlavorName(reader.GetString("flavor_name"))
                                .WithSizeName(reader.GetString("size_name"))
                                .WithPrice(reader.GetDouble("price"))
                                .WithDiscount(d)
                                .Build();
                            OrderItemModel xd = OrderItemModel.Builder()
                                //.WithNote(reader.GetString("order_note"))
                                //.WithNoteApproved(reader.GetBoolean("note_approve"))
                                .WithOrderItemId(reader.GetInt32("order_item_id"))
                                .WithPurchaseQty(reader.GetInt32("quantity"))
                                .WithPurchaseMenu(m)
                                .Build();
                            oim.Add(xd);

                            if (string.IsNullOrEmpty(orderId))
                            {
                                orderId = reader.GetString("order_id");
                                couponRate = reader.GetDouble("coupon_rate");
                            }

                        }
                    }
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
            OrderModel om = OrderModel.Builder()
                                .WithOrderId(orderId)
                                .WithCoupon(new CouponModel(couponRate))
                                .WithOrderItemList(oim)
                                .Build();

            return om;
        }
        public bool saveNewOrder(OrderModel order)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_NewOrder", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_json_orderList", order.JsonOrderList());
                    if (order.Coupon != null)
                        cmd.Parameters.AddWithValue("@p_coupon_code", order.Coupon.CouponCode);
                    else
                        cmd.Parameters.AddWithValue("@p_coupon_code", DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_order_type", order.OrderType);
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
        public bool payOrder(OrderModel order, int staff_id, string payment_method)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_Confirm_Payment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string json = JsonConvert.SerializeObject(order);
                    cmd.Parameters.AddWithValue("@p_order_json", json);
                    cmd.Parameters.AddWithValue("@p_staff_id", staff_id);
                    cmd.Parameters.AddWithValue("@p_payment_method ", payment_method);
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
        public bool isOrderPayed(string order_id)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM orders WHERE status = 'Paid' AND order_id = @order_id", conn))
                {
                    cmd.Parameters.AddWithValue("@order_id ", order_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true;
                        }
                    }

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
            return false;
        }
        public string getOrderId()
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_GenerateNextOrderId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var p = new MySqlParameter("p_order_id", MySqlDbType.VarChar, 255);
                    p.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(p);
                    cmd.ExecuteNonQuery();
                    return p.Value.ToString();
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
        public List<string> getAvailablePayments()
        {
            List<string> p = new List<string>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM payment_method WHERE isActive = 'Active'", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            p.Add(reader.GetString("payment_type"));
                        }
                        return p;
                    }
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
        public Tuple<TimeSpan, string> getTimeInvoiceWaiting(string order_id)
        {
            string query = @"
                        SELECT
                        o.estimated_max_time, 
                        i.invoice_id
                        FROM orders o
                        INNER JOIN invoice i ON i.order_id = o.order_id
                        WHERE @order_id = o.order_id
                        ";
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@order_id", order_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TimeSpan estimatedTime = reader.GetTimeSpan("estimated_max_time");
                            string invoiceId = reader.GetString("invoice_id");

                            return new Tuple<TimeSpan, string>(estimatedTime, invoiceId);
                        }

                    }
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
            return null;
        }

        public DataView getOrderView(int offSet)
        {
            string query = @"
                    SELECT 
                        order_id AS 'Order ID',
                        Total_Amount AS 'Total Amount',
                        Status AS 'Status',
                        order_type AS 'Dine-in or Take-Out',
                        Available_until AS 'Available Until'
                    FROM orders
                    LIMIT 50 OFFSET @offSet
                ";

            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@offSet", offSet);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
                DataView view = new DataView(dt);
                return view;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.closeConnection();
            }
        }

        public bool voidOrder(string orderId)
        {
            string query = @"
                  UPDATE orders SET status = 'Voided' WHERE order_id = @order_id
                ";

            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    cmd.ExecuteNonQuery();
                    return true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.closeConnection();
            }
        }
    }
}
