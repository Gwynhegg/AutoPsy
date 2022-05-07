using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Logic
{
    // Класс, содержащий методы вычисления линейной регрессии
    public static class LinearRegression
    {
        /// <summary>
        /// Метод вычисления скользящего среднего и последующего прогнозирования
        /// </summary>
        /// <param name="entries">Словарь типа (дата - значение), на основе которого происходит анализ</param>
        /// <param name="averageInterval">Значение среднего интервала между вхождениями, нужно для указания прогнозируемых интервалов</param>
        /// <param name="epochsValue">количество шагов, на которые мы будем строить прогноз</param>
        /// <returns></returns>
        public static Dictionary<DateTime, float> CreateMovingAverageRegression(Dictionary<DateTime, int> entries, double averageInterval, int epochsValue)
        {
            if (entries.Count < 3) return null;     // лучше всего метод скользящего среднего показал при выборке среднего от трех элементов
            var resultDict = new Dictionary<DateTime, float>();     // инициализируем результирующий словарь
            float[] values = new float[entries.Count + epochsValue];    // создаем расширенный вспомогательный массив
            for (int i = 0; i < entries.Count; i++)
                values[i] = (float)entries.ElementAt(i).Value;      // заполняем его значениями

            float[] movingAverage = new float[values.Length];       // создаем массив для вычисленных значений скользящего среднего значения
            for (int i = 1; i < entries.Count - 1; i++)
                movingAverage[i] = (values[i - 1] + values[i] + values[i + 1]) / 3;     // вычисляем среднее по трем элементам и помещаем в массив

            for (int i = entries.Count; i < values.Length; i++)     // на основе полученных средних вычисляем прогнозируемые значения
            {
                values[i] = movingAverage[i - 2] + 1 / 3 * (values[i - 1] - values[i - 2]);     // формула для вычисления = A(i-2) * 1/3 * (X(i-1) - X(i-2))
                movingAverage[i - 1] = (values[i - 2] + values[i - 1] + values[i]) / 3;     // считаем скользящее среднее с учетом новообразованного значения
            }

            for (int i = 1; i <= epochsValue; i++)      // записываем полученные значения в результирующий массив с указанием дат, полученных последовательным прибавлением среднего интервала
                resultDict.Add(entries.Last().Key.AddDays(i * averageInterval), values[i + entries.Count - 1]);

            return resultDict;      // вуаля
        }
    }
}
