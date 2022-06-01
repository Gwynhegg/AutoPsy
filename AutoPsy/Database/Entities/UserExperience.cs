using AutoPsy.Resources;
using SQLite;
using System;
using System.ComponentModel;

namespace AutoPsy.Database.Entities
{
    [Table("UserExperience")]
    public class UserExperience : INotifyPropertyChanged, ICloneable
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => this.id;
            set { this.id = value; OnPropertyChanged(nameof(this.Id)); }
        }

        private int userId;
        [NotNull]
        public int UserId
        {
            get => this.userId;
            set { this.userId = value; OnPropertyChanged(nameof(this.UserId)); }
        }

        private string nameOfClinic = AuxiliaryResources.NotMentioned;
        public string NameOfClinic
        {
            get => this.nameOfClinic;
            set { this.nameOfClinic = value; OnPropertyChanged(nameof(this.NameOfClinic)); }
        }

        private string treatingDoctor = AuxiliaryResources.NotMentioned;
        public string TreatingDoctor
        {
            get => this.treatingDoctor;
            set { this.treatingDoctor = value; OnPropertyChanged(nameof(this.TreatingDoctor)); }
        }

        private DateTime appointment;
        [NotNull]
        public DateTime Appointment
        {
            get => this.appointment;
            set { this.appointment = value; OnPropertyChanged(nameof(this.Appointment)); }
        }

        private string diagnosis;
        [NotNull]
        public string Diagnosis
        {
            get => this.diagnosis;
            set { this.diagnosis = value; OnPropertyChanged(nameof(this.Diagnosis)); }
        }

        private string indexOfMedicine;
        public string IndexOfMedicine
        {
            get => this.indexOfMedicine;
            set { this.indexOfMedicine = value; OnPropertyChanged(nameof(this.IndexOfMedicine)); }
        }

        private int score;
        public int Score
        {
            get => this.score;
            set { this.score = value; OnPropertyChanged(nameof(this.Score)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public object Clone()
        {
            var clone = new UserExperience
            {
                id = this.id,
                appointment = this.appointment
            };
            if (this.NameOfClinic != null) clone.nameOfClinic = string.Copy(this.nameOfClinic);
            if (this.IndexOfMedicine != null) clone.indexOfMedicine = string.Copy(this.indexOfMedicine);
            if (this.TreatingDoctor != null) clone.treatingDoctor = string.Copy(this.treatingDoctor);
            clone.diagnosis = string.Copy(this.diagnosis);
            clone.score = this.score;
            clone.userId = this.userId;
            return clone;
        }
    }
}
