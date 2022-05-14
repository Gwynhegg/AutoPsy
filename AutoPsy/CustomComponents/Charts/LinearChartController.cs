using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using System.Linq;
using SkiaSharp;

namespace AutoPsy.CustomComponents.Charts
{
    public class LinearChartController      // класс-контроллер для построения линейных диаграмм
    {
        private List<Dictionary<DateTime, int>> values;     // сгруппированные по датам значения статистических величин
        private List<string> labels;        // набор наименований элементов статистики
        private List<SKColor> colors;       // набор цветов для дифференциации элементов статистики
        private Chart chart;        // элемент, представляющий собой итоговую диаграмму
        public LinearChartController(List<Dictionary<DateTime, int>> values, List<string> labels)       // конструктор, принимающий значения стат. величин и их наименования
        {
            this.values = values;
            this.labels = labels;
            colors = new List<SKColor>();
            foreach (var label in labels)
                colors.Add(AuxServices.ColorPicker.GetRandomColor());       // каждому из элементов определяем индивидуальный цвет посредством вспомогательной библиотеки
        }

        /// <summary>
        /// Метод для задания массива значений, на основании которых будет построена диаграмма
        /// </summary>
        /// <param name="index">Индекс элемента во внутреннем хранилище класса</param>
        /// <param name="averageInterval">Средний интервал между записями - нужен для прогнозирования значений</param>
        public void AddValuesToChart(int index, double averageInterval)         //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!ДОПИЛИТЬ ВЫБОР ИТЕРАЦИЙ!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            var entries = new List<ChartEntry>();       // инициализируем новый набор значений для диаграммы
            var choosedItem = values[index];        // определяем выбранный элемент
            foreach (var pair in choosedItem)       // для каждой пары значений создаем вхождение в набор значений диаграммы
                entries.Add(new ChartEntry(pair.Value) { Color = colors[index], Label = pair.Key.ToShortDateString(), ValueLabel = pair.Value.ToString() });

            // Инициируем процедуру вычисления регрессии методом скользящего среднего
            var regression = Logic.LinearRegression.CreateMovingAverageRegression(values[index], averageInterval, 2);   
            if (regression != null)
                foreach (var item in regression)        // каждое значение "предсказания" добавляем в коллекцию значений диаграммы, но с другим цветов (для дифференциации)
                    entries.Add(new ChartEntry(item.Value) { Color = SKColor.Parse("#777777"), Label = item.Key.ToShortDateString(), ValueLabel = item.Value.ToString() });

            // Создаем новую линейную диаграмму с полученными значениями
            chart = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, LabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal  };
        }

        public Chart GetChart()     // метод для получения готовой диаграммы
        {
            return chart;
        } 
    }
}
