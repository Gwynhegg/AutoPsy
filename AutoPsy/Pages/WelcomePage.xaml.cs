using AutoPsy.Database.Entities;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                User user = App.Connector.SelectAll<User>().First();
                if (user.HashPassword is null || user.HashPassword.Equals(string.Empty))        // Если пароль пользователя не задан...
                    this.Navigation.PushModalAsync(new MainPage());      // переходим сразу на главную форму
                else
                    this.Navigation.PushModalAsync(new CheckPasswordPage());     // переходим на страницу для ввода пароля
            }
            else
            {
                SetPrivacyPoliceView();        // иначе отображаем условия работы с приложением для нового пользователя
            }
        }

        // Метод нажатия на кнопку согласия перенаправляет на страницу регистрации
        private async void AcceptPolicy_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new RegisterPage());

        // Отображаем правила пользования
        private void SetPrivacyPoliceView()
        {
            this.LoadingView.IsVisible = false;
            this.PolicyView.IsVisible = true;
        }

    }
}