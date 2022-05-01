using AutoPsy.CustomComponents.Charts;
using AutoPsy.Logic.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearChartHandler : Grid
    {
        LinearChartController chartController;
        string statName;
        Dictionary<string, DiaryResultRecords> statValues;
        ObservableCollection<ChartDateElementModel> setOfElements;
        public LinearChartHandler(Dictionary<string, DiaryResultRecords> choosedStats, string statName)
        {
            InitializeComponent();

            this.statValues = choosedStats;
            this.statName = statName;

            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            StatText.Text = statName;

            setOfElements = new ObservableCollection<ChartDateElementModel>();
            foreach (var pair in statValues)
            {
                if (pair.Value.Count == 1) continue;
                setOfElements.Add(new ChartDateElementModel()
                {
                    Name = App.Graph.GetNodeValue(pair.Key),
                    Values = pair.Value.dataEntries
                });;
            }

            ParameterPicker.ItemsSource = setOfElements;

            chartController = new LinearChartController(setOfElements.Select(x => x.Values).ToList(), setOfElements.Select(x => x.Name).ToList());
        }

        private void ParameterPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = setOfElements.IndexOf(ParameterPicker.SelectedItem as ChartDateElementModel);
            var currentId = App.Graph.GetNodeId(setOfElements[index].Name);
            var averageInterval = statValues[currentId].AverageInterval;
            chartController.AddValuesToChart(index, averageInterval);
            CurrentChart.Chart = chartController.GetChart();
        }
    }
}