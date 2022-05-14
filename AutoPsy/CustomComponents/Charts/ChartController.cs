using System;
using System.Collections.Generic;
using System.Text;
using Microcharts;
using SkiaSharp;
using AutoPsy.AuxServices;

namespace AutoPsy.CustomComponents.Charts
{
    public abstract class ChartController       // класс, описывающий основное взаимодействие с диаграммами. Отображение диграмм будет представлено в его наследниках, поскольку задействовано два типа диаграмм
    {
        protected List<float> values;       // список используемых значений
        protected List<string> labels;      // список названий элементов для их отображения на диаграмме
        protected List<SKColor> colors;     // список привязанных к каждому из элементов цветов
        protected Chart chart;
        public ChartController(List<float> values, List<string> labels)     // в конструктор класса передаются значения элементов и их названия
        {
            this.values = values;
            this.labels = labels;
            colors = new List<SKColor>();
            foreach (var label in labels)
                colors.Add(ColorPicker.GetRandomColor());       // для каждого элемента во вспомогательном классе создается индивидуальный цвет
        }

        public abstract void AddValuesToChart(List<int> indexes);       // абстрактный класс для выделения индексов тех элементов, которые будут отображены

        public Chart GetChart()
        {
            return chart;
        }
    }
}
