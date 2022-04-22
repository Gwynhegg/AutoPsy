using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.ComponentModel;

namespace AutoPsy.Database.Entities
{
    [Table("DiaryPages")]
    public class DiaryPage : INotifyPropertyChanged
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; OnPropertyChanged(nameof(Id)); }
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
    }
}
