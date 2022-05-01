using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutoPsy.CustomComponents.Charts
{
    public class ChartDateElementModel : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private Dictionary<DateTime, int> values;
        public Dictionary<DateTime, int> Values
        {
            get { return values; }
            set { this.values = value; OnPropertyChanged(nameof(Values)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
