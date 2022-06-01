using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public abstract class TableEntityHandler
    {
        protected Dictionary<string, List<ITableEntity>> tableController;

        public TableEntityHandler() => this.tableController = new Dictionary<string, List<ITableEntity>>();

        public abstract bool CheckEntityExisted();

        public byte GetEntityValue(string name, DateTime date) => this.tableController[name].Where(x => DateTime.Compare(date.Date, x.Time) == 0).Select(x => x.Value).First();

        public void AddParameter(string parameter)
        {
            if (!this.tableController.ContainsKey(parameter))
                this.tableController.Add(parameter, new List<ITableEntity>());
        }

        public void AddEntity<T>(string parameter, ITableEntity entity)
        {
            if (this.tableController[parameter].FirstOrDefault(x => x.IdValue.Equals(entity.IdValue) && DateTime.Compare(x.Time, entity.Time) == 0) == null)
            {
                this.tableController[parameter].Add(entity);
                CreateTableEntity<T>(entity);
            }

        }

        private void CreateTableEntity<T>(ITableEntity entity) => App.Connector.CreateAndInsertData<T>(entity);

        public Dictionary<string, List<ITableEntity>> GetValues(DateTime start, DateTime end)
        {
            if (!CheckEntityExisted()) return null;
            return this.tableController;
        }

        public List<string> GetFilterResults(DateTime start, DateTime end) => this.tableController.Where(x => x.Value.Any(t => t.Time >= start && t.Time <= end)).Select(x => x.Key).ToList();

        public List<ITableEntity> GetEntities(string parameter) => this.tableController[parameter];

        public List<string> GetAllParameters() => this.tableController.Keys.Select(x => App.TableGraph.GetNameByIdString(x)).ToList();

        public bool ContainsEntity(ITableEntity entity)
        {
            foreach (var record in this.tableController.Keys)
                if (record.Equals(entity.IdValue)) return true;
            return false;
        }

        public bool ContainsParameter(string parameter)
        {
            foreach (var key in this.tableController.Keys)
                if (key.Equals(parameter)) return true;
            return false;
        }

        protected void SelectAllItems<T>() where T : new()
        {
            IEnumerable<ITableEntity> items = App.Connector.SelectAll<T>().Cast<ITableEntity>();
            foreach (ITableEntity item in items)
            {
                if (!this.tableController.ContainsKey(item.IdValue))
                {
                    this.tableController.Add(item.IdValue, new List<ITableEntity>());
                    this.tableController[item.IdValue].Add(item);
                }
                else
                {
                    this.tableController[item.IdValue].Add(item);
                }
            }
        }

        public void DeleteParameter(string parameter)
        {
            IEnumerable<List<ITableEntity>> selectedItemsToDelete = this.tableController.Where(x => x.Key.Equals(parameter)).Select(x => x.Value);
            this.tableController.Remove(parameter);
            foreach (List<ITableEntity> item in selectedItemsToDelete)
                App.Connector.DeleteData(item);
        }

        public void UpdateParameter<T>(string parameter)
        {
            List<ITableEntity> selectedItemsToUpdate = this.tableController[parameter];
            foreach (ITableEntity item in selectedItemsToUpdate)
            {
                App.Connector.UpdateData<T>(item);
            }
        }

        public void UpdateEntityValue<T>(ITableEntity entity) where T : new()
        {
            T request = App.Connector.SelectData<T>(entity.Id);
            if (request == null)
                App.Connector.CreateAndInsertData<T>(entity);
            else
                App.Connector.UpdateData<T>(entity);
        }

        public static void StaticUpdateEntityValue<T>(ITableEntity entity) where T : new()
        {
            T request = App.Connector.SelectData<T>(entity.Id);
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
