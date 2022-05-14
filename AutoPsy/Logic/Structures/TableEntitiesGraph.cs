using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using AutoPsy.Const;
using AutoPsy.Database.Entities;

namespace AutoPsy.Logic.Structures
{
    public class TableEntitiesGraph
    {
        private List<TableRecomendation> recomendations;        // Список для хранения рекомендаций
        private List<TableCondition> conditions;        // Список для хранения состояний
        private List<TableTrigger> triggers;        // Список для хранения триггеров
        private ITableEntity[] tableEntities;       // Список с типом интерфейса для хранения и обработки агреггированных данных

        public TableEntitiesGraph()
        {
            recomendations = new List<TableRecomendation>();
            conditions = new List<TableCondition>();
            triggers = new List<TableTrigger>();

            ParseData();
        }

        public string GetIdStringByName(string name)        // Метод для получения строки ID по имени объекта
        {
            return tableEntities.Where(x => x.Name.Equals(name)).First().IdValue;
        }

        public string GetNameByIdString(string idString)
        {
            return tableEntities.Where(x => x.IdValue.Equals(idString)).First().Name;
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
            document.Load(assets.Open(Constants.TABLE_GRAPH_NAME));     // считываем XML-Документ (предсталвение графа)

            // Считываем узлы графа
            var nodes = document.SelectSingleNode(Constants.TABLE_GRAPH_PATH);

            CutNodes(nodes);
        }

        private void CutNodes(XmlNode nodes)        // Нарезаем узлы в сформированные структуры
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[Const.Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Constants.RECOMENDATIONS_TAG))       // Если это рекомендация, то...
                    recomendations.Add(new TableRecomendation() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });       // создаем элемент и помещаем в соответ. массив
                else if (node.Attributes[Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Constants.CONDITIONS_TAG))        // и т.д.
                    conditions.Add(new TableCondition() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
                else
                    triggers.Add(new TableTrigger() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
            }

            this.tableEntities = new ITableEntity[recomendations.Count + conditions.Count + triggers.Count];     // Объединяем наборы в общую структуру
            this.tableEntities = GetRecomendations().Union(GetConditions().Union(GetTriggers())).ToArray();
        }
    }

    
}
