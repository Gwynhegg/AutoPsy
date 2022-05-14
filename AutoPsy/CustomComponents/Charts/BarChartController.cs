using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using System.Linq;
namespace AutoPsy.CustomComponents.Charts
{
    public class BarChartController : ChartController
    {
        // Класс, отвечающий за отображений столбчатых диаграмм. Большая часть работы происходит в родителе, здесь - спецификация
        public BarChartController(List<float> values, List<string> labels) : base(values, labels)       // фиктивный конструктор
        {
        }

        /// <summary>
        /// Метод для отбора индексов тех величин, которые будут отображены на диаграмме
        /// </summary>
        /// <param name="indexes">Список индексов элементов для отображения</param>
        public override void AddValuesToChart(List<int> indexes)
        {
            var entries = new ChartEntry[indexes.Count];        // создаем новый массив данных для диаграмм
            for (int i = 0; i < indexes.Count; i++)
                entries[i] = new ChartEntry(values[indexes[i]]) { Label = labels[indexes[i]],       // заполняем его значениями в соответствии с индексами, задаем названия и цвета
                    Color = colors[indexes[i]], 
                    ValueLabel = values[indexes[i]].ToString()};

            // Создаем диаграмму для отображения
            chart = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal , LabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal};
        }
    }
}
