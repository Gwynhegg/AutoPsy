﻿using System;
using System.Collections.Generic;
using System.Text;

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
            switch (parameter)
            {
                case Const.Constants.STAT_COUNT: return Count; break;
                case Const.Constants.MAX_VALUE: return MaxValue; break;
                case Const.Constants.AVERAGE_VALUE: return AverageValue; break;
                case Const.Constants.MIN_INTERVAL: return MinInterval; break;
                case Const.Constants.MAX_INTERVAL: return MaxInterval; break;
                case Const.Constants.AVERAGE_INTERVAL: return AverageInterval; break;
                default: return 0; break;
            }
        }
        // Метод для высчитывания стат. величин
        public void Calculate(DateTime[] dates, int[] values)
        {
            dataEntries = new Dictionary<DateTime, int>();
            dataEntries.Add(dates[0], values[0]);
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
                    dataEntries.Add(dates[i], values[i]);

                    // Вычисление текущего интервала между записями и сравнение со связанными параметрами
                    var currInterval = (dates[i] - dates[i - 1]).Days;
                    if (currInterval < MinInterval) MinInterval = currInterval;
                    if (currInterval > MaxInterval) MaxInterval = currInterval;
                    AverageInterval += currInterval;

                    // Получение и сравнение текущего количества наложений со связанными параметрами
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