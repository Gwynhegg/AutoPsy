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
        LinearChartController chartController;      // контроллер линейных диаграмм
        string statName;        // имя текущей выбранной статистики
        Dictionary<string, DiaryResultRecords> statValues;      // словарь со статистическими величинами
        ObservableCollection<ChartDateElementModel> setOfElements;      // вспомогательные элементы для отображения в динамическом списке
        public LinearChartHandler(Dictionary<string, DiaryResultRecords> choosedStats, string statName)
        {
            InitializeComponent();

            this.statValues = choosedStats;
            this.statName = statName;

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

            // создаем обработчик, ответственный за построение линейных диаграмм
            chartController = new LinearChartController(setOfElements.Select(x => x.Values).ToList(), setOfElements.Select(x => x.Name).ToList());
        }

        private void ParameterPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)       // Метод, вызываемый при выборе элемента из списка
        {
            var index = setOfElements.IndexOf(ParameterPicker.SelectedItem as ChartDateElementModel);       // определяем его индекс
            var currentId = App.Graph.GetNodeId(setOfElements[index].Name);     // получаем ID элемента
            var averageInterval = statValues[currentId].AverageInterval;        // получаем средний интервал (уже расчитан)
            chartController.AddValuesToChart(index, averageInterval);       // отправляем значения в обработчик диаграмм, который строит нам линейную диаграмму
            CurrentChart.Chart = chartController.GetChart();        // поулчаем диаграмму и отображаем на форме
        }
    }
}