using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutoPsy.Database.Entities
{
    public interface ITableEntity
    {
        int Id { get; set; }
        string Type { get; set; }
        string IdValue { get; set; }
        string Name { get; set; }
        byte Value { get; set; }
        DateTime Time { get; set; }
        byte Importance { get; set; }
    }

    [Table("Recomendations")]
    public class TableRecomendation : ITableEntity
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
        }
        private string type = Const.Constants.RECOMENDATIONS_TAG;
        [NotNull]
        public string Type
        {
            get { return type; }
            set { this.type = Const.Constants.RECOMENDATIONS_TAG; OnPropertyChanged(Type); }
        }
        private string idValue;
        [NotNull]
        public string IdValue
        {
            get { return idValue; }
            set { idValue = value; OnPropertyChanged(nameof(IdValue)); }
        }
        private string name;
        [NotNull]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private byte value;
        [NotNull]
        public byte Value
        {
            get { return value; }
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }

        private DateTime time;
        [NotNull]
        public DateTime Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged(nameof(Time)); }
        }

        private byte importance;
        [NotNull]
        public byte Importance
        {
            get { return importance; }
            set { importance = value; OnPropertyChanged(nameof(Importance)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TableCondition : ITableEntity
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
        }
        private string type = Const.Constants.CONDITIONS_TAG;
        [NotNull]
        public string Type
        {
            get { return type; }
            set { this.type = Const.Constants.CONDITIONS_TAG; OnPropertyChanged(Type); }
        }
        private string idValue;
        [NotNull]
        public string IdValue
        {
            get { return idValue; }
            set { idValue = value; OnPropertyChanged(nameof(IdValue)); }
        }
        private string name;
        [NotNull]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private byte value;
        [NotNull]
        public byte Value
        {
            get { return value; }
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }

        private DateTime time;
        [NotNull]
        public DateTime Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged(nameof(Time)); }
        }

        private byte importance;
        [NotNull]
        public byte Importance
        {
            get { return importance; }
            set { importance = value; OnPropertyChanged(nameof(Importance)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TableTrigger : ITableEntity
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
        }
        private string type = Const.Constants.TRIGGERS_TAG;
        [NotNull]
        public string Type
        {
            get { return type; }
            set { this.type = Const.Constants.TRIGGERS_TAG; OnPropertyChanged(Type); }
        }
        private string idValue;
        [NotNull]
        public string IdValue
        {
            get { return idValue; }
            set { idValue = value; OnPropertyChanged(nameof(IdValue)); }
        }
        private string name;
        [NotNull]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private byte value;
        [NotNull]
        public byte Value
        {
            get { return value; }
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }

        private DateTime time;
        [NotNull]
        public DateTime Time
        {
            get { return time; }
            set { time = value; OnPropertyChanged(nameof(Time)); }
        }

        private byte importance;
        [NotNull]
        public byte Importance
        {
            get { return importance; }
            set { importance = value; OnPropertyChanged(nameof(Importance)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}
