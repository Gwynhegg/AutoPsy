using System;
using System.Collections.Generic;
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
        private List<Database.Entities.DiaryPage> diaryPages;
        public DiaryAnalysisPage()
        {
            InitializeComponent();
            diaryPages = new List<Database.Entities.DiaryPage>();
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            var diaryCalc = new Logic.DiaryPagesCalc();
            diaryCalc.ProcessRecords(diaryPages);
            diaryCalc.RecursiveFilling();
            diaryCalc.StatisticCalculation();

            // ВОТ ТУТ НАЧНЕТСЯ МАГИЯ С ГРАФИКАМИ
            var results = diaryCalc.GetOnlySymptoms();
        }

        private void SynchronizeDate()
        {
            diaryPages = App.Connector.SelectDiaryData(DateNavigatorStart.Date, DateNavigatorEnd.Date);
            pageFound = diaryPages.Count;
            if (pageFound != 0)
            {
                var countTitle = String.Format("Найдено: {0} записей. Продолжаем?", pageFound);
                ContinueButton.Text = countTitle;
                ContinueButton.IsEnabled = true;
            }
            else ContinueButton.IsEnabled = false;
        }

        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            SynchronizeDate();
        }

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigatorEnd.Date < DateNavigatorStart.Date) DateNavigatorEnd.Date = DateNavigatorStart.Date;
            SynchronizeDate();
        }
    }
}