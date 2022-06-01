using Microcharts;
using System.Collections.Generic;

namespace AutoPsy.CustomComponents.Charts
{
    public class StatLinearChartController : LinearChartController
    {
        public StatLinearChartController() { }
        public StatLinearChartController(List<float> values)
        {
            foreach (var value in values)
                this.entries.Add(new ChartEntry(value) { Color = color, Label = value.ToString("F1") });
        }
    }
}
