using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoPsy.CustomComponents
{
    public abstract class TableGridHandler
    {
        public Grid mainGrid { get; private set; }
        protected Database.Entities.TableEntityHandler entityHandler;
        protected DateTime start, end;

        public TableGridHandler(string topic, DateTime start, DateTime end)
        {
            entityHandler = new TableEntityHandler();
            this.start = start;
            this.end = end;

            mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = 20 });
            Label gridName = new Label() { Text = topic };
            mainGrid.Children.Add(gridName, 0, 0);
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 100 });
        }
        protected void AddParameter(string parameter)
        {
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = 20 });
            Label parameterName = new Label() { Text = parameter };
            mainGrid.Children.Add(parameterName, 0, mainGrid.RowDefinitions.Count - 1);
        }

        public TableEntityHandler GetEntityHandler()
        {
            return entityHandler;
        }

        public abstract void FillTableInformation();
        public abstract void FillTableInformation(DateTime start, DateTime end);

        protected void GetDateTimeResults(List<ITableEntity> entities)
        {
            foreach (var entity in entities)
            {
                AddParameter(entity.Name);

                var timeSpan = (end - start).Days;
                var values = new string[timeSpan];
                int iterator = 1;
                for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
                {
                    var entityValue = entityHandler.GetEntityValueString(entity.Name, i);
                    mainGrid.Children.Add(new Button() { Text = entityValue.ToString() }, iterator++, mainGrid.RowDefinitions.Count - 1);
                }
            }
        }
    }
}
