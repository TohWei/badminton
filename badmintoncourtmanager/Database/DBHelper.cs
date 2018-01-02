using System;
using badmintoncourtmanager.DS;
using badmintoncourtmanager.Managers.Database;
using Xamarin.Forms;

namespace badmintoncourtmanager.Database
{
    public static class DBHelper
    {
        public static readonly string dbFileName = "v1.db3";
        public static object Lock = new object();
        private static DatabaseWrapper dbWrapper = null;


        public static DatabaseWrapper DBWrapper
        {
            get
            {
                lock (Lock) { 
	                if (dbWrapper == null)
	                {
	                    dbWrapper = new DatabaseWrapper(dbFileName);
	                }
                }
                return dbWrapper;
            }
        }



        public static string GetDbPath()
        {
            return DependencyService.Get<ISQLite>().GetDBFilePath(dbFileName);
        }
    }
}
