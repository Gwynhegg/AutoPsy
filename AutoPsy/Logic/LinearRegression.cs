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


        public static List<float> CreateMovingAverageRegression(List<float> entries, int epochsValue)
        {
            if (entries.Count < 3) return null;
            var resultValues = new List<float>();
            float[] values = new float[entries.Count + epochsValue];
            for (int i = 0; i < entries.Count; i++)
                values[i] = entries[i];

            float[] movingAverage = new float[values.Length];       // создаем массив для вычисленных значений скользящего среднего значения
            for (int i = 1; i < entries.Count - 1; i++)
                movingAverage[i] = (values[i - 1] + values[i] + values[i + 1]) / 3;     // вычисляем среднее по трем элементам и помещаем в массив

            for (int i = entries.Count; i < values.Length; i++)     // на основе полученных средних вычисляем прогнозируемые значения
            {
                values[i] = movingAverage[i - 2] + 1 / 3 * (values[i - 1] - values[i - 2]);     // формула для вычисления = A(i-2) * 1/3 * (X(i-1) - X(i-2))
                movingAverage[i - 1] = (values[i - 2] + values[i - 1] + values[i]) / 3;     // считаем скользящее среднее с учетом новообразованного значения
            }

            for (int i = 1; i <= epochsValue; i++)      // записываем полученные значения в результирующий массив с указанием дат, полученных последовательным прибавлением среднего интервала
                resultValues.Add(values[i + entries.Count - 1]);

            return resultValues;
        }

        public static List<float> CreateHoltWintersRegression(List<float> entries, float alpha, float beta, float gamma, int epochsValue, bool isTrigger)     // Модель Холта-Винтерса
        {
            var result = new List<float>();
            var prognosedValues = new List<float>();

            float scalingFactor = 1.96f;
            var seasonIndex = GetSeasonIndex(entries);
            var trend = GetTrendValue(entries, seasonIndex);
            var seasonals = GetSeasonals(entries, seasonIndex);
            var smoothValues = new List<float>();
            var trendValues = new List<float>();
            var predictedDeviation = new List<float>();
            var upperBond = new List<float>();
            var lowerBond = new List<float>();
            var season = new List<float>();

            var smooth = entries[0];
            result.Add(entries[0]);
            smoothValues.Add(smooth);
            trendValues.Add(trend);
            season.Add(seasonals[0 % seasonIndex]);
            predictedDeviation.Add(0);
            upperBond.Add(result[0] + scalingFactor * predictedDeviation[0]);
            lowerBond.Add(result[0] - scalingFactor * predictedDeviation[0]);

            for (int i = 1; i < entries.Count; i++)
            {
                var lastSmooth = smooth;
                smooth = alpha * (entries[i] - seasonals[i % seasonIndex]) + (1 - alpha) * (smooth + trend);
                trend = beta * (smooth - lastSmooth) + (1 - beta) * trend;
                seasonals[i % seasonIndex] = gamma * (entries[i] - smooth) + (1 - gamma) * seasonals[i % seasonIndex];
                result.Add(smooth + trend + seasonals[i % seasonIndex]);
                predictedDeviation.Add(gamma * Math.Abs(entries[i] - result[i]) + (1 - gamma) * predictedDeviation.Last());

                upperBond.Add(result.Last() + scalingFactor * predictedDeviation.Last());
                lowerBond.Add(result.Last() - scalingFactor * predictedDeviation.Last());
            }

            for (int i = entries.Count; i < entries.Count + epochsValue; i++)
            {
                var m = i - entries.Count + 1;
                result.Add((smooth + m * trend) + seasonals[i % seasonIndex]);
                if (!isTrigger)
                    if (result.Last() > 5) prognosedValues.Add(5);
                    else if (result.Last() < 0) prognosedValues.Add(0);
                    else prognosedValues.Add(result.Last());
                else
                    if (result.Last() > 1) prognosedValues.Add(1);
                    else if (result.Last() < 0) prognosedValues.Add(0);
                    else prognosedValues.Add(result.Last());

                predictedDeviation.Add(predictedDeviation.Last() * 1.01f);
            }

            return prognosedValues;
        }

        private static Dictionary<int, float> GetSeasonals(List<float> entries, int seasonIndex)
        {
            var result = new Dictionary<int, float>();
            var averages = new List<float>();
            var numberOfSeasons = (int)(entries.Count / seasonIndex);

            for (int i = 0; i < numberOfSeasons; i++)
            {
                var sum = 0.0f;
                for (int j = seasonIndex * i; j < seasonIndex * i + seasonIndex; j += 1)
                    sum += entries[j];
                averages.Add(sum / seasonIndex);
            }

            for (int i = 0; i < seasonIndex; i++)
            {
                var sumOfValues = 0.0f;
                for (int j = 0; j < numberOfSeasons; j++)
                    sumOfValues += entries[seasonIndex * j + i] - averages[j];
                result.Add(i, sumOfValues / numberOfSeasons);
            }

            return result;
                
        }

        private static float GetTrendValue(List<float> entries, int seasonIndex)
        {
            var sum = 0.0f;
            for (var i = 0; i < entries.Count - seasonIndex; i++)
                if (i + seasonIndex < entries.Count) sum += (entries[i + seasonIndex] - entries[i]) / seasonIndex; else sum += entries[i] / seasonIndex;
            return sum / seasonIndex;
        }

        private static int GetSeasonIndex(List<float> entries)
        {
            int seasonIndex = entries.Count - 1;
            double? minSquares = null;
            for (int indexValue = seasonIndex; indexValue > 2; indexValue--)
            {
                var currentSquare = 0.0f;
                var averages = new List<float>();

                for (int elementIndex = 0; elementIndex < indexValue; elementIndex++)
                {
                    var selectedEntries = new List<float>();

                    for (int stepsIndex = elementIndex; stepsIndex < entries.Count; stepsIndex += indexValue)
                        selectedEntries.Add(entries[stepsIndex]);

                    float average = selectedEntries.Sum() / selectedEntries.Count;
                    var sumOfDiffs = 0.0f;

                    foreach (var entry in selectedEntries)
                        sumOfDiffs += (float)Math.Pow(average - entry, 2);

                    averages.Add(sumOfDiffs);
                }

                var globalAverage = averages.Sum() / averages.Count;
                minSquares = minSquares ?? globalAverage;

                if (globalAverage <= minSquares)
                {
                    minSquares = currentSquare;
                    seasonIndex = indexValue;
                }
            }

            return seasonIndex;
        }
    }
}
