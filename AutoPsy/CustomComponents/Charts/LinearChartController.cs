using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using System.Linq;
using SkiaSharp;

namespace AutoPsy.CustomComponents.Charts
{
    public class LinearChartController
    {
        private List<Dictionary<DateTime, int>> values;
        private List<string> labels;
        private List<SKColor> colors;
        private Chart chart;
        public LinearChartController(List<Dictionary<DateTime, int>> values, List<string> labels)
        {
            this.values = values;
            this.labels = labels;
            colors = new List<SKColor>();
            foreach (var label in labels)
                colors.Add(AuxServices.ColorPicker.GetRandomColor());
        }

        public void AddValuesToChart(int index, double averageInterval)
        {
            var entries = new List<ChartEntry>();
            var choosedItem = values[index];
            foreach (var pair in choosedItem)
                entries.Add(new ChartEntry(pair.Value) { Color = colors[index], Label = pair.Key.ToShortDateString(), ValueLabel = pair.Value.ToString() });

            var regression = Logic.LinearRegression.CreateMovingAverageRegression(values[index], averageInterval, 2);
            if (regression != null)
                foreach (var item in regression)
                    entries.Add(new ChartEntry(item.Value) { Color = SKColor.Parse("#777777"), Label = item.Key.ToShortDateString(), ValueLabel = item.Value.ToString() });

            chart = new LineChart() { Entries = entries, LabelOrientation = Orientation.Horizontal, LabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal  };
        }

        public Chart GetChart()
        {
            return chart;
        } 
    }
}
