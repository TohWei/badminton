using System;
using System.Collections.Generic;
using System.Linq;
using badmintoncourtmanager.Database.DBSchemas;
using badmintoncourtmanager.Models;
using Newtonsoft.Json;

namespace badmintoncourtmanager.Database.Repositories
{
    public class UserRepository
    {
        public static Tuple<User, DateTime>GetUser(int id)
        {
            lock(DBHelper.Lock)
            {
                var userRow = DBHelper.DBWrapper.GetComplex<TblUser>("SELECT * FROM TblUser WHERE ID = ?",
                new List<string>{"TblUser"}, new object[]{id});

                if (userRow.Any())
                {
                    var row = userRow.First();
                    var user = new User
                    {
                        ID = row.ID,
                        Name = row.Name,
                        Username = row.Username,
                        Password = row.Password,
                        UserRole = (Models.UserRoleType)row.UserRole
                    };
                    return new Tuple<User, DateTime>(user, row.LastModified);
                }
                return null;
            }
        }

        public static int SaveOrUpdateUser(User user)
        {
			lock (DBHelper.Lock)
			{
				if (DBHelper.DBWrapper.IsTableExists<TblUser>())
				{
                    var result = GetUser(user.ID);
                    if (result != null && result.Item1 != null)
                    {
                        return DBHelper.DBWrapper.Update("Update TblUser set Name = ?, Username =?, Password= ? , UserRole = ? where ID = ?"
                                                  , new object[]{user.Name, user.Username, user.Password, user.UserRole, user.ID});
                    }
				}

				var tblUserRow = new TblUser()
				{
					ID = user.ID,
					Name = user.Name,
					Username = user.Username,
					Password = user.Password,
					UserRole = (Database.DBSchemas.UserRoleType)user.UserRole,
				};

				return DBHelper.DBWrapper.InsertNewRow<TblUser>(tblUserRow);
			}

			//Insert or replace not quite working as expected.
			//return DBHelper.DBWrapper.InsertOrReplace<TblUser>(tblUserRow);
        }


        public static int CreateUser(User user)
        {
            lock(DBHelper.Lock)
            {
                if (DBHelper.DBWrapper.IsTableExists<TblUser>())
                {
                    DBHelper.DBWrapper.ExecuteNonQuery("DELETE from TblUser WHERE Username = ?", new object[] { user.Username });
                }

                var tblUserRow = new TblUser()
                {
                    ID = user.ID,
                    Name = user.Name,
                    Username = user.Username,
                    Password = user.Password,
                    UserRole = (Database.DBSchemas.UserRoleType)user.UserRole,
                };

                return DBHelper.DBWrapper.InsertNewRow<TblUser>(tblUserRow);
            }
        }

        public static Tuple<List<User>, DateTime>GetUserList()
        {
            IEnumerable<TblUser> rows = DBHelper.DBWrapper.Get<TblUser>();
            try{
                if (rows != null && rows.Any())
                {
                    List<User> userList = new List<User>();
                    foreach(var item in rows)
                    {
                        var user = new User
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Username = item.Username,
                            Password = item.Password,
                            UserRole = (Models.UserRoleType)item.UserRole
                        };
                        userList.Add(user);
                    }
                   return new Tuple<List<User>, DateTime>(userList, rows.First().LastModified);
                }
            }catch(Exception)
            {
                
            }
            return Tuple.Create<List<User>, DateTime>(null, DateTime.UtcNow);
        }

        public static bool DeleteUser(int id){
			lock (DBHelper.Lock)
			{
                var deleted = DBHelper.DBWrapper.DeleteData<TblUser>(id);
                return deleted;
			}
        }
    }
}
