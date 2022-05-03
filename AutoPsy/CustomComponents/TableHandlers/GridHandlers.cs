using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.CustomComponents.TableHandlers
{
    public class RecomendationsGridHandler : TableGridHandler
    {
        public RecomendationsGridHandler(string topic, DateTime start, DateTime end) : base(topic, start, end)
        {
        }

        public override void FillTableInformation()
        {
            var filterResults = entityHandler.SelectRecomendations(start, end);
            GetDateTimeResults(filterResults);
        }
        public override void FillTableInformation(DateTime start, DateTime end)
        {
            var filterResults = entityHandler.SelectRecomendations(start, end);
            GetDateTimeResults(filterResults);
        }


    }

    public class ConditionsGridHandler : TableGridHandler
    {
        public ConditionsGridHandler(string topic, DateTime start, DateTime end) : base(topic, start, end)
        {
        }

        public override void FillTableInformation() 
        {
            var filterResults = entityHandler.SelectConditions(start, end);
            GetDateTimeResults(filterResults);
        }

        public override void FillTableInformation(DateTime start, DateTime end)
        {
            var filterResults = entityHandler.SelectConditions(start, end);
            GetDateTimeResults(filterResults);
        }
    }

    public class TriggersGridHandler : TableGridHandler
    {
        public TriggersGridHandler(string topic, DateTime start, DateTime end) : base(topic, start, end)
        {
        }

        public override void FillTableInformation() 
        {
            var filterResults = entityHandler.SelectTriggers(start, end);
            GetDateTimeResults(filterResults);
        }

        public override void FillTableInformation(DateTime start, DateTime end)
        {
            var filterResults = entityHandler.SelectTriggers(start, end);
            GetDateTimeResults(filterResults);
        }
    }
}
