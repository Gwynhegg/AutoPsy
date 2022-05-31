using System;
using AutoPsy.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePasswordPage : ContentPage       // Страница создания пароля
    {
        private Database.Entities.UserHandler userHandler;
        private string passwordInstance = String.Empty;       // Создаем пустую строку для хранения
        private bool isFirstStep = true;        // Первый шаг - задание пароля, второй - его повторение
        public CreatePasswordPage(Database.Entities.UserHandler userHandler)
        {
            InitializeComponent();
            this.userHandler = userHandler;
        }

        public CreatePasswordPage()
        {
            userHandler = new Database.Entities.UserHandler();
            userHandler.GetUser();
        }

        private void ResetButton_Clicked(object sender, EventArgs e)        // Событие при нажатии на кнопку сброса пароля
        {
            isFirstStep = true;     // Возвраш=щаемся на первый шаг
            PasswordField.Text = String.Empty;   
            passwordInstance = String.Empty;      // Очищаем значение пароля
            StepLabel.Text = PasswordDefault.FirstStep;
        }

        private async void PasswordField_TextChanged(object sender, TextChangedEventArgs e)     // Метод изменения поля ввода пароля
        {
            if (PasswordField.Text.Length == 6)     // Длина пароля - 6 символов. Проверяем на уровень заполнения
            {
                if (isFirstStep)        // Если мы только инициализируем пароль, то...
                {
                    passwordInstance = PasswordField.Text;      // Присваиваем переменной значение поля
                    isFirstStep = false;        // Изменяем шаг на второй
                    PasswordField.Text = String.Empty;
                    StepLabel.Text = PasswordDefault.SecondStep;
                }
                else
                {
                    // Если уже второй шаг

                    if (passwordInstance == PasswordField.Text)     // Если значение пароля и введенное повторно значения совпадают, то...
                    {
                        var hashedPassword = Logic.Hashing.HashPassword(passwordInstance);      // Хэшируем пароль для его безопасного хранения в базе

                        userHandler.SetPassword(hashedPassword);
                        userHandler.UpdateUserInfo();
                        await Navigation.PushModalAsync(new MainPage());        // Переходим на главную страницу
                    }
                    else
                    {
                        await DisplayAlert(Alerts.AlertMessage, Alerts.WrongPasswordAlertMessage, AuxiliaryResources.ButtonOK);
                        PasswordField.Text = String.Empty;
                    }
                }
            }
        }

        private async void SkipButton_Clicked(object sender, EventArgs e)
        {
            userHandler.SetPassword(String.Empty);
            userHandler.UpdateUserInfo();
            await Navigation.PushModalAsync(new MainPage());
        }
    }
}