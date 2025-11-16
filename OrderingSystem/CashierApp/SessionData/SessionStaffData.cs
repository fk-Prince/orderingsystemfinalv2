using System.Drawing;
using OrderingSystem.Model;

namespace OrderingSystem.CashierApp.SessionData
{
    public class SessionStaffData
    {

        public static StaffModel StaffData { get; set; }
        public static int StaffId { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static StaffModel.StaffRole Role { get; set; }
        public static Image Image { get; set; }


        public static string getFullName() => $"{FirstName.Substring(0, 1) + FirstName.Substring(1)}  {LastName.Substring(0, 1) + LastName.Substring(1)}";

        public static void setSessionData(StaffModel loginStaff)
        {
            StaffData = loginStaff;
            StaffId = loginStaff.StaffId;
            FirstName = loginStaff.FirstName;
            LastName = loginStaff.LastName;
            Role = loginStaff.Role;
            Image = loginStaff.Image;
        }

    }
}
