using System;
using AutoPsy.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AutoPsy.Resources;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void ResetPassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CreatePasswordPage());
        }

        private void ResetData_Clicked(object sender, EventArgs e)
        {
            if (ResetFrom.IsChecked == true)
            {
                var diaryPages = App.Connector.SelectAll<DiaryPage>().Where(x => x.DateOfRecord < DateResetFrom.Date);
                DeleteData(diaryPages);
                var recomendations = App.Connector.SelectAll<TableRecomendation>().Where(x => x.Time < DateResetFrom.Date);
                DeleteData(recomendations);
                var conditions = App.Connector.SelectAll<TableCondition>().Where(x => x.Time < DateResetFrom.Date);
                DeleteData(conditions);
                var triggers = App.Connector.SelectAll<TableTrigger>().Where(x => x.Time < DateResetFrom.Date);
                DeleteData(triggers);
                var userExp = App.Connector.SelectAll<UserExperience>().Where(x => x.Appointment < DateResetFrom.Date);
                DeleteData(userExp);
            }
            else
            {
                SelectDataToDelete<DiaryPage>();
                SelectDataToDelete<TableRecomendation>();
                SelectDataToDelete<TableCondition>();
                SelectDataToDelete<TableTrigger>();
                SelectDataToDelete<UserExperience>();
            }
        }
        private void SelectDataToDelete<T>() where T : new()
        {
            var data = App.Connector.SelectAll<T>();
            DeleteData(data);
        }

        private void DeleteData<T>(IEnumerable<T> objects)
        {
            foreach (var obj in objects)
                App.Connector.DeleteData(obj);
        }

        private async void DeleteAll_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(Alerts.AlertMessage, Alerts.DeleteAllWarning, AuxiliaryResources.Yes, AuxiliaryResources.No);
            if (!answer)
                return;

            SelectDataToDelete<DiaryPage>();
            SelectDataToDelete<TableRecomendation>();
            SelectDataToDelete<TableCondition>();
            SelectDataToDelete<TableTrigger>();
            SelectDataToDelete<UserExperience>();
            SelectDataToDelete<User>();
            await Navigation.PushModalAsync(new WelcomePage());
        }

        private void ResetFrom_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true) DateResetFrom.IsEnabled = true; else DateResetFrom.IsEnabled = false;
        }
    }
}