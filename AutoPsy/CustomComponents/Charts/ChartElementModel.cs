using System.ComponentModel;

namespace AutoPsy.CustomComponents.Charts
{
    public class ChartElementModel : INotifyPropertyChanged     // вспомогательный класс для динамического отображения элементов статистики
    {
        private string name;        // наименования элемента статистики
        public string Name
        {
            get => this.name;
            set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
        }

        private string value;       // значение определенного статистического показателя элемента
        public string Value
        {
            get => this.value;
            set { this.value = value; OnPropertyChanged(nameof(this.Value)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
