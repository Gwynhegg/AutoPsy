using AutoPsy.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckPasswordPage : ContentPage        // Форма авторизации - первоначальный вход в приложение
    {
        public CheckPasswordPage() => InitializeComponent();

        private async void PasswordField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PasswordField.Text.Length == 6)     // Стандартная длина пароля - 6 символов, проверяем поле на уровень заполненности
            {
                Database.Entities.User user = App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);        // Получаем хэшированный пароль пользователя
                if (Logic.Hashing.VerifyHashedPassword(user.HashPassword, this.PasswordField.Text))      // Вызываем метод верификации хэша
                {
                    this.PasswordField.IsEnabled = false;
                    await this.Navigation.PushModalAsync(new MainPage());        // Если все успешно, переходим на главную форму
                }
                else
                {
                    await DisplayAlert(Alerts.AlertMessage, Alerts.AccessDeniedAlertMessage, AuxiliaryResources.ButtonOK);       // Иначе выводим сообщение об ошибке
                    this.PasswordField.Text = string.Empty;
                }
            }
        }
    }
}