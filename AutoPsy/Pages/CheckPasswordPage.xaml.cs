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
    public partial class CheckPasswordPage : ContentPage
    {
        public CheckPasswordPage()
        {
            InitializeComponent();
        }

        private async void PasswordField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordField.Text.Length == 6)
            {
                var user = (Database.Entities.User)App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);
                if (Logic.Hashing.VerifyHashedPassword(user.HashPassword, PasswordField.Text))
                    await Navigation.PushModalAsync(new MainPage());
                else
                {
                    await DisplayAlert("Упс!", "Хорошая попытка!", "ОК");
                    PasswordField.Text = "";
                }
            }
        }
    }
}