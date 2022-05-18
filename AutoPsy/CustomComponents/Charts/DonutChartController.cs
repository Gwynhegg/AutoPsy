using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.CustomComponents.Charts
{
    public class DonutChartController
    {
        private List<float> values;
        private List<string> labels;
        private List<SkiaSharp.SKColor> color = new List<SkiaSharp.SKColor>();
        private Chart chart;        // элемент, представляющий собой итоговую диаграмму

        public DonutChartController(List<float> values)
        {
            this.values = values;
            labels = new List<string>();
            for (int i = 0; i < values.Count; i++)
                labels.Add(i.ToString());
            foreach (var label in labels)
                color.Add(AuxServices.ColorPicker.GetRandomColor());
        }

        public void CreateChart()
        {
            var entries = new List<ChartEntry>();       // инициализируем новый набор значений для диаграммы
            for (int i = 0; i < values.Count; i++)
                if (labels.Count < 10)
                    entries.Add(new ChartEntry(values[i]) { Label = labels[i], Color = color[i] });
                else
                    entries.Add(new ChartEntry(values[i]) { Color = color[i] });

            // Создаем новую линейную диаграмму с полученными значениями
            chart = new DonutChart() { Entries = entries, LabelTextSize = 20 };
        }

        public Chart GetChart()     // метод для получения готовой диаграммы
        {
            return chart;
        }
    }
}
