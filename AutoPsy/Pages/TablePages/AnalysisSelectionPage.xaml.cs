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

        private async void SimplePrognosys_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SimpleRegressionPage(calculatedValues, start, end));
        }

        private async void FullCorellation_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new HeatMapPage(calculatedValues));
        }

        private async void ClusterAnalysis_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClusterHierarchyPage(calculatedValues));
        }

        protected override bool OnBackButtonPressed()
        {
            NavigateToMainAsync();
            return true;
        }

        private async Task NavigateToMainAsync()
        {
            await Navigation.PushModalAsync(new MainPage());

        }
    }
}