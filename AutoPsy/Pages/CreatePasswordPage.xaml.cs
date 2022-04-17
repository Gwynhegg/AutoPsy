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
    public partial class CreatePasswordPage : ContentPage
    {
        private string passwordInstance = "";
        private bool isFirstStep = true;
        public CreatePasswordPage()
        {
            InitializeComponent();
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            isFirstStep = true;
            PasswordField.Text = "";
            passwordInstance = "";
            StepLabel.Text = AutoPsy.Resources.PasswordDefault.FirstStep;
        }

        private async void PasswordField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PasswordField.Text.Length == 6)
            {
                if (isFirstStep)
                {
                    passwordInstance = PasswordField.Text;
                    isFirstStep = false;
                    PasswordField.Text = "";
                    StepLabel.Text = AutoPsy.Resources.PasswordDefault.SecondStep;
                }
                else
                {
                    if (passwordInstance == PasswordField.Text)
                    {
                        var hashedPassword = Logic.Hashing.HashPassword(passwordInstance);
                        var user = (Database.Entities.User)App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);
                        user.HashPassword = Logic.Hashing.HashPassword(passwordInstance);
                        App.Connector.UpdateData<Database.Entities.User>(user);
                        await Navigation.PushModalAsync(new MainPage());
                    }
                    else
                    {
                        await DisplayAlert("Упс!", "Введенный пароль не совпадает с заданным. Попробуйте еще раз!", "ОК");
                        PasswordField.Text = "";
                    }
                }
            }
        }
    }
}