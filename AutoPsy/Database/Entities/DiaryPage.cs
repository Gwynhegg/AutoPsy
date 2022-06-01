using SQLite;
using System;
using System.ComponentModel;

namespace AutoPsy.Database.Entities
{
    [Table("DiaryPages")]
    public class DiaryPage : INotifyPropertyChanged, ICloneable
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

        private DateTime dateOfRecord;
        [NotNull]
        public DateTime DateOfRecord
        {
            get => this.dateOfRecord;
            set { this.dateOfRecord = value; OnPropertyChanged(nameof(this.DateOfRecord)); }
        }


        private string topic;
        public string Topic
        {
            get => this.topic;
            set { this.topic = value; OnPropertyChanged(nameof(this.Topic)); }
        }

        private string mainText;
        [NotNull]
        public string MainText
        {
            get => this.mainText;
            set { this.mainText = value; OnPropertyChanged(nameof(this.MainText)); }
        }

        private string attachedSymptoms;
        public string AttachedSymptoms
        {
            get => this.attachedSymptoms;
            set { this.attachedSymptoms = value; OnPropertyChanged(nameof(this.AttachedSymptoms)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));

        public object Clone()
        {
            var page = new DiaryPage
            {
                Id = this.Id,
                userId = this.UserId,
                mainText = string.Copy(this.MainText)
            };
            if (this.Topic != null) page.Topic = string.Copy(this.Topic);
            page.DateOfRecord = this.DateOfRecord;
            if (this.AttachedSymptoms != null) page.AttachedSymptoms = string.Copy(this.AttachedSymptoms);
            return page;
        }
    }
}
