using System;
using SQLite;

namespace badmintoncourtmanager.Database.DBSchemas
{
	public class TblUser : TblBase
	{
		[NotNull, MaxLength(50)]
		public string Name
		{
			get;
			set;
		}

        [NotNull, MaxLength(20)]
		public string Username
		{
			get;
			set;
		}

        [NotNull, MaxLength(20)]
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
