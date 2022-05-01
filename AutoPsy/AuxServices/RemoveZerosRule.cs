using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.AuxServices
{
    public static class RemoveZerosRule
    {
        public static bool CheckRule(string parameter)
        {
            switch (parameter)
            {
                case Const.Constants.STAT_COUNT: return false; break;
                case Const.Constants.MAX_VALUE: return false; break;
                case Const.Constants.AVERAGE_VALUE: return true; break;
                case Const.Constants.MIN_INTERVAL: return true; break;
                case Const.Constants.MAX_INTERVAL: return true; break;
                case Const.Constants.AVERAGE_INTERVAL: return true; break;
                default: return false; break;
            }
        }
    }
}
