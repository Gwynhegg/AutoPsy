using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.ProfilePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class PersonalDataPage : ContentPage
    {
        private Database.Entities.UserHandler userHandler;      // класс-обертка для управления записями о пользователе
        private Database.Entities.User backupUser;      // на данной форме мы изменяем клона первоначальной записи чтобы избежать повреждения или замены данных
        private ProfilePage parentPage;     // ссылка на родительскую страницу
        public PersonalDataPage(ProfilePage parent)
        {
            InitializeComponent();
            parentPage = parent;

            userHandler = new Database.Entities.UserHandler();

            // Клонируем данные о текущем пользователе в хэндлер
            userHandler.Clone(App.Connector.SelectData<Database.Entities.User>(App.Connector.currentConnectedUser));
            backupUser = userHandler.GetUser();     // достаем оттуда юзера

            SetCurrentData();       // актуализируем данные на форме
        }

        // Метод для синхронизации данных. В нем все поля устанавливаются в позиции, привязанные к текущему юзеру
        private void SetCurrentData()
        {
            SurnameEntry.Text = userHandler.GetUserSurname();
            NameEntry.Text = userHandler.GetUserName();

            if (userHandler.GetUserPatronymic() is null) 
                PatronymicEntry.Text = AutoPsy.Resources.UserDefault.UserPatronymic; 
            else 
                PatronymicEntry.Text = userHandler.GetUserPatronymic();

            BirthDate.Date = userHandler.GetUserBirthDate();

            if (userHandler.GetUserGender() != null)
                foreach (RadioButton button in GenderFrame.Children)
                    if (button.Content.Equals(userHandler.GetUserGender())) button.IsChecked = true;
        }

        private void Option_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if ((sender as RadioButton).IsChecked)
            {
                userHandler.SetGender((sender as RadioButton).Content.ToString());
                foreach (RadioButton button in GenderFrame.Children)
                    if (!button.Content.Equals((sender as RadioButton).Content)) button.IsChecked = false;
            }                
        }

        private void BirthDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            userHandler.SetBirtdDate(BirthDate.Date);
        }

        private void SurnameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (SurnameEntry.Text == String.Empty) SurnameEntry.Text = backupUser.PersonSurname;
            else userHandler.AddSurnameToUser(SurnameEntry.Text);
        }

        private void NameEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (NameEntry.Text == String.Empty) NameEntry.Text = backupUser.PersonName;
            else userHandler.AddNameToUser(NameEntry.Text);
        }

        // Метод выхода и сохранения данных
        private async void SaveAndQuit_Clicked(object sender, EventArgs e)
        {
            // Активируем механизмы передачи данных
            SurnameEntry.Unfocus(); NameEntry.Unfocus(); PatronymicEntry.Unfocus();

            userHandler.UpdateUserInfo();       // обновляем информацию о пользователе

            parentPage.RefreshData();       // передаем измененные данные для отображения на родительской форме
            await Navigation.PopModalAsync();       // уходим на предыдущую форму
        }

        private void PatronymicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text == AutoPsy.Resources.UserDefault.UserPatronymic) PatronymicEntry.Text = String.Empty;
        }

        private void PatronymicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (PatronymicEntry.Text == String.Empty) PatronymicEntry.Text = backupUser.PersonPatronymic;
            else userHandler.AddPatronymicToUser(PatronymicEntry.Text);
        }
    }
}