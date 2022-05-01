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
        private Logic.DiaryPagesCalc diaryCalc;     // Класс для статистической обработки записей
        public DiaryAnalysisPage(Logic.DiaryPagesCalc diaryCalc)
        {
            InitializeComponent();
            this.diaryCalc = diaryCalc;
            ShowSymptomsButton_Clicked(ShowSymptomsButton, new EventArgs());        // отображаем результаты

        }

        // Метод для отображения симптомов и привязанной к ним статистикой
        private void ShowSymptomsButton_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (MainGrid.Children.Last() is CustomComponents.BarChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            ChartsSelector.IsVisible = true;
            var choosedStats = diaryCalc.GetOnlySymptoms();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new CustomComponents.BarChartHandler(choosedStats, 
                Const.Constants.STAT_COUNT, Const.Constants.MIN_INTERVAL, Const.Constants.MAX_INTERVAL, 
                Const.Constants.AVERAGE_INTERVAL);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }

        // Метод для отображения проявлений и привязанной к ним статистикой, остальное аналогично
        private void ShowDiseasesButton_Clicked(object sender, EventArgs e)
        {
            if (MainGrid.Children.Last() is CustomComponents.BarChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            var choosedStats = diaryCalc.GetOnlyDisplays();
            var chartHandler = new CustomComponents.BarChartHandler(choosedStats,
                Const.Constants.STAT_COUNT, Const.Constants.MAX_VALUE, Const.Constants.AVERAGE_VALUE,
                Const.Constants.MIN_INTERVAL, Const.Constants.MAX_INTERVAL, Const.Constants.AVERAGE_INTERVAL);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }

        // Метод для отображения категорий и привязанной к ним статистикой, остальное аналогично
        private void ShowCategories_Clicked(object sender, EventArgs e)
        {
            if (MainGrid.Children.Last() is CustomComponents.BarChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            var choosedStats = diaryCalc.GetOnlyCategories();
            var chartHandler = new CustomComponents.BarChartHandler(choosedStats,
                Const.Constants.STAT_COUNT, Const.Constants.MAX_VALUE, Const.Constants.AVERAGE_VALUE,
                Const.Constants.MIN_INTERVAL, Const.Constants.MAX_INTERVAL, Const.Constants.AVERAGE_INTERVAL);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }
    }
}