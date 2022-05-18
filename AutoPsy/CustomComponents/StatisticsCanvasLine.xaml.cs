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

            SetDatesRange();
        }

        private void SetDatesRange()        // метод для создания временного интервала и заполняющих его значений
        {
            for (DateTime i = start.Date; i <= end.Date; i = i.AddDays(1))
            {
                var day = i.Day.ToString().Length < 2 ? String.Concat("0", i.Day) : i.Day.ToString();       // получаем строку для отображения дня
                var month = i.Month.ToString().Length < 2 ? String.Concat("0", i.Month) : i.Month.ToString();       // получаем строку для отображения месяца
                dates.Add(String.Concat(day, ".", month));      // соединяем строки и помещаем в новый столбец
            }
        }

        public void DrawBasicGraph(List<float> values)
        {
            var chartHandler = new Charts.StatLinearChartController(values, dates);
            chartHandler.CreateChart();
            ResultContainer.Children.Add(new ChartView() { Chart = chartHandler.GetChart(), HeightRequest = 150 });
        }

        public void ShowBasicStatistic(List<string> values)
        {
            var stackLayout = new StackLayout() { HeightRequest = 300 };
            foreach (var entry in values)
                stackLayout.Children.Add(new Label() { Text = entry });
            ResultContainer.Children.Add(stackLayout);
        }

        public void ShowDynamicRange(List<float> values)
        {

            var label = new Label() { Text = AutoPsy.Resources.StatValues.DYNAMIC_RANGE_LABEL };
            var chartHandler = new Charts.StatLinearChartController(values, dates);
            chartHandler.CreateChart();
            ResultContainer.Children.Add(label);
            ResultContainer.Children.Add(new ChartView() { Chart = chartHandler.GetChart(), HeightRequest = 300 });
        }

        public void ShowDistributionRange(List<float> values)
        {
            var label = new Label() { Text = AutoPsy.Resources.StatValues.DISTRIBUTION_RANGE_LABEL };
            var chartHandler = new Charts.StatLinearChartController(values);
            chartHandler.CreateChart();
            var donutLabel = new Label() { Text = AutoPsy.Resources.StatValues.DISTRIBUTION_RATIO };
            var donutChartHandler = new Charts.DonutChartController(values);
            donutChartHandler.CreateChart();
            ResultContainer.Children.Add(label);
            ResultContainer.Children.Add(new ChartView() { Chart = chartHandler.GetChart(), HeightRequest = 300 });
            ResultContainer.Children.Add(donutLabel);
            ResultContainer.Children.Add(new ChartView() { Chart = donutChartHandler.GetChart(), HeightRequest = 300 });
        }

        public void ShowDistributionStatistic(List<string> values)
        {
            var stackLayout = new StackLayout() { HeightRequest = 300 };
            foreach (var entry in values)
                stackLayout.Children.Add(new Label() { Text = entry });
            ResultContainer.Children.Add(stackLayout);
        }
    }
}