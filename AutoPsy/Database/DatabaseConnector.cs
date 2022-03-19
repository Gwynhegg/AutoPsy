using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Database
{
    public sealed class DatabaseConnector
    {
        private SQLiteConnection sqliteConnection;
        private static DatabaseConnector databaseInstance;
        private DatabaseConnector()
        {
            var databaseName = "userdata.db3";
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), databaseName);
            this.sqliteConnection = new SQLiteConnection(path);
        }

        public static DatabaseConnector GetDatabaseConnector()
        {
            if (databaseInstance == null)
                databaseInstance = new DatabaseConnector();
            return databaseInstance;
        }

        public bool IsTableExisted(string tableName)
        {
            var t = this.sqliteConnection.GetTableInfo(tableName);
            if (t.Count == 0) return false; else return true;
        }

        public void CreateAndInsertData(object item)
        {
            this.sqliteConnection.CreateTable(item.GetType());
            this.sqliteConnection.Insert(item);
        }
    }
}
