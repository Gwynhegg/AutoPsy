using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.AuxServices
{
    public static class DiaryResultsSorter
    {
        public static Dictionary<string, Logic.Structures.DiaryResultRecords> GetSortedRecords(Dictionary<string, Logic.Structures.DiaryResultRecords> records, string parameter)
        {
            switch (parameter)
            {
                case Const.Constants.STAT_COUNT: return records.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value); break;
                case Const.Constants.MAX_VALUE: return records.OrderByDescending(x => x.Value.MaxValue).ToDictionary(x => x.Key, x => x.Value); break;
                case Const.Constants.AVERAGE_VALUE: return records.OrderByDescending(x => x.Value.AverageValue).ToDictionary(x => x.Key, x => x.Value); break;
                case Const.Constants.MIN_INTERVAL: return records.OrderBy(x => x.Value.MinInterval).ToDictionary(x => x.Key, x => x.Value); break;
                case Const.Constants.MAX_INTERVAL: return records.OrderByDescending(x => x.Value.MaxInterval).ToDictionary(x => x.Key, x => x.Value); break;
                case Const.Constants.AVERAGE_INTERVAL: return records.OrderByDescending(x => x.Value.AverageInterval).ToDictionary(x => x.Key, x => x.Value); break;
                default: return null; break;
            }
        }
    }
}
