using AutoPsy.CustomComponents;
using AutoPsy.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserExperienceEditorPage : ContentPage
    {
        private readonly UserExperiencePanel experiencePanel;       // панель для ввода пользовательских данных
        private readonly ISynchronizablePage parentPage;     // страница-родитель текущей для передачи и обновления данных
        public byte StateMode { get; private set; }
        public UserExperienceEditorPage(ISynchronizablePage parentPage)     // поскольку страниц-родителей может быть две, используем интерфейс
        {
            InitializeComponent();
            this.parentPage = parentPage;
            this.experiencePanel = new UserExperiencePanel(enabled: true);      // создаем панель для ввода
            this.MainGrid.Children.Add(this.experiencePanel, 0, 0);
        }

        // Если конструктор вызван с двумя аргументами, то это модификация пользовательских данных, которая немного отличается
        public UserExperienceEditorPage(ISynchronizablePage parentPage, Database.Entities.UserExperience userExperience)
        {
            InitializeComponent();
            this.parentPage = parentPage;

            // Перегружаем панель для ввода, заполняя уже существующие поля
            this.experiencePanel = new UserExperiencePanel(enabled: true, userExperience);
            this.MainGrid.Children.Add(this.experiencePanel, 0, 0);
        }

        private async void SaveAndReturn_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.experiencePanel.TrySave();      // пытаемся сохранить данные на панели
                this.parentPage.SynchronizeContentPages(this.experiencePanel);        // синхронизиуем данные со страницей-родителем
                await this.Navigation.PopModalAsync();       // уходим с этой страницы
            }
            catch (Exception)        // в случае возникновения ошибки уведомляем об этом
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.ExperienceAlertMessage, AuxiliaryResources.ButtonOK);
            }
        }
    }
}