using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace AutoPsy.Database
{
    public sealed class DatabaseConnector
    {
        private SQLiteConnection sqliteConnection;
        private static DatabaseConnector databaseInstance;
        public int currentConnectedUser { get; private set; }
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
            // ЗАГЛУШКА ДЛЯ ПРОВЕРКИ РЕГИСТРАЦИИ
            this.sqliteConnection.DropTable<Entities.User>();
            var t = this.sqliteConnection.GetTableInfo(tableName);
            if (t.Count == 0) return false; else return true;
        }

        public void CreateAndInsertData(object item)
        {
            this.sqliteConnection.CreateTable(item.GetType());
            this.sqliteConnection.Insert(item);

            if (item is Entities.User) currentConnectedUser = (item as Entities.User).Id;
        }

        public void UpdateData(object item)
        {
            this.sqliteConnection.CreateTable(item.GetType());
            this.sqliteConnection.Update(item);
        }

        public void DeleteData(object item)
        {
            this.sqliteConnection.Delete(item);
        }
    }
}
