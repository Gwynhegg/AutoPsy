using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.ComponentModel;

namespace AutoPsy.Database.Entities
{
    [Table("Medicine")]
    public class Medicine : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
        }

        private string nameOfMedicine;
        [NotNull]
        public string NameOfMedicine
        {
            get { return nameOfMedicine; }
            set { this.nameOfMedicine = value; OnPropertyChanged(nameof(NameOfMedicine)); }
        }

        private double dosage;
        [NotNull]
        public double Dosage
        {
            get { return dosage; }
            set { this.dosage = value; OnPropertyChanged(nameof(Dosage)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
