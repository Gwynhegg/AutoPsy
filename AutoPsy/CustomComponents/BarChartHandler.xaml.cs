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
    public partial class BarChartHandler : Grid
    {
        BarChartController chartController;
        string[] statNames;
        Dictionary<string, DiaryResultRecords> statValues;
        ObservableCollection<ChartElementModel> setOfElements;
        byte currentPage = 0;
        public BarChartHandler(Dictionary<string, DiaryResultRecords> choosedStats, params string[] values)
        {
            InitializeComponent();

            this.statValues = choosedStats;
            statNames = values;

            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            if (currentPage == 0) BackwardButton.IsEnabled = false; else BackwardButton.IsEnabled = true;
            if (currentPage == statNames.Length - 1) ForwardButton.IsEnabled = false; else ForwardButton.IsEnabled = true;

            StatText.Text = statNames[currentPage];
            var choosedStatValues = AuxServices.DiaryResultsSorter.GetSortedRecords(statValues, statNames[currentPage]);

            setOfElements = new ObservableCollection<ChartElementModel>();
            foreach (var pair in choosedStatValues)
            {
                if (pair.Value.Count == 1 && AuxServices.DeleteRules.CheckZeroRule(statNames[currentPage])) continue;
                setOfElements.Add(new ChartElementModel()
                {
                    Name = App.Graph.GetNodeValue(pair.Key),
                    Value = String.Format("{0:F2}",pair.Value.GetStatValue(statNames[currentPage]))
                });
            }

            ParameterPicker.ItemsSource = setOfElements;

            chartController = new BarChartController(setOfElements.Select(x => float.Parse(x.Value)).ToList(), setOfElements.Select(x => x.Name).ToList());
        }


        private void ParameterPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<int> indexes = new List<int>();
            foreach (var item in ParameterPicker.SelectedItems)
                indexes.Add(setOfElements.IndexOf(item as ChartElementModel));
            if (indexes.Count ==0) return;
            chartController.AddValuesToChart(indexes);
            CurrentChart.Chart = chartController.GetChart();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            currentPage--;
            ParameterPicker.SelectedItems.Clear();
            CurrentChart.Chart = null;
            LoadCurrentPage();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            currentPage++;
            ParameterPicker.SelectedItems.Clear();
            CurrentChart.Chart = null;
            LoadCurrentPage();
        }
    }
}