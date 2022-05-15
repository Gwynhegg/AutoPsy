using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using System.Linq;
using SkiaSharp;

namespace AutoPsy.CustomComponents.Charts
{
    public class BarChartController
    {
        private List<float> values;       // список используемых значений
        private List<string> labels;      // список названий элементов для их отображения на диаграмме
        private List<SKColor> colors;     // список привязанных к каждому из элементов цветов
        private Chart chart;
        // Класс, отвечающий за отображений столбчатых диаграмм
        public BarChartController(List<float> values, List<string> labels)
        {
            this.values = values;
            this.labels = labels;
            colors = new List<SKColor>();
            foreach (var label in labels)
                colors.Add(AuxServices.ColorPicker.GetRandomColor());       // для каждого элемента во вспомогательном классе создается индивидуальный цвет
        }

        /// <summary>
        /// Метод для отбора индексов тех величин, которые будут отображены на диаграмме
        /// </summary>
        /// <param name="indexes">Список индексов элементов для отображения</param>
        public void AddValuesToChart(List<int> indexes)
        {
            var entries = new ChartEntry[indexes.Count];        // создаем новый массив данных для диаграмм
            for (int i = 0; i < indexes.Count; i++)
                entries[i] = new ChartEntry(values[indexes[i]]) { Label = labels[indexes[i]],       // заполняем его значениями в соответствии с индексами, задаем названия и цвета
                    Color = colors[indexes[i]], 
                    ValueLabel = values[indexes[i]].ToString()};

            // Создаем диаграмму для отображения
            chart = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal , LabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal};
        }

        public void AddAllValuesToChart(bool useNormalColorScheme)
        {
            if (useNormalColorScheme)
                CreateSchemeChart(AuxServices.ColorPicker.SKColorScheme, 5);
            else
                CreateSchemeChart(AuxServices.ColorPicker.SKColorCriticalScheme, 1);
        }

        private void CreateSchemeChart(Dictionary<byte, SKColor> colorScheme, float maxValue)
        {
            var entries = new ChartEntry[labels.Count];
            for (int i = 0; i < labels.Count; i++)
                    entries[i] = new ChartEntry(values[i]) { Color = colorScheme[(byte)values[i]] };
            chart = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, MaxValue = maxValue };

        }

        public Chart GetChart()
        {
            return chart;
        }
    }
}
