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
using System.ComponentModel;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullVersionTablePage : ContentPage
    {
        private TableGridHandler recomendationsGridHandler;
        private TableGridHandler conditionsGridHandler;
        private TableGridHandler triggersGridHandler;

        private byte initialStep = 0;
        public byte InitialStep
        {
            get { return initialStep; }
        }
        public FullVersionTablePage()
        {
            InitializeComponent();
            DateNavigationStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            DateNavigationEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));

            DateNavigationStart.MaximumDate = DateTime.Now;
            DateNavigationEnd.MaximumDate = DateTime.Now.AddHours(1);

            DateNavigationStart.Date = DateTime.Now.Date;
            DateNavigationEnd.Date = DateTime.Now.Date;

            recomendationsGridHandler = new CustomComponents.TableHandlers.RecomendationsGridHandler(this, DateNavigationStart.Date.AddDays(-7), DateNavigationEnd.Date);
            conditionsGridHandler = new CustomComponents.TableHandlers.ConditionsGridHandler(this, DateNavigationStart.Date.AddDays(-7), DateNavigationEnd.Date);
            triggersGridHandler = new CustomComponents.TableHandlers.TriggersGridHandler(this, DateNavigationStart.Date.AddDays(-7), DateNavigationEnd.Date);

            DateNavigationStart.Date = DateNavigationStart.Date.AddDays(-7);

            SynchronizeContentPage(initialStep);
        }

        private TableGridHandler RefreshTable(byte currentStep)
        {
            switch (currentStep)
            {
                case 0: return recomendationsGridHandler;
                case 1: return conditionsGridHandler;
                case 2: return triggersGridHandler;  
                default: return null;
            }
        }
        public void SynchronizeContentPage(byte currentStep)
        {
            var currentTable = RefreshTable(currentStep);
          
            if (currentTable.GetEntityHandler().CheckEntityExisted())
            {
                initialStep = currentStep;
                FillHandlersInfo(currentTable);
            }
        }

        private void FillHandlersInfo(TableGridHandler handler)
        {
            handler.FillTableInformation();

            MainField.Children.Clear();
            MainField.Children.Add(handler.mainGrid, 0, 0);
        }

        public void AddRecomendationParameter(string parameter)
        {
            recomendationsGridHandler.AddParameter(parameter);
            initialStep = 0;
            FillHandlersInfo(recomendationsGridHandler);
        }

        public void AddConditionParameter(string parameter)
        {
            conditionsGridHandler.AddParameter(parameter);
            initialStep = 1;
            FillHandlersInfo(conditionsGridHandler);

        }

        public void AddTriggerParameter(string parameter)
        {
            triggersGridHandler.AddParameter(parameter);
            initialStep = 2;
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

            var table = RefreshTable(initialStep);
            table.UpdateDateTimes(DateNavigationStart.Date, DateNavigationEnd.Date);
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

 
        private void RecomendationsButton_Clicked(object sender, EventArgs e)
        {
            initialStep = 0;
            SynchronizeContentPage(initialStep);
        }

        private void TriggersButton_Clicked(object sender, EventArgs e)
        {
            initialStep = 2;
            SynchronizeContentPage(initialStep);
        }

        private void CondtionsButton_Clicked(object sender, EventArgs e)
        {
            initialStep = 1;
            SynchronizeContentPage(initialStep);
        }

        private async void AnalyzeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PreProcessingPage());
        }
    }
}