using System;

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

            this.ParameterName.Text = entity.Name;

            if (entity is Database.Entities.TableRecomendation) InitializeRecomendationValue();
            else if (entity is Database.Entities.TableCondition) InitializeConditionValue();
            else InitializeTriggerValue();
        }

        private void InitializeRecomendationValue()
        {
            var slider = new Slider() { Maximum = 5, Minimum = 1, Value = 3, MaximumTrackColor = Color.Green, MinimumTrackColor = Color.Red };
            this.ParameterValue.Children.Add(slider);
            slider.ValueChanged += SliderValueChanged;
        }

        private void InitializeConditionValue()
        {
            // ПРИДУМАТЬ, КАК МОЖНО МОДИФИЦИРОВАТЬ СВОЙСТВА ДАННОГО ПАРАМЕТРА
            var slider = new Slider() { Maximum = 5, Minimum = 1, Value = 3, MaximumTrackColor = Color.Green, MinimumTrackColor = Color.Red };
            this.ParameterValue.Children.Add(slider);
            slider.ValueChanged += SliderValueChanged;
        }

        private void InitializeTriggerValue()
        {
            var parameterSwitch = new Switch() { OnColor = Color.Red };
            this.ParameterValue.Children.Add(parameterSwitch);
            parameterSwitch.Toggled += SwitchToggled;
        }

        private async void CancelButton_Clicked(object sender, EventArgs e) => await this.Navigation.PopModalAsync();

        private async void AcceptButton_Clicked(object sender, EventArgs e) => await this.Navigation.PopModalAsync();

        private void SliderValueChanged(object sender, EventArgs e) => this.value = (byte)(sender as Slider).Value;

        private void SwitchToggled(object sender, EventArgs e) => this.value = (sender as Switch).IsToggled ? (byte)1 : (byte)0;
    }
}