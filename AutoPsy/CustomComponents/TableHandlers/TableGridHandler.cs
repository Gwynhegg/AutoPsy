using AutoPsy.Database.Entities;
using AutoPsy.CustomComponents;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using AutoPsy.Const;

namespace AutoPsy.CustomComponents
{
    public abstract class TableGridHandler
    {
        public Grid mainGrid { get; private set; }
        protected TableEntityHandler entityHandler;
        protected DateTime start, end;

        public TableGridHandler(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;

            mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = 50 });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 200 });

            for (var i = start.Date; i <= end.Date; i = i.AddDays(1))
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 50 });
        }
        protected void AddParameter(ITableEntity entity)
        {
            mainGrid.RowDefinitions.Add(new RowDefinition() { Height = 50 });
            var parameterName = new TableHandlers.EntityLabel(entity) { Label = entity.Name };
            mainGrid.Children.Add(parameterName, 0, mainGrid.RowDefinitions.Count - 1);
        }

        public virtual void AddParameter(string parameter, byte importance)
        {
            entityHandler.AddParameter(parameter);
        }

        public abstract TableEntityHandler GetEntityHandler();

        public abstract void FillTableInformation();

        protected void GetDateTimeResults(List<ITableEntity> entities)
        {
            ClearGrid();

            foreach (var entity in entities)
            {
                AddParameter(entity);
                int iterator = 1;
                for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
                {
                    var entityValue = entityHandler.GetEntityValueString(entity.Name, i);
                    if (entityValue.Equals(Constants.IS_ZERO)) 
                        mainGrid.Children.Add(new TableHandlers.EntityButton(entity) { BackgroundColor = Color.Gray }, iterator++, mainGrid.RowDefinitions.Count - 1);
                    else
                        mainGrid.Children.Add(new TableHandlers.EntityButton(entity) { Value = entityValue.ToString(), BackgroundColor = Color.Green }, iterator++, mainGrid.RowDefinitions.Count - 1);
                }
            }
        }

        protected void ClearGrid()
        {
            int indexator = 1;
            for (var i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                var day = i.Day.ToString().Length < 2 ? String.Concat("0", i.Day) : i.Day.ToString();
                var month = i.Month.ToString().Length < 2 ? String.Concat("0", i.Month) : i.Month.ToString();
                mainGrid.Children.Add(new Label() { Text = String.Concat(day,".",month)}, indexator++, 0);
            }
        }
    }
}
