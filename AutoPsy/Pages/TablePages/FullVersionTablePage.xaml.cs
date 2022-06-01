using AutoPsy.CustomComponents;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullVersionTablePage : ContentPage
    {
        private readonly TableGridHandler recomendationsGridHandler;
        private readonly TableGridHandler conditionsGridHandler;
        private readonly TableGridHandler triggersGridHandler;

        private byte initialStep = 0;
        public byte InitialStep => this.initialStep;
        public FullVersionTablePage()
        {
            InitializeComponent();
            this.DateNavigationStart.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));
            this.DateNavigationEnd.MinimumDate = DateTime.Now - (DateTime.Now - new DateTime(1950, 1, 1));

            this.DateNavigationStart.MaximumDate = DateTime.Now;
            this.DateNavigationEnd.MaximumDate = DateTime.Now.AddHours(1);

            this.DateNavigationStart.Date = DateTime.Now.Date;
            this.DateNavigationEnd.Date = DateTime.Now.Date;

            this.recomendationsGridHandler = new CustomComponents.TableHandlers.RecomendationsGridHandler(this, this.DateNavigationStart.Date.AddDays(-7), this.DateNavigationEnd.Date);
            this.conditionsGridHandler = new CustomComponents.TableHandlers.ConditionsGridHandler(this, this.DateNavigationStart.Date.AddDays(-7), this.DateNavigationEnd.Date);
            this.triggersGridHandler = new CustomComponents.TableHandlers.TriggersGridHandler(this, this.DateNavigationStart.Date.AddDays(-7), this.DateNavigationEnd.Date);

            this.DateNavigationStart.Date = this.DateNavigationStart.Date.AddDays(-7);

            SynchronizeContentPage(this.initialStep);
        }

        private TableGridHandler RefreshTable(byte currentStep)
        {
            switch (currentStep)
            {
                case 0: return this.recomendationsGridHandler;
                case 1: return this.conditionsGridHandler;
                case 2: return this.triggersGridHandler;
                default: return null;
            }
        }
        public void SynchronizeContentPage(byte currentStep)
        {
            TableGridHandler currentTable = RefreshTable(currentStep);

            if (currentTable.GetEntityHandler().CheckEntityExisted())
            {
                this.initialStep = currentStep;
                FillHandlersInfo(currentTable);
            }
        }

        private void FillHandlersInfo(TableGridHandler handler)
        {
            handler.FillTableInformation();

            this.MainField.Children.Clear();
            this.MainField.Children.Add(handler.mainGrid, 0, 0);
        }

        public void AddRecomendationParameter(string parameter)
        {
            this.recomendationsGridHandler.AddParameter(parameter);
            this.initialStep = 0;
            FillHandlersInfo(this.recomendationsGridHandler);
        }

        public void AddConditionParameter(string parameter)
        {
            this.conditionsGridHandler.AddParameter(parameter);
            this.initialStep = 1;
            FillHandlersInfo(this.conditionsGridHandler);

        }

        public void AddTriggerParameter(string parameter)
        {
            this.triggersGridHandler.AddParameter(parameter);
            this.initialStep = 2;
            FillHandlersInfo(this.triggersGridHandler);
        }

        private async void AddField_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new ParameterCreationPage(this));

        private void DateNavigationStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (this.DateNavigationStart.Date <= this.DateNavigationEnd.Date)
            {
                if ((this.DateNavigationEnd.Date - this.DateNavigationStart.Date).TotalDays > Const.Constants.WEEK)
                    this.DateNavigationEnd.Date = this.DateNavigationStart.Date.AddDays(Const.Constants.WEEK);
                else
                    this.DateNavigationEnd.Date = this.DateNavigationStart.Date.AddDays(Const.Constants.WEEK);
            }

            TableGridHandler table = RefreshTable(this.initialStep);
            table.UpdateDateTimes(this.DateNavigationStart.Date, this.DateNavigationEnd.Date);
            SynchronizeContentPage(this.initialStep);

        }

        private void DateNavigationEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (this.DateNavigationEnd.Date >= this.DateNavigationStart.Date)
            {
                if ((this.DateNavigationEnd.Date - this.DateNavigationStart.Date).TotalDays > Const.Constants.WEEK)
                    this.DateNavigationStart.Date = this.DateNavigationEnd.Date.AddDays(-Const.Constants.WEEK);
                else
                    this.DateNavigationStart.Date = this.DateNavigationEnd.Date.AddDays(-Const.Constants.WEEK);
            }

            SynchronizeContentPage(this.initialStep);

        }


        private void RecomendationsButton_Clicked(object sender, EventArgs e)
        {
            this.initialStep = 0;
            SynchronizeContentPage(this.initialStep);
        }

        private void TriggersButton_Clicked(object sender, EventArgs e)
        {
            this.initialStep = 2;
            SynchronizeContentPage(this.initialStep);
        }

        private void CondtionsButton_Clicked(object sender, EventArgs e)
        {
            this.initialStep = 1;
            SynchronizeContentPage(this.initialStep);
        }

        private async void AnalyzeButton_Clicked(object sender, EventArgs e) => await this.Navigation.PushModalAsync(new PreProcessingPage());
    }
}