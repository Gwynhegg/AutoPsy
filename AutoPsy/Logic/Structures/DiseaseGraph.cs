using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Android.Content.Res;
using System.Linq;

namespace AutoPsy.Logic.Structures
{
    public class DiseaseGraph       // Граф, содержащий в себе узлы симптомов, проявлений и категорий, а также свящи между ними
    {
        private List<CategoryNode> categories;      // Лист категорий
        private List<DiseaseNode> diseases;     // Лист проявлений
        private List<SymptomNode> symptoms;     // Лист симптомов
        private List<Link> links;       // Лист ребер графа (в виде объекта смежности)
        private INode[] nodes;      // Набор всех узлов графа

        public DiseaseGraph()
        {
            categories = new List<CategoryNode>();
            diseases = new List<DiseaseNode>();
            symptoms = new List<SymptomNode>();
            links = new List<Link>();

            ParseData();
        }

        public string[] SearchAncestorsLink(string target)      // Метод для получения массива значений узлов-родителей
        {
            var queryLinks = links.Where(x => x.Target.Equals(target)).Select(x => x.Source);
            var querySources = nodes.Where(x => queryLinks.Contains(x.Id)).Select(x => x.Value);
            return querySources.ToArray();
        }

        public string[] SearchAncestorsLinkId(string target)        // Метод для получения массива Id узлов-родителей
        {
            var queryLinks = links.Where(x => x.Target.Equals(target)).Select(x => x.Source);
            return queryLinks.ToArray();
        }

        public string[] SearchDescendersLink(string source)     // Метод для получения массива значений узлов-детей
        {
            var queryLinks = links.Where(x => x.Source.Equals(source)).Select(x => x.Target);
            var querySources = nodes.Where(x => queryLinks.Contains(x.Id)).Select(x => x.Value);
            return querySources.ToArray();
        }

        public INode[] GetSymptomNodes()        // Получение массива симптомов
        {
            return symptoms.ToArray();
        }

        public INode[] GetDiseasesNodes()       // Получение массива проявлений
        {
            return diseases.ToArray();
        }

        public INode[] GetCategoryNodes()       // Получение массива категорий
        {
            return categories.ToArray();
        }

        public INode[] GetAllItems()        // Получение массива всех узлов
        {  
            return nodes;
        }
        public string GetNodeId(string value)       // Получение ID узла по его значению
        {
            return nodes.First(x => x.Value.Equals(value)).Id;
        }

        public string GetNodeValue(string id)       // Получение значения узла по его ID
        {
            return nodes.First(x => x.Id.Equals(id)).Value;
        }

        // Метод для парсинга данных из файла-описания графа
        private void ParseData()
        {
            var document = new XmlDocument();       // Создаем обработчик XML-документа
            var assets = Android.App.Application.Context.Assets;        // Открываем папку ассетов
            document.Load(assets.Open(AutoPsy.Const.Constants.GRAPH_NAME));     // считываем XML-Документ (предсталвение графа)

            // Считываем ребра графа
            var nodeLinks = document.SelectSingleNode("/*[local-name()='DirectedGraph']/*[local-name()='Links']");
            // Считываем узлы графа
            var nodes = document.SelectSingleNode("/*[local-name()='DirectedGraph']/*[local-name()='Nodes']");

            // Нарезаем считанные данные в структуру графа
            CutNodesAndLinks(nodeLinks, nodes);
        }

        // Метод для нарезания узлов и ребер графа в структуру графа
        private void CutNodesAndLinks(XmlNode links, XmlNode nodes)
        {
            foreach (XmlNode node in nodes)
                if (node.NodeType != XmlNodeType.Comment)       // При разрезке желательно игнорировать записи комментариев
                {
                    if (node.Attributes.Count == 2)     // Если узео имеет только два атрибута, то он является категорией
                        categories.Add(new CategoryNode(node.Attributes[0].Value, node.Attributes[1].Value));
                    else
                    {
                        if (node.Attributes["Category"].Value.Equals("Symptoms"))       // Если в категории атрибута указано, что это симптом, то создаем узел симтома
                            symptoms.Add(new SymptomNode(node.Attributes[0].Value, node.Attributes[1].Value));
                        else
                            diseases.Add(new DiseaseNode(node.Attributes[0].Value, node.Attributes[1].Value, node.Attributes[2].Value));        // Иначе - проявления
                    }
                }

            foreach (XmlNode node in links)     // Для каждой записи о ребре графа...
                this.links.Add(new Link(node.Attributes[0].Value, node.Attributes[1].Value));       // Создаем ребро и связываем два указанных узла

            this.nodes = new INode[categories.Count + diseases.Count + symptoms.Count];     // Объединяем наборы в общую структуру
            this.nodes = GetSymptomNodes().Union(GetDiseasesNodes().Union(GetCategoryNodes())).ToArray();
        }
    }

    public interface INode
    {
        string Id { get; }
        string Value { get; set; }
    }       // Общий интерфейс для узлов

    public class CategoryNode : INode
    {
        private string id, value;
        public CategoryNode(string id, string value)
        {
            this.id = id;
            this.value = value;
        }
        public string Id
        {
            get { return this.id; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }       // Класс узлов-категорий

    public class DiseaseNode : INode
    {
        private string id, value, category;

        public DiseaseNode(string id, string value, string category)
        {
            this.id = id;
            this.value = value;
            this.category = category;
        }

        public string Id
        {
            get { return id;}
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }
    }       // Класс узлов-проявлений

    public class SymptomNode : INode        // Класс узлов-симптомов
    {
        private string id, value;
        public SymptomNode(string id, string value)
        {
            this.id = id;
            this.value = value;
        }

        public string Id
        {
            get { return id;}
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    public class Link
    {
        private string idSource, idTarget;
        private string source, target;
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        public string Target
        {
            get { return target; }
            set { target = value; }
        }

        public Link(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
    }       // Класс ребер графа
}
