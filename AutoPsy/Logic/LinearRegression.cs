using System;
using System.Collections.Generic;
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
            var values = new float[entries.Count + epochsValue];    // создаем расширенный вспомогательный массив
            for (var i = 0; i < entries.Count; i++)
                values[i] = entries.ElementAt(i).Value;      // заполняем его значениями

            var movingAverage = new float[values.Length];       // создаем массив для вычисленных значений скользящего среднего значения
            for (var i = 1; i < entries.Count - 1; i++)
                movingAverage[i] = (values[i - 1] + values[i] + values[i + 1]) / 3;     // вычисляем среднее по трем элементам и помещаем в массив

            for (var i = entries.Count; i < values.Length; i++)     // на основе полученных средних вычисляем прогнозируемые значения
            {
                values[i] = movingAverage[i - 2] + 1 / 3 * (values[i - 1] - values[i - 2]);     // формула для вычисления = A(i-2) * 1/3 * (X(i-1) - X(i-2))
                movingAverage[i - 1] = (values[i - 2] + values[i - 1] + values[i]) / 3;     // считаем скользящее среднее с учетом новообразованного значения
            }

            for (var i = 1; i <= epochsValue; i++)      // записываем полученные значения в результирующий массив с указанием дат, полученных последовательным прибавлением среднего интервала
                resultDict.Add(entries.Last().Key.AddDays(i * averageInterval), values[i + entries.Count - 1]);

            return resultDict;      // вуаля
        }

        public static List<float> CreateHoltWintersRegression(List<float> entries, float alpha, float beta, float gamma, int epochsValue, bool isTrigger)     // Модель Холта-Винтерса
        {
            var result = new List<float>();
            var prognosedValues = new List<float>();

            var scalingFactor = 1.96f;        // задание параметра масштабирования для лимитирования эксцессов
            var seasonIndex = GetSeasonIndex(entries);      // получение коэффициента сезонности
            var trend = GetTrendValue(entries, seasonIndex);        // получение параметра тренда
            Dictionary<int, float> seasonals = GetSeasonals(entries, seasonIndex);     // получение сезонных коэффициентов
            var smoothValues = new List<float>();
            var trendValues = new List<float>();
            var predictedDeviation = new List<float>();
            var upperBond = new List<float>();
            var lowerBond = new List<float>();
            var season = new List<float>();

            // установка первоначальных значений
            var smooth = entries[0];
            result.Add(entries[0]);
            smoothValues.Add(smooth);
            trendValues.Add(trend);
            season.Add(seasonals[0 % seasonIndex]);
            predictedDeviation.Add(0);
            upperBond.Add(result[0] + scalingFactor * predictedDeviation[0]);
            lowerBond.Add(result[0] - scalingFactor * predictedDeviation[0]);

            for (var i = 1; i < entries.Count; i++)     // процедура экспоненциального сглаживания
            {
                var lastSmooth = smooth;
                smooth = alpha * (entries[i] - seasonals[i % seasonIndex]) + (1 - alpha) * (smooth + trend);        // расчитываем показатель сглаживания
                trend = beta * (smooth - lastSmooth) + (1 - beta) * trend;      // рассчитыванием показатель тренда
                seasonals[i % seasonIndex] = gamma * (entries[i] - smooth) + (1 - gamma) * seasonals[i % seasonIndex];      // рассчитываем показатель сезонности
                result.Add(smooth + trend + seasonals[i % seasonIndex]);        // получаем результат сглаживания
                predictedDeviation.Add(gamma * Math.Abs(entries[i] - result[i]) + (1 - gamma) * predictedDeviation.Last());     // устанавливаем доверительный интервал

                upperBond.Add(result.Last() + scalingFactor * predictedDeviation.Last());       // рассчитываем новые границы интервала
                lowerBond.Add(result.Last() - scalingFactor * predictedDeviation.Last());
            }

            for (var i = entries.Count; i < entries.Count + epochsValue; i++)       // процедура прогнозирования
            {
                var m = i - entries.Count + 1;
                result.Add((smooth + m * trend) + seasonals[i % seasonIndex]);      // используем рассчитанные ранее значения для прогнозирования величины прогноза
                if (!isTrigger)
                {
                    if (result.Last() > 5) prognosedValues.Add(5);
                    else if (result.Last() < 0) prognosedValues.Add(0);
                    else prognosedValues.Add(result.Last());
                }
                else
                    if (result.Last() > 1)
                {
                    prognosedValues.Add(1);
                }
                else if (result.Last() < 0)
                {
                    prognosedValues.Add(0);
                }
                else
                {
                    prognosedValues.Add(result.Last());
                }

                predictedDeviation.Add(predictedDeviation.Last() * 1.01f);      // устанавливаем доверительный интервал
            }

            return prognosedValues;     // возвращаем список спрогнозированных значений
        }

        private static Dictionary<int, float> GetSeasonals(List<float> entries, int seasonIndex)        // вычисление интервалов сезона
        {
            var result = new Dictionary<int, float>();
            var averages = new List<float>();
            var numberOfSeasons = entries.Count / seasonIndex;       // определяем количество сезонов

            for (var i = 0; i < numberOfSeasons; i++)       // для каждого сезона...
            {
                var sum = 0.0f;
                for (var j = seasonIndex * i; j < seasonIndex * i + seasonIndex; j += 1)        // производим выборку конкретного сзона
                    sum += entries[j];      // расчитываем сумму
                averages.Add(sum / seasonIndex);        // вычисляем среднее значение по сезону
            }

            for (var i = 0; i < seasonIndex; i++)       // для элементов выборки, определенных на интервале сезонности...
            {
                var sumOfValues = 0.0f;
                for (var j = 0; j < numberOfSeasons; j++)       // для каждого сезона...
                    sumOfValues += entries[seasonIndex * j + i] - averages[j];      // вычисляем сумму отклонений значений от средней величины по сезону
                result.Add(i, sumOfValues / numberOfSeasons);       // добавляем сезонные коэффициенты в результирующий словарь
            }

            return result;      // возвращаем словарь

        }

        private static float GetTrendValue(List<float> entries, int seasonIndex)        // метод вычисления показателя тренда
        {
            var sum = 0.0f;
            for (var i = 0; i < entries.Count - seasonIndex; i++)
                if (i + seasonIndex < entries.Count) sum += (entries[i + seasonIndex] - entries[i]) / seasonIndex; else sum += entries[i] / seasonIndex;
            return sum / seasonIndex;
        }

        private static int GetSeasonIndex(List<float> entries)      // метод нахождения индекса сезонности
        {
            var seasonIndex = entries.Count - 1;        // определяем максимально возможный интервал
            double? minSquares = null;
            for (var indexValue = seasonIndex; indexValue > 2; indexValue--)        // перебираем интервалы различной длины
            {
                var currentSquare = 0.0f;       // инициализируем расчетные побъекты
                var averages = new List<float>();

                for (var elementIndex = 0; elementIndex < indexValue; elementIndex++)       // для каждого элемента совокупности...
                {
                    var selectedEntries = new List<float>();        // инициализируем выборку

                    for (var stepsIndex = elementIndex; stepsIndex < entries.Count; stepsIndex += indexValue)       // производим выборку элементов, расположенных на указанном выше интервале
                        selectedEntries.Add(entries[stepsIndex]);

                    var average = selectedEntries.Sum() / selectedEntries.Count;      // расчитываем среднее по выборке
                    var sumOfDiffs = 0.0f;

                    foreach (var entry in selectedEntries)      // высчитываем показатель разницы квадратов для выборки
                        sumOfDiffs += (float)Math.Pow(average - entry, 2);

                    averages.Add(sumOfDiffs);       // добавляем в список
                }

                // при окончании работы алгоритма для интервала определенной длины...
                var globalAverage = averages.Sum() / averages.Count;        // подсчитываем среднее значение по среднему от разницы квадратов
                minSquares = minSquares ?? globalAverage;

                if (globalAverage <= minSquares)        // если оно меньше опорного...
                {
                    minSquares = currentSquare;     // запоминаем наименьшее
                    seasonIndex = indexValue;       // запоминаем значение интервала
                }
            }

            return seasonIndex;     // возвращаем значение минимального интервала с наименьшим средним по разнице квадратов выборок
        }
    }
}
