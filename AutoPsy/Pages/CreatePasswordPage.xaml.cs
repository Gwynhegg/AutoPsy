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
    public partial class CreatePasswordPage : ContentPage       // Страница создания пароля
    {
        private string passwordInstance = "";       // Создаем пустую строку для хранения
        private bool isFirstStep = true;        // Первый шаг - задание пароля, второй - его повторение
        public CreatePasswordPage()
        {
            InitializeComponent();
        }

        private void ResetButton_Clicked(object sender, EventArgs e)        // Событие при нажатии на кнопку сброса пароля
        {
            isFirstStep = true;     // Возвраш=щаемся на первый шаг
            PasswordField.Text = String.Empty;   
            passwordInstance = String.Empty;      // Очищаем значение пароля
            StepLabel.Text = AutoPsy.Resources.PasswordDefault.FirstStep;
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
                    StepLabel.Text = AutoPsy.Resources.PasswordDefault.SecondStep;
                }
                else
                {
                    // Если уже второй шаг

                    if (passwordInstance == PasswordField.Text)     // Если значение пароля и введенное повторно значения совпадают, то...
                    {
                        var hashedPassword = Logic.Hashing.HashPassword(passwordInstance);      // Хэшируем пароль для его безопасного хранения в базе
                        var user = App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser);        // Получаем инстанс подключенного пользователя
                        user.HashPassword = Logic.Hashing.HashPassword(passwordInstance);       // Присваиваем ему созданный пароль
                        App.Connector.UpdateData<Database.Entities.User>(user);     // Обновляем данные о пользователе в базе
                        await Navigation.PushModalAsync(new MainPage());        // Переходим на главную страницу
                    }
                    else
                    {
                        await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, AutoPsy.Resources.AuxiliaryResources.WrongPasswordAlertMessage, AutoPsy.Resources.AuxiliaryResources.ButtonOK);
                        PasswordField.Text = String.Empty;
                    }
                }
            }
        }
    }
}