using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticsCanvasLine : ScrollView
    {
        DateTime start, end;
        private List<string> dates = new List<string>();
        public StatisticsCanvasLine(DateTime start, DateTime end)
        {
            InitializeComponent();
            this.start = start;
            this.end = end;
        }



        public void DrawBasicGraph(List<float> values)
        {
            var chartHandler = new Charts.StatLinearChartController();
            chartHandler.AddValuesToChart(values, start, end);
            ResultContainer.Children.Add(new ChartView() { Chart = chartHandler.GetChart(), HeightRequest = 300, VerticalOptions=LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand });
        }

        public void ShowBasicStatistic(List<string> values)
        {
            var stackLayout = new StackLayout() { HeightRequest = 300, Background = Brush.Transparent };
            foreach (var entry in values)
                stackLayout.Children.Add(new Label() { Text = entry, VerticalOptions =  LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand });
            ResultContainer.Children.Add(stackLayout);
        }

        public void ShowDynamicRange(List<float> values)
        {

            var label = new Label() { Text = AutoPsy.Resources.StatValues.DYNAMIC_RANGE_LABEL, Margin = 5, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
            var chartHandler = new Charts.StatLinearChartController();
            chartHandler.AddValuesToChart(values, start, end);
            ResultContainer.Children.Add(label);
            ResultContainer.Children.Add(new ChartView() { Chart = chartHandler.GetChart(), HeightRequest = 300, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand });
        }

        public void ShowDistributionRange(List<float> values)
        {
            var label = new Label() { Text = AutoPsy.Resources.StatValues.DISTRIBUTION_RANGE_LABEL, Margin = 5, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
            var chartHandler = new Charts.StatLinearChartController(values);
            var donutLabel = new Label() { Text = AutoPsy.Resources.StatValues.DISTRIBUTION_RATIO, Margin = 5, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
            var donutChartHandler = new Charts.DonutChartController(values);
            ResultContainer.Children.Add(label);
            ResultContainer.Children.Add(new ChartView() { Chart = chartHandler.GetChart(), HeightRequest = 300, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand });
            ResultContainer.Children.Add(donutLabel);
            ResultContainer.Children.Add(new ChartView() { Chart = donutChartHandler.GetChart(), HeightRequest = 300, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand });
        }

        public void ShowDistributionStatistic(List<string> values)
        {
            var stackLayout = new StackLayout() { HeightRequest = 200, Background = Brush.Transparent };
            foreach (var entry in values)
                stackLayout.Children.Add(new Label() { Text = entry, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand });
            ResultContainer.Children.Add(stackLayout);
        }
    }
}