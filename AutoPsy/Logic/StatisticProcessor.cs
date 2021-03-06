using AutoPsy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPsy.Logic
{
    public class StatisticProcessor
    {
        private int[] frequencyRate;
        private float moda;
        private readonly int[] weights = new int[] { 0, 3, 2, 1, 3, 5 };
        private float basicAverage;
        private float weightedAverage;
        private float standartDeviation;

        public List<string> GetBasicStatistics(List<float> values, bool isTrigger)
        {
            var result = new List<string>();

            result.AddRange(CalculateFrequency(values, isTrigger));
            if (!isTrigger) result.Add(CalculateSimpleAverage(values));
            if (!isTrigger) result.Add(CalculateWeightedAverage(values));
            if (!isTrigger) result.Add(CalculateGeometricalAverage(values));

            return result;
        }

        public List<float> GetDistributionRange(bool isTrigger)
        {

            var distributionRange = new List<float>();
            int min, max;
            if (isTrigger) { min = 0; max = 1; } else { min = 1; max = 5; }
            for (var i = min; i <= max; i++)
                distributionRange.Add(this.frequencyRate[i]);

            return distributionRange;
        }

        public List<string> GetDistributionStatistic(List<float> values)
        {
            var result = new List<string>();

            result.AddRange(GetStandartDeviation(values));
            result.Add(GetDispersion());
            result.Add(GetOscillationCoef(values));
            result.Add(GetVariationCoef());
            result.Add(GetAsymmetryValue());
            return result;
        }

        private string GetAsymmetryValue()
        {
            var result = string.Format(StatValues.ASYMMETRY_VALUE, (this.basicAverage - this.moda) / this.standartDeviation);
            return result;
        }

        private string GetVariationCoef()
        {
            var result = string.Format(StatValues.VARIATION_COEF, this.standartDeviation / this.basicAverage);
            return result;
        }

        private string GetOscillationCoef(List<float> values)
        {
            var scope = values.Max() - values.Where(x => x != 0).Min();
            var oscillation = scope / this.basicAverage;

            var result = string.Format(StatValues.OSCILLATION_COEF, oscillation);
            return result;
        }

        private List<string> GetStandartDeviation(List<float> values)
        {
            var result = new List<string>();
            this.standartDeviation = GetStandartDeviationValue(values, this.basicAverage);
            result.Add(string.Format(StatValues.STANDART_DEVIATION, this.standartDeviation));
            result.Add(string.Format(StatValues.DEVIATION_DIFFERENCE, Math.Abs(this.standartDeviation - this.basicAverage)));
            return result;
        }

        public static float GetStandartDeviationValue(List<float> values, float average)        // метод вычисления среднеквадратичного отклонения
        {
            var sum = 0.0d;
            foreach (var value in values)
                sum += Math.Pow(value - average, 2);

            var standartDeviation = (float)Math.Sqrt(sum / values.Count);

            return standartDeviation;
        }

        private string GetDispersion()
        {
            var result = string.Format(StatValues.DISPERSION, Math.Pow(this.standartDeviation, 2));
            return result;
        }

        // Метод для вычисления частоты, моды и частости параметра
        private List<string> CalculateFrequency(List<float> values, bool isTrigger)
        {
            int min, max;
            if (isTrigger) { min = 0; max = 1; } else { min = 1; max = 5; };
            var frequencyList = new List<string>();
            this.frequencyRate = new int[max + 1];

            this.moda = 0;
            var choosedModaValue = 0;
            for (var i = min; i <= max; i++)
            {
                var valueCount = values.Where(x => x == i).Count();
                this.frequencyRate[i] = valueCount;
                frequencyList.Add(string.Concat(string.Format(StatValues.FREQUENCY, i), valueCount.ToString(), string.Format(StatValues.RELATIONAL_FREQUENCY, (float)valueCount / values.Count)));
                if (valueCount > choosedModaValue)
                {
                    choosedModaValue = valueCount;
                    this.moda = i;
                }
            }

            frequencyList.Add(string.Format(StatValues.STAT_MODA, this.moda));

            return frequencyList;
        }

        // Метод для вычисления средней арифметической,
        private string CalculateSimpleAverage(List<float> values)
        {

            this.basicAverage = CalculateAverage(values);
            var average = string.Format(StatValues.AVERAGE_VOLUME, this.basicAverage);

            return average;
        }

        public static float CalculateAverage(List<float> values)        // метод вычисления среднего
        {
            var sum = 0.0f;
            foreach (var value in values)
                sum += value;

            var average = sum / values.Count;

            return average;
        }

        private string CalculateWeightedAverage(List<float> values)
        {
            var sum = 0.0f;
            var weightsSum = 0;
            foreach (var value in values)
            {
                sum += value * this.weights[(int)value];
                weightsSum += this.weights[(int)value];
            }

            this.weightedAverage = weightsSum != 0 ? sum / weightsSum : 0;
            var average = string.Format(StatValues.WEIGHTED_AVERAGE_VOLUME, this.weightedAverage);

            return average;
        }

        private string CalculateGeometricalAverage(List<float> values)
        {
            var accumulation = 1.0f;
            foreach (var value in values)
                accumulation *= value;

            accumulation = (float)Math.Pow(accumulation, 1 / values.Count);

            var average = string.Format(StatValues.GEOMETRIC_AVERAGE, accumulation);

            return average;
        }

        public List<float> CalculateDynamicValue(List<float> values)
        {
            var dynamicRange = new List<float>() { 1.0f };
            for (var i = 1; i < values.Count; i++)
                dynamicRange.Add(values[i] / values[i - 1]);
            return dynamicRange;
        }
    }
}
