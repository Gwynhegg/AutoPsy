using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Logic
{
    public static class CorrelationProcessor
    {
        public static float CalculateCorrelationValue(List<float> X, List<float> Y)
        {
            var averageX = StatisticProcessor.CalculateAverage(X);
            var averageY = StatisticProcessor.CalculateAverage(Y);

            var stdeviationX = StatisticProcessor.GetStandartDeviationValue(X, averageX);
            var stdeviationY = StatisticProcessor.GetStandartDeviationValue(Y, averageY);

            var composition = new List<float>();
            for (int i = 0; i < X.Count; i++)
                composition.Add(X[i] * Y[i]);
            var averageComposition = StatisticProcessor.CalculateAverage(composition);

            var linearCorrelation = stdeviationX != 0 && stdeviationY != 0 ? (averageComposition - averageX * averageY) / (stdeviationX * stdeviationY) : 0;

            return linearCorrelation;
        }
    }
}
