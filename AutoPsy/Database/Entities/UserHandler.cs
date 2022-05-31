using System;
using System.Collections.Generic;
using System.Text;
using AutoPsy.Resources;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class UserHandler
    {
        private User user;
        
        public UserHandler()
        {
            user = new User();
            user.PersonName = String.Empty;
            user.PersonSurname = String.Empty;
        }

        public void AddNameToUser(string name)
        {
            if (name == null || name == String.Empty) throw new ArgumentException();
            user.PersonName = name;
        }

        public string GetUserName()
        {
            return user.PersonName;
        }

        public void AddSurnameToUser(string surname)
        {
            if (surname == null || surname == String.Empty) throw new ArgumentException();
            user.PersonSurname = surname;
        }

        public string GetUserSurname()
        {
            return user.PersonSurname;
        }

        public void AddPatronymicToUser(string patronymic)
        {
            if (patronymic == null || patronymic == UserDefault.UserPatronymic) patronymic = String.Empty;
            user.PersonPatronymic = patronymic;
        }

        public string GetUserPatronymic()
        {
            return user.PersonPatronymic;
        }

        public void SetGender(string gender)
        {
            user.Gender = gender;
        }

        public string GetUserGender()
        {
            return user.Gender;
        }

        public void SetBirtdDate(DateTime date)
        {
            user.BirthDate = date;
        }

        public DateTime GetUserBirthDate()
        {
            return user.BirthDate;
        }

        public void SetPassword(string password)
        {
            user.HashPassword = password;
        }

        public User GetUser()
        {
            return user;
        }

        public void SetUser()
        {
            this.user = App.Connector.SelectData<User>(App.Connector.currentConnectedUser);
        }

        public bool CheckCorrectness()
        {
            if (user.Gender == null) user.Gender = UserDefault.UnknownSex;
            if (user.PersonName != String.Empty && user.PersonSurname != String.Empty && 
                user.PersonName != UserDefault.UserName &&
                user.PersonSurname != UserDefault.UserSurname) return true; else return false;
        }

        public void CreateUserInfo()
        {
            App.Connector.CreateAndInsertData<User>(user);
        }

        public void UpdateUserInfo()
        {
            App.Connector.UpdateData<User>(user);
        }


        public void Clone(User otherUser)
        {
            user.PersonName = otherUser.PersonName.Clone().ToString();
            user.PersonSurname = otherUser.PersonSurname.Clone().ToString();
            if (otherUser.PersonPatronymic != null) user.PersonPatronymic = otherUser.PersonPatronymic.Clone().ToString();
            user.Id = otherUser.Id;
            user.BirthDate = otherUser.BirthDate;
            if (otherUser.Gender != null) user.Gender = otherUser.Gender.Clone().ToString();
            if (otherUser.HashPassword != null) user.HashPassword = otherUser.HashPassword.Clone().ToString();
        }
    }
}
