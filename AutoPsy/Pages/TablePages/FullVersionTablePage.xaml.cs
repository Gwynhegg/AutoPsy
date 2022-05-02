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
    public partial class FullVersionTablePage : ContentPage
    {
        private Database.Entities.TableEntityHandler entityHandler;
        public FullVersionTablePage()
        {
            InitializeComponent();
            entityHandler = new Database.Entities.TableEntityHandler();

            DateNavigationEnd.Date = DateTime.Now.Date;
            DateNavigationStart.Date = DateTime.Now.Date.AddDays(-5);

            SynchronizeContentPages();
        }

        private void SynchronizeContentPages()
        {
            MainField.ColumnDefinitions.Clear();
            MainField.RowDefinitions.Clear();

            MainField.RowDefinitions.Add(new RowDefinition() { Height = 50 });
            MainField.ColumnDefinitions.Add(new ColumnDefinition() { Width = 200 });
            int iterator = 1;

            for (DateTime i = DateNavigationStart.Date; i <= DateNavigationEnd.Date; i = i.AddDays(1))
            {
                MainField.ColumnDefinitions.Add(new ColumnDefinition() { Width = 50 });
                MainField.Children.Add(new Label() { Text = i.ToShortDateString() }, iterator++, 0);
            }

            if (entityHandler.CheckEntityExisted<Database.Entities.TableRecomendation>()) 
            {
                MainField.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star)});
                MainField.Children.Add(new Label() { Text = "Рекомендации" }, MainField.RowDefinitions.Count - 1, 0);
                var filterResults = entityHandler.SelectRecomendations(DateNavigationStart.Date, DateNavigationEnd.Date);
                GetDateTimeResults(filterResults);
            }

            if (entityHandler.CheckEntityExisted<Database.Entities.TableCondition>())
            {
                MainField.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                MainField.Children.Add(new Label() { Text = "Состояния" }, MainField.RowDefinitions.Count - 1, 0);
                var filterResults = entityHandler.SelectConditions(DateNavigationStart.Date, DateNavigationEnd.Date);
                GetDateTimeResults(filterResults);
            }

            if (entityHandler.CheckEntityExisted<Database.Entities.TableTrigger>())
            {
                MainField.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                MainField.Children.Add(new Label() { Text = "Триггеры" }, MainField.RowDefinitions.Count - 1, 0);
                var filterResults = entityHandler.SelectTriggers(DateNavigationStart.Date, DateNavigationEnd.Date);
                GetDateTimeResults(filterResults);
            }
        }

        private void GetDateTimeResults(List<ITableEntity> entities)
        {
            foreach (var entity in entities)
            {
                MainField.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                MainField.Children.Add(new Label() { Text = entity.Name }, MainField.RowDefinitions.Count - 1, 0);

                int iterator = 1;
                for (DateTime i = DateNavigationStart.Date; i <= DateNavigationEnd.Date; i = i.AddDays(1))
                    MainField.Children.Add(new Button() { Text = entityHandler.GetEntityValueString(entity.Name, i) }, iterator++, MainField.ColumnDefinitions.Count - 1);
            }
        }

        public void GetParameter(string parameter)
        {
            entityHandler.AddParameter(parameter);
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