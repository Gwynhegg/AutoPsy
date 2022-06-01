using AutoPsy.Resources;
using System;
using System.Collections.Generic;

namespace AutoPsy.Logic.Structures
{
    public class DiaryResultRecords
    {
        public string Node { get; set; }        // Имя обрабатываемого узла
        public int Count { get; private set; }      // Количество вхождений узла на указанном интервале
        public int MaxValue { get; private set; }       // Максимальное количество наложений (вхождений). Актуально для проявлений и категорий
        public double AverageValue { get; private set; }        // Среднее количество наложений (вхождений)
        public int MinInterval { get; private set; }        // Минимальный интервал между записями. В случае, если запись одна - не учитывается
        public int MaxInterval { get; private set; }        // Максимальный интервал между записями. В случае, если запись одна - не учитывается
        public double AverageInterval { get; private set; }     // Средний интервал между записями. В случае, если запись одна - не учитывается
        public Dictionary<DateTime, int> dataEntries { get; private set; }      // Коллекция для хранения дат и количества вхождений (наложений)

        public double GetStatValue(string parameter)        // Акцессор для получения стат. результатов по параметру
        {
            if (parameter.Equals(Constants.STAT_COUNT)) return this.Count;
            if (parameter.Equals(Constants.MAX_VALUE)) return this.MaxValue;
            if (parameter.Equals(Constants.AVERAGE_VALUE)) return this.AverageValue;
            if (parameter.Equals(Constants.MIN_INTERVAL)) return this.MinInterval;
            if (parameter.Equals(Constants.MAX_INTERVAL)) return this.MaxInterval;
            if (parameter.Equals(Constants.AVERAGE_INTERVAL)) return this.AverageInterval;
            return 0;
        }
        // Метод для высчитывания стат. величин
        public void Calculate(DateTime[] dates, int[] values)
        {
            this.dataEntries = new Dictionary<DateTime, int>
            {
                { dates[0], values[0] }
            };
            this.Count = dates.Length;       // Установка показателя количества встреченных вхождений объекта

            if (dates.Length > 1)
            {
                this.MinInterval = (dates[1] - dates[0]).Days;     // Задаем опорные интервалы для сравнения
                this.MaxInterval = (dates[1] - dates[0]).Days;

                // Инициализируем все необходимые статистические переменные
                this.AverageInterval = 0;
                this.MaxValue = values[0];
                this.AverageValue = values[0];

                for (var i = 1; i < this.Count; i++)
                {
                    if (!this.dataEntries.ContainsKey(dates[i])) this.dataEntries.Add(dates[i], values[i]); else this.dataEntries[dates[i]] += values[i];

                    // Вычисление текущего интервала между записями и сравнение со связанными параметрами
                    var currInterval = (dates[i] - dates[i - 1]).Days;
                    if (currInterval < this.MinInterval) this.MinInterval = currInterval;
                    if (currInterval > this.MaxInterval) this.MaxInterval = currInterval;
                    this.AverageInterval += currInterval;

                    // Получение и сравнение текущего количества наложений со связанными параметрами
                    if (values[i] > this.MaxValue) this.MaxValue = values[i];
                    this.AverageValue += values[i];
                }

                this.AverageInterval /= this.Count;
                this.AverageValue /= this.Count;

            }
            else
            {
                this.MinInterval = 0;
                this.MaxInterval = 0;
                this.AverageInterval = 0;
                this.MaxValue = values[0];
                this.AverageValue = values[0];
            }
        }
    }
}
