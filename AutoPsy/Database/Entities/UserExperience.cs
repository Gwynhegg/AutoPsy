using System;
using System.Collections.Generic;
using System.ComponentModel;
using SQLite;
using System.Text;

namespace AutoPsy.Database.Entities
{
    [Table("UserExperience")]
    public class UserExperience : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private int userId;
        [NotNull]
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                this.userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private string nameOfClinic;
        public string NameOfClinic
        {
            get
            {
                return nameOfClinic;
            }
            set
            {
                this.NameOfClinic = value;
                OnPropertyChanged(nameof(NameOfClinic));
            }
        }

        private string treatingDoctor;
        public string TreatingDoctor
        {
            get
            {
                return treatingDoctor;
            }
            set
            {
                this.treatingDoctor = value;
                OnPropertyChanged(nameof(TreatingDoctor));
            }
        }

        private DateTime appointment;
        [NotNull]
        public DateTime Appointment
        {
            get
            {
                return appointment;
            }
            set
            {
                this.appointment = value;
                OnPropertyChanged(nameof(Appointment));
            }
        }

        private string diagnosis;
        [NotNull]
        public string Diagnosis
        {
            get
            {
                return diagnosis;
            }
            set
            {
                this.diagnosis = value;
                OnPropertyChanged(nameof(Diagnosis));
            }
        }

        private List<Medicine> listOfMedicine;
        public List<Medicine> ListOfMedicine
        {
            get
            {
                return listOfMedicine;
            }
            set
            {
                this.listOfMedicine = value;
                OnPropertyChanged(nameof(ListOfMedicine));
            }
        }

        private int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                this.score = value;
                OnPropertyChanged(nameof(Score));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}
