using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClusterHierarchyPage : ContentPage
    {
        private int numOfElements = 2;
        private Dictionary<string, float> tree;

        public ClusterHierarchyPage(Dictionary<string, List<float>> calculatedValues)
        {
            InitializeComponent();

            tree = Logic.ClusterHierarchy.CreateHierarchyTree(calculatedValues);

            NumStepper.Maximum = GetMaximumLength();
            NumOfElements.Text = String.Format(AutoPsy.Resources.AnalysisResources.NumOfElements, numOfElements);

            GraphTree();
        }

        private async void GraphTree()
        {
            Container.Children.Clear();

            var elementsOfTree = GetElementsOfSize();
            
            foreach (var element in elementsOfTree)
                Container.Children.Add(new Label() { Text = element, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 20});
            if (elementsOfTree == null || elementsOfTree.Count == 0) Container.Children.Add(new Label() { Text = AutoPsy.Resources.AnalysisResources.NoClusters, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = 20 });
        }

        private int GetMaximumLength()
        {
            var max = 1;
            foreach (var item in tree)
            {
                var query = item.Key.Count(x => x == '+') + 1;
                if (query > max) max = query;
            }

            return max;
        }

        private List<string> GetElementsOfSize()
        {
            var elements = tree.Where(x => x.Key.Count(y => y == '+') == numOfElements - 1);
            var list = new List<string>();
            foreach (var item in elements)
                list.Add(String.Concat(item.Key, " \n ", String.Format(AutoPsy.Resources.AnalysisResources.Shortage, item.Value.ToString("F2"))));
            return list;
        }

        private void EpochsValue_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            numOfElements = (int)NumStepper.Value;
            NumOfElements.Text = String.Format(AutoPsy.Resources.AnalysisResources.NumOfElements, numOfElements);
            GraphTree();
        }
    }
}