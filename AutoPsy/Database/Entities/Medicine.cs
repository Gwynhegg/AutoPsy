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
            get => this.id;
            set { this.id = value; OnPropertyChanged(nameof(this.Id)); }
        }

        private string nameOfMedicine;
        [NotNull]
        public string NameOfMedicine
        {
            get => this.nameOfMedicine;
            set { this.nameOfMedicine = value; OnPropertyChanged(nameof(this.NameOfMedicine)); }
        }

        private double dosage;
        [NotNull]
        public double Dosage
        {
            get => this.dosage;
            set { this.dosage = value; OnPropertyChanged(nameof(this.Dosage)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
