using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class TableEntityHandler
    {
        private Dictionary<string, List<ITableEntity>> tableController;
        
        public TableEntityHandler()
        {
            tableController = new Dictionary<string, List<ITableEntity>>();
        }

        public bool CheckEntityExisted<T>() where T : new()
        {
            if (App.Connector.IsTableExisted<T>())
            {
                SelectAllItems<T>();
                return true;
            }
            return false;
        }

        public byte GetEntityValue(string name, DateTime date)
        {
            return tableController[name].Where(x => DateTime.Compare(date.Date, x.Time) == 0).Select(x => x.Value).First();
        }

        public void AddParameter(string parameter)
        {
            tableController.Add(parameter, new List<ITableEntity>());
        }

        public string GetEntityValueString(string name, DateTime date)
        {
            return tableController[name].Where(x => DateTime.Compare(date.Date, x.Time) == 0).Select(x => x.Value).First().ToString();
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

        private void SelectAllItems<T>() where T : new()
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
}
