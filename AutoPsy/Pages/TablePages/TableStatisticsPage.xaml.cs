using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableStatisticsPage : ContentPage
    {
        private readonly Dictionary<string, List<float>> entityValues;
        private readonly DateTime start, end;
        private int currentStep = 0;
        public TableStatisticsPage(Dictionary<string, List<float>> entityValues, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.entityValues = entityValues;
            this.start = start;
            this.end = end;

            SynchronizeCurrentStep();
        }

        private void SynchronizeCurrentStep()
        {
            if (this.currentStep == 0) this.BackwardButton.IsEnabled = false; else this.BackwardButton.IsEnabled = true;
            if (this.currentStep == this.entityValues.Count - 1) this.ForwardButton.IsEnabled = false; else this.ForwardButton.IsEnabled = true;

            KeyValuePair<string, List<float>> currentItem = this.entityValues.ElementAt(this.currentStep);
            this.CurrentEntity.Text = App.TableGraph.GetNameByIdString(currentItem.Key);

            var statisticProcessor = new Logic.StatisticProcessor();

            this.CurrentStatistics.Children.Clear();
            var statisticCanvas = new CustomComponents.StatisticsCanvasLine(this.start, this.end);
            this.CurrentStatistics.Children.Add(statisticCanvas);

            statisticCanvas.DrawBasicGraph(currentItem.Value);

            var isTrigger = App.TableGraph.GetParameterType(currentItem.Key).Equals(Const.Constants.ENTITY_TRIGGER) ? true : false;
            List<string> basicStatistic = statisticProcessor.GetBasicStatistics(currentItem.Value, isTrigger);
            statisticCanvas.ShowBasicStatistic(basicStatistic);

            if (!isTrigger)
            {
                List<float> dynamicRange = statisticProcessor.CalculateDynamicValue(currentItem.Value);
                statisticCanvas.ShowDynamicRange(dynamicRange);
            }


            List<float> distributionRange = statisticProcessor.GetDistributionRange(isTrigger);
            statisticCanvas.ShowDistributionRange(distributionRange);
            if (!isTrigger)
            {
                List<string> distributionStatistic = statisticProcessor.GetDistributionStatistic(distributionRange);
                statisticCanvas.ShowDistributionStatistic(distributionStatistic);
            }


        }
        private void BackwardButton_Clicked(object sender, EventArgs e)
        {
            this.currentStep--;
            SynchronizeCurrentStep();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            this.currentStep++;
            SynchronizeCurrentStep();
        }

        private void SaveStatistics_Clicked(object sender, EventArgs e)
        {

        }
    }
}