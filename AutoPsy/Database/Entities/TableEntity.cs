using SQLite;
using System;
using System.ComponentModel;

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
            get => this.id;
            set { this.id = value; OnPropertyChanged(nameof(this.Id)); }
        }
        private string type = Const.Constants.RECOMENDATIONS_TAG;
        [NotNull]
        public string Type
        {
            get => this.type;
            set { this.type = Const.Constants.RECOMENDATIONS_TAG; OnPropertyChanged(this.Type); }
        }
        private string idValue;
        [NotNull]
        public string IdValue
        {
            get => this.idValue;
            set { this.idValue = value; OnPropertyChanged(nameof(this.IdValue)); }
        }
        private string name;
        [NotNull]
        public string Name
        {
            get => this.name;
            set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
        }
        private byte value;
        [NotNull]
        public byte Value
        {
            get => this.value;
            set { this.value = value; OnPropertyChanged(nameof(this.Value)); }
        }

        private DateTime time;
        [NotNull]
        public DateTime Time
        {
            get => this.time;
            set { this.time = value; OnPropertyChanged(nameof(this.Time)); }
        }

        private byte importance;
        public byte Importance
        {
            get => this.importance;
            set { this.importance = value; OnPropertyChanged(nameof(this.Importance)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));

        public ITableEntity Clone(DateTime time)
        {
            var clone = new TableRecomendation() { Type = type, IdValue = idValue, Name = name, Time = time, Value = 0, Importance = importance };
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
            get => this.id;
            set { this.id = value; OnPropertyChanged(nameof(this.Id)); }
        }
        private string type = Const.Constants.CONDITIONS_TAG;
        [NotNull]
        public string Type
        {
            get => this.type;
            set { this.type = Const.Constants.CONDITIONS_TAG; OnPropertyChanged(this.Type); }
        }
        private string idValue;
        [NotNull]
        public string IdValue
        {
            get => this.idValue;
            set { this.idValue = value; OnPropertyChanged(nameof(this.IdValue)); }
        }
        private string name;
        [NotNull]
        public string Name
        {
            get => this.name;
            set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
        }
        private byte value;
        [NotNull]
        public byte Value
        {
            get => this.value;
            set { this.value = value; OnPropertyChanged(nameof(this.Value)); }
        }

        private DateTime time;
        [NotNull]
        public DateTime Time
        {
            get => this.time;
            set { this.time = value; OnPropertyChanged(nameof(this.Time)); }
        }

        private byte importance;
        public byte Importance
        {
            get => this.importance;
            set { this.importance = value; OnPropertyChanged(nameof(this.Importance)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));

        public ITableEntity Clone(DateTime time)
        {
            var clone = new TableCondition() { Type = type, IdValue = idValue, Name = name, Time = time, Value = 0, Importance = importance };
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
            get => this.id;
            set { this.id = value; OnPropertyChanged(nameof(this.Id)); }
        }
        private string type = Const.Constants.TRIGGERS_TAG;
        [NotNull]
        public string Type
        {
            get => this.type;
            set { this.type = Const.Constants.TRIGGERS_TAG; OnPropertyChanged(this.Type); }
        }
        private string idValue;
        [NotNull]
        public string IdValue
        {
            get => this.idValue;
            set { this.idValue = value; OnPropertyChanged(nameof(this.IdValue)); }
        }
        private string name;
        [NotNull]
        public string Name
        {
            get => this.name;
            set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
        }
        private byte value;
        [NotNull]
        public byte Value
        {
            get => this.value;
            set { this.value = value; OnPropertyChanged(nameof(this.Value)); }
        }

        private DateTime time;
        [NotNull]
        public DateTime Time
        {
            get => this.time;
            set { this.time = value; OnPropertyChanged(nameof(this.Time)); }
        }

        private byte importance;
        public byte Importance
        {
            get => this.importance;
            set { this.importance = value; OnPropertyChanged(nameof(this.Importance)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));

        public ITableEntity Clone(DateTime time)
        {
            var clone = new TableTrigger() { Type = type, IdValue = idValue, Name = name, Time = time, Value = 0, Importance = importance };
            return clone;
        }
    }
}
