using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using SkiaSharp;
using AutoPsy.AuxServices;

namespace AutoPsy.CustomComponents.Charts
{
    public abstract class ChartController
    {
        protected List<float> values;
        protected List<string> labels;
        protected List<SKColor> colors;
        protected Chart chart;
        public ChartController(List<float> values, List<string> labels)
        {
            this.values = values;
            this.labels = labels;
            colors = new List<SKColor>();
            foreach (var label in labels)
                colors.Add(ColorPicker.GetRandomColor());
        }

        public abstract void AddValuesToChart(List<int> indexes);

        public Chart GetChart()
        {
            return chart;
        }
    }
}
