using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PoolAnalysisPage : ContentPage     // Форма для заполнения пустых значений до начала анализа данных
    {
        private Dictionary<string, List<float>> entityValues = new Dictionary<string, List<float>>();       // Лист, содержащий значения переданных сущностей-ячеек
        private Dictionary<string, List<float>> calculatedValues = new Dictionary<string, List<float>>();       // Лист с вычисленными значениями
        private Dictionary<string, List<ITableEntity>> entities;        // Лист переданных на данную формулу сущностей
        private List<string> listOfDate = new List<string>();       // Лист со списком дат, нужный для итераций и заполнения
        private DateTime start, end;        // вспомогательные элементы начала и конца выборки
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
            foreach (var parameters in entities.Keys)       // для каждого из присутствующих параметров...
                if (!App.TableGraph.GetParameterType(parameters).Equals(Const.Constants.ENTITY_TRIGGER))      // если он не является триггером...
                    calculatedValues.Add(parameters, Logic.PoolModelling.CreatePoolModel(entityValues[parameters]));        // создаем пул-модель и помещаем в локальный список
                else
                    calculatedValues.Add(parameters, entityValues[parameters]);     // иначе ничего не делаем (пул-модель не применима к триггерам, так как ограничения в 0 и 1 не дают разгуляться)
        }

        private void DisplayParameterData()      // отображаем данные по параметрам
        {
            var tempList = new List<string>();      // создаем временный лист для анименований параметров
            foreach (var pair in entities)      // для каждого параметра из списка...
                tempList.Add(App.TableGraph.GetNameByIdString(pair.Key));       // получаем наименование параметра и кладем во временное хранилище
            ItemsCollection.ItemsSource = tempList;     // устанавливаем список в качестве источника коллекции
        }

        private void SetDatesRange()        // метод для создания временного интервала и заполняющих его значений
        {
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                var day = i.Day.ToString().Length < 2 ? String.Concat("0", i.Day) : i.Day.ToString();       // получаем строку для отображения дня
                var month = i.Month.ToString().Length < 2 ? String.Concat("0", i.Month) : i.Month.ToString();       // получаем строку для отображения месяца
                listOfDate.Add(String.Concat(day, ".", month));      // соединяем строки и помещаем в новый столбец
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)       // метод нажатия на кнопку "Сохранить"
        {   
            foreach (var pair in entities)      // для каждого параметра из списка сущностей...
            {
                var entityPattern = pair.Value.First();     // получаем шаблон по первому элементу из коллекции
                var iterator = 0;       // инициализируем итератор
            for (DateTime date = start; date <= end; date = date.AddDays(1))        // для каждой даты из интервала выборки...
                {
                    var entity = pair.Value.FirstOrDefault(x => DateTime.Compare(date, x.Time) == 0);       // пробуем получить сущность, попадающий в дату
                    if (entity == null)     // если она не найдена...
                    {
                        var clone = entityPattern.Clone(date);      // создаем клон по шаблону
                        clone.Value = (byte)calculatedValues[pair.Key][iterator++];     // помещаем вычисленное с помощб. пула значение
                        TableEntityHandler.UpdateEntityValue(clone);        // посылаем запрос на обновление
                    }
                    else
                    {
                        entity.Value = (byte)calculatedValues[pair.Key][iterator++];        // иначе оперируем прямо с найденным значением
                        TableEntityHandler.UpdateEntityValue(entity);       // посылаем запрос на обновление
                    }
                }
            }
            await Navigation.PushModalAsync(new AnalysisSelectionPage(calculatedValues, start, end));       // возвращаемся на предыдущую форму
        }

        private void ItemsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)       // при выборе элемента из коллекции срабатывает метод
        {
            var selectedParameter = App.TableGraph.GetIdStringByName(ItemsCollection.SelectedItem as string);       // получаем выбранный параметр
            
            // Создаем и отображаем график изначальных значений на форме
            CustomComponents.Charts.BarChartController chart = new CustomComponents.Charts.BarChartController(entityValues[selectedParameter], listOfDate);
            if (App.TableGraph.GetParameterType(selectedParameter).Equals(Const.Constants.ENTITY_TRIGGER)) chart.AddAllValuesToChart(false); else chart.AddAllValuesToChart(true);
            PastChart.Chart = chart.GetChart();
            
            // создаем и отображаем график вычисленных значений
            CustomComponents.Charts.BarChartController postChart = new CustomComponents.Charts.BarChartController(calculatedValues[selectedParameter], listOfDate);
            if (App.TableGraph.GetParameterType(selectedParameter).Equals(Const.Constants.ENTITY_TRIGGER)) postChart.AddAllValuesToChart(false); else postChart.AddAllValuesToChart(true);
            PostChart.Chart = postChart.GetChart();
        }
    }
}