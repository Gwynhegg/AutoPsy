using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AutoPsy.Database.Entities;

namespace AutoPsy.Logic.Structures
{
    public class TableEntitiesGraph
    {
        private List<TableRecomendation> recomendations;
        private List<TableCondition> conditions;
        private List<TableTrigger> triggers;
        private ITableEntity[] tableEntities;

        public TableEntitiesGraph()
        {
            recomendations = new List<TableRecomendation>();
            conditions = new List<TableCondition>();
            triggers = new List<TableTrigger>();

            ParseData();
        }

        public ITableEntity[] GetRecomendations()
        {
            return recomendations.ToArray();
        }
        public ITableEntity[] GetConditions()
        {
            return conditions.ToArray();
        }
        public ITableEntity[] GetTriggers()
        {
            return triggers.ToArray();
        }
        private void ParseData()
        {
            var document = new XmlDocument();       // Создаем обработчик XML-документа
            var assets = Android.App.Application.Context.Assets;        // Открываем папку ассетов
            document.Load(assets.Open(AutoPsy.Const.Constants.TABLE_GRAPH_NAME));     // считываем XML-Документ (предсталвение графа)

            // Считываем узлы графа
            var nodes = document.SelectSingleNode("/*[local-name()='DirectedGraph']/*[local-name()='Nodes']");

            CutNodes(nodes);
        }

        private void CutNodes(XmlNode nodes)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[Const.Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Const.Constants.RECOMENDATIONS_TAG))
                    recomendations.Add(new TableRecomendation() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
                else if (node.Attributes[Const.Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Const.Constants.CONDITIONS_TAG))
                    conditions.Add(new TableCondition() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
                else
                    triggers.Add(new TableTrigger() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
            }

            this.tableEntities = new ITableEntity[recomendations.Count + conditions.Count + triggers.Count];     // Объединяем наборы в общую структуру
            this.tableEntities = GetRecomendations().Union(GetConditions().Union(GetTriggers())).ToArray();
        }
    }

    
}
