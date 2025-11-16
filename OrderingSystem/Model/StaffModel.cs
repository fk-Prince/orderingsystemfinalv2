using System;
using System.Drawing;

namespace OrderingSystem.Model
{
    public class StaffModel
    {

        public enum StaffRole
        {
            Cashier,
            Manager
        }
        public enum StaffStatus
        {
            Active,
            InActive
        }

        public int StaffId { get; set; }
        public string Username { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public DateTime HiredDate { get; protected set; }
        public string Password { get; protected set; }
        public StaffRole Role { get; protected set; }
        public StaffStatus Status { get; protected set; }
        public Image Image { get; protected set; }
        public static StaffBuilder Builder() => new StaffBuilder();

        public static StaffRole getRole(string s)
        {
            return s.ToLower() == "cashier" ? StaffRole.Cashier : StaffRole.Manager;
        }
        public static StaffStatus getStatus(string s)
        {
            return s.ToLower() == "active" ? StaffStatus.Active : StaffStatus.InActive;
        }
        public string getFullName()
        {
            return FirstName.Substring(0, 1).ToUpper() + FirstName.Substring(1).ToLower() + "  " + LastName.Substring(0, 1).ToUpper() + LastName.Substring(1).ToLower();
        }
        public interface IStaffBuilder
        {
            StaffBuilder WithStaffId(int txt);
            StaffBuilder WithUsername(string txt);
            StaffBuilder WithFirstName(string txt);
            StaffBuilder WithLastName(string txt);
            StaffBuilder WithImage(Image txt);
            StaffBuilder WithPhoneNumber(string txt);
            StaffBuilder WithHiredDate(DateTime txt);
            StaffBuilder WithPassword(string txt);
            StaffBuilder WithRole(StaffRole txt);
            StaffBuilder WithStatus(StaffStatus txt);
            StaffModel Build();
        }
        public class StaffBuilder : IStaffBuilder
        {
            private StaffModel staff;

            public StaffBuilder()
            {
                staff = new StaffModel();
            }

            public StaffBuilder WithUsername(string username)
            {
                this.staff.Username = username;
                return this;
            }
            public StaffBuilder WithPassword(string txt)
            {
                this.staff.Password = txt;
                return this;
            }

            public StaffModel Build()
            {
                return staff;
            }

            public StaffBuilder WithStaffId(int txt)
            {
                this.staff.StaffId = txt;
                return this;
            }

            public StaffBuilder WithRole(StaffRole txt)
            {
                this.staff.Role = txt;
                return this;
            }

            public StaffBuilder WithFirstName(string txt)
            {
                this.staff.FirstName = txt;
                return this;
            }

            public StaffBuilder WithLastName(string txt)
            {
                this.staff.LastName = txt;
                return this;
            }

            public StaffBuilder WithPhoneNumber(string txt)
            {
                this.staff.PhoneNumber = txt;
                return this;
            }

            public StaffBuilder WithHiredDate(DateTime txt)
            {
                this.staff.HiredDate = txt;
                return this;
            }

            public StaffBuilder WithStatus(StaffStatus txt)
            {
                this.staff.Status = txt;
                return this;
            }

            public StaffBuilder WithImage(Image txt)
            {
                this.staff.Image = txt;
                return this;
            }
        }
    }
}
