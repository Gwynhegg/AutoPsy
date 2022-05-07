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

        public void AddEntity(string parameter, ITableEntity entity)
        {
            tableController[parameter].Add(entity);
            CreateTableEntity(entity);
        }

        public abstract void CreateTableEntity(ITableEntity entity);

        public List<byte?> GetValues(int lastIndex, DateTime start, DateTime end)
        {
            var result = new List<byte?>();
            var records = tableController.ElementAt(lastIndex).Value;
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                var record = records.Where(x => x.Time == date).FirstOrDefault();
                if (record != null) result.Add(record.Value); else result.Add(null);
            }
            return result;
        }

        public bool ContainsEntity(ITableEntity entity)
        {
            foreach (var record in tableController.Keys)
                if (record.Equals(entity.Name)) return true;
            return false;
        }

        public string GetEntityValueString(string name, DateTime date)
        {
            return tableController[name].Where(x => DateTime.Compare(date.Date, x.Time) == 0).Select(x => x.Value).FirstOrDefault().ToString();
        }

        public List<ITableEntity> SelectRecomendations(DateTime start, DateTime end)
        {
            return App.TableGraph.GetRecomendations().Where(x => tableController.ContainsKey(x.Name)).ToList();
        }

        public List<ITableEntity> SelectConditions(DateTime start, DateTime end)
        {
            return App.TableGraph.GetConditions().Where(x => tableController.ContainsKey(x.Name)).ToList();
        }

        public List<ITableEntity> SelectTriggers(DateTime start, DateTime end)
        {
            return App.TableGraph.GetTriggers().Where(x => tableController.ContainsKey(x.Name)).ToList();
        }

        protected void SelectAllItems<T>() where T : new()
        {
            var items = App.Connector.SelectAll<T>().Cast<ITableEntity>();
            foreach (var item in items)
                if (!tableController.ContainsKey(item.Name))
                {
                    tableController.Add(item.Name, new List<ITableEntity>());
                    tableController[item.Name].Add(item);
                }
                else
                    tableController[item.Name].Add(item);
        }
    }

    public class RecomendationTableEntityHandler : TableEntityHandler
    {
        public override void CreateTableEntity(ITableEntity entity)
        {
            App.Connector.CreateAndInsertData<TableRecomendation>(entity);
        }

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
        public override void CreateTableEntity(ITableEntity entity)
        {
            App.Connector.CreateAndInsertData<TableCondition>(entity);
        }
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
        public override void CreateTableEntity(ITableEntity entity)
        {
            App.Connector.CreateAndInsertData<TableTrigger>(entity);
        }
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
