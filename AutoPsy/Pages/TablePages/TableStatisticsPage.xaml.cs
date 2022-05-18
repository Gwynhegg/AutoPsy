using AutoPsy.Database.Entities;
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
    public partial class TableStatisticsPage : ContentPage
    {
        private Dictionary<string, List<float>> entityValues;
        private DateTime start, end;
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
            if (currentStep == 0) BackwardButton.IsEnabled = false; else BackwardButton.IsEnabled = true;
            if (currentStep == entityValues.Count - 1) ForwardButton.IsEnabled = false; else ForwardButton.IsEnabled = true;

            var currentItem = entityValues.ElementAt(currentStep);
            CurrentEntity.Text = currentItem.Key;

            var statisticProcessor = new Logic.StatisticProcessor();

            CurrentStatistics.Children.Clear();
            var statisticCanvas = new CustomComponents.StatisticsCanvasLine(start, end);
            CurrentStatistics.Children.Add(statisticCanvas);

            statisticCanvas.DrawBasicGraph(currentItem.Value);

            var isTrigger = App.TableGraph.GetParameterType(currentItem.Key).Equals(Const.Constants.ENTITY_TRIGGER) ? true: false;
            var basicStatistic = statisticProcessor.GetBasicStatistics(currentItem.Value, isTrigger);
            statisticCanvas.ShowBasicStatistic(basicStatistic);

            if (!isTrigger)
            {
                var dynamicRange = statisticProcessor.CalculateDynamicValue(currentItem.Value);
                statisticCanvas.ShowDynamicRange(dynamicRange);
            }


            var distributionRange = statisticProcessor.GetDistributionRange(isTrigger);
            statisticCanvas.ShowDistributionRange(distributionRange);
            if (!isTrigger)
            {
                var distributionStatistic = statisticProcessor.GetDistributionStatistic(distributionRange);
                statisticCanvas.ShowDistributionStatistic(distributionStatistic);
            }


        }
        private void BackwardButton_Clicked(object sender, EventArgs e)
        {
            currentStep--;
            SynchronizeCurrentStep();
        }

        private void ForwardButton_Clicked(object sender, EventArgs e)
        {
            currentStep++;
            SynchronizeCurrentStep();
        }

        private void SaveStatistics_Clicked(object sender, EventArgs e)
        {

        }
    }
}