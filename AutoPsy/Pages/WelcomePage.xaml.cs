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
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            var usersExisted = App.Connector.IsTableExisted<Database.Entities.User>();      // проверка существования зарегистрированного пользователя

            if (usersExisted)       // если пользователь существует, то...
                Navigation.PushModalAsync(new CheckPasswordPage());     // переходим на страницу для ввода пароля
            else 
                SetPrivacyPoliceView();        // иначе отображаем условия работы с приложением для нового пользователя
        }

        // Метод нажатия на кнопку согласия перенаправляет на страницу регистрации
        private async void AcceptPolicy_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Pages.RegisterPage());
        
        // Отображаем правила пользования
        private void SetPrivacyPoliceView()
        {
            LoadingView.IsVisible = false;
            PolicyView.IsVisible = true;
        }

    }
}