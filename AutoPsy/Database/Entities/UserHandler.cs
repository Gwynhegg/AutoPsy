using AutoPsy.Resources;
using System;

namespace AutoPsy.Database.Entities
{
    public class UserHandler
    {
        private readonly User user;

        public UserHandler() => this.user = new User
        {
            PersonName = string.Empty,
            PersonSurname = string.Empty
        };

        public void AddNameToUser(string name)
        {
            if (name == null || name == string.Empty) throw new ArgumentException();
            this.user.PersonName = name;
        }

        public string GetUserName() => this.user.PersonName;

        public void AddSurnameToUser(string surname)
        {
            if (surname == null || surname == string.Empty) throw new ArgumentException();
            this.user.PersonSurname = surname;
        }

        public string GetUserSurname() => this.user.PersonSurname;

        public void AddPatronymicToUser(string patronymic)
        {
            if (patronymic == null || patronymic == UserDefault.UserPatronymic) patronymic = string.Empty;
            this.user.PersonPatronymic = patronymic;
        }

        public string GetUserPatronymic() => this.user.PersonPatronymic;

        public void SetGender(string gender) => this.user.Gender = gender;

        public string GetUserGender() => this.user.Gender;

        public void SetBirtdDate(DateTime date) => this.user.BirthDate = date;

        public DateTime GetUserBirthDate() => this.user.BirthDate;

        public void SetPassword(string password) => this.user.HashPassword = password;

        public User GetUser() => this.user;

        public bool CheckCorrectness()
        {
            if (this.user.Gender == null) this.user.Gender = UserDefault.UnknownSex;
            if (this.user.PersonName != string.Empty && this.user.PersonSurname != string.Empty &&
                this.user.PersonName != UserDefault.UserName &&
                this.user.PersonSurname != UserDefault.UserSurname)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateUserInfo() => App.Connector.CreateAndInsertData<User>(this.user);

        public void UpdateUserInfo() => App.Connector.UpdateData<User>(this.user);


        public void Clone(User otherUser)
        {
            this.user.PersonName = otherUser.PersonName.Clone().ToString();
            this.user.PersonSurname = otherUser.PersonSurname.Clone().ToString();
            if (otherUser.PersonPatronymic != null) this.user.PersonPatronymic = otherUser.PersonPatronymic.Clone().ToString();
            this.user.Id = otherUser.Id;
            this.user.BirthDate = otherUser.BirthDate;
            if (otherUser.Gender != null) this.user.Gender = otherUser.Gender.Clone().ToString();
            if (otherUser.HashPassword != null) this.user.HashPassword = otherUser.HashPassword.Clone().ToString();
        }
    }
}
