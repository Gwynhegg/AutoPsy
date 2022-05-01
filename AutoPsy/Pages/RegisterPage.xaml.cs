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
        private Database.Entities.UserHandler userHandler;      // класс-обертка для управления классом пользователем на основе конструктора
        public RegisterPage()
        {
            InitializeComponent();
            userHandler = new Database.Entities.UserHandler();
            userHandler.SetBirtdDate(DateTime.Now);     // устанавливаем дату по умолчанию
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

        private async void Continue_Clicked(object sender, EventArgs e)
        {
            NameEntry.Unfocus(); SurnameEntry.Unfocus(); PatronymicEntry.Unfocus();     // С помощью анфокуса принудительно заставляем сработать обработчик события

            if (userHandler.CheckCorrectness())     // проверяем корректность введенных данных
            {
                userHandler.CreateUserInfo();       // если все нормально, вызываем метод создания юзера
                if (HasExperience.IsChecked)        // если пользователь указал, что у него имеется опыт...
                    await Navigation.PushModalAsync(new PrimaryUserExperiencePage());       // переходим на страницу указания опыта
                else
                    await Navigation.PushModalAsync(new CreatePasswordPage());      // ... или на страницу задания пароля
            }
            else
            {
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, AutoPsy.Resources.AuxiliaryResources.RegisterAlertMessage, AutoPsy.Resources.AuxiliaryResources.ButtonOK);
            }
        }

        private void LoginVK_Clicked(object sender, EventArgs e)
        {
            // ЛОГИКА ДЛЯ ЛОГИНА ЧЕРЕЗ ВК
        }

        private void SurnameEntry_Focused(object sender, FocusEventArgs e)
        {
            if (SurnameEntry.Text == AutoPsy.Resources.UserDefault.UserSurname) SurnameEntry.Text = String.Empty;
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            if (NameEntry.Text == AutoPsy.Resources.UserDefault.UserName) NameEntry.Text = String.Empty;
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text == AutoPsy.Resources.UserDefault.UserPatronymic) PatronymicEntry.Text = String.Empty;
        }

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)         // если введено корректное значение, заносим данные в обертку
        {
            if (SurnameEntry.Text != String.Empty && SurnameEntry.Text != AutoPsy.Resources.UserDefault.UserSurname)
                userHandler.AddSurnameToUser(SurnameEntry.Text);
            else
                SurnameEntry.Text = AutoPsy.Resources.UserDefault.UserSurname;
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)        // если введено корректное значение, заносим данные в обертку
        {
            if (NameEntry.Text != String.Empty && NameEntry.Text != AutoPsy.Resources.UserDefault.UserName)
                userHandler.AddNameToUser(NameEntry.Text);
            else
                NameEntry.Text = AutoPsy.Resources.UserDefault.UserName;
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)     // если введено корректное значение, заносим данные в обертку
        {
            if (PatronymicEntry.Text != String.Empty && PatronymicEntry.Text != AutoPsy.Resources.UserDefault.UserPatronymic)
                userHandler.AddPatronymicToUser(PatronymicEntry.Text);
            else
                PatronymicEntry.Text = AutoPsy.Resources.UserDefault.UserPatronymic;
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)      // если один из переключателей был выбран...
                userHandler.SetGender((sender as RadioButton).Content.ToString());      // сохраняем данные и заносим в класс

            foreach (RadioButton button in GenderFrame.Children)        // снимаем выделение с других переключателей на форме
                if (!button.Content.Equals((sender as RadioButton).Content)) button.IsChecked = false;
        }

        // при смене даты на форме заносим изменения в обертку
        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e) => userHandler.SetBirtdDate(BirthDate.Date);      
    }
}