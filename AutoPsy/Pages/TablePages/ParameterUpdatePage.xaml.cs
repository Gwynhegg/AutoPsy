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
    public partial class ParameterUpdatePage: ContentPage       // Форма для обновления данных о параметре
    {
        private Database.Entities.ITableEntity entity;      // сущность-ячейка, служащая шаблоном для заполнения некоторых полей
        private TableGridHandler tableGridHandler;      // ссылка на класс-обработчик таблиц
        public ParameterUpdatePage(Database.Entities.ITableEntity entity, TableGridHandler tableGridHandler)
        {
            InitializeComponent();
            this.tableGridHandler = tableGridHandler;
            this.entity = entity;

            ParameterLabel.Text = entity.Name;
            ImportanceSetter.Value = entity.Importance;
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(Alerts.AlertMessage, Alerts.DeleteStringWarning, AuxiliaryResources.Yes, AuxiliaryResources.No);
            if (answer)
            {
                tableGridHandler.DeleteParameter(entity.IdValue);
                await Navigation.PopModalAsync();
            }
            else 
                return;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (ImportanceSetter.Value != entity.Importance)
            {
                bool answer = await DisplayAlert(Alerts.AlertMessage, Alerts.UpdateStringWarning, AuxiliaryResources.Yes, AuxiliaryResources.No);
                if (answer)
                {
                    tableGridHandler.UpdateParameter(entity.IdValue, (byte)ImportanceSetter.Value);
                    await Navigation.PopModalAsync();
                }
                else return;
            }
            else
                await Navigation.PopModalAsync();
        }
    }
}