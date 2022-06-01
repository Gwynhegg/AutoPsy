using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage() => InitializeComponent();

        private async void ResetPassword_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new CreatePasswordPage());

        private void ResetData_Clicked(object sender, EventArgs e)
        {
            if (this.ResetFrom.IsChecked == true)
            {
                IEnumerable<DiaryPage> diaryPages = App.Connector.SelectAll<DiaryPage>().Where(x => x.DateOfRecord < this.DateResetFrom.Date);
                DeleteData(diaryPages);
                IEnumerable<TableRecomendation> recomendations = App.Connector.SelectAll<TableRecomendation>().Where(x => x.Time < this.DateResetFrom.Date);
                DeleteData(recomendations);
                IEnumerable<TableCondition> conditions = App.Connector.SelectAll<TableCondition>().Where(x => x.Time < this.DateResetFrom.Date);
                DeleteData(conditions);
                IEnumerable<TableTrigger> triggers = App.Connector.SelectAll<TableTrigger>().Where(x => x.Time < this.DateResetFrom.Date);
                DeleteData(triggers);
                IEnumerable<UserExperience> userExp = App.Connector.SelectAll<UserExperience>().Where(x => x.Appointment < this.DateResetFrom.Date);
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
            List<T> data = App.Connector.SelectAll<T>();
            DeleteData(data);
        }

        private void DeleteData<T>(IEnumerable<T> objects)
        {
            foreach (T obj in objects)
                App.Connector.DeleteData(obj);
        }

        private async void DeleteAll_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert(Alerts.AlertMessage, Alerts.DeleteAllWarning, AuxiliaryResources.Yes, AuxiliaryResources.No);
            if (!answer)
                return;

            SelectDataToDelete<DiaryPage>();
            SelectDataToDelete<TableRecomendation>();
            SelectDataToDelete<TableCondition>();
            SelectDataToDelete<TableTrigger>();
            SelectDataToDelete<UserExperience>();
            SelectDataToDelete<User>();
            await this.Navigation.PushModalAsync(new WelcomePage());
        }

        private void ResetFrom_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true) this.DateResetFrom.IsEnabled = true; else this.DateResetFrom.IsEnabled = false;
        }
    }
}