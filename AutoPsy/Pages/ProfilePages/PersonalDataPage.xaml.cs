using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class PersonalDataPage : ContentPage
    {
        private readonly UserHandler userHandler;      // класс-обертка для управления записями о пользователе
        private readonly User backupUser;      // на данной форме мы изменяем клона первоначальной записи чтобы избежать повреждения или замены данных
        private readonly ProfilePage parentPage;     // ссылка на родительскую страницу
        public PersonalDataPage(ProfilePage parent)
        {
            InitializeComponent();
            this.parentPage = parent;

            this.userHandler = new UserHandler();

            // Клонируем данные о текущем пользователе в хэндлер
            this.userHandler.Clone(App.Connector.SelectData<User>(App.Connector.currentConnectedUser));
            this.backupUser = this.userHandler.GetUser();     // достаем оттуда юзера

            SetCurrentData();       // актуализируем данные на форме
        }

        // Метод для синхронизации данных. В нем все поля устанавливаются в позиции, привязанные к текущему юзеру
        private void SetCurrentData()
        {
            this.SurnameEntry.Text = this.userHandler.GetUserSurname();
            this.NameEntry.Text = this.userHandler.GetUserName();

            if (this.userHandler.GetUserPatronymic() is null)
                this.PatronymicEntry.Text = UserDefault.UserPatronymic;
            else
                this.PatronymicEntry.Text = this.userHandler.GetUserPatronymic();

            this.BirthDate.Date = this.userHandler.GetUserBirthDate();

            if (this.userHandler.GetUserGender() != null)
            {
                foreach (RadioButton button in this.GenderFrame.Children)
                    if (button.Content.Equals(this.userHandler.GetUserGender())) button.IsChecked = true;
            }
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked)
            {
                this.userHandler.SetGender((sender as RadioButton).Content.ToString());
                foreach (RadioButton button in this.GenderFrame.Children)
                    if (!button.Content.Equals((sender as RadioButton).Content)) button.IsChecked = false;
            }
        }

        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e) => this.userHandler.SetBirtdDate(this.BirthDate.Date);

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (this.SurnameEntry.Text == string.Empty) this.SurnameEntry.Text = this.backupUser.PersonSurname;
            else this.userHandler.AddSurnameToUser(this.SurnameEntry.Text);
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (this.NameEntry.Text == string.Empty) this.NameEntry.Text = this.backupUser.PersonName;
            else this.userHandler.AddNameToUser(this.NameEntry.Text);
        }

        // Метод выхода и сохранения данных
        private async void SaveAndQuit_Clicked(object sender, EventArgs e)
        {
            // Активируем механизмы передачи данных
            this.SurnameEntry.Unfocus(); this.NameEntry.Unfocus(); this.PatronymicEntry.Unfocus();

            this.userHandler.UpdateUserInfo();       // обновляем информацию о пользователе

            this.parentPage.RefreshData();       // передаем измененные данные для отображения на родительской форме
            await this.Navigation.PopModalAsync();       // уходим на предыдущую форму
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.PatronymicEntry.Text == UserDefault.UserPatronymic) this.PatronymicEntry.Text = string.Empty;
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (this.PatronymicEntry.Text == string.Empty) this.PatronymicEntry.Text = this.backupUser.PersonPatronymic;
            else this.userHandler.AddPatronymicToUser(this.PatronymicEntry.Text);
        }
    }
}