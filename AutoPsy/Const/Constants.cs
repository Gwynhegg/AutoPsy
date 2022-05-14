using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Const
{
    public static class Constants
    {
        public const string TABLE_GRAPH_PATH = "/*[local-name()='DirectedGraph']/*[local-name()='Nodes']";      // локальный путь до графа, содержащего состояния, рекомендации и триггеры
        public const string SYMPTOM_GRAPH_PATH = "/*[local-name()='DirectedGraph']/*[local-name()='Nodes']";        // локальный путь до графа с вершинами симптомов и болезней
        public const string SYMPTOM_GRAPH_LINKS = "/*[local-name()='DirectedGraph']/*[local-name()='Links']";       // локальный путь до графа, содержащего ребра (исп. с предыдущим)
        public const string DATABASE_NAME = "userdata.db3";     // наименование локальной базы данных
        public const string GRAPH_NAME = "diseasegraph.dgml";       // наименование графа симптомов и болезней
        public const string TABLE_GRAPH_NAME = "tableEntities.dgml";        // наименование графа с сущностями для таблицы состояний
        public const string SYMPTOMS_TAG = "Symptoms";      // вспомогательный тег "Симптомы"
        public const string RECOMENDATIONS_TAG = "Recomendations"; // вспомогательный тег "Рекомендации"
        public const string CONDITIONS_TAG = "Conditions";      // вспомогательный тег "Состояния"
        public const string TRIGGERS_TAG = "Triggers";      // вспомогательный тег "Триггеры"
        public const string GRAPH_ATTRIBUTE_CATEGORY = "Category";      // вспомогательный аттрибут графа "Категория"
        public const string ENTITY_RECOMENDATION = "Recomendations";        // название категории "Рекомендация"
        public const string ENTITY_CONDITION = "Conditions";        // название категории "Состояния"
        public const string ENTITY_TRIGGER = "Triggers";        // название категории "Триггеры"
        public const string IS_ZERO = "0";      // вспомогательная константа для проверки на ноль

        public const int RECOMMENDATION_PARAMETER = 0;      
        public const int CONDITION_PARAMETER = 1;
        public const int TRIGGER_PARAMETER = 2;
        public const int WEEK = 7;      // константа количества дней в неделе (хоть что-то в этой жизни неизменно)
    }
}
