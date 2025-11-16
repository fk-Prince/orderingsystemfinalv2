using System.Collections.Generic;
using System.Text.RegularExpressions;
using OrderingSystem.CashierApp.SessionData;
using OrderingSystem.Exceptions;
using OrderingSystem.Model;
using OrderingSystem.Repository.Staff;

namespace OrderingSystem.Services
{
    public class StaffServices
    {
        private readonly IStaffRepository staffRepository;
        public StaffServices()
        {
            staffRepository = new StaffRepository();
        }

        public bool isInputValidated(StaffModel staff)
        {
            string numberRegex = @"^09\d{9}$";
            string letterRegex = @"^[A-Za-z]+$";
            string numberLetterRegex = @"^[A-Za-z0-9]+$";
            if (!string.IsNullOrWhiteSpace(staff.PhoneNumber) && !Regex.IsMatch(staff.PhoneNumber, numberRegex))
                throw new InvalidInput("Invalid Phone number.");

            if (!Regex.IsMatch(staff.FirstName, letterRegex) || !Regex.IsMatch(staff.LastName, letterRegex))
                throw new InvalidInput("Invalid Name.");

            if (!string.IsNullOrWhiteSpace(staff.Password) && !Regex.IsMatch(staff.Password, numberLetterRegex))
                throw new InvalidInput("Password should not contain Special Characters.");

            if (isUsernameExists(staff))
                throw new InvalidInput("Username exists, Try another one");

            return true;
        }
        public bool updateStaff(StaffModel model)
        {
            return staffRepository.updateStaff(model);
        }
        public bool fireStaff(int staffIdFire)
        {
            if (staffIdFire == SessionStaffData.StaffId)
            {
                throw new InvalidAction("You are unable to fire yourself");
            }
            return staffRepository.fireStaff(staffIdFire);
        }
        public bool isUsernameExists(StaffModel staff)
        {
            return staffRepository.usernameExists(staff);
        }
        public List<StaffModel> getStaffs()
        {
            return staffRepository.getStaff();
        }
        public bool addStaff(StaffModel staff)
        {
            if (!isInputValidated(staff)) return false;
            if (isUsernameExists(staff))
            {
                throw new InvalidInput("Username exists, Try another one");
            }

            return staffRepository.addStaff(staff);
        }
    }
}
