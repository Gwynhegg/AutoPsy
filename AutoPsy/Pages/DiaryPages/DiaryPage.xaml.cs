using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AutoPsy.Database;
using AutoPsy.Resources;
using AutoPsy.CustomComponents;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPage : ContentPage, ISynchronizablePage, ISynchronizablePageWithQuery
    {
        private ObservableCollection<Database.Entities.DiaryPage> diaryPages;     // Определяем коллекцию для хранения карт посещений
        public DiaryPage()
        {
            InitializeComponent();

            diaryPages = new ObservableCollection<Database.Entities.DiaryPage>();

            DateNavigatorStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigatorStart.MaximumDate = DateTime.Now;
            DateNavigatorEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigatorEnd.MaximumDate = DateTime.Now.AddHours(1);
            DateNavigatorStart.Date = DateTime.Now;
            DateNavigatorEnd.Date = DateTime.Now;

            SynchronizeContentPages();      // Синхронизируем информацию на странице согласно найденным записям
        }

        // Событие добавления новой записи в дневник
        private async void AddButton_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new DiaryEditPage(this));

        public void SynchronizeContentPages()       // Метод для синхронизации данных на форме с базой (обычный)
        {
            if (!App.Connector.IsTableExisted<Database.Entities.DiaryPage>()) return;      // проверяем существование таблицы с записями дневника

            // Выбираем те из них, даты которых попадают в заданный интервал
            var queryPages = App.Connector.SelectAll<Database.Entities.DiaryPage>().
                Where(
                x => DateTime.Compare(x.DateOfRecord, DateNavigatorStart.Date) >=0 &&
                DateTime.Compare(x.DateOfRecord, DateNavigatorEnd.Date) <=0)
                .Cast<Database.Entities.DiaryPage>().ToList();

            // Отображаем количество доступных для анализа данных, попадающих в заданный интервал
            AnalyzeButton.Text = String.Format(AuxiliaryResources.AnalysisPlaceholder, queryPages.Count);
            if (queryPages.Count > 0) AnalyzeButton.IsEnabled = true; else AnalyzeButton.IsEnabled = false;

            if (queryPages.Count == 0) return;      // Если таковых нет, возвращаемся

            diaryPages.Clear();
            foreach (var dairyPage in queryPages)      // Иначе помещаем каждую из них в коллекцию
                diaryPages.Add(dairyPage);

            PagesCarouselView.ItemsSource = diaryPages;       // Отображаем колллекцию на форме
                                                              

        }

        // Отличие данного метода от предыдущего - чтобы не тянуть данные каждый раз из базы данных (что при большом объеме информации
        // ощутимо, достаточно  просто добавить созданную на другом экране страницу в локальный список страниц
        public void SynchronizeContentPages(IСustomComponent diaryPanel)
        {
            var addedPage = (diaryPanel as DiaryPagePanel).diaryHandler.GetDiaryPage();
            if (DateTime.Compare(addedPage.DateOfRecord, DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(addedPage.DateOfRecord, DateNavigatorEnd.Date) <= 0)
            {
                var index = diaryPages.IndexOf(this.diaryPages.FirstOrDefault(x => x.Id == addedPage.Id));
                if (index != -1)
                    diaryPages[index] = addedPage;
                else
                    diaryPages.Add(addedPage);
                PagesCarouselView.ItemsSource = diaryPages;

                AnalyzeButton.Text = String.Format(AuxiliaryResources.AnalysisPlaceholder, diaryPages.Count);
                if (diaryPages.Count > 0) AnalyzeButton.IsEnabled = true; else AnalyzeButton.IsEnabled = false;
            }
        }

        // Метод для создания файла с данными выбранных записей
        [Obsolete]
        private async void PrintPages_Clicked(object sender, EventArgs e)
        {
            try
            {
                // получаем директорию документов на устройстве Android
                var directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

                // С помощью статического класса - библиотеки создаем документ и сохраняем по указанному пути
                AuxServices.PdfWriter.CreateDocument(App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser), diaryPages.ToList());
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.Success, String.Format(AutoPsy.Resources.DiaryPageDefault.FileSavePlaceholder, directory), AutoPsy.Resources.AuxiliaryResources.ButtonOK);
            }
            catch
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.SaveErrorAlertMessage, AuxiliaryResources.ButtonOK);
            }
        }

        private async void EditPages_Clicked(object sender, EventArgs e)
        {
            if (diaryPages.Count == 0)
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToEditAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }
            var temp = PagesCarouselView.CurrentItem as Database.Entities.DiaryPage;

            if (temp != null) await Navigation.PushModalAsync(new DiaryEditPage(this, temp));
        }

        private async void DeletePages_Clicked(object sender, EventArgs e)
        {
            if (diaryPages.Count == 0)
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToDeleteAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }

            var temp = PagesCarouselView.CurrentItem as Database.Entities.DiaryPage;

            diaryPages.Remove(temp);
            PagesCarouselView.ItemsSource = diaryPages;

            if (temp != null) App.Connector.DeleteData(temp);
        }

        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e) => SynchronizeContentPages();

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigatorEnd.Date < DateNavigatorStart.Date) DateNavigatorEnd.Date = DateNavigatorStart.Date;
            SynchronizeContentPages();
        }

        private async void InfoButton_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new SymptomViewer());

        private async void AnalyzeButton_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new AnalysisTabsPage(DateNavigatorStart.Date, DateNavigatorEnd.Date));
    }
}