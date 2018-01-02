using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public interface IUserManager
    {
        Task<List<User>>GetUserList();
        Task<int> CreateUser(User user);
        Task<int> CreateOrUpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}
