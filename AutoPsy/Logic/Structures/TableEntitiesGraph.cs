using AutoPsy.Const;
using AutoPsy.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace AutoPsy.Logic.Structures
{
    public class TableEntitiesGraph
    {
        private readonly List<TableRecomendation> recomendations;        // Список для хранения рекомендаций
        private readonly List<TableCondition> conditions;        // Список для хранения состояний
        private readonly List<TableTrigger> triggers;        // Список для хранения триггеров
        private ITableEntity[] tableEntities;       // Список с типом интерфейса для хранения и обработки агреггированных данных

        public TableEntitiesGraph()
        {
            this.recomendations = new List<TableRecomendation>();
            this.conditions = new List<TableCondition>();
            this.triggers = new List<TableTrigger>();

            ParseData();
        }

        public string GetParameterType(string parameter)
        {
            if (this.recomendations.FirstOrDefault(x => x.IdValue.Equals(parameter)) != null) return Const.Constants.ENTITY_RECOMENDATION;
            if (this.conditions.FirstOrDefault(x => x.IdValue.Equals(parameter)) != null) return Const.Constants.ENTITY_CONDITION;
            if (this.triggers.FirstOrDefault(x => x.IdValue.Equals(parameter)) != null) return Const.Constants.ENTITY_TRIGGER;
            return null;

        }

        public string GetIdStringByName(string name)        // Метод для получения строки ID по имени объекта
=> this.tableEntities.Where(x => x.Name.Equals(name)).First().IdValue;

        public string GetNameByIdString(string idString) => this.tableEntities.Where(x => x.IdValue.Equals(idString)).First().Name;

        public ITableEntity[] GetRecomendations() => this.recomendations.ToArray();
        public ITableEntity[] GetConditions() => this.conditions.ToArray();
        public ITableEntity[] GetTriggers() => this.triggers.ToArray();
        private void ParseData()
        {
            var document = new XmlDocument();       // Создаем обработчик XML-документа
            Android.Content.Res.AssetManager assets = Android.App.Application.Context.Assets;        // Открываем папку ассетов
            document.Load(assets.Open(Constants.TABLE_GRAPH_NAME));     // считываем XML-Документ (предсталвение графа)

            // Считываем узлы графа
            XmlNode nodes = document.SelectSingleNode(Constants.TABLE_GRAPH_PATH);

            CutNodes(nodes);
        }

        private void CutNodes(XmlNode nodes)        // Нарезаем узлы в сформированные структуры
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Attributes[Const.Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Constants.RECOMENDATIONS_TAG))       // Если это рекомендация, то...
                    this.recomendations.Add(new TableRecomendation() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });       // создаем элемент и помещаем в соответ. массив
                else if (node.Attributes[Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Constants.CONDITIONS_TAG))        // и т.д.
                    this.conditions.Add(new TableCondition() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
                else
                    this.triggers.Add(new TableTrigger() { IdValue = node.Attributes[0].Value, Name = node.Attributes[1].Value });
            }

            this.tableEntities = new ITableEntity[this.recomendations.Count + this.conditions.Count + this.triggers.Count];     // Объединяем наборы в общую структуру
            this.tableEntities = GetRecomendations().Union(GetConditions().Union(GetTriggers())).ToArray();
        }
    }


}
