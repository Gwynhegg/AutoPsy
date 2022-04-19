using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalDataPage : ContentPage
    {
        private Database.Entities.UserHandler userHandler;
        private Database.Entities.User backupUser;
        private ProfilePage parentPage;
        public PersonalDataPage(ProfilePage parent)
        {
            InitializeComponent();
            parentPage = parent;

            userHandler = new Database.Entities.UserHandler();
            userHandler.Clone(App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser));
            backupUser = userHandler.GetUser();

            SetCurrentData();
        }

        private void SetCurrentData()
        {
            SurnameEntry.Text = userHandler.GetUserSurname();
            NameEntry.Text = userHandler.GetUserName();

            if (userHandler.GetUserPatronymic() is null) 
                PatronymicEntry.Text = AutoPsy.Resources.UserDefault.UserPatronymic; 
            else 
                PatronymicEntry.Text = userHandler.GetUserPatronymic();

            BirthDate.Date = userHandler.GetUserBirthDate();

            if (userHandler.GetUserGender() != null)
                foreach (RadioButton button in GenderFrame.Children)
                    if (button.Content.Equals(userHandler.GetUserGender())) button.IsChecked = true;
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked)
            {
                userHandler.SetGender((sender as RadioButton).Content.ToString());
                foreach (RadioButton button in GenderFrame.Children)
                    if (!button.Content.Equals((sender as RadioButton).Content)) button.IsChecked = false;
            }                
        }

        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            userHandler.SetBirtdDate(BirthDate.Date);
        }

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (SurnameEntry.Text == "") SurnameEntry.Text = backupUser.PersonSurname;
            else userHandler.AddSurnameToUser(SurnameEntry.Text);
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (NameEntry.Text == "") NameEntry.Text = backupUser.PersonName;
            else userHandler.AddNameToUser(NameEntry.Text);
        }

        private async void SaveAndQuit_Clicked(object sender, EventArgs e)
        {
            SurnameEntry.Unfocus(); NameEntry.Unfocus(); PatronymicEntry.Unfocus();

            userHandler.UpdateUserInfo();

            parentPage.RefreshData();
            await Navigation.PopModalAsync();
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text == AutoPsy.Resources.UserDefault.UserPatronymic) PatronymicEntry.Text = "";
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text == "") PatronymicEntry.Text = backupUser.PersonPatronymic;
            else userHandler.AddPatronymicToUser(PatronymicEntry.Text);
        }
    }
}