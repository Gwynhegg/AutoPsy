using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoPsy.Database.Entities;
using AutoPsy.CustomComponents;
using AutoPsy.Resources;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrimaryUserExperiencePage : ContentPage, ISynchronizablePage
    {
        private UserHandler userHandler;
        private ObservableCollection<UserExperience> experiencePages;     // данная коллекция содержит "карточки посещений" пользователя с указанием базовой информации
        public PrimaryUserExperiencePage(UserHandler userHandler)
        {
            InitializeComponent();
            this.userHandler = userHandler;
            experiencePages = new ObservableCollection<UserExperience>();
        }

        // Метод для добавления карточки
        // при нажатии кнопки переходим на экран создания карточки посещений

        private async void Button_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new UserExperienceEditorPage(this));

        // Метод для синхронизации текущей формы и коллекции с созданной карточкой посещений
        public void SynchronizeContentPages(IСustomComponent experiencePanel)
        {
            var addedExperience = (experiencePanel as UserExperiencePanel).experienceHandler.GetUserExperience();        // через класс обёртку получаем инстанс карточки

            // Пытаемся найти карточку с таким же ID в коллекции
            int indexOfElement = experiencePages.IndexOf(experiencePages.Where(x => x.Id == addedExperience.Id).FirstOrDefault());

            if (indexOfElement == -1)       // если ее нет, то...
                experiencePages.Add(addedExperience);     // добавляем ее в коллекцию
            else
                experiencePages[indexOfElement] = addedExperience;      // ..иначе обновляем ее значение на полученное
            ExperienceCarouselView.ItemsSource = experiencePages;
        }

        // Метод для вызова окна редактирования карточки
        private async void EditButton_Clicked(object sender, EventArgs e)
        {

            if (experiencePages.Count == 0)     // если коллекция не содержит записей, то выводим сообщение об ошибке
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToEditAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }
            var temp = ExperienceCarouselView.CurrentItem as UserExperience;      // Если есть, получаем выбранный инстанс

            if (temp != null) await Navigation.PushModalAsync(new UserExperienceEditorPage(this, temp));        // Передаем его в форму для редактирования
        }

        // Метод для удаления карточки
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (experiencePages.Count == 0)     // если коллекция не содержит записей, то выводим сообщение об ошибке
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToDeleteAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }

            var temp = ExperienceCarouselView.CurrentItem as UserExperience;      // Если есть, получаем выбранный инстанс

            experiencePages.Remove(temp);       // удаляем элемент из коллекции
            ExperienceCarouselView.ItemsSource = experiencePages;

            if (temp != null) App.Connector.DeleteData(temp);       // вызываем метод базы данных для удаления записи
        }

        // Метод для перехода к следующей странице - созданию пароля
        private async void MoveForward_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new CreatePasswordPage(userHandler));
    }
}