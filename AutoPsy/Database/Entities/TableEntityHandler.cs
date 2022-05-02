using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class TableEntityHandler
    {
        private Dictionary<string, Dictionary<DateTime, byte>> tableController;
        
        public TableEntityHandler()
        {
            tableController = new Dictionary<string, Dictionary<DateTime, byte>>();
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
            return tableController[name][date];
        }

        public void AddParameter(string parameter)
        {
            tableController.Add(parameter, new Dictionary<DateTime, byte>());
        }

        public string GetEntityValueString(string name, DateTime date)
        {
            return tableController[name][date].ToString();
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
                    tableController.Add(item.Name, new Dictionary<DateTime, byte>());
                    tableController[item.Name].Add(item.Time, item.Value);
                }
                else
                    tableController[item.Name].Add(item.Time, item.Value);
        }
    }
}
