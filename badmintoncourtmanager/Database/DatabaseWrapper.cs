using System;
using System.Collections.Generic;
using System.Linq;
using badmintoncourtmanager.Database.DBSchemas;
using badmintoncourtmanager.DS;
using SQLite;
using Xamarin.Forms;

namespace badmintoncourtmanager.Managers.Database
{
    public class DatabaseWrapper
    {
        private SQLiteConnection conn;

        public DatabaseWrapper(string fileName)
        {
            if (conn == null)
            {
                conn = DependencyService.Get<ISQLite>().GetConnection(fileName);
            }
        }

        ~DatabaseWrapper()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }


        #region Generic wrapper methods to native SQL operation.
        public void CreateTable<T>()
        {
            conn.CreateTable<T>();
        }

        public IEnumerable<T> Get<T>() where T: class, new()
        {
            try{
                if (IsTableExists<T>())
                {
                    return (from b in conn.Table<T>() select b).ToList();
                }else{
                    return null;
                }
            }catch (SQLiteException ex)
            {
                return null;
            }
        }


		public int GetCount<T>(string query, params object[] args)
		{
            int rowCount = 0;
			try
			{
				SQLiteCommand cmd = conn.CreateCommand(query, args);
                rowCount = Convert.ToInt32(cmd.ExecuteScalar<T>());
                return rowCount;
			}
			catch (Exception)
			{
                return rowCount;
			}
		}


		
        public IEnumerable<T> Get<T>(string query, params object[] args)
        {
            try {
                SQLiteCommand cmd = conn.CreateCommand(query, args);
                return cmd.ExecuteDeferredQuery<T>();
            }catch (Exception){
                return null;
            }
        }


        public IEnumerable<T>GetComplex<T>(string query, List<string> tableList, params object[] args)
        {
            if (tableList == null)
            {
                throw new Exception("Tble list cannot be null. Please provide the tables use in inner or outter join.");
            }

            if (IsTableExists(tableList)){
                try{
                    var cmd = conn.CreateCommand(query, args);
                    return cmd.ExecuteDeferredQuery<T>();
                }catch(Exception ex)
                {
                    return null;
                }
            }else{
                return null;
            }
        }

        public T Get<T> (int id)
        {
            Type t = typeof(T);
            string query = string.Format("select * from \"{0}\" where ID = ? ",t.Name);
            return Get<T>(query, id).FirstOrDefault();
        }


        public bool DeleteData<T>(int id)
        {
            if (IsTableExists<T>()){
                conn.Delete<T>(id);
                return true;
            }
            return false;
        }

        public bool DeleteAllData<T>()
        {
            if (IsTableExists<T>())
            {
                conn.DeleteAll<T>();
                return true;
            }
            return false;
        }

        public int InsertOrReplace<T>(T tbl) where T : TblBase
        {
            tbl.LastModified = DateTime.UtcNow;
            var affectedRow = conn.InsertOrReplace(tbl);
			if (affectedRow >= 0)
			{
                return affectedRow;
			}
			else
			{
				return -1;
			}
        }

        public int InsertNewRow<T>(T tbl) where T: TblBase{
            conn.CreateTable<T>();
            tbl.LastModified = DateTime.UtcNow;
            var affectedRow = conn.Insert(tbl);

            if (affectedRow >= 0)
            {
                SQLiteCommand cmd = conn.CreateCommand("SELECT last_insert_rowid()", 1);
                return cmd.ExecuteScalar<int>();
            }else{
                return -1;
            }
        }


        public int UpdateRow<T>(T tbl) where T: TblBase{
            tbl.LastModified = DateTime.UtcNow;
            return conn.Update(tbl);
        }


        public int Update(string query,  object[] args)
        {
            var cmd = conn.CreateCommand(query, args);
            int r = cmd.ExecuteNonQuery();
            return r;
        }


        public int ExecuteNonQuery(string query, object[] args)
        {
            var cmd = conn.CreateCommand(query, args);
            int r = cmd.ExecuteNonQuery();
            return r;
        }


        public void DropTable<T>(){
            conn.DropTable<T>();
        }


        public void CLoseConn()
        {
            if (conn != null){
                try{
                    conn.Close();;
                }finally{
                    
                }
            }
        }


        public bool IsTableExists<T>()
        {
            const string cmdText = "Select name from sqlite_master where type = 'table' and name=?";
            var cmd = conn.CreateCommand(cmdText, typeof(T).Name);
            return cmd.ExecuteScalar<string>() != null;
        }

        private bool IsTableExists(List<string> tableList)
        {
            var formatTableList = from x in tableList
                                  select string.Format("'{0}'", x);
            var p = string.Join(",", formatTableList);

            string cmdText = string.Format("Select count(name) from sqlite_master where type = 'table' and name in ({0})", p);
            var cmd = conn.CreateCommand(cmdText, 1);
            bool status = cmd.ExecuteScalar<int>() == tableList.Count;
            return status;
        }


        private void GetTableInfo(string tableName)
        {
            var query = "pragma table_info(\"" + tableName + "\")";
            var tblInfo = conn.Query<SQLiteConnection.ColumnInfo>(query);

            foreach (var info in tblInfo)
            {
                System.Diagnostics.Debug.WriteLine(info.Name);
            }
        }
        #endregion


    }
}
