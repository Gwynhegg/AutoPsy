using AutoPsy.CustomComponents;
using AutoPsy.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryEditPage : ContentPage
    {
        private readonly DiaryPagePanel pagePanel;
        private readonly ISynchronizablePage parentPage;

        public DiaryEditPage(ISynchronizablePage page)
        {
            InitializeComponent();
            this.parentPage = page;
            this.pagePanel = new DiaryPagePanel(enabled: true, this);      // создаем панель для ввода
            this.CurrentItem.Children.Insert(0, this.pagePanel);
        }
        public DiaryEditPage(ISynchronizablePage parentPage, Database.Entities.DiaryPage diaryPage)
        {
            InitializeComponent();
            this.parentPage = parentPage;

            // Перегружаем панель для ввода, заполняя уже существующие поля
            this.pagePanel = new DiaryPagePanel(enabled: true, this, diaryPage);
            this.CurrentItem.Children.Insert(0, this.pagePanel);
        }


        private async void SaveAndReturn_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.pagePanel.TrySave();      // пытаемся сохранить данные на панели
                this.parentPage.SynchronizeContentPages(this.pagePanel);        // синхронизиуем данные со страницей-родителем
                await this.Navigation.PopModalAsync();       // уходим с этой страницы
            }
            catch (Exception)        // в случае возникновения ошибки уведомляем об этом
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.DiaryAlertMessage, AuxiliaryResources.ButtonOK);
            }
        }
    }
}