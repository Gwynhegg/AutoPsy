using System;
using System.Collections.Generic;
using System.Text;
using AutoPsy.Database.Entities;

namespace AutoPsy.CustomComponents.TableHandlers
{
    public class RecomendationsGridHandler : TableGridHandler
    {
        public RecomendationsGridHandler(DateTime start, DateTime end) : base(start, end)
        {
            entityHandler = new RecomendationTableEntityHandler();
        }

        public override void FillTableInformation()
        {
            var filterResults = entityHandler.SelectRecomendations(start, end);
            GetDateTimeResults(filterResults);
        }

        public override void AddParameter(string parameter, byte importance)
        {
            base.AddParameter(parameter, importance);
            
            var entity = new TableRecomendation() { IdValue = App.TableGraph.GetIdStringByName(parameter), Name = parameter, Importance = importance, Time = end.Date, Value = 1, Type = "Recomendation" };

            entityHandler.AddEntity(parameter, entity);
        }
        public override TableEntityHandler GetEntityHandler()
        {
            return entityHandler;
        }

    }

    public class ConditionsGridHandler : TableGridHandler
    {
        public ConditionsGridHandler(DateTime start, DateTime end) : base(start, end)
        {
            entityHandler = new CondtiionTableEntityHandler();
        }

        public override void FillTableInformation() 
        {
            var filterResults = entityHandler.SelectConditions(start, end);
            GetDateTimeResults(filterResults);
        }

        public override void AddParameter(string parameter, byte importance)
        {
            base.AddParameter(parameter, importance);

            var entity = new TableCondition() { IdValue = App.TableGraph.GetIdStringByName(parameter), Name = parameter, Importance = importance, Time = end.Date, Value = 1, Type = "Condition" };

            entityHandler.AddEntity(parameter, entity);
        }
        public override TableEntityHandler GetEntityHandler()
        {
            return entityHandler;
        }
    }

    public class TriggersGridHandler : TableGridHandler
    {
        public TriggersGridHandler(DateTime start, DateTime end) : base(start, end)
        {
            entityHandler = new TriggerTableEntityHandler();
        }

        public override void FillTableInformation() 
        {
            var filterResults = entityHandler.SelectTriggers(start, end);
            GetDateTimeResults(filterResults);
        }

        public override void AddParameter(string parameter, byte importance)
        {
            base.AddParameter(parameter, importance);

            var entity = new TableTrigger() { IdValue = App.TableGraph.GetIdStringByName(parameter), Name = parameter, Importance = importance, Time = end.Date, Value = 0, Type = "Trigger" };

            entityHandler.AddEntity(parameter, entity);
        }

        public override TableEntityHandler GetEntityHandler()
        {
            return entityHandler;
        }
    }
}
