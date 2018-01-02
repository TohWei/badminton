using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using badmintoncourtmanager.Database.Repositories;
using badmintoncourtmanager.Models;
using SQLite;

namespace badmintoncourtmanager.Managers
{
    public class UserManager : IUserManager
    {
        
		public Task<List<User>> GetUserList()
        {
            return Task.Factory.StartNew(() => UserRepository.GetUserList().Item1);
		}

        public Task<int> CreateUser(User user)
        {
            return Task.Factory.StartNew(() => UserRepository.CreateUser(user));
        }

        public Task<int> CreateOrUpdateUser(User user)
        {
            return Task.Factory.StartNew(() => UserRepository.SaveOrUpdateUser(user));
        }

		public Task<bool> DeleteUser(int id)
		{
            return Task.Factory.StartNew(() => UserRepository.DeleteUser(id));
		}

    }
}

/*
        private readonly SQLiteConnection conn;

        public string StatusMessage{
            get;set;
        }

        public UserManager(string dbPath)
        {
            conn = new SQLiteConnection(dbPath);
            conn.CreateTable<TblUser>();
        }

        public Task CreateUser(TblUser tblUser)
        {
            try{
                if (string.IsNullOrWhiteSpace(tblUser.Name))
                    throw new Exception("Name is required");

                //Insert or update user
                return Task.Factory.StartNew(() =>
                {
                    var result = conn.InsertOrReplace(tblUser);
                    StatusMessage = $"{result} record(s) added [Contact Name: {tblUser.Name}]";
                });
            } catch (Exception ex   ){
                StatusMessage = $"Failed to create user: {tblUser.Name}. Error: {ex.Message}";
                return null;
            }
        }

        public Task<List<TblUser>> GetAllUsers()
        {
            return Task.Factory.StartNew(() => conn.Table<TblUser>().ToList());
        }
        */

