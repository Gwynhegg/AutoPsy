namespace AutoPsy.Database.Entities
{
    public interface INode
    {
        string Id { get; }
        string Value { get; set; }
    }       // Общий интерфейс для узлов

    public class CategoryNode : INode
    {
        private readonly string id;
        private string value;

        public CategoryNode(string id, string value)
        {
            this.id = id;
            this.value = value;
        }
        public string Id => this.id;

        public string Value
        {
            get => this.value;
            set => this.value = value;
        }
    }       // Класс узлов-категорий

    public class DiseaseNode : INode
    {
        private readonly string id;
        private string value;
        private string category;

        public DiseaseNode(string id, string value, string category)
        {
            this.id = id;
            this.value = value;
            this.category = category;
        }

        public string Id => this.id;
        public string Value
        {
            get => this.value;
            set => this.value = value;
        }

        public string Category
        {
            get => this.category;
            set => this.category = value;
        }
    }       // Класс узлов-проявлений

    public class SymptomNode : INode        // Класс узлов-симптомов
    {
        private readonly string id;
        private string value;

        public SymptomNode(string id, string value)
        {
            this.id = id;
            this.value = value;
        }

        public string Id => this.id;
        public string Value
        {
            get => this.value;
            set => this.value = value;
        }
    }

    public class Link
    {
        private string source, target;
        public string Source
        {
            get => this.source;
            set => this.source = value;
        }

        public string Target
        {
            get => this.target;
            set => this.target = value;
        }

        public Link(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
    }       // Класс ребер графа
}
