using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryEditPage : ContentPage
    {
        private CustomComponents.DiaryPagePanel pagePanel;
        private ISynchronizablePage parentPage;

        public DiaryEditPage(ISynchronizablePage page)
        {
            InitializeComponent();
            parentPage = page;
            pagePanel = new CustomComponents.DiaryPagePanel(enabled:true, this);      // создаем панель для ввода
            CurrentItem.Children.Insert(0, pagePanel);
        }
        public DiaryEditPage(ISynchronizablePage parentPage, Database.Entities.DiaryPage diaryPage)
        {
            InitializeComponent();
            this.parentPage = parentPage;

            // Перегружаем панель для ввода, заполняя уже существующие поля
            pagePanel = new CustomComponents.DiaryPagePanel(enabled: true, this, diaryPage);
            CurrentItem.Children.Insert(0, pagePanel);
        }


        private async void SaveAndReturn_Clicked(object sender, EventArgs e)
        {
            try
            {
                pagePanel.TrySave();      // пытаемся сохранить данные на панели
                parentPage.SynchronizeContentPages(pagePanel);        // синхронизиуем данные со страницей-родителем
                await Navigation.PopModalAsync();       // уходим с этой страницы
            }
            catch (Exception ex)        // в случае возникновения ошибки уведомляем об этом
            {
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, AutoPsy.Resources.AuxiliaryResources.DiaryAlertMessage, AutoPsy.Resources.AuxiliaryResources.ButtonOK);
            }
        }
    }
}