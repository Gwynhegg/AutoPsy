using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutoPsy.Database.Entities
{
    public interface ITableEntity
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }
        [NotNull]
        string Type { get; set; }
        [NotNull]
        string IdValue { get; set; }
        [NotNull]
        string Name { get; set; }
        [NotNull]
        byte Value { get; set; }
        [NotNull]
        DateTime Time { get; set; }
        [NotNull]
        byte Importance { get; set; }
        ITableEntity Clone(DateTime time);
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

        public ITableEntity Clone(DateTime time)
        {
            var clone = new TableRecomendation() { Type = type, IdValue = idValue, Name = name, Time = time, Value = value, Importance = importance };
            return clone;
        }
    }

    [Table("TableConditions")]
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

        public ITableEntity Clone(DateTime time)
        {
            var clone = new TableCondition() { Type = type, IdValue = idValue, Name = name, Time = time, Value = value, Importance = importance };
            return clone;
        }
    }

    [Table("TableTriggers")]
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

        public ITableEntity Clone(DateTime time)
        {
            var clone = new TableTrigger() { Type = type, IdValue = idValue, Name = name, Time = time, Value = value, Importance = importance };
            return clone;
        }
    }
}
