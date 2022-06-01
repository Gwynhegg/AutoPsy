using System;
using System.Collections.Generic;
using System.Linq;

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

            List<(float, int, int)> correlationMatrix = CreateCorrelationMatrix(values);
            CreateMatrixGrid(correlationMatrix, values.Count, values.Keys.Select(x => App.TableGraph.GetNameByIdString(x)).ToList());

        }

        private void CreateMatrixGrid(List<(float, int, int)> matrix, int matrixSize, List<string> labels)
        {

            for (var i = 0; i < matrixSize + 1; i++)
            {
                this.MatrixContainer.RowDefinitions.Add(new RowDefinition() { Height = 100 });
                this.MatrixContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = 100 });
            }

            for (var i = 0; i < matrixSize; i++)
            {
                this.MatrixContainer.Children.Add(new Button() { Text = labels[i], VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#74A594"), TextColor = Color.White, FontSize = 14, Padding = 0.5 }, i + 1, 0);
                this.MatrixContainer.Children.Add(new Button() { Text = labels[i], VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#74A594"), TextColor = Color.White, FontSize = 14, Padding = 0.5 }, 0, i + 1);
            }

            foreach ((float, int, int) row in matrix)
            {
                var leftBorder = row.Item3 + 1;
                var upperBorder = row.Item2 + 1;
                var value = (float)((int)(Math.Abs(row.Item1) * 20 + 0.0499999)) / 20;

                this.MatrixContainer.Children.Add(new Button() { Text = value.ToString("F2"), BackgroundColor = AuxServices.ColorPicker.MatrixCriticityColor[value], FontSize = 20, TextColor = Color.White }, leftBorder, upperBorder);
                this.MatrixContainer.Children.Add(new Button() { Text = value.ToString("F2"), BackgroundColor = AuxServices.ColorPicker.MatrixCriticityColor[value], FontSize = 20, TextColor = Color.White }, upperBorder, leftBorder);
            }
        }


        private List<(float, int, int)> CreateCorrelationMatrix(Dictionary<string, List<float>> values)     // вычисление коэффициента корреляции
        {
            var correlationMatrix = new List<(float, int, int)>();      // создание матричцы корреляции

            for (var i = 0; i < values.Count; i++)
            {
                for (var j = i + 1; j < values.Count; j++)
                {
                    var correlationValue = Logic.CorrelationProcessor.CalculateCorrelationValue(values.ElementAt(i).Value, values.ElementAt(j).Value);      // вычисление значения корреляции
                    correlationMatrix.Add((correlationValue, i, j));
                }
            }

            return correlationMatrix;
        }
    }
}