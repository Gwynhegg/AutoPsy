using AutoPsy.CustomComponents;
using AutoPsy.Logic;
using AutoPsy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryAnalysisPage : ContentPage
    {
        private readonly DiaryPagesCalc diaryCalc;     // Класс для статистической обработки записей
        public DiaryAnalysisPage(DiaryPagesCalc diaryCalc)
        {
            InitializeComponent();
            this.diaryCalc = diaryCalc;
            ShowSymptomsButton_Clicked(this.ShowSymptomsButton, new EventArgs());        // отображаем результаты

        }

        // Метод для отображения симптомов и привязанной к ним статистикой
        private void ShowSymptomsButton_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (this.MainGrid.Children.Last() is BarChartHandler) this.MainGrid.Children.Remove(this.MainGrid.Children.Last());

            this.ChartsSelector.IsVisible = true;
            Dictionary<string, Logic.Structures.DiaryResultRecords> choosedStats = this.diaryCalc.GetOnlySymptoms();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new BarChartHandler(choosedStats,
                Constants.STAT_COUNT, Constants.MIN_INTERVAL, Constants.MAX_INTERVAL,
                Constants.AVERAGE_INTERVAL);

            this.MainGrid.Children.Add(chartHandler, 0, 0);
        }

        // Метод для отображения проявлений и привязанной к ним статистикой, остальное аналогично
        private void ShowDiseasesButton_Clicked(object sender, EventArgs e)
        {
            if (this.MainGrid.Children.Last() is BarChartHandler) this.MainGrid.Children.Remove(this.MainGrid.Children.Last());

            Dictionary<string, Logic.Structures.DiaryResultRecords> choosedStats = this.diaryCalc.GetOnlyDisplays();
            var chartHandler = new BarChartHandler(choosedStats,
                Constants.STAT_COUNT, Constants.MAX_VALUE, Constants.AVERAGE_VALUE,
                Constants.MIN_INTERVAL, Constants.MAX_INTERVAL, Constants.AVERAGE_INTERVAL);

            this.MainGrid.Children.Add(chartHandler, 0, 0);
        }

        // Метод для отображения категорий и привязанной к ним статистикой, остальное аналогично
        private void ShowCategories_Clicked(object sender, EventArgs e)
        {
            if (this.MainGrid.Children.Last() is BarChartHandler) this.MainGrid.Children.Remove(this.MainGrid.Children.Last());

            Dictionary<string, Logic.Structures.DiaryResultRecords> choosedStats = this.diaryCalc.GetOnlyCategories();
            var chartHandler = new BarChartHandler(choosedStats,
                Constants.STAT_COUNT, Constants.MAX_VALUE, Constants.AVERAGE_VALUE,
                Constants.MIN_INTERVAL, Constants.MAX_INTERVAL, Constants.AVERAGE_INTERVAL);

            this.MainGrid.Children.Add(chartHandler, 0, 0);
        }
    }
}