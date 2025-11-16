using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Coupon
{
    public class CouponRepository : ICouponRepository
    {
        public DataView generateCoupon(CouponModel co)
        {
            var db = DatabaseHandler.getInstance();
            DataTable dt = new DataTable();
            try
            {
                var conn = db.getConnection();

                MySqlCommand cmd = new MySqlCommand("p_GenerateCoupon", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MessageBox.Show(co.NumberOfTimes.ToString());
                cmd.Parameters.AddWithValue("p_times", co.NumberOfTimes);
                cmd.Parameters.AddWithValue("p_rate", co.CouponRate);
                cmd.Parameters.AddWithValue("p_expiry_date", co.ExpiryDate);
                cmd.Parameters.AddWithValue("p_description", co.Description);
                cmd.Parameters.AddWithValue("p_type", co.getType());
                cmd.Parameters.AddWithValue("p_min", co.CouponMin);
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
                DataView view = new DataView(dt);
                return view;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            return null;
        }

        public List<CouponModel> getAllCoupon()
        {

            var db = DatabaseHandler.getInstance();
            List<CouponModel> list = new List<CouponModel>();
            try
            {
                var conn = db.getConnection();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Coupon", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new CouponModel(
                        reader.GetString("coupon_code"),
                        reader.GetString("status"),
                        reader.GetDouble("rate"),
                        reader.GetDateTime("expiry_Date"),
                         reader.IsDBNull(reader.GetOrdinal("coupon_description")) ? "" : reader.GetString(reader.GetOrdinal("coupon_description")),
                          CouponModel.getType(reader.GetString("type")), reader.GetDouble("min")
                       ));
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.closeConnection();
            }
            return list;
        }

        public CouponModel getCoupon(string code)
        {
            var db = DatabaseHandler.getInstance();

            try
            {
                var conn = db.getConnection();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM Coupon WHERE coupon_code = @coupon_code LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@coupon_code", code);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    return new CouponModel(
                        reader.GetString("coupon_code"),
                        reader.GetString("status"),
                        reader.GetDouble("rate"),
                        reader.GetDateTime("expiry_Date"),
                        reader.IsDBNull(reader.GetOrdinal("coupon_description")) ? "" : reader.GetString(reader.GetOrdinal("coupon_description")),
                         CouponModel.getType(reader.GetString("type")), reader.GetDouble("min")
                       );
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            return null;
        }


    }
}
