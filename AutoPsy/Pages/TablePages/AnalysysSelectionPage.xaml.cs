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
    public partial class AnalysysSelectionPage : ContentPage
    {
        private Dictionary<string, List<ITableEntity>> entityValues = new Dictionary<string, List<ITableEntity>>();
        private List<TableEntityHandler> handlers = new List<TableEntityHandler>()
        {
            new RecomendationTableEntityHandler(),
            new CondtiionTableEntityHandler(),
            new TriggerTableEntityHandler()
        };
        public AnalysysSelectionPage()
        {
            InitializeComponent();
            DateNavigationStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigationEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));

            DateNavigationStart.MaximumDate = DateTime.Now;
            DateNavigationEnd.MaximumDate = DateTime.Now.AddHours(1);

            DateNavigationStart.Date = DateTime.Now.Date;
            DateNavigationEnd.Date = DateTime.Now.Date;

            DateNavigationStart.Date = DateNavigationStart.Date.AddDays(-7);

        }

        public void SynchronizeEntities()
        {
            entityValues.Clear();
            foreach (var handler in handlers)
                foreach (var pair in handler.GetValues(DateNavigationStart.Date, DateNavigationEnd.Date))
                    entityValues.Add(pair.Key, pair.Value);
        }

        private void DateNavigationStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigationStart.Date > DateNavigationEnd.Date) DateNavigationStart.Date = DateNavigationEnd.Date;
            SynchronizeEntities();
        }

        private void DateNavigationEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigationEnd.Date < DateNavigationStart.Date) DateNavigationEnd.Date = DateNavigationStart.Date;
            SynchronizeEntities();
        }

        private async void StatisticsButton_Clicked(object sender, EventArgs e)
        {

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
            await Navigation.PushModalAsync(new PoolAnalysisPage(this, entityValues, DateNavigationStart.Date, DateNavigationEnd.Date));
        }
    }
}