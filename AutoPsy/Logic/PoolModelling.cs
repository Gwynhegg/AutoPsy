using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Logic
{
    // Функция создания пул-моделлинга. В основе метода лежит представление, что негативные проявления (особенно критически высокие), содержат вокруг себя "зоны влияния".
    // Иными словами, вокруг каждого из критических значений в определенном радиусе остается остаточный шельф, который может быть не замечен пользователем.
    // ПО убывающему принципу при удалении от критической точки влияние параметра падает, однако если оно вступает в конфликт с уже существующим значением, то радиус влияния может
    // расшириться и обновить счетчик значений. Поскольку распространение влияние не однонаправленно, по массиву производится двойная проверка - левосторонняя и правосторонняя,
    // после чего для корректности распределения вычисляется среднее значение, которое и принимается в качестве актуального.
    public static class PoolModelling
    {
        public static List<float> CreatePoolModel(List<float> baseState)        // метод создания пул-модели
        {
            List<float> rightHandStepping = new List<float>();      // массив, содержащий элементы правостороннего обхода
            List<float> leftHandStepping = new List<float>();       // массив, содержащий элементы левостороннего обхода

            float leftCurrentState = baseState[0]; 
           
            for (int i = 0; i < baseState.Count; i++)       // последовательно идем по значениям слева направо
            {
                if (leftCurrentState < baseState[i])        // если при обходе слева мы встречаем значение выше...
                    leftCurrentState = baseState[i];        // то обновляем его на встреченное
                if (leftCurrentState - 1 >= 1) leftHandStepping.Add(leftCurrentState--); else leftHandStepping.Add(leftCurrentState);       // с каждым шагом уменьшаем счетчик значения (уменьшаем влияние)
            }

            float rightCurrentState = baseState[baseState.Count - 1];
            for (int i = baseState.Count - 1; i >= 0; i--)      // последовательно идет по значениям справа налево
            {
                if (rightCurrentState < leftHandStepping[i])        // если при обходе справа встречаем значение выше...
                    rightCurrentState = leftHandStepping[i];        // то обновляем его на встреченное
                if (rightCurrentState - 1 >= 1) rightHandStepping.Insert(0, rightCurrentState--); else rightHandStepping.Insert(0, rightCurrentState);      // аналогично предыдущему шагу
            }

            var result = GetAverageValues(baseState, leftHandStepping, rightHandStepping);      // получаем результирующее значение вычислением среднего
            
            return result;      // возвращаем результат
        }

        private static List<float> GetAverageValues(List<float> baseState, List<float> leftState, List<float> rightState)       // метод получения среднего значения по массиву
        {
            List<float> result = new List<float>();     // инициализируем результирующий массив
            for (int i = 0; i < baseState.Count; i++)       
                result.Add((float)Math.Ceiling((baseState[i] + leftState[i] + rightState[i]) / 3));       // вычисляем среднее по трем расчитанным массивам
            return result;      // возвращаем результат
        }

    }
}
