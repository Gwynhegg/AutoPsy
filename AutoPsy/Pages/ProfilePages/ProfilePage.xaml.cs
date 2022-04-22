using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage, ISynchronizablePage     // Страница персональных данных пользователя, а также журнал его посещений
    {
        private ObservableCollection<Database.Entities.UserExperience> experiencePages;     // Определяем коллекцию для хранения карт посещений
        private Database.Entities.User user;        // Определяем подключенного к системе юзера
        public ProfilePage()
        {
            InitializeComponent();

            // Ограничиваем значения полей для ввода дат разумными рамками и задаем начальные значения
            DateNavigatorStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigatorStart.MaximumDate = DateTime.Now;
            DateNavigatorEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigatorEnd.MaximumDate = DateTime.Now.AddHours(1);
            DateNavigatorStart.Date= DateTime.Now;
            DateNavigatorEnd.Date= DateTime.Now;

            experiencePages = new ObservableCollection<Database.Entities.UserExperience>();     // Создаем коллекцию карточек посещений
            user = App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);        // Получаем инстанс подключенного пользователя

            SynchronizeExperiencePages();       // Синхронизируем найденные карты посещений пользователя

            SetProfileName();       // Устанавливаем имя пользователя для отображения
        }

        public void RefreshData()       // Метод для обновления персональных данных на актуальные
        {
            user = App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);        // Получаем актуальные данные о пользователе
            SetProfileName();       // УСтанавливаем новое имя пользователя для отображения
        }

        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e)     // При каждой смене дат синхронизируем найденные карточки с заданным интервалом
        {
            SynchronizeExperiencePages();
        }

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)       // При каждой смене дат синхронизируем найденные карточки с заданным интервалом
        {
            if (DateNavigatorEnd.Date < DateNavigatorStart.Date) DateNavigatorEnd.Date = DateNavigatorStart.Date;
            SynchronizeExperiencePages();
        }

        private void SynchronizeExperiencePages()
        {
            if (!App.Connector.IsTableExisted<Database.Entities.UserExperience>()) return;      // проверяем существование таблицы с карточками посещений

            // Выбираем те из них, даты которых попадают в заданный интервал
            var queryPages = App.Connector.SelectAll<Database.Entities.UserExperience>().
                Where(
                x => x.Appointment.Year >= DateNavigatorStart.Date.Year &&
                x.Appointment.Month >= DateNavigatorStart.Date.Month &&
                x.Appointment.Day >= DateNavigatorStart.Date.Day &&
                x.Appointment.Year <= DateNavigatorEnd.Date.Year &&
                x.Appointment.Month <= DateNavigatorEnd.Date.Month &&
                x.Appointment.Day <= DateNavigatorEnd.Date.Day
                ).Cast<Database.Entities.UserExperience>().ToList();

            if (queryPages.Count == 0) return;      // Если таковых нет, возвращаемся

            experiencePages.Clear();
            foreach (var experiencePage in queryPages)      // Иначе помещаем каждую из них в коллекцию
                experiencePages.Add(experiencePage);

            ExperienceCarouselView.ItemsSource = experiencePages;       // Отображаем колллекцию на форме
        }

        public void SynchronizeContentPages(CustomComponents.UserExperiencePanel experiencePanel)
        {
            var addedExperience = experiencePanel.experienceHandler.GetUserExperience();
            if (addedExperience.Appointment.Year >= DateNavigatorStart.Date.Year &&
                addedExperience.Appointment.Month >= DateNavigatorStart.Date.Month &&
                addedExperience.Appointment.Day >= DateNavigatorStart.Date.Day &&
                addedExperience.Appointment.Year <= DateNavigatorEnd.Date.Year &&
                addedExperience.Appointment.Month <= DateNavigatorEnd.Date.Month &&
                addedExperience.Appointment.Day <= DateNavigatorEnd.Date.Day)
            {
                var index = experiencePages.IndexOf(experiencePages.FirstOrDefault(x => x.Id == addedExperience.Id));
                if (index != -1)
                    experiencePages[index] = addedExperience;
                else
                    experiencePages.Add(addedExperience);
                ExperienceCarouselView.ItemsSource = experiencePages;
            }
        }

        private void SetProfileName()
        {
            var age = DateTime.Now.Year - user.BirthDate.Year;
            if (DateTime.Now.DayOfYear < user.BirthDate.DayOfYear)
                age++;

            var userDataString = String.Join(" ", user.PersonSurname, user.PersonName, String.Format("({0} лет/года)", age));
            PersonalDataButton.Text = userDataString;
        }

        // Метод для добавления карточки посещений
        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserExperienceEditorPage(this));
        }

        // Метод для редактирования карточки посещений
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            if (experiencePages.Count == 0)
            {
                await DisplayAlert("Упс!", "Пока у вас нет записей для редактирования", "OK");
                return;
            }
            var temp = ExperienceCarouselView.CurrentItem as AutoPsy.Database.Entities.UserExperience;

            if (temp != null) await Navigation.PushModalAsync(new UserExperienceEditorPage(this, temp));
        }

        // Метод для удаления карточки посещений
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (experiencePages.Count == 0)
            {
                await DisplayAlert("Упс!", "Пока у вас нет записей для удаления", "OK");
                return;
            }

            var temp = ExperienceCarouselView.CurrentItem as AutoPsy.Database.Entities.UserExperience;

            experiencePages.Remove(temp);
            ExperienceCarouselView.ItemsSource = experiencePages;

            if (temp != null) App.Connector.DeleteData(temp);
        }

        private async void PersonalDataButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PersonalDataPage(this));
        }


    }
}