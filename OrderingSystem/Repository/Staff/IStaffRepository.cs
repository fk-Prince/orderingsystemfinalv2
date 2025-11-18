using System.Collections.Generic;
using OrderingSystem.Model;

namespace OrderingSystem.Repository.Staff
{
    public interface IStaffRepository
    {
        List<StaffModel> getStaff();
        StaffModel loginStaff(string username, string password);
        bool updateStaff(StaffModel staff);
        bool fireStaff(int staffId);
        bool addStaff(StaffModel staff);
        bool isUsernameExists(StaffModel staff);
    }
}
