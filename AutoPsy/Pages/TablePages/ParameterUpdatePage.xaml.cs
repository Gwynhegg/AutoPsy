using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPsy.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParameterUpdatePage : ContentPage
    {
        private Database.Entities.ITableEntity entity;
        public ParameterUpdatePage(Database.Entities.ITableEntity entity)
        {
            InitializeComponent();
            this.entity = entity;

            ParameterLabel.Text = entity.Name;
            ImportanceSetter.Value = entity.Importance;
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(Alerts.AlertMessage, Alerts.DeleteStringWarning, AuxiliaryResources.Yes, AuxiliaryResources.No);
            if (answer)
            {
                // УДАЛИТЬ ТУТ
                await Navigation.PopModalAsync();
            }
        }
    }
}