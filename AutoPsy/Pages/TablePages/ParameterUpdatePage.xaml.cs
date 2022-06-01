using AutoPsy.CustomComponents;
using AutoPsy.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParameterUpdatePage : ContentPage       // Форма для обновления данных о параметре
    {
        private readonly Database.Entities.ITableEntity entity;      // сущность-ячейка, служащая шаблоном для заполнения некоторых полей
        private readonly TableGridHandler tableGridHandler;      // ссылка на класс-обработчик таблиц
        public ParameterUpdatePage(Database.Entities.ITableEntity entity, TableGridHandler tableGridHandler)
        {
            InitializeComponent();
            this.tableGridHandler = tableGridHandler;
            this.entity = entity;

            this.ParameterLabel.Text = entity.Name;
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert(Alerts.AlertMessage, Alerts.DeleteStringWarning, AuxiliaryResources.Yes, AuxiliaryResources.No);
            if (answer)
            {
                this.tableGridHandler.DeleteParameter(this.entity.IdValue);
                await this.Navigation.PopModalAsync();
            }
            else
            {
                return;
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e) => await this.Navigation.PopModalAsync();
    }
}