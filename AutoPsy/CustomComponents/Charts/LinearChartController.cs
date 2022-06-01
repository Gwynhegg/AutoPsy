using Microcharts;
using System;
using System.Collections.Generic;

namespace AutoPsy.CustomComponents.Charts
{
    public class LinearChartController : IChartController     // класс-контроллер для построения линейных диаграмм
    {

        public void AddValuesToChart(List<float> values, List<string> labels)
        {
            for (var i = 0; i < values.Count; i++)
                this.entries.Add(new ChartEntry(values[i]) { Color = color, Label = labels[i], ValueLabel = values[i].ToString("F1") });
        }

        public void AddValuesToChart(List<float> values, DateTime start, DateTime end)
        {
            List<string> labels = CreateDateIntervals(start, end);
            for (var i = 0; i < values.Count; i++)
                this.entries.Add(new ChartEntry(values[i]) { Color = color, Label = labels[i], ValueLabel = values[i].ToString("F1") });
        }

        public override Chart GetChart()     // метод для получения готовой диаграммы
        {
            this.chart = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, ValueLabelTextSize = 20, LabelTextSize = 20 };
            return this.chart;
        }
    }
}
