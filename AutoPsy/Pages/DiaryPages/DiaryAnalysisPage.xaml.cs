using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Microcharts.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryAnalysisPage : ContentPage
    {
        private int pageFound = 0;
        private List<Database.Entities.DiaryPage> diaryPages;       // Коллекция, содержащая выбранные записи из дневника
        private Logic.DiaryPagesCalc diaryCalc;     // Класс для статистической обработки записей
        public DiaryAnalysisPage(DateTime start, DateTime end)
        {
            InitializeComponent();
            DateNavigatorStart.Date = start;
            DateNavigatorEnd.Date = end;

            diaryPages = App.Connector.SelectData(start, end);     // По переданным в конструкторе параметрам получаем данные из таблицы

            diaryCalc = new Logic.DiaryPagesCalc();     // Инициализируем класс стат. обработки
            diaryCalc.ProcessRecords(diaryPages);       // Передаем данные и производим базовую обработку - сортировку, инициализацию, создание вспомогательных структур 
            diaryCalc.RecursiveFilling();       // Заполняем данные для последующей обработки методом обхода графа в глубину
            diaryCalc.StatisticCalculation();       // Высчитываем статистику по каждому вхождению

            ChartsSelector.IsVisible = true;

            ShowSymptomsButton_Clicked(ShowSymptomsButton, new EventArgs());        // отображаем результаты

        }

        // Метод для отображения симптомов и привязанной к ним статистикой
        private void ShowSymptomsButton_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (MainGrid.Children.Last() is CustomComponents.ChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            ChartsSelector.IsVisible = true;
            var choosedStats = diaryCalc.GetOnlySymptoms();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new CustomComponents.ChartHandler(choosedStats, 
                Const.Constants.STAT_COUNT, Const.Constants.MIN_INTERVAL, Const.Constants.MAX_INTERVAL, 
                Const.Constants.AVERAGE_INTERVAL);

            MainGrid.Children.Add(chartHandler, 0, 2);
        }

        // Метод для отображения проявлений и привязанной к ним статистикой, остальное аналогично
        private void ShowDiseasesButton_Clicked(object sender, EventArgs e)
        {
            if (MainGrid.Children.Last() is CustomComponents.ChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            var choosedStats = diaryCalc.GetOnlyDisplays();
            var chartHandler = new CustomComponents.ChartHandler(choosedStats,
                Const.Constants.STAT_COUNT, Const.Constants.MAX_VALUE, Const.Constants.AVERAGE_VALUE,
                Const.Constants.MIN_INTERVAL, Const.Constants.MAX_INTERVAL, Const.Constants.AVERAGE_INTERVAL);

            MainGrid.Children.Add(chartHandler, 0, 2);
        }

        // Метод для отображения категорий и привязанной к ним статистикой, остальное аналогично
        private void ShowCategories_Clicked(object sender, EventArgs e)
        {
            if (MainGrid.Children.Last() is CustomComponents.ChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            var choosedStats = diaryCalc.GetOnlyCategories();
            var chartHandler = new CustomComponents.ChartHandler(choosedStats,
                Const.Constants.STAT_COUNT, Const.Constants.MAX_VALUE, Const.Constants.AVERAGE_VALUE,
                Const.Constants.MIN_INTERVAL, Const.Constants.MAX_INTERVAL, Const.Constants.AVERAGE_INTERVAL);

            MainGrid.Children.Add(chartHandler, 0, 2);
        }

        private async void ReturnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}