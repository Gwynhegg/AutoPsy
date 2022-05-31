using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AutoPsy.CustomComponents.Charts
{
    public class ChartElementModel : INotifyPropertyChanged     // вспомогательный класс для динамического отображения элементов статистики
    {
        private string name;        // наименования элемента статистики
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string value;       // значение определенного статистического показателя элемента
        public string Value
        {
            get { return value; }
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
