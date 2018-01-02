using System;
using SQLite;

namespace badmintoncourtmanager.DS
{
    public interface ISQLite
    {
		SQLiteConnection GetConnection(string fileName);
		string GetDBFilePath(string fileName);
    }
}
