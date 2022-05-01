using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutoPsy.CustomComponents.Charts
{
    public class ChartElementModel : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private double value;
        public double Value
        {
            get { return value; }
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
