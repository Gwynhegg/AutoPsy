using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Logic
{
    public static class LinearRegression
    {
        public static Dictionary<DateTime, float> CreateMovingAverageRegression(Dictionary<DateTime, int> entries, double averageInterval, int epochsValue)
        {
            if (entries.Count < 3) return null;
            var resultDict = new Dictionary<DateTime, float>();
            float[] values = new float[entries.Count + epochsValue];
            for (int i = 0; i < entries.Count; i++)
                values[i] = (float)entries.ElementAt(i).Value;

            float[] movingAverage = new float[values.Length];
            for (int i = 1; i < entries.Count - 1; i++)
                movingAverage[i] = (values[i - 1] + values[i] + values[i + 1]) / 3;

            for (int i = entries.Count; i < values.Length; i++)
            {
                values[i] = movingAverage[i - 2] + 1 / 3 * (values[i - 1] - values[i - 2]);
                movingAverage[i - 1] = (values[i - 2] + values[i - 1] + values[i]) / 3;
            }

            for (int i = 1; i <= epochsValue; i++)
                resultDict.Add(entries.Last().Key.AddDays(i * averageInterval), values[i + entries.Count - 1]);

            return resultDict;                  
        }
    }
}
