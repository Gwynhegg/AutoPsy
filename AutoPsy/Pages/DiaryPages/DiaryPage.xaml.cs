using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

            SynchronizeContentPages();
        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DiaryEditPage(this));
        }

        public void SynchronizeContentPages()
        {
            if (!App.Connector.IsTableExisted<Database.Entities.DiaryPage>()) return;      // проверяем существование таблицы с карточками посещений

            // Выбираем те из них, даты которых попадают в заданный интервал
            var queryPages = App.Connector.SelectAll<Database.Entities.DiaryPage>().
                Where(
                x => x.DateOfRecord.Year >= DateNavigatorStart.Date.Year &&
                x.DateOfRecord.Month >= DateNavigatorStart.Date.Month &&
                x.DateOfRecord.Day >= DateNavigatorStart.Date.Day &&
                x.DateOfRecord.Year <= DateNavigatorEnd.Date.Year &&
                x.DateOfRecord.Month <= DateNavigatorEnd.Date.Month &&
                x.DateOfRecord.Day <= DateNavigatorEnd.Date.Day
                ).Cast<Database.Entities.DiaryPage>().ToList();

            if (queryPages.Count == 0) return;      // Если таковых нет, возвращаемся

            diaryPages.Clear();
            foreach (var dairyPage in queryPages)      // Иначе помещаем каждую из них в коллекцию
                diaryPages.Add(dairyPage);

            PagesCarouselView.ItemsSource = diaryPages;       // Отображаем колллекцию на форме          
        }

        public void SynchronizeContentPages(CustomComponents.IСustomComponent diaryPanel)
        {
            var addedPage = (diaryPanel as CustomComponents.DiaryPagePanel).diaryHandler.GetDiaryPage();
            if (addedPage.DateOfRecord.Year >= DateNavigatorStart.Date.Year &&
                addedPage.DateOfRecord.Month >= DateNavigatorStart.Date.Month &&
                addedPage.DateOfRecord.Day >= DateNavigatorStart.Date.Day &&
                addedPage.DateOfRecord.Year <= DateNavigatorEnd.Date.Year &&
                addedPage.DateOfRecord.Month <= DateNavigatorEnd.Date.Month &&
                addedPage.DateOfRecord.Day <= DateNavigatorEnd.Date.Day)
            {
                var index = diaryPages.IndexOf(this.diaryPages.FirstOrDefault(x => x.Id == addedPage.Id));
                if (index != -1)
                    diaryPages[index] = addedPage;
                else
                    diaryPages.Add(addedPage);
                PagesCarouselView.ItemsSource = diaryPages;
            }
        }

        private async void PrintPages_Clicked(object sender, EventArgs e)
        {
            try
            {
                var directory = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments);

                AuxServices.PdfWriter.CreateDocument(App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser), diaryPages.ToList());
                await DisplayAlert("Успех!", String.Format("Файл успешно сохранен в {0}", directory), "OK");
            }
            catch
            {
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, "Не удалось создать PDF-файл", AutoPsy.Resources.AuxiliaryResources.ButtonOK);
            }
        }

        private async void EditPages_Clicked(object sender, EventArgs e)
        {
            if (diaryPages.Count == 0)
            {
                await DisplayAlert("Упс!", "Пока у вас нет записей для редактирования", "OK");
                return;
            }
            var temp = PagesCarouselView.CurrentItem as AutoPsy.Database.Entities.DiaryPage;

            if (temp != null) await Navigation.PushModalAsync(new DiaryEditPage(this, temp));
        }

        private async void DeletePages_Clicked(object sender, EventArgs e)
        {
            if (diaryPages.Count == 0)
            {
                await DisplayAlert("Упс!", "Пока у вас нет записей для удаления", "OK");
                return;
            }

            var temp = PagesCarouselView.CurrentItem as AutoPsy.Database.Entities.DiaryPage;

            diaryPages.Remove(temp);
            PagesCarouselView.ItemsSource = diaryPages;

            if (temp != null) App.Connector.DeleteData(temp);
        }

        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            SynchronizeContentPages();
        }

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigatorEnd.Date < DateNavigatorStart.Date) DateNavigatorEnd.Date = DateNavigatorStart.Date;
            SynchronizeContentPages();
        }

        private async void InfoButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SymptomViewer());
        }
    }
}