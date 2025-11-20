using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Order
{
    public class OrderRepository : IOrderRepository
    {
        public bool isOrderAvailable(string order_id)
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
        public bool isOrderExists(string order_id)
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
            string couponType = "";
            string orderStatus = "";

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

                            DiscountModel d = new DiscountModel(reader.GetInt32("discount_id"), reader.GetDouble("rate"));

                            MenuDetailModel m = MenuDetailModel.Builder()
                                .WithMenuDetailId(reader.GetInt32("menu_detail_id"))
                                .WithMenuName(reader.GetString("menu_name"))
                                .WithFlavorName(reader.GetString("flavor_name"))
                                .WithSizeName(reader.GetString("size_name"))
                                .WithPrice(reader.GetDouble("price"))
                                .WithMenuImage(ImageHelper.GetImageFromBlob(reader, "menu"))
                                .WithDiscount(d)
                                .Build();

                            OrderItemModel oxm = new OrderItemModel(reader.GetInt32("order_item_id"), reader.GetInt32("quantity"), m);
                            oim.Add(oxm);
                            oxm.Status = reader.GetString("status");

                            if (string.IsNullOrEmpty(orderId))
                            {
                                orderId = reader.GetString("order_id");
                                orderStatus = reader.GetString("orderstatus");
                                couponRate = reader.GetDouble("coupon_rate");
                                couponType = reader.GetString("type");
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
            OrderModel om = new OrderModel(orderId, new CouponModel(couponRate, CouponModel.getType(couponType)), oim);
            om.OrderStatus = orderStatus;
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
                    cmd.Parameters.AddWithValue("@p_order_type", order.Type.ToString().Replace("_", "-"));
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
        public bool payOrder(InvoiceModel i)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_Confirm_Payment", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@v_order_id", i.Order.OrderId);
                    cmd.Parameters.AddWithValue("@p_staff_id", i.Staff.StaffId);
                    cmd.Parameters.AddWithValue("@p_payment_method ", i.payment.PaymentName);
                    cmd.Parameters.AddWithValue("@p_sp ", i.specialDiscount);
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
        public bool isOrderPaid(string order_id)
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
        public string getLastestOrderID()
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
        public Tuple<TimeSpan, string, string> getTimeInvoiceWaiting(string order_id)
        {
            string query = @"
                        SELECT
                        o.estimated_max_time, 
                        i.invoice_id,
                        o.order_type
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
                            string type = reader.GetString("order_type");

                            return new Tuple<TimeSpan, string, string>(estimatedTime, invoiceId, type);
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
        public bool adjustOrderingTime()
        {
            string query = @"
                  UPDATE orders SET available_until = @date WHERE status = 'Pending';
                ";

            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.AddMinutes(30));
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
        public double getFeePaymentMethod(string paymentName)
        {
            string query = @"
                  SELECT rate FROM payment_method WHERE payment_type = @payment_type;
                ";

            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@payment_type", paymentName);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDouble("rate");
                        }
                    }
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
            return 0;
        }

        public bool voidOrderItem(string orderItemId)
        {
            var db = DatabaseHandler.getInstance();
            var conn = db.getConnection();

            try
            {
                string getOrderIdQuery = @"
                        SELECT order_id 
                        FROM order_item 
                        WHERE order_item_id = @order_item_id
                    ";

                string orderId = null;
                using (var cmd = new MySqlCommand(getOrderIdQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@order_item_id", orderItemId);
                    orderId = (string)cmd.ExecuteScalar();
                }

                if (orderId == null) return false;

                string voidItemQuery = @"
                        UPDATE order_item 
                        SET status = 'Voided' 
                        WHERE order_item_id = @order_item_id
                    ";
                using (var cmd = new MySqlCommand(voidItemQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@order_item_id", orderItemId);
                    cmd.ExecuteNonQuery();
                }
                string c = @"
                        SELECT COUNT(*) 
                        FROM order_item 
                        WHERE order_id = @order_id AND status <> 'Voided'
                    ";

                int r;
                using (var cmd = new MySqlCommand(c, conn))
                {
                    cmd.Parameters.AddWithValue("@order_id", orderId);
                    r = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (r == 0)
                {
                    string voidQ = @"
                            UPDATE orders 
                            SET status = 'Voided' 
                            WHERE order_id = @order_id
                         ";

                    using (var cmd = new MySqlCommand(voidQ, conn))
                    {
                        cmd.Parameters.AddWithValue("@order_id", orderId);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            finally
            {
                db.closeConnection();
            }
        }


        public void addQuantityOrder(OrderItemModel om, int value)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_addQuantity", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_order_item_id", om.OrderItemId);
                    cmd.Parameters.AddWithValue("@p_qty", value);
                    cmd.ExecuteNonQuery();
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

        public List<SpecialDiscount> getSpecialDiscount()
        {
            List<SpecialDiscount> d = new List<SpecialDiscount>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM discount_special", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            d.Add(new SpecialDiscount(reader.GetInt32("s_discount_id"), reader.GetString("discount_type"), reader.GetDouble("rate")));
                        }
                    }
                    cmd.ExecuteNonQuery();
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
            return d;
        }
    }
}
