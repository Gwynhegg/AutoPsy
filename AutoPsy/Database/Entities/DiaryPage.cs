using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
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
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
        }

        private int userId;
        [NotNull]
        public int UserId
        {
            get { return userId; }
            set { this.userId = value; OnPropertyChanged(nameof(UserId)); }
        }

        private DateTime dateOfRecord;
        [NotNull]
        public DateTime DateOfRecord
        {
            get { return dateOfRecord;}
            set { this.dateOfRecord = value; OnPropertyChanged(nameof(DateOfRecord)); }
        }
        

        private string topic;
        public string Topic
        {
            get { return topic; }
            set { topic = value; OnPropertyChanged(nameof(Topic)); }
        }

        private string mainText;
        [NotNull]
        public string MainText
        {
            get { return mainText; }
            set { mainText = value; OnPropertyChanged(nameof(MainText)); }
        }

        private string attachedSymptoms;
        public string AttachedSymptoms
        {
            get { return attachedSymptoms; }
            set { attachedSymptoms = value; OnPropertyChanged(nameof(AttachedSymptoms)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            var page = new DiaryPage();
            page.Id = this.Id;
            page.userId = this.UserId;
            page.mainText = String.Copy(this.MainText);
            if (this.Topic != null) page.Topic = String.Copy(this.Topic);
            page.DateOfRecord = this.DateOfRecord;
            if (this.AttachedSymptoms != null) page.AttachedSymptoms = String.Copy(this.AttachedSymptoms);
            return page;
        }
    }
}
