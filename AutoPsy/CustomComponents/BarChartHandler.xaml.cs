using AutoPsy.CustomComponents.Charts;
using AutoPsy.Logic.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BarChartHandler : Grid
    {
        private BarChartController chartController;
        private readonly string[] statNames;
        private readonly Dictionary<string, DiaryResultRecords> statValues;
        private ObservableCollection<ChartElementModel> setOfElements;
        private byte currentPage = 0;
        public BarChartHandler(Dictionary<string, DiaryResultRecords> choosedStats, params string[] values)
        {
            InitializeComponent();

            this.statValues = choosedStats;
            this.statNames = values;

            LoadCurrentPage();
        }

        private void LoadCurrentPage()
        {
            if (this.currentPage == 0) this.BackwardButton.IsEnabled = false; else this.BackwardButton.IsEnabled = true;
            if (this.currentPage == this.statNames.Length - 1) this.ForwardButton.IsEnabled = false; else this.ForwardButton.IsEnabled = true;

            this.StatText.Text = this.statNames[this.currentPage];
            Dictionary<string, DiaryResultRecords> choosedStatValues = AuxServices.DiaryResultsSorter.GetSortedRecords(this.statValues, this.statNames[this.currentPage]);

            this.setOfElements = new ObservableCollection<ChartElementModel>();
            foreach (KeyValuePair<string, DiaryResultRecords> pair in choosedStatValues)
            {
                if (pair.Value.Count == 1 && AuxServices.DeleteRules.CheckZeroRule(this.statNames[this.currentPage])) continue;
                this.setOfElements.Add(new ChartElementModel()
                {
                    Name = App.Graph.GetNodeValue(pair.Key),
                    Value = string.Format("{0:F2}", pair.Value.GetStatValue(this.statNames[this.currentPage]))
                });
            }

            this.ParameterPicker.ItemsSource = this.setOfElements;

            this.chartController = new BarChartController(this.setOfElements.Select(x => float.Parse(x.Value)).ToList(), this.setOfElements.Select(x => x.Name).ToList());
        }


        private void ParameterPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var indexes = new List<int>();
            foreach (var item in this.ParameterPicker.SelectedItems)
                indexes.Add(this.setOfElements.IndexOf(item as ChartElementModel));
            if (indexes.Count == 0) return;
            this.chartController.AddValuesToChart(indexes);
            this.CurrentChart.Chart = this.chartController.GetChart();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.currentPage--;
            this.ParameterPicker.SelectedItems.Clear();
            this.CurrentChart.Chart = null;
            LoadCurrentPage();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            this.currentPage++;
            this.ParameterPicker.SelectedItems.Clear();
            this.CurrentChart.Chart = null;
            LoadCurrentPage();
        }
    }
}