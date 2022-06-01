using Android.Content.Res;
using AutoPsy.Const;
using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace AutoPsy.Logic.Structures
{
    public class DiseaseGraph       // Граф, содержащий в себе узлы симптомов, проявлений и категорий, а также свящи между ними
    {
        private readonly List<CategoryNode> categories;      // Лист категорий
        private readonly List<DiseaseNode> diseases;     // Лист проявлений
        private readonly List<SymptomNode> symptoms;     // Лист симптомов
        private readonly List<Link> links;       // Лист ребер графа (в виде объекта смежности)
        private INode[] nodes;      // Набор всех узлов графа

        public DiseaseGraph()
        {
            this.categories = new List<CategoryNode>();
            this.diseases = new List<DiseaseNode>();
            this.symptoms = new List<SymptomNode>();
            this.links = new List<Link>();

            ParseData();
        }

        public string[] SearchAncestorsLink(string target)      // Метод для получения массива значений узлов-родителей
        {
            IEnumerable<string> queryLinks = this.links.Where(x => x.Target.Equals(target)).Select(x => x.Source);
            IEnumerable<string> querySources = this.nodes.Where(x => queryLinks.Contains(x.Id)).Select(x => x.Value);
            return querySources.ToArray();
        }

        public string[] SearchAncestorsLinkId(string target)        // Метод для получения массива Id узлов-родителей
        {
            IEnumerable<string> queryLinks = this.links.Where(x => x.Target.Equals(target)).Select(x => x.Source);
            return queryLinks.ToArray();
        }

        public string[] SearchDescendersLink(string source)     // Метод для получения массива значений узлов-детей
        {
            IEnumerable<string> queryLinks = this.links.Where(x => x.Source.Equals(source)).Select(x => x.Target);
            IEnumerable<string> querySources = this.nodes.Where(x => queryLinks.Contains(x.Id)).Select(x => x.Value);
            return querySources.ToArray();
        }

        public INode[] GetSymptomNodes()        // Получение массива симптомов
=> this.symptoms.ToArray();

        public INode[] GetDiseasesNodes()       // Получение массива проявлений
=> this.diseases.ToArray();

        public INode[] GetCategoryNodes()       // Получение массива категорий
=> this.categories.ToArray();

        public INode[] GetAllItems()        // Получение массива всех узлов
=> this.nodes;
        public string GetNodeId(string value)       // Получение ID узла по его значению
=> this.nodes.First(x => x.Value.Equals(value)).Id;

        public string GetNodeValue(string id)       // Получение значения узла по его ID
=> this.nodes.First(x => x.Id.Equals(id)).Value;

        // Метод для парсинга данных из файла-описания графа
        private void ParseData()
        {
            var document = new XmlDocument();       // Создаем обработчик XML-документа
            AssetManager assets = Android.App.Application.Context.Assets;        // Открываем папку ассетов
            document.Load(assets.Open(Constants.GRAPH_NAME));     // считываем XML-Документ (предсталвение графа)

            // Считываем ребра графа
            XmlNode nodeLinks = document.SelectSingleNode(Constants.SYMPTOM_GRAPH_LINKS);
            // Считываем узлы графа
            XmlNode nodes = document.SelectSingleNode(Constants.SYMPTOM_GRAPH_PATH);

            // Нарезаем считанные данные в структуру графа
            CutNodesAndLinks(nodeLinks, nodes);
        }

        // Метод для нарезания узлов и ребер графа в структуру графа
        private void CutNodesAndLinks(XmlNode links, XmlNode nodes)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.NodeType != XmlNodeType.Comment)       // При разрезке желательно игнорировать записи комментариев
                {
                    if (node.Attributes.Count == 2)     // Если узел имеет только два атрибута, то он является категорией
                    {
                        this.categories.Add(new CategoryNode(node.Attributes[0].Value, node.Attributes[1].Value));
                    }
                    else
                    {
                        if (node.Attributes[Const.Constants.GRAPH_ATTRIBUTE_CATEGORY].Value.Equals(Const.Constants.SYMPTOMS_TAG))       // Если в категории атрибута указано, что это симптом, то создаем узел симтома
                            this.symptoms.Add(new SymptomNode(node.Attributes[0].Value, node.Attributes[1].Value));
                        else
                            this.diseases.Add(new DiseaseNode(node.Attributes[0].Value, node.Attributes[1].Value, node.Attributes[2].Value));        // Иначе - проявления
                    }
                }
            }

            foreach (XmlNode node in links)     // Для каждой записи о ребре графа...
                this.links.Add(new Link(node.Attributes[0].Value, node.Attributes[1].Value));       // Создаем ребро и связываем два указанных узла

            this.nodes = new INode[this.categories.Count + this.diseases.Count + this.symptoms.Count];     // Объединяем наборы в общую структуру
            this.nodes = GetSymptomNodes().Union(GetDiseasesNodes().Union(GetCategoryNodes())).ToArray();
        }
    }


}
