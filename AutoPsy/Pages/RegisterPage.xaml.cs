using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private Database.Entities.UserHandler userHandler;
        public RegisterPage()
        {
            InitializeComponent();
            userHandler = new Database.Entities.UserHandler();
        }

        private void Register_Clicked(object sender, EventArgs e)
        {
            LoginOptions.IsVisible = false;
            RegisterForm.IsVisible = true;
        }

        private void HideRegisterForm_Clicked(object sender, EventArgs e)
        {
            LoginOptions.IsVisible = true;
            RegisterForm.IsVisible = false;
        }

        private async void Continue_Clicked(object sender, EventArgs e)
        {
            if (userHandler.CheckCorrectness())
            {
                userHandler.CreateUserInfo();
                if (HasExperience.IsChecked)
                {
                    PrimaryUserExperiencePage experiencePage = new PrimaryUserExperiencePage();
                    await Navigation.PushModalAsync(experiencePage);
                }
            }
            else
            {
                await DisplayAlert("Упс!", "Кажется, вы забыли указать имя или фамилию", "ОК");
            }
        }

        private void LoginVK_Clicked(object sender, EventArgs e)
        {
            // ЛОГИКА ДЛЯ ЛОГИНА ЧЕРЕЗ ВК
        }

        private void SurnameEntry_Focused(object sender, FocusEventArgs e)
        {
            SurnameEntry.Text = "";
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            NameEntry.Text = "";
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            PatronymicEntry.Text = "";
        }

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (SurnameEntry.Text != "" && SurnameEntry.Text != AutoPsy.Resources.UserDefault.UserSurname)
                userHandler.AddSurnameToUser(SurnameEntry.Text);
            else
                SurnameEntry.Text = AutoPsy.Resources.UserDefault.UserSurname;
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (NameEntry.Text != "" && NameEntry.Text != AutoPsy.Resources.UserDefault.UserName)
                userHandler.AddNameToUser(NameEntry.Text);
            else
                NameEntry.Text = AutoPsy.Resources.UserDefault.UserName;
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text != "" && PatronymicEntry.Text != AutoPsy.Resources.UserDefault.UserPatronymic)
                userHandler.AddPatronymicToUser(PatronymicEntry.Text);
            else
                PatronymicEntry.Text = AutoPsy.Resources.UserDefault.UserPatronymic;
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)
                userHandler.SetGender((sender as RadioButton).Content.ToString());
        }
        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            userHandler.SetBirtdDate(BirthDate.Date);
        }
    }
}