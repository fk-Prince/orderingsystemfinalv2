using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using OrderingSystem.DatabaseConnection;

namespace OrderingSystem.Repository.Reports
{
    public class ReportRepository : IReportRepository
    {
        public DataView getInventoryReports()
        {
            string query = @"
                        SELECT
                            CONCAT(
                                UPPER(SUBSTRING(s.firstName, 1, 1)), LOWER(SUBSTRING(s.firstName, 2)),
                                ' ',
                                UPPER(SUBSTRING(s.lastName, 1, 1)), LOWER(SUBSTRING(s.lastName, 2))
                            ) AS 'Staff Incharge',
                            i.ingredient_name AS 'Ingredient Name',
                            oss.current_stock AS 'Current Stock',
                            (mi.quantity - oss.current_stock - COALESCE(d.quantity, 0)) AS 'Used',
                            COALESCE(d.quantity, 0) AS 'Deductions',
	                        COALESCE(d.reasons,'') AS 'Deductions Reason',
                            oss.expiry_date AS 'Expiry Date',
                            mi.created_at AS 'IN - Recorded At'
                        FROM monitor_inventory mi
                        INNER JOIN staff s ON s.staff_id = mi.staff_id
                        INNER JOIN ingredient_stock oss ON oss.ingredient_stock_id = mi.ingredient_stock_id
                        INNER JOIN ingredients i ON i.ingredient_id = oss.ingredient_id
                        LEFT JOIN (
                            SELECT ingredient_stock_id, SUM(quantity) AS quantity, CONCAT('(',COUNT(*),') ' , GROUP_CONCAT(reason SEPARATOR ', ')) AS reasons 
                            FROM monitor_inventory
                            WHERE type = 'Deduct'
                            GROUP BY ingredient_stock_id
                        ) d ON d.ingredient_stock_id = mi.ingredient_stock_id
                        WHERE mi.type = 'Add'
                        ORDER BY mi.created_at;
                        ";

            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
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
        public DataView getIngredientExpiry()
        {

            string query = @"
                        SELECT
                            i.ingredient_name AS 'Ingredient Name',
                            oss.current_stock AS 'Current Stock',
                            oss.expiry_date AS 'Expiry Date',
                            mi.created_at AS 'IN - Recorded At'
                        FROM monitor_inventory mi
                        INNER JOIN ingredient_stock oss ON oss.ingredient_stock_id = mi.ingredient_stock_id
                        INNER JOIN ingredients i ON i.ingredient_id = oss.ingredient_id
                        LEFT JOIN (
                            SELECT ingredient_stock_id, SUM(quantity) quantity, CONCAT('(',COUNT(*),') ' , GROUP_CONCAT(reason SEPARATOR ', ')) AS reasons 
                            FROM monitor_inventory
                            WHERE type = 'Deduct'
                            GROUP BY ingredient_stock_id
                        ) d ON d.ingredient_stock_id = mi.ingredient_stock_id
                        WHERE mi.type = 'Add'
                        ORDER BY mi.created_at;
                        ";

            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
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
        public DataView getIngredientTrackerView()
        {
            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                string query = @"
                    SELECT 
                       CONCAT(
                           UPPER(SUBSTRING(s.firstName, 1, 1)), LOWER(SUBSTRING(s.firstName, 2)),
                           ' ',
                           UPPER(SUBSTRING(s.lastName, 1, 1)), LOWER(SUBSTRING(s.lastName, 2)) 
                        ) AS 'Staff Incharge',
                        i.ingredient_name as 'Ingredient Name',
                        mi.quantity as Quantity, 
                        mi.type as Movement, 
                        mi.reason as Reason, 
                        mi.created_at as 'Date-Action'
                    FROM monitor_inventory mi 
                    INNER JOIN staff s ON s.staff_id = mi.staff_id
                    INNER JOIN ingredient_stock oss ON oss.ingredient_stock_id = mi.ingredient_stock_id
                    INNER JOIN ingredients i ON i.ingredient_id = oss.ingredient_id
                    ORDER BY mi.created_at";

                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
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
        public DataView getIngredientsUsage()
        {
            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM view_report_ingredientusage", conn))
                {
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
        public DataView getMenuPopularity()
        {
            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("SELECT * FROM view_report_popularity", conn))
                {
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
        public DataView getInvoice()
        {
            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_retrieve_invoice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
        public DataView getSupplier()
        {
            var db = DatabaseHandler.getInstance();
            string query = @"
                        SELECT s.supplier_name AS 'Supplier Name',i.ingredient_name AS 'Ingredeint Name',SUM(mi.quantity) AS 'Total Supplied', MAX(si.date_supplied) AS 'Recently Supplied' FROM suppliers s
                        INNER JOIN supplier_ingredient_stock si ON si.supplier_id = s.supplier_id 
                        INNER JOIN monitor_inventory mi ON mi.ingredient_stock_id = si.ingredient_stock_id
                        INNER JOIN ingredient_stock oss ON mi.ingredient_stock_id = oss.ingredient_stock_id
                        INNER JOIN ingredients i ON i.ingredient_id = oss.ingredient_id
                        WHERE mi.type = 'Add'
                        GROUP BY s.supplier_name,i.ingredient_name
                        ORDER BY CASE WHEN s.supplier_name = 'N/A' THEN 1 ELSE 0 END, s.supplier_name
                        ";
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand(query, conn))
                {
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

        public Tuple<string, string> getTransactions(DateTime now)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                using (var conn = db.getConnection())
                {
                    string query = @"
                            WITH totals AS (
                              SELECT
                                COALESCE(SUM(CASE 
                                  WHEN MONTH(available_until) = MONTH(@start_date)
                                       AND YEAR(available_until) = YEAR(@start_date)
                                  THEN total_amount ELSE 0 END), 0) AS this_month,
                                COALESCE(SUM(CASE 
                                  WHEN MONTH(available_until) = MONTH(DATE_SUB(@start_date, INTERVAL 1 MONTH))
                                       AND YEAR(available_until) = YEAR(DATE_SUB(@start_date, INTERVAL 1 MONTH))
                                  THEN total_amount ELSE 0 END), 0) AS last_month
                              FROM orders
                              WHERE status = 'Paid'
                            )
                            SELECT
                              this_month,
                              last_month,
                              COALESCE(
                                ROUND(
                                  CASE 
                                    WHEN last_month = 0 AND this_month > 0 THEN 100     
                                    WHEN last_month = 0 AND this_month = 0 THEN 0  
                                    ELSE ((this_month - last_month) / last_month) * 100
                                  END, 
                                  2
                                ), 0
                              ) AS percent_change
                            FROM totals;
                        ";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@start_date", now);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal thisMOneth = reader.GetDecimal("this_month");
                                decimal percentChange = reader.GetDecimal("percent_change");
                                return Tuple.Create(thisMOneth.ToString(), percentChange.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching transaction data", ex);
            }
            finally
            {
                db.closeConnection();
            }
            return null;
        }
        public Tuple<string, string> getOrders(DateTime now, string x)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                using (var conn = db.getConnection())
                {
                    string query = @"
                              WITH monthly_totals AS (
                                    SELECT
                                        COALESCE(SUM(CASE 
                                            WHEN MONTH(available_until) = MONTH(@start_date)
                                                 AND YEAR(available_until) = YEAR(@start_date)
                                            THEN total_amount ELSE 0 END), 0) AS this_month,
                                        COALESCE(SUM(CASE 
                                            WHEN MONTH(available_until) = MONTH(DATE_SUB(@start_date, INTERVAL 1 MONTH))
                                                 AND YEAR(available_until) = YEAR(DATE_SUB(@start_date, INTERVAL 1 MONTH))
                                            THEN total_amount ELSE 0 END), 0) AS last_month
                                    FROM orders
                                    WHERE (@status = '' OR status = @status)
                                )
                                SELECT
                                    this_month,
                                    last_month,
                                    COALESCE(
                                        ROUND(
                                            CASE 
                                                WHEN last_month = 0 AND this_month > 0 THEN 100     
                                                WHEN last_month = 0 AND this_month = 0 THEN 0  
                                                ELSE ((this_month - last_month) / last_month) * 100
                                            END, 
                                            2
                                        ), 0
                                    ) AS percent_change
                                FROM monthly_totals;
                        ";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@start_date", now);
                        cmd.Parameters.AddWithValue("@status", x);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal thisMOneth = reader.GetDecimal("this_month");
                                decimal percentage = reader.GetDecimal("percent_change");
                                return Tuple.Create(thisMOneth.ToString(), percentage.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching transaction data", ex);
            }
            finally
            {
                db.closeConnection();
            }
            return null;
        }
        public Tuple<string, string> getTotalOrders(DateTime now, string x)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                using (var conn = db.getConnection())
                {
                    string query = @"
                            WITH monthly_totals AS (
                                SELECT
                                    COUNT(CASE 
                                        WHEN MONTH(available_until) = MONTH(@start_date)
                                             AND YEAR(available_until) = YEAR(@start_date)
                                        THEN 1 END) AS this_month,
                                    COUNT(CASE 
                                        WHEN MONTH(available_until) = MONTH(DATE_SUB(@start_date, INTERVAL 1 MONTH))
                                             AND YEAR(available_until) = YEAR(DATE_SUB(@start_date, INTERVAL 1 MONTH))
                                        THEN 1 END) AS last_month
                                FROM orders
                                WHERE (@status = '' OR status = @status OR (@status = 'cancelled' AND status = 'voided'))
                            )
                            SELECT
                                this_month,
                                last_month,
                                COALESCE((this_month - last_month), 0) AS change_value
                            FROM monthly_totals;
                        ";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@start_date", now);
                        cmd.Parameters.AddWithValue("@status", x);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int thisMonth = reader.GetInt32("this_month");
                                int changeValue = reader.GetInt32("change_value");

                                return Tuple.Create(thisMonth.ToString(), changeValue.ToString());
                            }
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
            return null;
        }
        public List<Tuple<DateTime, int, int, int>> getOrderMonthly()
        {
            string query = @"
                  WITH RECURSIVE date_sequence AS (
                    SELECT DATE_FORMAT(CURRENT_DATE, '%Y-%m-01') AS `Order-Date`
                    UNION ALL
                    SELECT DATE_ADD(`Order-Date`, INTERVAL 1 DAY)
                    FROM date_sequence
                    WHERE `Order-Date` < CURRENT_DATE 
                )
                SELECT 
                    ds.`Order-Date`,
                    COALESCE(COUNT(o.order_id), 0) AS 'Total Orders',
                    COALESCE(SUM(CASE WHEN o.status = 'Paid' THEN 1 ELSE 0 END), 0) AS 'Paid Orders',
                    COALESCE(SUM(CASE WHEN o.status = 'Cancelled' OR o.status = 'Voided' THEN 1 ELSE 0 END), 0) AS 'Cancelled Orders'
                FROM 
                    date_sequence ds
                LEFT JOIN 
                    orders o 
                    ON DATE_FORMAT(DATE_SUB(o.available_until, INTERVAL 30 MINUTE), '%Y-%m-%d') = ds.`Order-Date`
                WHERE 
                    YEAR(ds.`Order-Date`) = YEAR(CURDATE()) 
                    AND MONTH(ds.`Order-Date`) = MONTH(CURDATE())
                GROUP BY 
                    ds.`Order-Date`
                ORDER BY 
                    ds.`Order-Date`;
                ";

            var result = new List<Tuple<DateTime, int, int, int>>();
            var db = DatabaseHandler.getInstance();
            try
            {
                using (var connection = db.getConnection())
                {
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DateTime orderDate = reader.GetDateTime("Order-Date");
                                int totalOrders = reader.GetInt32("Total Orders");
                                int paidOrder = reader.GetInt32("Paid Orders");
                                int totalCancelled = reader.GetInt32("Cancelled Orders");
                                result.Add(new Tuple<DateTime, int, int, int>(orderDate, totalOrders, paidOrder, totalCancelled));
                            }
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
            return result;
        }
    }
}
