using System;
using AutoPsy.Logic;
using AutoPsy.CustomComponents;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPsy.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearAnalysisPage : ContentPage
    {
        private DiaryPagesCalc diaryCalc;     // Класс для статистической обработки записей
        private DateTime start, end;
        public LinearAnalysisPage(DiaryPagesCalc diaryCalc, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.diaryCalc = diaryCalc;
            this.start = start;
            this.end = end;
            ShowDiseasesButton_Clicked(ShowDiseasesButton, new EventArgs());
        }

        private void ShowDiseasesButton_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (MainGrid.Children.Last() is LinearChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            ChartsSelector.IsVisible = true;
            var choosedStats = diaryCalc.GetOnlyDisplays();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new LinearChartHandler(choosedStats,
                Constants.DATA_ENTRIES, start, end);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }

        private void ShowCategories_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (MainGrid.Children.Last() is LinearChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            ChartsSelector.IsVisible = true;
            var choosedStats = diaryCalc.GetOnlyCategories();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new LinearChartHandler(choosedStats,
                Constants.DATA_ENTRIES, start, end);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }
    }
}