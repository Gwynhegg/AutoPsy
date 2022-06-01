using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimpleRegressionPage : ContentPage
    {
        private readonly Dictionary<string, List<float>> entityValues;
        private readonly DateTime start, end;
        private int epochsValue = 3;
        private int currentStep = 0;

        public SimpleRegressionPage(Dictionary<string, List<float>> entityValues, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.entityValues = entityValues;
            this.start = start;
            this.end = end;

            this.PrognosisText.Text = string.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, this.EpochsValue.Value);

            SynchronizeCurrentStep();
        }

        private void SynchronizeCurrentStep()
        {
            if (this.currentStep == 0) this.BackwardButton.IsEnabled = false; else this.BackwardButton.IsEnabled = true;
            if (this.currentStep == this.entityValues.Count - 1) this.ForwardButton.IsEnabled = false; else this.ForwardButton.IsEnabled = true;

            KeyValuePair<string, List<float>> currentItem = this.entityValues.ElementAt(this.currentStep);
            this.CurrentEntity.Text = App.TableGraph.GetNameByIdString(currentItem.Key);

            var isTrigger = App.TableGraph.GetParameterType(currentItem.Key).Equals(Const.Constants.ENTITY_TRIGGER) ? true : false;

            List<float> r = Logic.LinearRegression.CreateHoltWintersRegression(currentItem.Value, 0.3f, 0.5f, 0.7f, this.epochsValue, isTrigger);
            var chartController = new CustomComponents.Charts.LinearChartController();
            chartController.AddValuesToChart(currentItem.Value, this.start, this.end);
            chartController.ChangeColor();
            chartController.AddValuesToChart(r, this.end.AddDays(1), this.end.AddDays(1 + this.epochsValue));
            this.RegressionChart.Chart = chartController.GetChart();
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

        private void EpochsValue_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.epochsValue = (int)this.EpochsValue.Value;
            this.PrognosisText.Text = string.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, this.EpochsValue.Value);
            SynchronizeCurrentStep();
        }
    }
}