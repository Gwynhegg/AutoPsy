using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using AutoPsy.Database.Entities;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            var usersExisted = App.Connector.IsTableExisted<User>();      // проверка существования зарегистрированного пользователя

            if (usersExisted)       // если пользователь существует, то...
            {
                var user = App.Connector.SelectAll<User>().First();
                if (user.HashPassword is null || user.HashPassword.Equals(String.Empty))        // Если пароль пользователя не задан...
                    Navigation.PushModalAsync(new MainPage());      // переходим сразу на главную форму
                else
                    Navigation.PushModalAsync(new CheckPasswordPage());     // переходим на страницу для ввода пароля
            }
            else 
                SetPrivacyPoliceView();        // иначе отображаем условия работы с приложением для нового пользователя
        }

        // Метод нажатия на кнопку согласия перенаправляет на страницу регистрации
        private async void AcceptPolicy_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new RegisterPage());
        
        // Отображаем правила пользования
        private void SetPrivacyPoliceView()
        {
            LoadingView.IsVisible = false;
            PolicyView.IsVisible = true;
        }

    }
}