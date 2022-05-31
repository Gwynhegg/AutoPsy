using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using System.Linq;
using SkiaSharp;

namespace AutoPsy.CustomComponents.Charts
{
    public class LinearChartController : IChartController     // класс-контроллер для построения линейных диаграмм
    {

        public void AddValuesToChart(List<float> values, List<string> labels) 
        {
            for (int i = 0; i < values.Count; i++)
                entries.Add(new ChartEntry(values[i]) { Color = color, Label = labels[i], ValueLabel = values[i].ToString("F1") });
        }

        public void AddValuesToChart(List<float> values, DateTime start, DateTime end)
        {
            var labels = CreateDateIntervals(start, end);
            for (int i = 0; i < values.Count; i++)
                entries.Add(new ChartEntry(values[i]) { Color = color, Label = labels[i], ValueLabel = values[i].ToString("F1") });
        }

        public override Chart GetChart()     // метод для получения готовой диаграммы
        {
            var textSize = (float)Math.Round((double)200 / entries.Count());
            var valueTextSize = textSize / 1.2f;
            chart = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, ValueLabelTextSize = valueTextSize, LabelTextSize = textSize };
            return chart;
        } 
    }
}
