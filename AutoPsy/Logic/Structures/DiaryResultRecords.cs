using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Logic.Structures
{
    public class DiaryResultRecords
    {
        public string Node { get; set; }
        public int Count { get; private set; }
        public int MaxValue { get; private set; }
        public double AverageValue { get; private set; }
        public int MinInterval { get; private set; }
        public int MaxInterval { get; private set; }
        public double AverageInterval { get; private set; }

        public void Calculate(DateTime[] dates, int[] values)
        {
            Count = dates.Length;       // Установка показателя количества встреченных вхождений объекта

            if (dates.Length > 1)
            {
                MinInterval = (dates[1] - dates[0]).Days;     // Задаем опорные интервалы для сравнения
                MaxInterval = (dates[1] - dates[0]).Days;

                // Инициализируем все необходимые статистические переменные
                AverageInterval = 0;
                MaxValue = values[0];
                AverageValue = values[0];

                for (int i = 1; i < Count; i++)
                {
                    var currInterval = (dates[i] - dates[i - 1]).Days;
                    if (currInterval < MinInterval) MinInterval = currInterval;
                    if (currInterval > MaxInterval) MaxInterval = currInterval;
                    AverageInterval += currInterval;

                    if (values[i] > MaxValue) MaxValue = values[i];
                    AverageValue += values[i];
                }

                AverageInterval /= Count;
                AverageValue /= Count;

            }
            else
            {
                MinInterval = 0;
                MaxInterval = 0;
                AverageInterval = 0;
                MaxValue = values[0];
                AverageValue = values[0];
            }
        }
    }
}
