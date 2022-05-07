using AutoPsy.Pages.TablePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SliderPopUp : ContentPage
    {
        private byte value;
        public SliderPopUp(Database.Entities.ITableEntity entity)
        {
            InitializeComponent();

            ParameterName.Text = entity.Name;

            if (entity is Database.Entities.TableRecomendation) InitializeRecomendationValue();
            else if (entity is Database.Entities.TableCondition) InitializeConditionValue();
            else InitializeTriggerValue();
        }

        private void InitializeRecomendationValue()
        {
            Slider slider = new Slider() { Maximum = 5, Minimum = 1, Value = 3, MaximumTrackColor = Color.Green, MinimumTrackColor = Color.Red };
            ParameterValue.Children.Add(slider);
            slider.ValueChanged += SliderValueChanged;
        }

        private void InitializeConditionValue()
        {
            // ПРИДУМАТЬ, КАК МОЖНО МОДИФИЦИРОВАТЬ СВОЙСТВА ДАННОГО ПАРАМЕТРА
            Slider slider = new Slider() { Maximum = 5, Minimum = 1, Value = 3, MaximumTrackColor = Color.Green, MinimumTrackColor = Color.Red };
            ParameterValue.Children.Add(slider);
            slider.ValueChanged += SliderValueChanged;
        }

        private void InitializeTriggerValue()
        {
            Switch parameterSwitch = new Switch() { OnColor = Color.Red };
            ParameterValue.Children.Add(parameterSwitch);
            parameterSwitch.Toggled += SwitchToggled;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e) => await Navigation.PopModalAsync();

        private async void AcceptButton_Clicked(object sender, EventArgs e) => await Navigation.PopModalAsync();

        private void SliderValueChanged(object sender, EventArgs e) => value = (byte)(sender as Slider).Value;

        private void SwitchToggled(object sender, EventArgs e) => value = (sender as Switch).IsToggled ? (byte)1 : (byte)0;
    }
}