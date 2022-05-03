using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPsy.CustomComponents;

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

        public FullVersionTablePage()
        {
            InitializeComponent();


            DateNavigationEnd.Date = DateTime.Now.Date;
            DateNavigationStart.Date = DateTime.Now.Date.AddDays(-5);

            recomendationsGridHandler = new CustomComponents.TableHandlers.RecomendationsGridHandler("Рекомендации", DateNavigationStart.Date, DateNavigationEnd.Date);
            conditionsGridHandler = new CustomComponents.TableHandlers.RecomendationsGridHandler("Состояния", DateNavigationStart.Date, DateNavigationEnd.Date);
            triggersGridHandler = new CustomComponents.TableHandlers.TriggersGridHandler("Триггеры", DateNavigationStart.Date, DateNavigationEnd.Date);

            SynchronizeContentPages();
        }

        private void SynchronizeContentPages()
        {

            if (recomendationsGridHandler.GetEntityHandler().CheckEntityExisted<Database.Entities.TableRecomendation>())
                FillHandlersInfo(recomendationsGridHandler);

            if (conditionsGridHandler.GetEntityHandler().CheckEntityExisted<Database.Entities.TableCondition>())
                FillHandlersInfo(conditionsGridHandler);

            conditionsGridHandler.FillTableInformation();

            if (triggersGridHandler.GetEntityHandler().CheckEntityExisted<Database.Entities.TableTrigger>())
                FillHandlersInfo(triggersGridHandler);
        }

        private void FillHandlersInfo(TableGridHandler handler)
        {
            handler.FillTableInformation();
            MainField.RowDefinitions.Add(new RowDefinition());
            MainField.Children.Add(handler.mainGrid, 0, MainField.RowDefinitions.Count - 1);
        }

        public void GetParameter(string parameter)
        {
            //entityHandler.AddParameter(parameter);
            SynchronizeContentPages();
        }

        private async void AddField_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ParameterCreationPage(this));
        }

        private void DeleteField_Clicked(object sender, EventArgs e)
        {

        }
    }
}