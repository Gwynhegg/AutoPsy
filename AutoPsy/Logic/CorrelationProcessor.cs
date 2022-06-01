using System.Collections.Generic;

namespace AutoPsy.Logic
{
    public static class CorrelationProcessor
    {
        public static float CalculateCorrelationValue(List<float> X, List<float> Y)
        {
            var averageX = StatisticProcessor.CalculateAverage(X);      // получаем среднее по X
            var averageY = StatisticProcessor.CalculateAverage(Y);      // получаем среднее по Y

            var stdeviationX = StatisticProcessor.GetStandartDeviationValue(X, averageX);       // получаем среднеквадратичное отклонение для X
            var stdeviationY = StatisticProcessor.GetStandartDeviationValue(Y, averageY);       // получаем среднеквадратичное отклонение для Y

            var composition = new List<float>();
            for (var i = 0; i < X.Count; i++)
                composition.Add(X[i] * Y[i]);
            var averageComposition = StatisticProcessor.CalculateAverage(composition);      // расчитываем среднее по произведениям

            var linearCorrelation = stdeviationX != 0 && stdeviationY != 0 ? (averageComposition - averageX * averageY) / (stdeviationX * stdeviationY) : 0;        // высчитываем коэффициент корреляции

            return linearCorrelation;
        }
    }
}
