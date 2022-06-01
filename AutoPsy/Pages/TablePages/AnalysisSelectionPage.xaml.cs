using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalysisSelectionPage : ContentPage
    {
        private readonly DateTime start, end;
        private readonly Dictionary<string, List<float>> calculatedValues;
        public AnalysisSelectionPage(Dictionary<string, List<float>> calculatedValues, DateTime start, DateTime end)
        {
            InitializeComponent();
            this.calculatedValues = calculatedValues;
            this.start = start;
            this.end = end;
        }


        private async void StatisticsButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new TableStatisticsPage(this.calculatedValues, this.start, this.end));

        private async void SimplePrognosys_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new SimpleRegressionPage(this.calculatedValues, this.start, this.end));

        private async void FullCorellation_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new HeatMapPage(this.calculatedValues));

        private async void ClusterAnalysis_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new ClusterHierarchyPage(this.calculatedValues));

        protected override bool OnBackButtonPressed()
        {
            NavigateToMainAsync();
            return true;
        }

        private async Task NavigateToMainAsync() => await this.Navigation.PushModalAsync(new MainPage());
    }
}