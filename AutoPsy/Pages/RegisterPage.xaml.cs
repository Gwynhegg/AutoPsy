using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private readonly UserHandler userHandler;      // класс-обертка для управления классом пользователем на основе конструктора
        public RegisterPage()
        {
            InitializeComponent();
            this.userHandler = new UserHandler();
            this.userHandler.SetBirtdDate(DateTime.Now);     // устанавливаем дату по умолчанию
        }
        private async void Continue_Clicked(object sender, EventArgs e)
        {
            this.NameEntry.Unfocus(); this.SurnameEntry.Unfocus(); this.PatronymicEntry.Unfocus();     // С помощью анфокуса принудительно заставляем сработать обработчик события

            if (this.userHandler.CheckCorrectness())     // проверяем корректность введенных данных
            {
                this.userHandler.CreateUserInfo();
                if (this.HasExperience.IsChecked)        // если пользователь указал, что у него имеется опыт...
                    await this.Navigation.PushModalAsync(new PrimaryUserExperiencePage(this.userHandler));       // переходим на страницу указания опыта
                else
                    await this.Navigation.PushModalAsync(new CreatePasswordPage(this.userHandler));      // ... или на страницу задания пароля
            }
            else
            {
                await DisplayAlert(Alerts.AlertMessage, Alerts.RegisterAlertMessage, AuxiliaryResources.ButtonOK);
            }
        }

        private void SurnameEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.SurnameEntry.Text == UserDefault.UserSurname) this.SurnameEntry.Text = string.Empty;
        }

        private void NameEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.NameEntry.Text == UserDefault.UserName) this.NameEntry.Text = string.Empty;
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.PatronymicEntry.Text == UserDefault.UserPatronymic) this.PatronymicEntry.Text = string.Empty;
        }

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)         // если введено корректное значение, заносим данные в обертку
        {
            if (this.SurnameEntry.Text != string.Empty && this.SurnameEntry.Text != UserDefault.UserSurname)
                this.userHandler.AddSurnameToUser(this.SurnameEntry.Text);
            else
                this.SurnameEntry.Text = UserDefault.UserSurname;
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)        // если введено корректное значение, заносим данные в обертку
        {
            if (this.NameEntry.Text != string.Empty && this.NameEntry.Text != UserDefault.UserName)
                this.userHandler.AddNameToUser(this.NameEntry.Text);
            else
                this.NameEntry.Text = UserDefault.UserName;
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)     // если введено корректное значение, заносим данные в обертку
        {
            if (this.PatronymicEntry.Text != string.Empty && this.PatronymicEntry.Text != UserDefault.UserPatronymic)
                this.userHandler.AddPatronymicToUser(this.PatronymicEntry.Text);
            else
                this.PatronymicEntry.Text = UserDefault.UserPatronymic;
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked == true)      // если один из переключателей был выбран...
                this.userHandler.SetGender((sender as RadioButton).Content.ToString());      // сохраняем данные и заносим в класс
            else
                return;

            foreach (RadioButton button in this.GenderFrame.Children)        // снимаем выделение с других переключателей на форме
                if (!button.Content.Equals((sender as RadioButton).Content)) button.IsChecked = false;
        }

        // при смене даты на форме заносим изменения в обертку
        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e) => this.userHandler.SetBirtdDate(this.BirthDate.Date);

    }
}