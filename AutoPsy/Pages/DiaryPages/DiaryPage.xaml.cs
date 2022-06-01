using AutoPsy.CustomComponents;
using AutoPsy.Resources;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPage : ContentPage, ISynchronizablePage, ISynchronizablePageWithQuery
    {
        private readonly ObservableCollection<Database.Entities.DiaryPage> diaryPages;     // Определяем коллекцию для хранения карт посещений
        public DiaryPage()
        {
            InitializeComponent();

            this.diaryPages = new ObservableCollection<Database.Entities.DiaryPage>();

            this.DateNavigatorStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.DateNavigatorStart.MaximumDate = DateTime.Now;
            this.DateNavigatorEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.DateNavigatorEnd.MaximumDate = DateTime.Now.AddHours(1);
            this.DateNavigatorStart.Date = DateTime.Now;
            this.DateNavigatorEnd.Date = DateTime.Now;

            SynchronizeContentPages();      // Синхронизируем информацию на странице согласно найденным записям
        }

        // Событие добавления новой записи в дневник
        private async void AddButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new DiaryEditPage(this));

        public void SynchronizeContentPages()       // Метод для синхронизации данных на форме с базой (обычный)
        {
            if (!App.Connector.IsTableExisted<Database.Entities.DiaryPage>()) return;      // проверяем существование таблицы с записями дневника

            // Выбираем те из них, даты которых попадают в заданный интервал
            var queryPages = App.Connector.SelectAll<Database.Entities.DiaryPage>().
                Where(
                x => DateTime.Compare(x.DateOfRecord, this.DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(x.DateOfRecord, this.DateNavigatorEnd.Date) <= 0)
                .Cast<Database.Entities.DiaryPage>().ToList();

            // Отображаем количество доступных для анализа данных, попадающих в заданный интервал
            this.AnalyzeButton.Text = string.Format(AuxiliaryResources.AnalysisPlaceholder, queryPages.Count);
            if (queryPages.Count > 0) this.AnalyzeButton.IsEnabled = true; else this.AnalyzeButton.IsEnabled = false;

            if (queryPages.Count == 0) return;      // Если таковых нет, возвращаемся

            this.diaryPages.Clear();
            foreach (Database.Entities.DiaryPage dairyPage in queryPages)      // Иначе помещаем каждую из них в коллекцию
                this.diaryPages.Add(dairyPage);

            this.PagesCarouselView.ItemsSource = this.diaryPages;       // Отображаем колллекцию на форме


        }

        // Отличие данного метода от предыдущего - чтобы не тянуть данные каждый раз из базы данных (что при большом объеме информации
        // ощутимо, достаточно  просто добавить созданную на другом экране страницу в локальный список страниц
        public void SynchronizeContentPages(IСustomComponent diaryPanel)
        {
            Database.Entities.DiaryPage addedPage = (diaryPanel as DiaryPagePanel).diaryHandler.GetDiaryPage();
            if (DateTime.Compare(addedPage.DateOfRecord, this.DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(addedPage.DateOfRecord, this.DateNavigatorEnd.Date) <= 0)
            {
                var index = this.diaryPages.IndexOf(this.diaryPages.FirstOrDefault(x => x.Id == addedPage.Id));
                if (index != -1)
                    this.diaryPages[index] = addedPage;
                else
                    this.diaryPages.Add(addedPage);
                this.PagesCarouselView.ItemsSource = this.diaryPages;

                this.AnalyzeButton.Text = string.Format(AuxiliaryResources.AnalysisPlaceholder, this.diaryPages.Count);
                if (this.diaryPages.Count > 0) this.AnalyzeButton.IsEnabled = true; else this.AnalyzeButton.IsEnabled = false;
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
                AuxServices.PdfWriter.CreateDocument(App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser), this.diaryPages.ToList());
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.Success, string.Format(AutoPsy.Resources.DiaryPageDefault.FileSavePlaceholder, directory), AutoPsy.Resources.AuxiliaryResources.ButtonOK);
            }
            catch
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.SaveErrorAlertMessage, AuxiliaryResources.ButtonOK);
            }
        }

        private async void EditPages_Clicked(object sender, EventArgs e)
        {
            if (this.diaryPages.Count == 0)
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToEditAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }
            var temp = this.PagesCarouselView.CurrentItem as Database.Entities.DiaryPage;

            if (temp != null) await this.Navigation.PushModalAsync(new DiaryEditPage(this, temp));
        }

        private async void DeletePages_Clicked(object sender, EventArgs e)
        {
            if (this.diaryPages.Count == 0)
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToDeleteAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }

            var temp = this.PagesCarouselView.CurrentItem as Database.Entities.DiaryPage;

            this.diaryPages.Remove(temp);
            this.PagesCarouselView.ItemsSource = this.diaryPages;

            if (temp != null) App.Connector.DeleteData(temp);
        }

        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e) => SynchronizeContentPages();

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (this.DateNavigatorEnd.Date < this.DateNavigatorStart.Date) this.DateNavigatorEnd.Date = this.DateNavigatorStart.Date;
            SynchronizeContentPages();
        }

        private async void InfoButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new SymptomViewer());

        private async void AnalyzeButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new AnalysisTabsPage(this.DateNavigatorStart.Date, this.DateNavigatorEnd.Date));
    }
}