using System;
using AutoPsy.Database.Entities;
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
    public partial class RegisterPage : ContentPage
    {
        private UserHandler userHandler;      // класс-обертка для управления классом пользователем на основе конструктора
        public RegisterPage()
        {
            InitializeComponent();
            userHandler = new UserHandler();
            userHandler.SetBirtdDate(DateTime.Now);     // устанавливаем дату по умолчанию
        }
        private async void Continue_Clicked(object sender, EventArgs e)
        {
            NameEntry.Unfocus(); SurnameEntry.Unfocus(); PatronymicEntry.Unfocus();     // С помощью анфокуса принудительно заставляем сработать обработчик события

            if (userHandler.CheckCorrectness())     // проверяем корректность введенных данных
            {
                userHandler.CreateUserInfo();
                if (HasExperience.IsChecked)        // если пользователь указал, что у него имеется опыт...
                    await Navigation.PushModalAsync(new PrimaryUserExperiencePage(userHandler));       // переходим на страницу указания опыта
                else
                    await Navigation.PushModalAsync(new CreatePasswordPage(userHandler));      // ... или на страницу задания пароля
            }   
            else
                await DisplayAlert(Alerts.AlertMessage, Alerts.RegisterAlertMessage, AuxiliaryResources.ButtonOK);
        }

        private void SurnameEntry_Focused(object sender, FocusEventArgs e)
        {
            if (SurnameEntry.Text == UserDefault.UserSurname) SurnameEntry.Text = String.Empty;
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            if (NameEntry.Text == UserDefault.UserName) NameEntry.Text = String.Empty;
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text == UserDefault.UserPatronymic) PatronymicEntry.Text = String.Empty;
        }

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)         // если введено корректное значение, заносим данные в обертку
        {
            if (SurnameEntry.Text != String.Empty && SurnameEntry.Text != UserDefault.UserSurname)
                userHandler.AddSurnameToUser(SurnameEntry.Text);
            else
                SurnameEntry.Text = UserDefault.UserSurname;
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)        // если введено корректное значение, заносим данные в обертку
        {
            if (NameEntry.Text != String.Empty && NameEntry.Text != UserDefault.UserName)
                userHandler.AddNameToUser(NameEntry.Text);
            else
                NameEntry.Text = UserDefault.UserName;
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)     // если введено корректное значение, заносим данные в обертку
        {
            if (PatronymicEntry.Text != String.Empty && PatronymicEntry.Text != UserDefault.UserPatronymic)
                userHandler.AddPatronymicToUser(PatronymicEntry.Text);
            else
                PatronymicEntry.Text = UserDefault.UserPatronymic;
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)      // если один из переключателей был выбран...
                userHandler.SetGender((sender as RadioButton).Content.ToString());      // сохраняем данные и заносим в класс
            else
                return;

            foreach (RadioButton button in GenderFrame.Children)        // снимаем выделение с других переключателей на форме
                if (!button.Content.Equals((sender as RadioButton).Content)) button.IsChecked = false;
        }

        // при смене даты на форме заносим изменения в обертку
        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e) => userHandler.SetBirtdDate(BirthDate.Date); 
        
    }
}