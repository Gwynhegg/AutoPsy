using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoPsy.Logic
{
    public static class NormalizationProcessor
    {
        public static List<float> NormalizeArray(List<float> values)
        {
            var maximum = values.Max();
            var minimum = values.Min();
            var delta = maximum - minimum;
            var resultArray = new List<float>();

            foreach (var value in values)
            {
                var normValue = delta == 0? 1: (value - minimum) / delta;
                resultArray.Add(normValue);
            }

            return resultArray;
        }
    }
}
