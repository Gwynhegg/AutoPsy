using AutoPsy.CustomComponents.Charts;
using AutoPsy.Logic.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microcharts;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearChartHandler : Grid
    {
        LinearChartController chartController;      // контроллер линейных диаграмм
        private string statName;        // имя текущей выбранной статистики
        private DateTime start, end;
        private int prognosisCount = 3;
        Dictionary<string, DiaryResultRecords> statValues;      // словарь со статистическими величинами
        ObservableCollection<ChartDateElementModel> setOfElements;      // вспомогательные элементы для отображения в динамическом списке
        public LinearChartHandler(Dictionary<string, DiaryResultRecords> choosedStats, string statName, DateTime start, DateTime end)
        {
            InitializeComponent();

            this.statValues = choosedStats;
            this.statName = statName;
            this.start = start;
            this.end = end;

            PrognosisText.Text = String.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, PrognosysFar.Value);
            LoadCurrentPage();
        }

        private void LoadCurrentPage()      // Метод для загрузки текущей страницы (в контексте выбранного стат. параметра)
        {
            StatText.Text = statName;       // отображаем имя выбранного параметра

            setOfElements = new ObservableCollection<ChartDateElementModel>();      // инициализируем новую коллекцию наблюдаемых элементов
            foreach (var pair in statValues)        // Из каждой пары значений выделяем составляющие...
            {
                if (pair.Value.Count == 1) continue;
                setOfElements.Add(new ChartDateElementModel()       
                {
                    Name = App.Graph.GetNodeValue(pair.Key),
                    Values = pair.Value.dataEntries
                });;        // и комбинируем их в новый элемент, который помещаем в список наблюдений
            }

            ParameterPicker.ItemsSource = setOfElements;        // отображаем сформированную коллекцию на форме
        }

        private void CreatePrognosisGraph()
        {
            if (ParameterPicker.SelectedItem is null) return;

            var index = setOfElements.IndexOf(ParameterPicker.SelectedItem as ChartDateElementModel);       // определяем его индекс
            var element = setOfElements[index];
            var elementValues = element.Values.Values.Select(x => (float)x).ToList();
            var elementLabels = element.Values.Keys.ToList();
            var currentId = App.Graph.GetNodeId(setOfElements[index].Name);     // получаем ID элемента
            var averageInterval = statValues[currentId].AverageInterval;        // получаем средний интервал (уже расчитан)

            // создаем обработчик, ответственный за построение линейных диаграмм
            chartController = new LinearChartController();
            chartController.AddValuesToChart(elementValues, start, end);
            var regression = Logic.LinearRegression.CreateMovingAverageRegression(element.Values, averageInterval, prognosisCount);
            if (regression != null && regression.Count > 0)
            {
                var newStart = regression.First().Key;
                var newEnd = regression.Last().Key;
                chartController.ChangeColor();
                chartController.AddValuesToChart(regression.Values.ToList(), newStart, newEnd);
            }

            CurrentChart.Chart = chartController.GetChart();        // поулчаем диаграмму и отображаем на форме
        }

        private void PrognosysFar_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            prognosisCount = (int)PrognosysFar.Value;
            PrognosisText.Text = String.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, PrognosysFar.Value);
            CreatePrognosisGraph();
        }

        private void ParameterPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)       // Метод, вызываемый при выборе элемента из списка
        {
            CreatePrognosisGraph();
        }
    }
}