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
    public partial class AnalysisSelectionPage : ContentPage
    {
        private DateTime start, end;
        private Dictionary<string, List<float>> calculatedValues;
        public AnalysisSelectionPage(Dictionary<string, List<float>> calculatedValues, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.calculatedValues = calculatedValues;
            this.start = start;
            this.end = end;
        }


        private async void StatisticsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TableStatisticsPage(calculatedValues, start, end));
        }

        private void SimplePrognosys_Clicked(object sender, EventArgs e)
        {

        }

        private void FullRegression_Clicked(object sender, EventArgs e)
        {

        }

        private void DistributionAnalysis_Clicked(object sender, EventArgs e)
        {

        }

        private void ClusterAnalysis_Clicked(object sender, EventArgs e)
        {

        }

        private async void PoolAnalysis_Clicked(object sender, EventArgs e)
        {
        }
    }
}