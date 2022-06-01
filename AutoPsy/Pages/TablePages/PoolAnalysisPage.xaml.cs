using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoolAnalysisPage : ContentPage     // Форма для заполнения пустых значений до начала анализа данных
    {
        private readonly Dictionary<string, List<float>> entityValues = new Dictionary<string, List<float>>();       // Лист, содержащий значения переданных сущностей-ячеек
        private readonly Dictionary<string, List<float>> calculatedValues = new Dictionary<string, List<float>>();       // Лист с вычисленными значениями
        private readonly Dictionary<string, List<ITableEntity>> entities;        // Лист переданных на данную формулу сущностей
        private readonly List<string> listOfDate = new List<string>();       // Лист со списком дат, нужный для итераций и заполнения
        private readonly DateTime start, end;        // вспомогательные элементы начала и конца выборки
        public PoolAnalysisPage(Dictionary<string, List<ITableEntity>> entities, Dictionary<string, List<float>> entityValues, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.entities = entities;
            this.entityValues = entityValues;
            this.start = start;
            this.end = end;

            SetDatesRange();        // создаем список записей, представляющий собой итерацию по временному интервалу
            DisplayParameterData();      // отображаем коллекцию наименований переданных параметров
            CalculatePools();       // вычисляем "прудики" - объяснение см. в PoolModelling
        }

        private void CalculatePools()       // моделируем значения по временным интервалам
        {
            foreach (var parameters in this.entities.Keys)       // для каждого из присутствующих параметров...
            {
                if (!App.TableGraph.GetParameterType(parameters).Equals(Const.Constants.ENTITY_TRIGGER))      // если он не является триггером...
                    this.calculatedValues.Add(parameters, Logic.PoolModelling.CreatePoolModel(this.entityValues[parameters]));        // создаем пул-модель и помещаем в локальный список
                else
                    this.calculatedValues.Add(parameters, this.entityValues[parameters]);     // иначе ничего не делаем (пул-модель не применима к триггерам, так как ограничения в 0 и 1 не дают разгуляться)
            }
        }

        private void DisplayParameterData()      // отображаем данные по параметрам
        {
            var tempList = new List<string>();      // создаем временный лист для анименований параметров
            foreach (KeyValuePair<string, List<ITableEntity>> pair in this.entities)      // для каждого параметра из списка...
                tempList.Add(App.TableGraph.GetNameByIdString(pair.Key));       // получаем наименование параметра и кладем во временное хранилище
            this.ItemsCollection.ItemsSource = tempList;     // устанавливаем список в качестве источника коллекции
        }

        private void SetDatesRange()        // метод для создания временного интервала и заполняющих его значений
        {
            for (DateTime i = this.start.Date; i <= this.end.Date; i = i.AddDays(1))
            {
                var day = i.Day.ToString().Length < 2 ? string.Concat("0", i.Day) : i.Day.ToString();       // получаем строку для отображения дня
                var month = i.Month.ToString().Length < 2 ? string.Concat("0", i.Month) : i.Month.ToString();       // получаем строку для отображения месяца
                this.listOfDate.Add(string.Concat(day, ".", month));      // соединяем строки и помещаем в новый столбец
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)       // метод нажатия на кнопку "Сохранить"
        {
            foreach (KeyValuePair<string, List<ITableEntity>> pair in this.entities)      // для каждого параметра из списка сущностей...
            {
                ITableEntity entityPattern = pair.Value.First();     // получаем шаблон по первому элементу из коллекции
                var iterator = 0;       // инициализируем итератор
                for (DateTime date = this.start; date <= this.end; date = date.AddDays(1))        // для каждой даты из интервала выборки...
                {
                    ITableEntity entity = pair.Value.FirstOrDefault(x => DateTime.Compare(date, x.Time) == 0);       // пробуем получить сущность, попадающий в дату
                    if (entity == null)     // если она не найдена...
                    {
                        ITableEntity clone = entityPattern.Clone(date);      // создаем клон по шаблону
                        clone.Value = (byte)this.calculatedValues[pair.Key][iterator++];     // помещаем вычисленное с помощб. пула значение
                        TableEntityHandler.UpdateEntityValue(clone);        // посылаем запрос на обновление
                    }
                    else
                    {
                        entity.Value = (byte)this.calculatedValues[pair.Key][iterator++];        // иначе оперируем прямо с найденным значением
                        TableEntityHandler.UpdateEntityValue(entity);       // посылаем запрос на обновление
                    }
                }
            }
            await this.Navigation.PushModalAsync(new AnalysisSelectionPage(this.calculatedValues, this.start, this.end));       // возвращаемся на предыдущую форму
        }

        private void ItemsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)       // при выборе элемента из коллекции срабатывает метод
        {
            this.BeforeText.IsVisible = true; this.AfterText.IsVisible = true;

            var selectedParameter = App.TableGraph.GetIdStringByName(this.ItemsCollection.SelectedItem as string);       // получаем выбранный параметр

            // Создаем и отображаем график изначальных значений на форме
            var chart = new CustomComponents.Charts.BarChartController(this.entityValues[selectedParameter], this.listOfDate);
            if (App.TableGraph.GetParameterType(selectedParameter).Equals(Const.Constants.ENTITY_TRIGGER)) chart.AddAllValuesToChart(false); else chart.AddAllValuesToChart(true);
            this.PastChart.Chart = chart.GetChart();

            // создаем и отображаем график вычисленных значений
            var postChart = new CustomComponents.Charts.BarChartController(this.calculatedValues[selectedParameter], this.listOfDate);
            if (App.TableGraph.GetParameterType(selectedParameter).Equals(Const.Constants.ENTITY_TRIGGER)) postChart.AddAllValuesToChart(false); else postChart.AddAllValuesToChart(true);
            this.PostChart.Chart = postChart.GetChart();
        }
    }
}