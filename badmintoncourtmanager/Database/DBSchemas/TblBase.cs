using System;
using SQLite;

namespace badmintoncourtmanager.Database.DBSchemas
{
    public class TblBase
    {
        [PrimaryKey, AutoIncrement]
        public int ID  
		{
            get;
            set;
        }

        public DateTime LastModified
        {
            get;set;
        }
    }
}
