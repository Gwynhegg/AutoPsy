using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClusterHierarchyPage : ContentPage
    {
        private int numOfElements = 2;
        private readonly Dictionary<string, float> tree;

        public ClusterHierarchyPage(Dictionary<string, List<float>> calculatedValues)
        {
            InitializeComponent();

            this.tree = Logic.ClusterHierarchy.CreateHierarchyTree(calculatedValues);

            this.NumStepper.Maximum = GetMaximumLength();
            this.NumOfElements.Text = string.Format(AutoPsy.Resources.AnalysisResources.NumOfElements, this.numOfElements);

            GraphTree();
        }

        private async void GraphTree()
        {
            this.Container.Children.Clear();

            List<string> elementsOfTree = GetElementsOfSize();

            foreach (var element in elementsOfTree)
                this.Container.Children.Add(new Label() { Text = element, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 20 });
            if (elementsOfTree == null || elementsOfTree.Count == 0) this.Container.Children.Add(new Label() { Text = AutoPsy.Resources.AnalysisResources.NoClusters, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 20 });
        }

        private int GetMaximumLength()
        {
            var max = 1;
            foreach (KeyValuePair<string, float> item in this.tree)
            {
                var query = item.Key.Count(x => x == '+') + 1;
                if (query > max) max = query;
            }

            return max;
        }

        private List<string> GetElementsOfSize()
        {
            IEnumerable<KeyValuePair<string, float>> elements = this.tree.Where(x => x.Key.Count(y => y == '+') == this.numOfElements - 1);
            var list = new List<string>();
            foreach (KeyValuePair<string, float> item in elements)
                list.Add(string.Concat(item.Key, " \n ", string.Format(AutoPsy.Resources.AnalysisResources.Shortage, item.Value.ToString("F2"))));
            return list;
        }

        private void EpochsValue_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.numOfElements = (int)this.NumStepper.Value;
            this.NumOfElements.Text = string.Format(AutoPsy.Resources.AnalysisResources.NumOfElements, this.numOfElements);
            GraphTree();
        }
    }
}