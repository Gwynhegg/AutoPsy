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
    public partial class LinearChartHandler : Grid
    {
        private LinearChartController chartController;      // контроллер линейных диаграмм
        private readonly string statName;        // имя текущей выбранной статистики
        private readonly DateTime start, end;
        private int prognosisCount = 3;
        private readonly Dictionary<string, DiaryResultRecords> statValues;      // словарь со статистическими величинами
        private ObservableCollection<ChartDateElementModel> setOfElements;      // вспомогательные элементы для отображения в динамическом списке
        public LinearChartHandler(Dictionary<string, DiaryResultRecords> choosedStats, string statName, DateTime start, DateTime end)
        {
            InitializeComponent();

            this.statValues = choosedStats;
            this.statName = statName;
            this.start = start;
            this.end = end;

            this.PrognosisText.Text = string.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, this.PrognosysFar.Value);
            LoadCurrentPage();
        }

        private void LoadCurrentPage()      // Метод для загрузки текущей страницы (в контексте выбранного стат. параметра)
        {
            this.StatText.Text = this.statName;       // отображаем имя выбранного параметра

            this.setOfElements = new ObservableCollection<ChartDateElementModel>();      // инициализируем новую коллекцию наблюдаемых элементов
            foreach (KeyValuePair<string, DiaryResultRecords> pair in this.statValues)        // Из каждой пары значений выделяем составляющие...
            {
                if (pair.Value.Count == 1) continue;
                this.setOfElements.Add(new ChartDateElementModel()
                {
                    Name = App.Graph.GetNodeValue(pair.Key),
                    Values = pair.Value.dataEntries
                }); ;        // и комбинируем их в новый элемент, который помещаем в список наблюдений
            }

            this.ParameterPicker.ItemsSource = this.setOfElements;        // отображаем сформированную коллекцию на форме
        }

        private void CreatePrognosisGraph()
        {
            if (this.ParameterPicker.SelectedItem is null) return;

            var index = this.setOfElements.IndexOf(this.ParameterPicker.SelectedItem as ChartDateElementModel);       // определяем его индекс
            ChartDateElementModel element = this.setOfElements[index];
            var elementValues = element.Values.Values.Select(x => (float)x).ToList();
            var elementLabels = element.Values.Keys.ToList();
            var currentId = App.Graph.GetNodeId(this.setOfElements[index].Name);     // получаем ID элемента
            var averageInterval = this.statValues[currentId].AverageInterval;        // получаем средний интервал (уже расчитан)

            // создаем обработчик, ответственный за построение линейных диаграмм
            this.chartController = new LinearChartController();
            this.chartController.AddValuesToChart(elementValues, this.start, this.end);
            Dictionary<DateTime, float> regression = Logic.LinearRegression.CreateMovingAverageRegression(element.Values, averageInterval, this.prognosisCount);
            if (regression != null && regression.Count > 0)
            {
                DateTime newStart = regression.First().Key;
                DateTime newEnd = regression.Last().Key;
                this.chartController.ChangeColor();
                this.chartController.AddValuesToChart(regression.Values.ToList(), newStart, newEnd);
            }

            this.CurrentChart.Chart = this.chartController.GetChart();        // поулчаем диаграмму и отображаем на форме
        }

        private void PrognosysFar_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.prognosisCount = (int)this.PrognosysFar.Value;
            this.PrognosisText.Text = string.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, this.PrognosysFar.Value);
            CreatePrognosisGraph();
        }

        private void ParameterPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)       // Метод, вызываемый при выборе элемента из списка
=> CreatePrognosisGraph();
    }
}