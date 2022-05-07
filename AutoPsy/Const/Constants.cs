using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Const
{
    public static class Constants
    {
        public const string TABLE_GRAPH_PATH = "/*[local-name()='DirectedGraph']/*[local-name()='Nodes']";
        public const string SYMPTOM_GRAPH_PATH = "/*[local-name()='DirectedGraph']/*[local-name()='Nodes']";
        public const string SYMPTOM_GRAPH_LINKS = "/*[local-name()='DirectedGraph']/*[local-name()='Links']";
        public const string DATABASE_NAME = "userdata.db3";
        public const string GRAPH_NAME = "diseasegraph.dgml";
        public const string TABLE_GRAPH_NAME = "tableEntities.dgml";
        public const string SYMPTOMS_TAG = "Symptoms";
        public const string RECOMENDATIONS_TAG = "Recomendations";
        public const string CONDITIONS_TAG = "Conditions";
        public const string TRIGGERS_TAG = "Triggers";
        public const string GRAPH_ATTRIBUTE_CATEGORY = "Category";
        public const string IS_ZERO = "0";

        public const int RECOMMENDATION_PARAMETER = 0;
        public const int CONDITION_PARAMETER = 1;
        public const int TRIGGER_PARAMETER = 2;
        public const int WEEK = 7;
    }
}
