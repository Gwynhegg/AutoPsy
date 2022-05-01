﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinearAnalysisPage : ContentPage
    {
        private Logic.DiaryPagesCalc diaryCalc;     // Класс для статистической обработки записей
        public LinearAnalysisPage(Logic.DiaryPagesCalc diaryCalc)
        {
            InitializeComponent();
            this.diaryCalc = diaryCalc;
            ShowDiseasesButton_Clicked(ShowDiseasesButton, new EventArgs());
        }

        private void ShowDiseasesButton_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (MainGrid.Children.Last() is CustomComponents.LinearChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            ChartsSelector.IsVisible = true;
            var choosedStats = diaryCalc.GetOnlyDisplays();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new CustomComponents.LinearChartHandler(choosedStats,
                Const.Constants.DATA_ENTRIES);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }

        private void ShowCategories_Clicked(object sender, EventArgs e)
        {
            // Чтобы избежать утечек памяти, обновляем компонент для отображения графиков при каждой смене категории
            if (MainGrid.Children.Last() is CustomComponents.LinearChartHandler) MainGrid.Children.Remove(MainGrid.Children.Last());

            ChartsSelector.IsVisible = true;
            var choosedStats = diaryCalc.GetOnlyCategories();     // Отображаем симптомы

            // Создаем объект отображения графиков по указанным в аргументах статистических величин
            var chartHandler = new CustomComponents.LinearChartHandler(choosedStats,
                Const.Constants.DATA_ENTRIES);

            MainGrid.Children.Add(chartHandler, 0, 0);
        }
    }
}