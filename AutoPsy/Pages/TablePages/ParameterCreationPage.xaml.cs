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
    public partial class ParameterCreationPage : ContentPage
    {
        private FullVersionTablePage parent;
        private delegate void AddMethod(string parameter, byte importance);
        AddMethod addMethod;
        public ParameterCreationPage(FullVersionTablePage parent)
        {
            InitializeComponent();
            this.parent = parent;

            TypePicker.Items.Add(Constants.RECOMENDATIONS_TAG_RUS);
            TypePicker.Items.Add(Constants.CONDITIONS_TAG_RUS);
            TypePicker.Items.Add(Constants.TRIGGERS_TAG_RUS);
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> values = new List<string>();
            if (TypePicker.SelectedIndex == Const.Constants.RECOMMENDATION_PARAMETER)
            {
                addMethod = parent.AddRecomendationParameter;
                values = App.TableGraph.GetRecomendations().Select(x => x.Name).ToList();
            }
            else if (TypePicker.SelectedIndex == Const.Constants.CONDITION_PARAMETER)
            {
                addMethod = parent.AddConditionParameter;
                values = App.TableGraph.GetConditions().Select(x => x.Name).ToList();
            }
            else if (TypePicker.SelectedIndex == Const.Constants.TRIGGER_PARAMETER) 
            {
                addMethod = parent.AddTriggerParameter;
                values = App.TableGraph.GetTriggers().Select(x => x.Name).ToList();
            }

            EntityPicker.ItemsSource = values;
            EntityPicker.IsEnabled = true;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            addMethod(EntityPicker.SelectedItem.ToString(), (byte)ImportanceSetter.Value);
            await Navigation.PopModalAsync();
        }
    }
}