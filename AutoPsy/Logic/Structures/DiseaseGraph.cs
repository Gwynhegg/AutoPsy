using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Android.Content.Res;
using System.Linq;

namespace AutoPsy.Logic.Structures
{
    public class DiseaseGraph
    {
        private List<CategoryNode> categories;
        private List<DiseaseNode> diseases;
        private List<SymptomNode> symptoms;
        private List<Link> links;
        private INode[] nodes;

        public DiseaseGraph()
        {
            categories = new List<CategoryNode>();
            diseases = new List<DiseaseNode>();
            symptoms = new List<SymptomNode>();
            links = new List<Link>();

            ParseData();
        }

        public string[] SearchAncestorsLink(string target)
        {
            var queryLinks = links.Where(x => x.Target.Equals(target)).Select(x => x.Source);
            var querySources = nodes.Where(x => queryLinks.Contains(x.Id)).Select(x => x.Value);
            return querySources.ToArray();
        }

        public string[] SearchAncestorsLinkId(string target)
        {
            var queryLinks = links.Where(x => x.Target.Equals(target)).Select(x => x.Source);
            return queryLinks.ToArray();
        }

        public string[] SearchDescendersLink(string source)
        {
            var queryLinks = links.Where(x => x.Source.Equals(source)).Select(x => x.Target);
            var querySources = nodes.Where(x => queryLinks.Contains(x.Id)).Select(x => x.Value);
            return querySources.ToArray();
        }

        public INode[] GetSymptomNodes()
        {
            return symptoms.ToArray();
        }

        public INode[] GetDiseasesNodes()
        {
            return diseases.ToArray();
        }

        public INode[] GetCategoryNodes()
        {
            return categories.ToArray();
        }

        public INode[] GetAllItems()
        {  
            return nodes;
        }
        public string GetNodeId(string value)
        {
            return nodes.First(x => x.Value.Equals(value)).Id;
        }


        private void ParseData()
        {
            var document = new XmlDocument();
            var assets = Android.App.Application.Context.Assets;
            document.Load(assets.Open(AutoPsy.Const.Constants.GRAPH_NAME));

            var nodeLinks = document.SelectSingleNode("/*[local-name()='DirectedGraph']/*[local-name()='Links']");
            var nodes = document.SelectSingleNode("/*[local-name()='DirectedGraph']/*[local-name()='Nodes']");

            CutNodesAndLinks(nodeLinks, nodes);
        }

        private void CutNodesAndLinks(XmlNode links, XmlNode nodes)
        {
            foreach (XmlNode node in nodes)
                if (node.NodeType != XmlNodeType.Comment) 
                {
                    if (node.Attributes.Count == 2)
                        categories.Add(new CategoryNode(node.Attributes[0].Value, node.Attributes[1].Value));
                    else
                    {
                        if (node.Attributes["Category"].Value.Equals("Symptoms"))
                            symptoms.Add(new SymptomNode(node.Attributes[0].Value, node.Attributes[1].Value));
                        else
                            diseases.Add(new DiseaseNode(node.Attributes[0].Value, node.Attributes[1].Value, node.Attributes[2].Value));
                    }
                }

            foreach (XmlNode node in links)
                this.links.Add(new Link(node.Attributes[0].Value, node.Attributes[1].Value));

            this.nodes = new INode[categories.Count + diseases.Count + symptoms.Count];
            this.nodes = GetSymptomNodes().Union(GetDiseasesNodes().Union(GetCategoryNodes())).ToArray();
        }
    }

    public interface INode
    {
        string Id { get; }
        string Value { get; set; }
    }

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
    }

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
    }

    public class SymptomNode : INode
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
    }
}
