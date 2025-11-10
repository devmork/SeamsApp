using SeamsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamsApp.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User> GetUserById(int userId);
        Task<List<string>> GetAllUsernames();
        Task<User> GetByUsername(string identifier);

    }
}
