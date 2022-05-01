﻿using System;
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
    public partial class ProfilePage : ContentPage, ISynchronizablePage, ISynchronizablePageWithQuery
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

            SynchronizeContentPages();       // Синхронизируем найденные карты посещений пользователя

            SetProfileName();       // Устанавливаем имя пользователя для отображения
        }

        public void RefreshData()       // Метод для обновления персональных данных на актуальные
        {
            user = App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);        // Получаем актуальные данные о пользователе
            SetProfileName();       // УСтанавливаем новое имя пользователя для отображения
        }

        private void DateNavigatorStart_DateSelected(object sender, DateChangedEventArgs e)     // При каждой смене дат синхронизируем найденные карточки с заданным интервалом
        {
            SynchronizeContentPages();
        }

        private void DateNavigatorEnd_DateSelected(object sender, DateChangedEventArgs e)       // При каждой смене дат синхронизируем найденные карточки с заданным интервалом
        {
            if (DateNavigatorEnd.Date < DateNavigatorStart.Date) DateNavigatorEnd.Date = DateNavigatorStart.Date;
            SynchronizeContentPages();
        }

        public void SynchronizeContentPages()
        {
            if (!App.Connector.IsTableExisted<Database.Entities.UserExperience>()) return;      // проверяем существование таблицы с карточками посещений

            // Выбираем те из них, даты которых попадают в заданный интервал
            var queryPages = App.Connector.SelectAll<Database.Entities.UserExperience>().
                Where(
                x => DateTime.Compare(x.Appointment, DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(x.Appointment, DateNavigatorEnd.Date) <= 0)
                .Cast<Database.Entities.UserExperience>().ToList();

            if (queryPages.Count == 0) return;      // Если таковых нет, возвращаемся

            experiencePages.Clear();
            foreach (var experiencePage in queryPages)      // Иначе помещаем каждую из них в коллекцию
                experiencePages.Add(experiencePage);

            ExperienceCarouselView.ItemsSource = experiencePages;       // Отображаем колллекцию на форме
        }

        public void SynchronizeContentPages(CustomComponents.IСustomComponent experiencePanel)
        {
            var addedExperience = (experiencePanel as CustomComponents.UserExperiencePanel).experienceHandler.GetUserExperience();
            if (DateTime.Compare(addedExperience.Appointment, DateNavigatorStart.Date) >= 0 &&
                DateTime.Compare(addedExperience.Appointment, DateNavigatorEnd.Date) <= 0)
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

            var userDataString = String.Join(" ", user.PersonSurname, user.PersonName, String.Format(AutoPsy.Resources.UserDefault.UserAgePlaceholder, age));
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
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, AutoPsy.Resources.AuxiliaryResources.NoRecordsToEditAlertMessage, AutoPsy.Resources.AuxiliaryResources.ButtonOK);
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
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, AutoPsy.Resources.AuxiliaryResources.NoRecordsToDeleteAlertMessage, AutoPsy.Resources.AuxiliaryResources.ButtonOK);
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