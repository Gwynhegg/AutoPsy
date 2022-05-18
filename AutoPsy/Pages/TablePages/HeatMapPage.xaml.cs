using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeatMapPage : ContentPage
    {
        public HeatMapPage(Dictionary<string, List<float>> values)
        {
            InitializeComponent();

            var correlationMatrix = CreateCorrelationMatrix(values);
            CreateMatrixGrid(correlationMatrix, values.Count - 1);
            
        }

        private void CreateMatrixGrid(List<(string, string, float, int, int)> matrix, int matrixSize)
        {
            MatrixContainer.RowDefinitions.Add(new RowDefinition() { Height = 200 });
            MatrixContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = 200 });

            for (int i = 0; i < matrixSize + 1; i++)
            {
                MatrixContainer.RowDefinitions.Add(new RowDefinition() { Height = 100 });
                MatrixContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = 100 });
            }

            foreach (var row in matrix)
            {
                var leftBorder = row.Item5;
                var upperBorder = row.Item4;

                MatrixContainer.Children.Add(new Label() { Text = row.Item2}, leftBorder + 1, 0);
                MatrixContainer.Children.Add(new Label() { Text = row.Item1}, 0, upperBorder + 1); 

                var value = (float)((int)(Math.Abs(row.Item3) * 20 + 0.0499999)) / 20;

                MatrixContainer.Children.Add(new Button() { Text = row.Item1 + row.Item2, BackgroundColor = AuxServices.ColorPicker.MatrixCriticityColor[value]}, leftBorder + 1, upperBorder + 1);
            }
        }


        private List<(string, string, float, int, int)> CreateCorrelationMatrix(Dictionary<string, List<float>> values)
        {
            var correlationMatrix = new List<(string, string, float, int, int)>();

            for (int i = 0; i < values.Count; i++)
                for (int j = i + 1; j < values.Count; j++)
                {
                    var correlationValue = Logic.CorrelationProcessor.CalculateCorrelationValue(values.ElementAt(i).Value, values.ElementAt(j).Value);
                    correlationMatrix.Add((values.ElementAt(i).Key, values.ElementAt(j).Key, correlationValue, i, j));
                }
            return correlationMatrix;
        }
    }
}