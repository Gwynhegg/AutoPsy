using System;
using System.Collections.Generic;
using AutoPsy.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalysisTabsPage : TabbedPage
    {
        public AnalysisTabsPage(DateTime start, DateTime end)
        {
            InitializeComponent();

            var diaryPages = App.Connector.SelectData(start, end);     // По переданным в конструкторе параметрам получаем данные из таблицы
            var diaryCalc = new Logic.DiaryPagesCalc();     // Инициализируем класс стат. обработки
            diaryCalc.ProcessRecords(diaryPages);       // Передаем данные и производим базовую обработку - сортировку, инициализацию, создание вспомогательных структур 
            diaryCalc.RecursiveFilling();       // Заполняем данные для последующей обработки методом обхода графа в глубину
            diaryCalc.StatisticCalculation();       // Высчитываем статистику по каждому вхождению

            this.Children.Add(new DiaryAnalysisPage(diaryCalc));
            this.Children.Last().Title = PageTitles.Stats;
            this.Children.Add(new LinearAnalysisPage(diaryCalc));
            this.Children.Last().Title = PageTitles.Charts;
        }
    }
}