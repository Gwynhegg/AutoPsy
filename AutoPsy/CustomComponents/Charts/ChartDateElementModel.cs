using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutoPsy.CustomComponents.Charts
{
    public class ChartDateElementModel : INotifyPropertyChanged     // вспомогательный класс для отображения динамического списка элементов статистики
    {
        private string name;        // названия элементов статистики
        public string Name
        {
            get => this.name;
            set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
        }

        private Dictionary<DateTime, int> values;       // статистические виличины, определенные по датам
        public Dictionary<DateTime, int> Values
        {
            get => this.values;
            set { this.values = value; OnPropertyChanged(nameof(this.Values)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
