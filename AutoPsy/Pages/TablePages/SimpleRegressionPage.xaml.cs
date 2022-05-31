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
    public partial class SimpleRegressionPage : ContentPage
    {
        private Dictionary<string, List<float>> entityValues;
        private DateTime start, end;
        private int epochsValue = 3;
        private int currentStep = 0;

        public SimpleRegressionPage(Dictionary<string, List<float>> entityValues, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.entityValues = entityValues;
            this.start = start;
            this.end = end;

            PrognosisText.Text = String.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, EpochsValue.Value);

            SynchronizeCurrentStep();
        }

        private void SynchronizeCurrentStep()
        {
            if (currentStep == 0) BackwardButton.IsEnabled = false; else BackwardButton.IsEnabled = true;
            if (currentStep == entityValues.Count - 1) ForwardButton.IsEnabled = false; else ForwardButton.IsEnabled = true;

            var currentItem = entityValues.ElementAt(currentStep);
            CurrentEntity.Text = App.TableGraph.GetNameByIdString(currentItem.Key);

            var isTrigger = App.TableGraph.GetParameterType(currentItem.Key).Equals(Const.Constants.ENTITY_TRIGGER) ? true : false;

            var r = Logic.LinearRegression.CreateHoltWintersRegression(currentItem.Value, 0.3f, 0.5f, 0.7f, epochsValue, isTrigger);
            var chartController = new CustomComponents.Charts.LinearChartController();
            chartController.AddValuesToChart(currentItem.Value, start, end);
            chartController.ChangeColor();
            chartController.AddValuesToChart(r, end.AddDays(1), end.AddDays(1 + epochsValue));
            RegressionChart.Chart = chartController.GetChart();
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

        private void EpochsValue_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            epochsValue = (int)EpochsValue.Value;
            PrognosisText.Text = String.Format(AutoPsy.Resources.DiaryAnalysisResources.PrognosisFar, EpochsValue.Value);
            SynchronizeCurrentStep();
        }
    }
}