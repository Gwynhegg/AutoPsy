using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public abstract class TableEntityHandler
    {
        protected Dictionary<string, List<ITableEntity>> tableController;

        public TableEntityHandler()
        {
            tableController = new Dictionary<string, List<ITableEntity>>();
        }

        public abstract bool CheckEntityExisted();

        public byte GetEntityValue(string name, DateTime date)
        {
            return tableController[name].Where(x => DateTime.Compare(date.Date, x.Time) == 0).Select(x => x.Value).First();
        }

        public void AddParameter(string parameter)
        {
            if (!tableController.ContainsKey(parameter))
                tableController.Add(parameter, new List<ITableEntity>());
        }

        public void AddEntity<T>(string parameter, ITableEntity entity)
        {
            if (tableController[parameter].FirstOrDefault(x => x.IdValue.Equals(entity.IdValue) && DateTime.Compare(x.Time, entity.Time) == 0) == null)
            {
                tableController[parameter].Add(entity);
                CreateTableEntity<T>(entity);
            }

        }

        public void CreateTableEntity<T>(ITableEntity entity)
        {
            App.Connector.CreateAndInsertData<T>(entity);
        }

        public Dictionary<string, List<ITableEntity>> GetValues(DateTime start, DateTime end)
        {
            if (!CheckEntityExisted()) return null;
            return tableController;
        }

        public List<string> GetFilterResults(DateTime start, DateTime end)
        {
            return tableController.Where(x => x.Value.Any(t => t.Time >= start && t.Time <= end)).Select(x => x.Key).ToList();
        }

        public List<ITableEntity> GetEntities(string parameter)
        {
            return tableController[parameter];
        }

        public List<string> GetAllParameters()
        {
            return tableController.Keys.Select(x => App.TableGraph.GetNameByIdString(x)).ToList();
        }

        public bool ContainsEntity(ITableEntity entity)
        {
            foreach (var record in tableController.Keys)
                if (record.Equals(entity.IdValue)) return true;
            return false;
        }

        public bool ContainsParameter(string parameter)
        {
            foreach (var key in tableController.Keys)
                if (key.Equals(parameter)) return true;
            return false;
        }

        protected void SelectAllItems<T>() where T : new()
        {
            var items = App.Connector.SelectAll<T>().Cast<ITableEntity>();
            foreach (var item in items)
                if (!tableController.ContainsKey(item.IdValue))
                {
                    tableController.Add(item.IdValue, new List<ITableEntity>());
                    tableController[item.IdValue].Add(item);
                }
                else
                    tableController[item.IdValue].Add(item);
        }

        public void DeleteParameter(string parameter)
        {
            var selectedItemsToDelete = tableController.Where(x => x.Key.Equals(parameter)).Select(x => x.Value);
            tableController.Remove(parameter);
            foreach (var item in selectedItemsToDelete)
                App.Connector.DeleteData(item);
        }

        public void UpdateParameter<T>(string parameter, byte newValue)
        {
            var selectedItemsToUpdate = tableController[parameter];
            foreach (var item in selectedItemsToUpdate)
            {
                item.Importance = newValue;
                App.Connector.UpdateData<T>(item);
            }
        }

        public void UpdateEntityValue<T>(ITableEntity entity) where T : new()
        {
            var request = App.Connector.SelectData<T>(entity.Id);
            if (request == null)
                App.Connector.CreateAndInsertData<T>(entity);
            else
                App.Connector.UpdateData<T>(entity);
        }

        public static void StaticUpdateEntityValue<T>(ITableEntity entity) where T : new()
        {
            var request = App.Connector.SelectData<T>(entity.Id);
            if (request == null)
                App.Connector.CreateAndInsertData<T>(entity);
            else
                App.Connector.UpdateData<T>(entity);
        }

        public static void UpdateEntityValue(ITableEntity entity)
        {
            if (entity is TableRecomendation) StaticUpdateEntityValue<TableRecomendation>(entity);
            if (entity is TableCondition) StaticUpdateEntityValue<TableCondition>(entity);
            if (entity is TableTrigger) StaticUpdateEntityValue<TableTrigger>(entity);
        }

    }

    public class RecomendationTableEntityHandler : TableEntityHandler
    {
        public override bool CheckEntityExisted()
        {
            if (App.Connector.IsTableExisted<TableRecomendation>())
            {
                SelectAllItems<TableRecomendation>();
                return true;
            }
            return false;
        }
    }


    public class CondtiionTableEntityHandler : TableEntityHandler
    {
        public override bool CheckEntityExisted()
        {
            if (App.Connector.IsTableExisted<TableCondition>())
            {
                SelectAllItems<TableCondition>();
                return true;
            }
            return false;
        }
    }

    public class TriggerTableEntityHandler : TableEntityHandler
    {
        public override bool CheckEntityExisted()
        {
            if (App.Connector.IsTableExisted<TableTrigger>())
            {
                SelectAllItems<TableTrigger>();
                return true;
            }
            return false;
        }
    }
}
