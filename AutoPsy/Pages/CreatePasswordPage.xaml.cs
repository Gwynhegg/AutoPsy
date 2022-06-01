using AutoPsy.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePasswordPage : ContentPage       // Страница создания пароля
    {
        private readonly Database.Entities.UserHandler userHandler;
        private string passwordInstance = string.Empty;       // Создаем пустую строку для хранения
        private bool isFirstStep = true;        // Первый шаг - задание пароля, второй - его повторение
        public CreatePasswordPage(Database.Entities.UserHandler userHandler)
        {
            InitializeComponent();
            this.userHandler = userHandler;
        }

        public CreatePasswordPage()
        {
            this.userHandler = new Database.Entities.UserHandler();
            this.userHandler.GetUser();
        }

        private void ResetButton_Clicked(object sender, EventArgs e)        // Событие при нажатии на кнопку сброса пароля
        {
            this.isFirstStep = true;     // Возвраш=щаемся на первый шаг
            this.PasswordField.Text = string.Empty;
            this.passwordInstance = string.Empty;      // Очищаем значение пароля
            this.StepLabel.Text = PasswordDefault.FirstStep;
        }

        private async void PasswordField_TextChanged(object sender, TextChangedEventArgs e)     // Метод изменения поля ввода пароля
        {
            if (this.PasswordField.Text.Length == 6)     // Длина пароля - 6 символов. Проверяем на уровень заполнения
            {
                if (this.isFirstStep)        // Если мы только инициализируем пароль, то...
                {
                    this.passwordInstance = this.PasswordField.Text;      // Присваиваем переменной значение поля
                    this.isFirstStep = false;        // Изменяем шаг на второй
                    this.PasswordField.Text = string.Empty;
                    this.StepLabel.Text = PasswordDefault.SecondStep;
                }
                else
                {
                    // Если уже второй шаг

                    if (this.passwordInstance == this.PasswordField.Text)     // Если значение пароля и введенное повторно значения совпадают, то...
                    {
                        var hashedPassword = Logic.Hashing.HashPassword(this.passwordInstance);      // Хэшируем пароль для его безопасного хранения в базе

                        this.userHandler.SetPassword(hashedPassword);
                        this.userHandler.UpdateUserInfo();
                        await this.Navigation.PushModalAsync(new MainPage());        // Переходим на главную страницу
                    }
                    else
                    {
                        await DisplayAlert(Alerts.AlertMessage, Alerts.WrongPasswordAlertMessage, AuxiliaryResources.ButtonOK);
                        this.PasswordField.Text = string.Empty;
                    }
                }
            }
        }

        private async void SkipButton_Clicked(object sender, EventArgs e)
        {
            this.userHandler.SetPassword(string.Empty);
            this.userHandler.UpdateUserInfo();
            await this.Navigation.PushModalAsync(new MainPage());
        }
    }
}