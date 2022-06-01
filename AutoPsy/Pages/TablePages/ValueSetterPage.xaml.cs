using AutoPsy.CustomComponents;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.TablePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValueSetterPage : ContentPage
    {
        private readonly TableGridHandler parentGridHandler;
        private readonly Database.Entities.ITableEntity entity;
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
            if (this.entity.Type.Equals(Const.Constants.ENTITY_TRIGGER))
            {
                var switchElement = new Switch();
                switchElement.Toggled += SwitchElementToggled;
                this.MainGrid.Children.Add(switchElement, 0, 1);
            }
            else
            {
                var sliderElement = new Slider() { MinimumTrackColor = Color.Green, MaximumTrackColor = Color.Gray };
                sliderElement.Minimum = 0; sliderElement.Maximum = 5; sliderElement.Value = 3;
                sliderElement.ValueChanged += SliderValueChanged;
                this.MainGrid.Children.Add(sliderElement, 0, 1);
            }
        }

        private async void SaveValueButton_Clicked(object sender, EventArgs e)
        {
            this.entity.Value = this.value;
            this.parentGridHandler.UpdateValue(this.entity);
            await this.Navigation.PopModalAsync();
        }

        private void SliderValueChanged(object sender, EventArgs e)
        {
            var element = sender as Slider;
            this.value = (byte)Math.Round(element.Value);
            element.MinimumTrackColor = AuxServices.ColorPicker.ColorScheme[this.value];
        }
        private void SwitchElementToggled(object sender, EventArgs e)
        {
            var element = sender as Switch;
            if (element.IsToggled) this.value = 1; else this.value = 0;
        }
    }
}