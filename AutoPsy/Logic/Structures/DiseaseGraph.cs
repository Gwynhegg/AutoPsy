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
        private HashSet<CategoryNode> categories;
        private HashSet<DiseaseNode> diseases;
        private HashSet<SymptomNode> symptoms;
        private HashSet<Link> links;

        public DiseaseGraph()
        {
            categories = new HashSet<CategoryNode>();
            diseases = new HashSet<DiseaseNode>();
            symptoms = new HashSet<SymptomNode>();
            links = new HashSet<Link>();

            ParseData();
        }

        public string[] GetSymptomNodes()
        {
            string[] nodes = new string[symptoms.Count];
            int iterator = 0;
            foreach (var node in symptoms)
                nodes[iterator++] = node.Value;
            return nodes;
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
        }
    }

    public class CategoryNode
    {
        private string id, value;
        public CategoryNode(string id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }

    public class DiseaseNode
    {
        private string id, value, category;

        public DiseaseNode(string id, string value, string category)
        {
            this.id = id;
            this.value = value;
            this.category = category;
        }
    }

    public class SymptomNode
    {
        private string id, value;
        public SymptomNode(string id, string value)
        {
            this.id = id;
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }
    }

    public class Link
    {
        private string source, target;

        public Link(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
    }
}
