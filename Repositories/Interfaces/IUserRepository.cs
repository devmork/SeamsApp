using AttendanceManagementSystem.Models.Derived;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email, string password);
        void AddUser(User user);
    }
}
