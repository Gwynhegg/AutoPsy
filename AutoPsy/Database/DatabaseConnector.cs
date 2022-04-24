using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoPsy.Database
{
    public class DatabaseConnector
    {
        readonly SQLiteConnection sqliteConnection;
        public int currentConnectedUser { get; private set; }
        public DatabaseConnector()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Const.Constants.DATABASE_NAME);
            var flags =
                SQLite.SQLiteOpenFlags.ReadWrite |
                SQLite.SQLiteOpenFlags.Create |
                SQLite.SQLiteOpenFlags.SharedCache;

            this.sqliteConnection = new SQLiteConnection(path, flags);
        }

        public bool IsTableExisted<T>() where T : new()
        {
            // ЗАГЛУШКА ДЛЯ ПРОВЕРКИ РЕГИСТРАЦИИ
            //this.sqliteConnection.DropTable<Entities.DiaryPage>();
            //this.sqliteConnection.DropTable<Entities.UserExperience>();

            try
            {
                var result = this.sqliteConnection.Table<T>().First();

                if (result is Entities.User) currentConnectedUser = (result as Entities.User).Id;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateAndInsertData<T>(object item)
        {
            var t = this.sqliteConnection.CreateTable<T>();
            this.sqliteConnection.Insert(item);

            if (item is Entities.User) currentConnectedUser = (item as Entities.User).Id;

        }

        public void UpdateData<T>(object item)
        {
            this.sqliteConnection.CreateTable<T>();
            this.sqliteConnection.Update(item);
        }

        public void DeleteData(object item)
        {
            this.sqliteConnection.Delete(item);
        }

        public T SelectData<T>(int ID) where T : new()
        {
            return this.sqliteConnection.Find<T>(ID);
        }

        public List<T> SelectAll<T>() where T : new()
        {
            return this.sqliteConnection.Table<T>().ToList();
        }

        public void CloseConnection()
        {
            this.sqliteConnection.Close();
        }
    }
}
