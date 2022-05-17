using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.CustomComponents.Charts
{
    public class StatLinearChartController
    {
        private List<float> values;
        private List<string> dates;
        private SkiaSharp.SKColor color;
        private Chart chart;        // элемент, представляющий собой итоговую диаграмму
        public StatLinearChartController(List<float> values, List<string> dates)       
        {
            this.values = values;
            this.dates = dates;
            color = AuxServices.ColorPicker.GetRandomColor();
        }

        public void CreateChart()
        {
            var entries = new List<ChartEntry>();       // инициализируем новый набор значений для диаграммы
            for (int i = 0; i < values.Count; i++)
                if (dates.Count < 10)
                    entries.Add(new ChartEntry(values[i]) { ValueLabel = values[i].ToString(), Label = dates[i], Color = color });
                else
                    entries.Add(new ChartEntry(values[i]) { Color = color});

            // Создаем новую линейную диаграмму с полученными значениями
            chart = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, LabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal };
        }

        public Chart GetChart()     // метод для получения готовой диаграммы
        {
            return chart;
        }
    }
}
