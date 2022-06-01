using Microcharts;
using System.Collections.Generic;

namespace AutoPsy.CustomComponents.Charts
{
    public class DonutChartController : IChartController
    {
        protected List<ChartEntry> entries = new List<ChartEntry>();
        private Chart chart;        // элемент, представляющий собой итоговую диаграмму

        public DonutChartController(List<float> values)
        {
            for (var i = 0; i < values.Count; i++)
                if (values[i] != 0) this.entries.Add(new ChartEntry(values[i]) { Color = AuxServices.ColorPicker.GetRandomColor(), Label = i.ToString("F1") });
        }

        public override Chart GetChart()     // метод для получения готовой диаграммы
        {
            this.chart = new DonutChart() { Entries = entries, LabelTextSize = 40 };
            return this.chart;
        }
    }
}
