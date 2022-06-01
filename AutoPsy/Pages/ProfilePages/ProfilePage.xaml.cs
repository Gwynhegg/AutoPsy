using AutoPsy.CustomComponents;
using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage, ISynchronizablePage, ISynchronizablePageWithQuery
    {
        private readonly ObservableCollection<UserExperience> experiencePages;     // Определяем коллекцию для хранения карт посещений
        private User user;        // Определяем подключенного к системе юзера
        public ProfilePage()
        {
            InitializeComponent();

            // Ограничиваем значения полей для ввода дат разумными рамками и задаем начальные значения
            this.DateNavigatorStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.DateNavigatorStart.MaximumDate = DateTime.Now;
            this.DateNavigatorEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.DateNavigatorEnd.MaximumDate = DateTime.Now.AddHours(1);
            this.DateNavigatorStart.Date = DateTime.Now;
            this.DateNavigatorEnd.Date = DateTime.Now;

            this.experiencePages = new ObservableCollection<UserExperience>();     // Создаем коллекцию карточек посещений
            this.user = App.Connector.SelectData<User>(App.Connector.currentConnectedUser);        // Получаем инстанс подключенного пользователя

            SynchronizeContentPages();       // Синхронизируем найденные карты посещений пользователя

            SetProfileName();       // Устанавливаем имя пользователя для отображения
        }

        public void RefreshData()       // Метод для обновления персональных данных на актуальные
        {
            this.user = App.Connector.SelectData<User>(App.Connector.currentConnectedUser);        // Получаем актуальные данные о пользователе
            SetProfileName();       // УСтанавливаем новое имя пользователя для отображения
        }

        // При каждой смене дат синхронизируем найденные карточки с заданным интервалом
        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e) => SynchronizeContentPages();

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)       // При каждой смене дат синхронизируем найденные карточки с заданным интервалом
        {
            if (this.DateNavigatorEnd.Date < this.DateNavigatorStart.Date) this.DateNavigatorEnd.Date = this.DateNavigatorStart.Date;
            SynchronizeContentPages();
        }

        public void SynchronizeContentPages()
        {
            if (!App.Connector.IsTableExisted<UserExperience>()) return;      // проверяем существование таблицы с карточками посещений
            // Выбираем те из них, даты которых попадают в заданный интервал
            var queryPages = App.Connector.SelectAll<UserExperience>().
                Where(
                x => DateTime.Compare(x.Appointment, this.DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(x.Appointment, this.DateNavigatorEnd.Date) <= 0)
                .Cast<UserExperience>().ToList();

            if (queryPages.Count == 0) return;      // Если таковых нет, возвращаемся

            this.experiencePages.Clear();
            foreach (UserExperience experiencePage in queryPages)      // Иначе помещаем каждую из них в коллекцию
                this.experiencePages.Add(experiencePage);

            this.ExperienceCarouselView.ItemsSource = this.experiencePages;       // Отображаем колллекцию на форме
        }

        public void SynchronizeContentPages(IСustomComponent experiencePanel)
        {
            UserExperience addedExperience = (experiencePanel as UserExperiencePanel).experienceHandler.GetUserExperience();
            if (DateTime.Compare(addedExperience.Appointment, this.DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(addedExperience.Appointment, this.DateNavigatorEnd.Date) <= 0)
            {
                var index = this.experiencePages.IndexOf(this.experiencePages.FirstOrDefault(x => x.Id == addedExperience.Id));
                if (index != -1)
                    this.experiencePages[index] = addedExperience;
                else
                    this.experiencePages.Add(addedExperience);
                this.ExperienceCarouselView.ItemsSource = this.experiencePages;
            }
        }

        private void SetProfileName()
        {
            var age = DateTime.Now.Year - this.user.BirthDate.Year;
            if (DateTime.Now.DayOfYear < this.user.BirthDate.DayOfYear)
                age--;

            var userDataString = string.Join(" ", this.user.PersonSurname, this.user.PersonName, string.Format(UserDefault.UserAgePlaceholder, age));
            this.PersonalDataButton.Text = userDataString;
        }

        // Метод для добавления карточки посещений
        private async void AddButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new UserExperienceEditorPage(this));

        // Метод для редактирования карточки посещений
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            if (this.experiencePages.Count == 0)
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToEditAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }
            var temp = this.ExperienceCarouselView.CurrentItem as UserExperience;

            if (temp != null) await this.Navigation.PushModalAsync(new UserExperienceEditorPage(this, temp));
        }

        // Метод для удаления карточки посещений
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (this.experiencePages.Count == 0)
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToDeleteAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }

            var temp = this.ExperienceCarouselView.CurrentItem as UserExperience;

            this.experiencePages.Remove(temp);
            this.ExperienceCarouselView.ItemsSource = this.experiencePages;

            if (temp != null) App.Connector.DeleteData(temp);
        }

        private async void PersonalDataButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new PersonalDataPage(this));

        private async void SettingsButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new SettingsPage());
    }
}