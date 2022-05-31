using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Logic
{
    public static class ClusterHierarchy
    {
        private static float CalcManhattanDistance(List<float> column1, List<float> column2)
        {
            var sum = 0.0f;
            for (int i = 0; i < column1.Count; i++)
                sum += Math.Abs(column1[i] - column2[i]);
            return sum;
        }

        private static KeyValuePair<string, List<float>> JoinColumns(KeyValuePair<string, List<float>> column1, KeyValuePair<string, List<float>> column2)
        {
            var columnName = String.Format("{0} + {1}", column1.Key, column2.Key);
            var columnValue = new List<float>();
            for (int i = 0; i < column1.Value.Count; i++)
                columnValue.Add((column1.Value[i] + column2.Value[i]) / 2);
            var keyValuePair = new KeyValuePair<string, List<float>>(columnName, columnValue);
            return keyValuePair;
        }

        private static Dictionary<string,float> CalculateClusterHierarchy(Dictionary<string, List<float>> data, Dictionary<string, float> cluster)
        {
            if (data.Count < 2) return cluster;
            int firstMinIndex = 0, secondMinIndex = 0;
            var minimalDistance = CalcManhattanDistance(data.ElementAt(0).Value, data.ElementAt(1).Value);

            for (int i = 0; i < data.Count; i++)
                for (int j = i + 1; j < data.Count; j++)
                {
                    var currentDistance = CalcManhattanDistance(data.ElementAt(i).Value, data.ElementAt(j).Value);
                    if (currentDistance <= minimalDistance)
                    {
                        firstMinIndex = i;
                        secondMinIndex = j;
                        minimalDistance = currentDistance;
                    }
                }

            var newKeyValuePair = JoinColumns(data.ElementAt(firstMinIndex), data.ElementAt(secondMinIndex));
            var key1 = data.ElementAt(firstMinIndex).Key;
            var key2 = data.ElementAt(secondMinIndex).Key;

            data.Remove(key1);
            data.Remove(key2);
            data.Add(newKeyValuePair.Key, newKeyValuePair.Value);
            cluster.Add(newKeyValuePair.Key, StatisticProcessor.CalculateAverage(newKeyValuePair.Value));

            return CalculateClusterHierarchy(data, cluster);
        }

        public static Dictionary<string, float> CreateHierarchyTree(Dictionary<string, List<float>> data)
        {
            var normalizedData = new Dictionary<string, List<float>>();
            foreach(var pair in data)
                normalizedData.Add(App.TableGraph.GetNameByIdString(pair.Key), NormalizationProcessor.NormalizeArray(pair.Value));

            var resultData = CalculateClusterHierarchy(normalizedData, new Dictionary<string, float>());

            return resultData;
        }



    }
}
