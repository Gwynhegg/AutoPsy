using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.ComponentModel;

namespace AutoPsy.Database.Entities
{
    [Table("Users")]
    public class User : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string personName;
        [NotNull]
        public string PersonName
        {
            get { return personName; }
            set { this.personName = value; OnPropertyChanged(nameof(PersonName)); }
        }

        private string personSurname;
        [NotNull]
        public string PersonSurname
        {
            get
            { return personSurname; }
            set { this.personSurname = value; OnPropertyChanged(nameof(PersonSurname)); }
        }

        private string personPatronymic;
        public string PersonPatronymic
        {
            get { return personPatronymic; }
            set { this.personPatronymic = value; OnPropertyChanged(nameof(PersonPatronymic)); }
        }

        private DateTime birthDate;
        [NotNull]
        public DateTime BirthDate
        {
            get { return birthDate; }
            set { this.birthDate = value; OnPropertyChanged(nameof(BirthDate)); }
        }

        private string gender;
        public string Gender
        {
            get { return gender; }
            set { this.gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        private string hashPassword;
        public string HashPassword
        {
            get { return hashPassword; }
            set { this.hashPassword = value; OnPropertyChanged(nameof(HashPassword)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
