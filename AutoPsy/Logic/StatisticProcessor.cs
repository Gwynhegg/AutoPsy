using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoPsy.Resources;

namespace AutoPsy.Logic
{
    public class StatisticProcessor
    {
        private int[] frequencyRate;
        private float moda;
        public List<string> GetBasicStatistics(List<float> values, bool isTrigger)
        {
            var result = new List<string>();

            result.AddRange(CalculateFrequency(values));
            if (!isTrigger) result.AddRange(CalculateSimpleAverage(values));
            result.AddRange(CalculateWeightedAverage(values));
            if (!isTrigger) result.AddRange(CalculateGeometricalAverage(values));

            return result;
        }

        // Метод для вычисления частоты, моды и частости параметра
        private List<string> CalculateFrequency(List<float> values)
        {
            var minimalValue = (int)values.Min();
            var maximumValue = (int)values.Max();
            var frequencyList = new List<string>();
            frequencyRate = new int[maximumValue + 1];

            moda = 0;
            var choosedModaValue = 0;
            for (int i = minimalValue; i <= maximumValue; i++)
            {
                var valueCount = values.Where(x => x == i).Count();
                frequencyRate[i] = valueCount;
                frequencyList.Add(String.Concat(String.Format(StatValues.FREQUENCY, i), valueCount.ToString(), String.Format(StatValues.RELATIONAL_FREQUENCY, (float)valueCount / values.Count)));
                if (valueCount > choosedModaValue)
                {
                    choosedModaValue = valueCount;
                    moda = i;
                }
            }

            frequencyList.Add(String.Format(StatValues.STAT_MODA, moda));

            return frequencyList;
        }

        // Метод для вычисления средней арифметической,
        private List<string> CalculateSimpleAverage(List<float> values)
        {
            var average = new List<string>();
            var sum = 0.0f;
            foreach (var value in values)
                sum += value;

            average.Add(String.Format(StatValues.AVERAGE_VOLUME, (sum / values.Count)));

            return average;
        }

        private List<string> CalculateWeightedAverage(List<float> values)
        {
            int[] weights = new int[] { 0, 3, 2, 1, 3, 5 };
            var average = new List<string>();
            var sum = 0.0f;
            var weightsSum = 0;
            foreach (var value in values)
            {
                sum += value * weights[(int)value];
                weightsSum += weights[(int)value];
            }

            average.Add(String.Format(StatValues.WEIGHTED_AVERAGE_VOLUME, (sum / weightsSum)));

            return average;
        }

        private List<string>  CalculateGeometricalAverage(List<float> values)
        {
            var average = new List<string>();
            var accumulation = 1.0f;
            foreach (var value in values)
                accumulation *= value;

            accumulation = (float)Math.Pow(accumulation, 1 / values.Count);

            average.Add(String.Format(StatValues.GEOMETRIC_AVERAGE, accumulation));

            return average;
        }

        public List<float> CalculateDynamicValue(List<float> values)
        {
            List<float> dynamicRange = new List<float>() { 1.0f };
            for (int i = 1; i < values.Count; i++)
                dynamicRange.Add(values[i] / values[i - 1]);
            return dynamicRange;
        }
    }
}
