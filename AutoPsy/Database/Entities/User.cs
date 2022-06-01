using SQLite;
using System;
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
            get => this.id;
            set { this.id = value; OnPropertyChanged(nameof(this.Id)); }
        }

        private string personName;
        [NotNull]
        public string PersonName
        {
            get => this.personName;
            set { this.personName = value; OnPropertyChanged(nameof(this.PersonName)); }
        }

        private string personSurname;
        [NotNull]
        public string PersonSurname
        {
            get => this.personSurname;
            set { this.personSurname = value; OnPropertyChanged(nameof(this.PersonSurname)); }
        }

        private string personPatronymic;
        public string PersonPatronymic
        {
            get => this.personPatronymic;
            set { this.personPatronymic = value; OnPropertyChanged(nameof(this.PersonPatronymic)); }
        }

        private DateTime birthDate;
        [NotNull]
        public DateTime BirthDate
        {
            get => this.birthDate;
            set { this.birthDate = value; OnPropertyChanged(nameof(this.BirthDate)); }
        }

        private string gender;
        public string Gender
        {
            get => this.gender;
            set { this.gender = value; OnPropertyChanged(nameof(this.Gender)); }
        }

        private string hashPassword;
        public string HashPassword
        {
            get => this.hashPassword;
            set { this.hashPassword = value; OnPropertyChanged(nameof(this.HashPassword)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
