using AutoPsy.CustomComponents;
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
    public partial class ValueSetterPage : ContentPage
    {
        private TableGridHandler parentGridHandler;
        private Database.Entities.ITableEntity entity;
        private byte value = 0;
        public ValueSetterPage(Database.Entities.ITableEntity entity, TableGridHandler parentGridHandler)
        {
            InitializeComponent();
            this.entity = entity;
            this.parentGridHandler = parentGridHandler;
            InitializeSetterElement();
        }

        private void InitializeSetterElement()
        {
            if (entity.Type.Equals(Const.Constants.ENTITY_TRIGGER))
            {
                Switch switchElement = new Switch();
                switchElement.Toggled += SwitchElementToggled;
                MainGrid.Children.Add(switchElement, 0, 1);
            }
            else
            {
                Slider sliderElement = new Slider();
                sliderElement.Minimum = 0; sliderElement.Maximum = 5; sliderElement.Value = 3;
                sliderElement.ValueChanged += SliderValueChanged;
                MainGrid.Children.Add(sliderElement, 0, 1);
            }
        }

        private async void SaveValueButton_Clicked(object sender, EventArgs e)
        {
            entity.Value = value;
            parentGridHandler.UpdateValue(entity);
            await Navigation.PopModalAsync();
        }

        private void SliderValueChanged(object sender, EventArgs e)
        {
            var element = sender as Slider;
            value = (byte)element.Value;
        }
        private void SwitchElementToggled(object sender, EventArgs e)
        {
            var element = sender as Switch;
            if (element.IsToggled) value = 1; else value = 0;
        }
    }
}