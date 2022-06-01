using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoPsy.Database
{
    public class DatabaseConnector
    {
        private readonly SQLiteConnection sqliteConnection;
        public int currentConnectedUser { get; private set; }
        public DatabaseConnector()
        {
            // Получаем путь к локальной базе данных
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Const.Constants.DATABASE_NAME);
            SQLiteOpenFlags flags =
                SQLite.SQLiteOpenFlags.ReadWrite |
                SQLite.SQLiteOpenFlags.Create |
                SQLite.SQLiteOpenFlags.SharedCache;

            this.sqliteConnection = new SQLiteConnection(path, flags);      // Устанавливаем соединение с базой данных
        }

        public bool IsTableExisted<T>() where T : new()
        {

            try
            {
                T result = this.sqliteConnection.Table<T>().First();      // Пытаемся получить первую запись в таблице

                if (result is Entities.User) this.currentConnectedUser = (result as Entities.User).Id;       // Если результат является пользователем, то сохраняем его в качестве текущего
                return true;
            }
            catch
            {
                return false;       // Если не получилось, то таблица либо пуста, либо не существует (что на SQLite почти одно и то же)
            }
        }

        // Метод для создания таблицы и добавления в нее записи (с использованием дженериков)
        public void CreateAndInsertData<T>(object item)
        {
            CreateTableResult t = this.sqliteConnection.CreateTable<T>();     // Если таблица уже есть, данный шаг проигнорируется
            this.sqliteConnection.Insert(item);     // Вставляем запись

            if (item is Entities.User) this.currentConnectedUser = (item as Entities.User).Id;

        }

        public void DropTable<T>() => this.sqliteConnection.DropTable<T>();

        public void UpdateData<T>(object item)      // Метод для обновления данных (с использованием дженериков)
        {
            this.sqliteConnection.CreateTable<T>();     // Аналогично предыдущему методу
            this.sqliteConnection.Update(item);     // обновляем запись в таблице
        }

        public void DeleteData(object item)     // Метод для удаления объекта из базы
=> this.sqliteConnection.Delete(item);

        public T SelectData<T>(int ID) where T : new()      // Метод для выборки объекта определенного типа по ID
=> this.sqliteConnection.Find<T>(ID);

        public List<T> SelectAll<T>() where T : new()       // Метод для выборки всех объектов определенного типа
=> this.sqliteConnection.Table<T>().ToList();

        public void CloseConnection()       // Метод для закрытия соединения
=> this.sqliteConnection.Close();

        // Отдельный метод для выборки данных из дневника (поскольку в нем фигурируют даты начала и конца записей)
        public List<Entities.DiaryPage> SelectData(DateTime dateStart, DateTime dateEnd) => this.sqliteConnection.Table<Entities.DiaryPage>().Where(x => x.DateOfRecord >= dateStart && x.DateOfRecord <= dateEnd).ToList();
    }
}
