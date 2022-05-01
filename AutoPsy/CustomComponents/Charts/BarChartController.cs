using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
namespace AutoPsy.CustomComponents.Charts
{
    public class BarChartController : ChartController
    {
        public BarChartController(List<float> values, List<string> labels) : base(values, labels)
        {
        }

        public override void AddValuesToChart(List<int> indexes)
        {
            var entries = new ChartEntry[indexes.Count];
            for (int i = 0; i < indexes.Count; i++)
                entries[i] = new ChartEntry(values[indexes[i]]) { Label = labels[indexes[i]], 
                    Color = colors[indexes[i]], 
                    ValueLabel = values[indexes[i]].ToString()};

            chart = new BarChart() { Entries = entries, LabelOrientation = Orientation.Horizontal , LabelTextSize = 20, ValueLabelOrientation = Orientation.Horizontal};
        }
    }
}
