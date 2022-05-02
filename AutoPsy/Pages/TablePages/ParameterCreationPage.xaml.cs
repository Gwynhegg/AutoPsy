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
    public partial class ParameterCreationPage : ContentPage
    {
        private FullVersionTablePage parent;
        public ParameterCreationPage(FullVersionTablePage parent)
        {
            InitializeComponent();
            this.parent = parent;

            TypePicker.Items.Add(Const.Constants.RECOMENDATIONS_TAG_RUS);
            TypePicker.Items.Add(Const.Constants.CONDITIONS_TAG_RUS);
            TypePicker.Items.Add(Const.Constants.TRIGGERS_TAG_RUS);
        }

        private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> values = new List<string>();
            if (TypePicker.SelectedIndex == 0) values = App.TableGraph.GetRecomendations().Select(x => x.Name).ToList();
            else if (TypePicker.SelectedIndex == 1) values = App.TableGraph.GetConditions().Select(x => x.Name).ToList();
            else if (TypePicker.SelectedIndex == 2) values = App.TableGraph.GetTriggers().Select(x => x.Name).ToList();
            EntityPicker.ItemsSource = values;
            EntityPicker.IsEnabled = true;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            parent.GetParameter(EntityPicker.SelectedItem.ToString());
            await Navigation.PopModalAsync();
        }
    }
}