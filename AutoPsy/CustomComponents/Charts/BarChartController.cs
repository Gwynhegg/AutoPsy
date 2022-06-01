using Microcharts;
using SkiaSharp;
using System.Collections.Generic;

namespace AutoPsy.CustomComponents.Charts
{
    public class BarChartController
    {
        private readonly List<float> values;       // список используемых значений
        private readonly List<string> labels;      // список названий элементов для их отображения на диаграмме
        private readonly List<SKColor> colors;     // список привязанных к каждому из элементов цветов
        private Chart chart;
        // Класс, отвечающий за отображений столбчатых диаграмм
        public BarChartController(List<float> values, List<string> labels)
        {
            this.values = values;
            this.labels = labels;
            this.colors = new List<SKColor>();
            foreach (var label in labels)
                this.colors.Add(AuxServices.ColorPicker.GetRandomColor());       // для каждого элемента во вспомогательном классе создается индивидуальный цвет
        }

        /// <summary>
        /// Метод для отбора индексов тех величин, которые будут отображены на диаграмме
        /// </summary>
        /// <param name="indexes">Список индексов элементов для отображения</param>
        public void AddValuesToChart(List<int> indexes)
        {
            var entries = new ChartEntry[indexes.Count];        // создаем новый массив данных для диаграмм
            for (var i = 0; i < indexes.Count; i++)
            {
                entries[i] = new ChartEntry(this.values[indexes[i]])
                {
                    Label = this.labels[indexes[i]],       // заполняем его значениями в соответствии с индексами, задаем названия и цвета
                    Color = this.colors[indexes[i]],
                    ValueLabel = this.values[indexes[i]].ToString()
                };
            }

            // Создаем диаграмму для отображения
            this.chart = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, LabelTextSize = 20, ValueLabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal };
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
            var entries = new ChartEntry[this.labels.Count];
            for (var i = 0; i < this.labels.Count; i++)
                entries[i] = new ChartEntry(this.values[i]) { Color = colorScheme[(byte)this.values[i]] };

            this.chart = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 20, ValueLabelTextSize = 20, MaxValue = maxValue };

        }

        public Chart GetChart() => this.chart;
    }
}
