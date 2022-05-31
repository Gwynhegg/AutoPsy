using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.CustomComponents.Charts
{
    public class StatLinearChartController : LinearChartController
    {
        public StatLinearChartController() { }
        public StatLinearChartController(List<float> values)
        {
            foreach(var value in values)
                entries.Add(new ChartEntry(value) { Color = color, Label = value.ToString("F1")});
        }
    }
}
