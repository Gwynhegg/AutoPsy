using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPsy.Logic
{
    public static class ClusterHierarchy
    {
        private static float CalcManhattanDistance(List<float> column1, List<float> column2)        // метод вычисления манхэттенского расстояния
        {
            var sum = 0.0f;
            for (var i = 0; i < column1.Count; i++)
                sum += Math.Abs(column1[i] - column2[i]);
            return sum;
        }

        private static KeyValuePair<string, List<float>> JoinColumns(KeyValuePair<string, List<float>> column1, KeyValuePair<string, List<float>> column2)      // метод слияния двух объектов
        {
            var columnName = string.Format("{0} + {1}", column1.Key, column2.Key);
            var columnValue = new List<float>();
            for (var i = 0; i < column1.Value.Count; i++)
                columnValue.Add((column1.Value[i] + column2.Value[i]) / 2);     // ячейка нового элемента равна среднему значению двух старых
            var keyValuePair = new KeyValuePair<string, List<float>>(columnName, columnValue);
            return keyValuePair;
        }

        private static Dictionary<string, float> CalculateClusterHierarchy(Dictionary<string, List<float>> data, Dictionary<string, float> cluster)
        {
            if (data.Count < 2) return cluster;     // если количество объектов в списке = 1, то возвращаем результат
            int firstMinIndex = 0, secondMinIndex = 0;
            var minimalDistance = CalcManhattanDistance(data.ElementAt(0).Value, data.ElementAt(1).Value);      // принимаем расстояние между первым и вторым кластером за минимальное

            for (var i = 0; i < data.Count; i++)        // для каждой пары наборов значений...
            {
                for (var j = i + 1; j < data.Count; j++)
                {
                    var currentDistance = CalcManhattanDistance(data.ElementAt(i).Value, data.ElementAt(j).Value);      // вычисляем расстояние между текущей парой
                    if (currentDistance <= minimalDistance)     // если она меньше текущей минимальной, перезаписываем
                    {
                        firstMinIndex = i;
                        secondMinIndex = j;
                        minimalDistance = currentDistance;
                    }
                }
            }

            KeyValuePair<string, List<float>> newKeyValuePair = JoinColumns(data.ElementAt(firstMinIndex), data.ElementAt(secondMinIndex));       // сливаем два объекта данных в один
            var key1 = data.ElementAt(firstMinIndex).Key;
            var key2 = data.ElementAt(secondMinIndex).Key;

            data.Remove(key1);      // удаляем старые объекты
            data.Remove(key2);
            data.Add(newKeyValuePair.Key, newKeyValuePair.Value);       // добавляем сформированный объект
            cluster.Add(newKeyValuePair.Key, StatisticProcessor.CalculateAverage(newKeyValuePair.Value));       // добавляем объект в результирующий набор       

            return CalculateClusterHierarchy(data, cluster);        // идем на следующую итерацию рекурсии
        }

        public static Dictionary<string, float> CreateHierarchyTree(Dictionary<string, List<float>> data)       // метод построения дерева кластеров
        {
            var normalizedData = new Dictionary<string, List<float>>();        // инициализация словаря нормализованных величин
            foreach (KeyValuePair<string, List<float>> pair in data)
                normalizedData.Add(App.TableGraph.GetNameByIdString(pair.Key), NormalizationProcessor.NormalizeArray(pair.Value));      // процедура нормализации

            Dictionary<string, float> resultData = CalculateClusterHierarchy(normalizedData, new Dictionary<string, float>());        // запуск алгоритма кластерного анализа

            return resultData;      // возвращение результирующего списка
        }



    }
}
