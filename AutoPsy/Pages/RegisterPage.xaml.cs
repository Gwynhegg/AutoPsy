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

        private void Continue_Clicked(object sender, EventArgs e)
        {
            if (userHandler.CheckCorrectness())
                userHandler.CreateUserInfo();
        }

        private void LoginVK_Clicked(object sender, EventArgs e)
        {

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
            userHandler.AddSurnameToUser(SurnameEntry.Text);
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            userHandler.AddNameToUser(NameEntry.Text);
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            userHandler.AddPatronymicToUser(PatronymicEntry.Text);
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)
                userHandler.SetGender((sender as RadioButton).Content.ToString());
        }
    }
}