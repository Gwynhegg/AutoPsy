using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class UserHandler
    {
        private User user;
        
        public UserHandler()
        {
            user = new User();
            user.PersonName = "";
            user.PersonSurname = "";
        }

        public void AddNameToUser(string name)
        {
            if (name == null || name == "") throw new ArgumentException();
            user.PersonName = name;
        }

        public void AddSurnameToUser(string surname)
        {
            if (surname == null || surname == "") throw new ArgumentException();
            user.PersonSurname = surname;
        }

        public void AddPatronymicToUser(string patronymic)
        {
            if (patronymic == null || patronymic == "Отчество") patronymic = "";
            user.PersonPatronymic = patronymic;
        }

        public void SetGender(string gender)
        {
            user.Gender = gender;
        }

        public void SetBirtdDate(DateTime date)
        {
            user.BirthDate = date;
        }

        public User GetUser()
        {
            return user;
        }

        public bool CheckCorrectness()
        {
            if (user.PersonName != "" && user.PersonSurname != "") return true; else return false;
        }

        public void CreateUserInfo()
        {
            App.Connector.CreateAndInsertData<User>(user);
        }
    }
}
