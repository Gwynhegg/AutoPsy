using AutoPsy.Resources;

namespace AutoPsy.AuxServices
{
    public static class DeleteRules
    {
        // Вспомогательный класс для проверки правил удаления записей из набора результатов
        public static bool CheckZeroRule(string parameter)
        {
            if (parameter.Equals(Constants.STAT_COUNT)) return false;
            if (parameter.Equals(Constants.MAX_VALUE)) return false;
            if (parameter.Equals(Constants.AVERAGE_VALUE)) return true;
            if (parameter.Equals(Constants.MIN_INTERVAL)) return true;
            if (parameter.Equals(Constants.MAX_INTERVAL)) return true;
            if (parameter.Equals(Constants.AVERAGE_INTERVAL)) return true;
            return false;
        }
    }
}
