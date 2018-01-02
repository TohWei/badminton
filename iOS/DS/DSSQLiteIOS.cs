using System;
using System.IO;
using badmintoncourtmanager.DS;
using badmintoncourtmanager.iOS.DS;
using SQLite;
using Xamarin.Forms;

[assembly:Dependency(typeof(DSSQLiteIOS))]
namespace badmintoncourtmanager.iOS.DS
{
    public class DSSQLiteIOS : ISQLite
    {
        const string prefix = "iOS";
        private string fileName;

        public DSSQLiteIOS()
        {
        }

        public SQLiteConnection GetConnection(string fileName)
        {
            var path = GetDBFilePath(fileName);
            var conn = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true);
            return conn;
        }

        public string GetDBFilePath(string fileName)
        {
			fileName = string.Format("{0}{1}", prefix, fileName);
			var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(documentPath, "..", "Library");
			var path = Path.Combine(libraryPath, fileName);

            Console.WriteLine("DB path::: " + path);
            return path;
        }
    }
}
