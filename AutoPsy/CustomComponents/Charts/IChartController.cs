using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;

namespace AutoPsy.CustomComponents.Charts
{
    public abstract class IChartController
    {
        protected List<ChartEntry> entries = new List<ChartEntry>();
        protected SKColor color = AuxServices.ColorPicker.GetRandomColor();       // набор цветов для дифференциации элементов статистики
        protected Chart chart;        // элемент, представляющий собой итоговую диаграмму
        public void ChangeColor() => this.color = SKColor.Parse("#000000");
        protected List<string> CreateDateIntervals(DateTime start, DateTime end)
        {
            var labels = new List<string>();
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                var day = i.Day.ToString().Length < 2 ? string.Concat("0", i.Day) : i.Day.ToString();       // получаем строку для отображения дня
                var month = i.Month.ToString().Length < 2 ? string.Concat("0", i.Month) : i.Month.ToString();       // получаем строку для отображения месяца
                labels.Add(string.Concat(day, ".", month));      // соединяем строки и помещаем в новый столбец
            }
            return labels;
        }

        public abstract Chart GetChart();
    }
}
