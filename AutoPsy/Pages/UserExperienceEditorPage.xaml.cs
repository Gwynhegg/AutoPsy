using System;
using AutoPsy.CustomComponents;
using AutoPsy.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserExperienceEditorPage : ContentPage
    {
        UserExperiencePanel experiencePanel;       // панель для ввода пользовательских данных
        private ISynchronizablePage parentPage;     // страница-родитель текущей для передачи и обновления данных
        public byte StateMode { get; private set; }
        public UserExperienceEditorPage(ISynchronizablePage parentPage)     // поскольку страниц-родителей может быть две, используем интерфейс
        {
            InitializeComponent();
            this.parentPage = parentPage;
            experiencePanel = new UserExperiencePanel(enabled: true);      // создаем панель для ввода
            CurrentItem.Children.Insert(0, experiencePanel);
        }

        // Если конструктор вызван с двумя аргументами, то это модификация пользовательских данных, которая немного отличается
        public UserExperienceEditorPage(ISynchronizablePage parentPage, Database.Entities.UserExperience userExperience)
        {
            InitializeComponent();
            this.parentPage = parentPage;

            // Перегружаем панель для ввода, заполняя уже существующие поля
            experiencePanel = new UserExperiencePanel(enabled: true, userExperience);
            CurrentItem.Children.Insert(0, experiencePanel);
        }

        private async void SaveAndReturn_Clicked(object sender, EventArgs e)
        {
            try
            {
                experiencePanel.TrySave();      // пытаемся сохранить данные на панели
                parentPage.SynchronizeContentPages(experiencePanel);        // синхронизиуем данные со страницей-родителем
                await Navigation.PopModalAsync();       // уходим с этой страницы
            }
            catch (Exception ex)        // в случае возникновения ошибки уведомляем об этом
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.ExperienceAlertMessage, AuxiliaryResources.ButtonOK);
            }
        }
    }
}