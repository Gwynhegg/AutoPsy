using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoolAnalysisPage : ContentPage
    {
        private AnalysysSelectionPage parentPage;
        private Dictionary<string, List<float>> entityValues = new Dictionary<string, List<float>>(); 
        private Dictionary<string, List<float>> calculatedValues = new Dictionary<string, List<float>>();
        private Dictionary<string, List<ITableEntity>> entities;
        private List<string> listOfDate = new List<string>();
        DateTime start, end;
        public PoolAnalysisPage(AnalysysSelectionPage parentPage, Dictionary<string, List<ITableEntity>> entities, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.parentPage = parentPage;
            this.entities = entities;
            this.start = start;
            this.end = end;

            SetDatesRange();
            CreateFloatValues();
            DisplayData();
            CalculatePools();
        }

        private void CalculatePools()
        {
            foreach (var parameters in entities.Keys)
                if (!entities[parameters].First().Type.Equals(Const.Constants.ENTITY_TRIGGER))
                    calculatedValues.Add(parameters, Logic.PoolModelling.CreatePoolModel(entityValues[parameters]));
                else
                    calculatedValues.Add(parameters, entityValues[parameters]);
        }

        private void DisplayData()
        {
            var tempList = new List<string>();
            foreach (var pair in entities)
                tempList.Add(App.TableGraph.GetNameByIdString(pair.Key));
            ItemsCollection.ItemsSource = tempList;
        }

        private void SetDatesRange()
        {
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                var day = i.Day.ToString().Length < 2 ? String.Concat("0", i.Day) : i.Day.ToString();       // получаем строку для отображения дня
                var month = i.Month.ToString().Length < 2 ? String.Concat("0", i.Month) : i.Month.ToString();       // получаем строку для отображения месяца
                listOfDate.Add(String.Concat(day, ".", month));      // соединяем строки и помещаем в новый столбец
            }
        }

        private void CreateFloatValues()
        {          
                foreach (var pair in entities)
                {
                    entityValues[pair.Key] = new List<float>();

                    for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
                    {
                        var entity = pair.Value.FirstOrDefault(x => DateTime.Compare(x.Time, i) == 0);
                        if (entity == null) entityValues[pair.Key].Add(0); else entityValues[pair.Key].Add(entity.Value);
                    }
                }           
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            foreach (var pair in entities)
            {
                var entityPattern = pair.Value.First();
                var iterator = 0;
            for (DateTime date = start; date <= end; date = date.AddDays(1))
                {
                    var entity = pair.Value.FirstOrDefault(x => DateTime.Compare(date, x.Time) == 0);
                    if (entity == null)
                    {
                        var clone = entityPattern.Clone(date);
                        clone.Value = (byte)calculatedValues[pair.Key][iterator++];
                        TableEntityHandler.UpdateEntityValue(clone);
                    }
                    else
                    {
                        entity.Value = (byte)calculatedValues[pair.Key][iterator++];
                        TableEntityHandler.UpdateEntityValue(entity);
                    }
                }
            }
            parentPage.SynchronizeEntities();
            await Navigation.PopModalAsync();
        }

        private void ItemsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedParameter = App.TableGraph.GetIdStringByName(ItemsCollection.SelectedItem as string);
            
            CustomComponents.Charts.BarChartController chart = new CustomComponents.Charts.BarChartController(entityValues[selectedParameter], listOfDate);
            if (App.TableGraph.GetParameterType(selectedParameter).Equals(Const.Constants.ENTITY_TRIGGER)) chart.AddAllValuesToChart(false); else chart.AddAllValuesToChart(true);
            PastChart.Chart = chart.GetChart();
            
            CustomComponents.Charts.BarChartController postChart = new CustomComponents.Charts.BarChartController(calculatedValues[selectedParameter], listOfDate);
            if (App.TableGraph.GetParameterType(selectedParameter).Equals(Const.Constants.ENTITY_TRIGGER)) postChart.AddAllValuesToChart(false); else postChart.AddAllValuesToChart(true);
            PostChart.Chart = postChart.GetChart();
        }
    }
}