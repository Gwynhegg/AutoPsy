using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoPsy.Resources;

namespace AutoPsy.AuxServices
{
    public static class DiaryResultsSorter
    {
        // Вспомогательный класс для упорядочивания записей в дневника по определенному статистическому показателю
        public static Dictionary<string, Logic.Structures.DiaryResultRecords> GetSortedRecords(Dictionary<string, Logic.Structures.DiaryResultRecords> records, string parameter)
        {
            if (parameter.Equals(Constants.STAT_COUNT)) return records.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
            if (parameter.Equals(Constants.MAX_VALUE)) return records.OrderByDescending(x => x.Value.MaxValue).ToDictionary(x => x.Key, x => x.Value);
            if (parameter.Equals(Constants.AVERAGE_VALUE)) return records.OrderByDescending(x => x.Value.AverageValue).ToDictionary(x => x.Key, x => x.Value);
            if (parameter.Equals(Constants.MIN_INTERVAL)) records.OrderBy(x => x.Value.MinInterval).ToDictionary(x => x.Key, x => x.Value);
            if (parameter.Equals(Constants.MAX_INTERVAL)) return records.OrderByDescending(x => x.Value.MaxInterval).ToDictionary(x => x.Key, x => x.Value);
            if (parameter.Equals(Constants.AVERAGE_INTERVAL)) return records.OrderByDescending(x => x.Value.AverageInterval).ToDictionary(x => x.Key, x => x.Value);
            return null;
        }
    }
}
