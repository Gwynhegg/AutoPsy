using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPsy.CustomComponents;
using AutoPsy.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullVersionTablePage : ContentPage
    {
        private TableGridHandler recomendationsGridHandler;
        private TableGridHandler conditionsGridHandler;
        private TableGridHandler triggersGridHandler;

        private byte initialStep = 0;
        public FullVersionTablePage()
        {
            InitializeComponent();
            DateNavigationStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigationEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));

            DateNavigationStart.MaximumDate = DateTime.Now;
            DateNavigationEnd.MaximumDate = DateTime.Now.AddHours(1);

            DateNavigationStart.Date = DateTime.Now.Date;
            DateNavigationEnd.Date = DateTime.Now.Date;

            DateNavigationStart.Date = DateNavigationStart.Date.AddDays(-7);

            SynchronizeContentPage(initialStep);
        }

        private TableGridHandler RefreshTable(byte currentStep)
        {
            switch (currentStep)
            {
                case 0: recomendationsGridHandler = new CustomComponents.TableHandlers.RecomendationsGridHandler(DateNavigationStart.Date, DateNavigationEnd.Date);
                    return recomendationsGridHandler;

                case 1: conditionsGridHandler = new CustomComponents.TableHandlers.ConditionsGridHandler(DateNavigationStart.Date, DateNavigationEnd.Date);
                    return conditionsGridHandler;

                case 2: triggersGridHandler = new CustomComponents.TableHandlers.TriggersGridHandler(DateNavigationStart.Date, DateNavigationEnd.Date);
                    return triggersGridHandler;  

                default: return null;
            }
        }
        private void SynchronizeContentPage(byte currentStep)
        {
            if (initialStep == 0) StepBackward.IsEnabled = false; else StepBackward.IsEnabled = true;
            if (initialStep == 2) StepForward.IsEnabled = false; else StepForward.IsEnabled = true;

            var currentTable = RefreshTable(currentStep);

            if (currentTable.GetEntityHandler().CheckEntityExisted())
                FillHandlersInfo(currentTable);
        }

        private void FillHandlersInfo(TableGridHandler handler)
        {
            handler.FillTableInformation();

            MainField.Children.Clear();
            MainField.Children.Add(handler.mainGrid, 0, 0);
        }

        public void AddRecomendationParameter(string parameter, byte importance)
        {
            recomendationsGridHandler.AddParameter(parameter, importance);
            FillHandlersInfo(recomendationsGridHandler);
        }

        public void AddConditionParameter(string parameter, byte importance)
        {
            conditionsGridHandler.AddParameter(parameter, importance);
            FillHandlersInfo(conditionsGridHandler);

        }

        public void AddTriggerParameter(string parameter, byte importance)
        {
            triggersGridHandler.AddParameter(parameter, importance);
            FillHandlersInfo(triggersGridHandler);
        }

        private async void AddField_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ParameterCreationPage(this));
        }

        private void DateNavigationStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigationStart.Date <= DateNavigationEnd.Date)
                if ((DateNavigationEnd.Date - DateNavigationStart.Date).TotalDays > Const.Constants.WEEK)
                    DateNavigationEnd.Date = DateNavigationStart.Date.AddDays(Const.Constants.WEEK);
            else
                    DateNavigationEnd.Date = DateNavigationStart.Date.AddDays(Const.Constants.WEEK);

            SynchronizeContentPage(initialStep);

        }

        private void DateNavigationEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (DateNavigationEnd.Date >= DateNavigationStart.Date)
                if ((DateNavigationEnd.Date - DateNavigationStart.Date).TotalDays > Const.Constants.WEEK)
                    DateNavigationStart.Date = DateNavigationEnd.Date.AddDays(-Const.Constants.WEEK);
                else
                    DateNavigationStart.Date = DateNavigationEnd.Date.AddDays(-Const.Constants.WEEK);

            SynchronizeContentPage(initialStep);

        }

        private void StepBackward_Clicked(object sender, EventArgs e)
        {
            SynchronizeContentPage(--initialStep);
        }

        private void StepForward_Clicked(object sender, EventArgs e)
        {
            SynchronizeContentPage(++initialStep);
        }
    }
}