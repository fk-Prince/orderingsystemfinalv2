using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using OrderingSystem.DatabaseConnection;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Staff
{
    public class StaffRepository : IStaffRepository
    {

        public bool addStaff(StaffModel staff)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_newStaff", conn))
                {
                    byte[] image = null;
                    if (staff.Image != null)
                        image = ImageHelper.GetImageFromFile(staff.Image);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_username", staff.Username);
                    cmd.Parameters.AddWithValue("@p_password", staff.Password);
                    cmd.Parameters.AddWithValue("@p_firstName", staff.FirstName);
                    cmd.Parameters.AddWithValue("@p_lastName", staff.LastName);
                    cmd.Parameters.AddWithValue("@p_phone", staff.PhoneNumber);
                    cmd.Parameters.AddWithValue("@p_date", staff.HiredDate);
                    cmd.Parameters.AddWithValue("@p_role", staff.Role);
                    cmd.Parameters.AddWithValue("@p_image", image);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public bool fireStaff(int staffId)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("UPDATE staff SET status = 'Inactive' WHERE staff_id = @staff_id", conn))
                {
                    cmd.Parameters.AddWithValue("@staff_id", staffId);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<StaffModel> getStaff()
        {
            List<StaffModel> list = new List<StaffModel>();
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                string query = "SELECT * FROM staff";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(StaffModel.Builder()
                                .WithStaffId(reader.GetInt32("staff_id"))
                                .WithUsername(reader.GetString("username"))
                                .WithRole(StaffModel.getRole(reader.GetString("role")))
                                .WithFirstName(reader.GetString("firstName"))
                                .WithImage(ImageHelper.GetImageFromBlob(reader, "staff"))
                                .WithLastName(reader.GetString("lastname"))
                                .WithPhoneNumber(!reader.IsDBNull(reader.GetOrdinal("phone")) ? reader.GetString("phone") : "")
                                .WithHiredDate(reader.GetDateTime("hire_date"))
                                .WithStatus(StaffModel.getStatus(reader.GetString("status")))
                                .Build());
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return list;
        }
        public StaffModel loginStaff(string username, string password)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                string query = "SELECT * FROM staff WHERE username = @username AND password = SHA2(@password, 256)";


                using (var cmd = new MySqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return StaffModel.Builder()
                                .WithStaffId(reader.GetInt32("staff_id"))
                                .WithUsername(reader.GetString("username"))
                                .WithRole(StaffModel.getRole(reader.GetString("role")))
                                .WithFirstName(reader.GetString("firstName"))
                                .WithImage(ImageHelper.GetImageFromBlob(reader, "staff"))
                                .WithLastName(reader.GetString("lastname"))
                                .WithPhoneNumber(!reader.IsDBNull(reader.GetOrdinal("phone")) ? reader.GetString("phone") : "")
                                .WithHiredDate(reader.GetDateTime("hire_date"))
                                .WithStatus(StaffModel.getStatus(reader.GetString("status")))
                                .Build();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }
        public bool updateStaff(StaffModel staff)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                using (var cmd = new MySqlCommand("p_updateStaff", conn))
                {
                    byte[] image = null;
                    if (staff.Image != null)
                        image = ImageHelper.GetImageFromFile(staff.Image);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_staff_id", staff.StaffId);
                    cmd.Parameters.AddWithValue("@p_username", staff.Username);
                    cmd.Parameters.AddWithValue("@p_password", staff.Password);
                    cmd.Parameters.AddWithValue("@p_firstName", staff.FirstName);
                    cmd.Parameters.AddWithValue("@p_lastName", staff.LastName);
                    cmd.Parameters.AddWithValue("@p_phone", staff.PhoneNumber);
                    cmd.Parameters.AddWithValue("@p_role", staff.Role);
                    cmd.Parameters.AddWithValue("@p_image", image);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool isUsernameExists(StaffModel staff)
        {
            var db = DatabaseHandler.getInstance();
            try
            {
                var conn = db.getConnection();
                string query = "";
                if (staff.StaffId != 0) query = "SELECT * FROM staff WHERE username = @username AND staff_id <> @staff_id LIMIT 1";
                else query = "SELECT * FROM staff WHERE username = @username LIMIT 1";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", staff.Username);
                    if (staff.StaffId != 0)
                        cmd.Parameters.AddWithValue("@staff_id", staff.StaffId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
