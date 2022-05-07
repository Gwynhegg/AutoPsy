using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Database.Entities
{
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
            get { return id; }
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
            get { return id; }
        }
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }

    public class Link
    {
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
