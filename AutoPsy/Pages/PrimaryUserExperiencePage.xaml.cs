using AutoPsy.CustomComponents;
using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrimaryUserExperiencePage : ContentPage, ISynchronizablePage
    {
        private readonly UserHandler userHandler;
        private readonly ObservableCollection<UserExperience> experiencePages;     // данная коллекция содержит "карточки посещений" пользователя с указанием базовой информации
        public PrimaryUserExperiencePage(UserHandler userHandler)
        {
            InitializeComponent();
            this.userHandler = userHandler;
            this.experiencePages = new ObservableCollection<UserExperience>();
        }

        // Метод для добавления карточки
        // при нажатии кнопки переходим на экран создания карточки посещений

        private async void Button_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new UserExperienceEditorPage(this));

        // Метод для синхронизации текущей формы и коллекции с созданной карточкой посещений
        public void SynchronizeContentPages(IСustomComponent experiencePanel)
        {
            UserExperience addedExperience = (experiencePanel as UserExperiencePanel).experienceHandler.GetUserExperience();        // через класс обёртку получаем инстанс карточки

            // Пытаемся найти карточку с таким же ID в коллекции
            var indexOfElement = this.experiencePages.IndexOf(this.experiencePages.Where(x => x.Id == addedExperience.Id).FirstOrDefault());

            if (indexOfElement == -1)       // если ее нет, то...
                this.experiencePages.Add(addedExperience);     // добавляем ее в коллекцию
            else
                this.experiencePages[indexOfElement] = addedExperience;      // ..иначе обновляем ее значение на полученное
            this.ExperienceCarouselView.ItemsSource = this.experiencePages;
        }

        // Метод для вызова окна редактирования карточки
        private async void EditButton_Clicked(object sender, EventArgs e)
        {

            if (this.experiencePages.Count == 0)     // если коллекция не содержит записей, то выводим сообщение об ошибке
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToEditAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }
            var temp = this.ExperienceCarouselView.CurrentItem as UserExperience;      // Если есть, получаем выбранный инстанс

            if (temp != null) await this.Navigation.PushModalAsync(new UserExperienceEditorPage(this, temp));        // Передаем его в форму для редактирования
        }

        // Метод для удаления карточки
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (this.experiencePages.Count == 0)     // если коллекция не содержит записей, то выводим сообщение об ошибке
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.NoRecordsToDeleteAlertMessage, AuxiliaryResources.ButtonOK);
                return;
            }

            var temp = this.ExperienceCarouselView.CurrentItem as UserExperience;      // Если есть, получаем выбранный инстанс

            this.experiencePages.Remove(temp);       // удаляем элемент из коллекции
            this.ExperienceCarouselView.ItemsSource = this.experiencePages;

            if (temp != null) App.Connector.DeleteData(temp);       // вызываем метод базы данных для удаления записи
        }

        // Метод для перехода к следующей странице - созданию пароля
        private async void MoveForward_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new CreatePasswordPage(this.userHandler));
    }
}