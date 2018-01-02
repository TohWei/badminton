using System;
namespace badmintoncourtmanager.Models
{
    public class User : BaseClass
    {
        public User()
        {
        }

        public string Name
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public UserRoleType UserRole
        {
            get;
            set;
        }

    }

    public enum UserRoleType
    {
        Admin,
    }

}
