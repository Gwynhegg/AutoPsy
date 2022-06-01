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
    public partial class LinearAnalysisPage : ContentPage
    {
        private readonly DiaryPagesCalc diaryCalc;     // Класс для статистической обработки записей
        private readonly DateTime start, end;
        public LinearAnalysisPage(DiaryPagesCalc diaryCalc, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.diaryCalc = diaryCalc;
            this.start = start;
            this.end = end;
            ShowDiseasesButton_Clicked(this.ShowDiseasesButton, new EventArgs());
        }

        private void ShowDiseasesButton_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (this.MainGrid.Children.Last() is LinearChartHandler) this.MainGrid.Children.Remove(this.MainGrid.Children.Last());

            this.ChartsSelector.IsVisible = true;
            Dictionary<string, Logic.Structures.DiaryResultRecords> choosedStats = this.diaryCalc.GetOnlyDisplays();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new LinearChartHandler(choosedStats,
                Constants.DATA_ENTRIES, this.start, this.end);

            this.MainGrid.Children.Add(chartHandler, 0, 0);
        }

        private void ShowCategories_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (this.MainGrid.Children.Last() is LinearChartHandler) this.MainGrid.Children.Remove(this.MainGrid.Children.Last());

            this.ChartsSelector.IsVisible = true;
            Dictionary<string, Logic.Structures.DiaryResultRecords> choosedStats = this.diaryCalc.GetOnlyCategories();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new LinearChartHandler(choosedStats,
                Constants.DATA_ENTRIES, this.start, this.end);

            this.MainGrid.Children.Add(chartHandler, 0, 0);
        }
    }
}