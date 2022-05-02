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
    public partial class TablePage : ContentPage
    {
        public TablePage()
        {
            InitializeComponent();
        }

        private void AnalyzeButton_Clicked(object sender, EventArgs e)
        {

        }

        private async void FullVersionButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FullVersionTablePage());
        }
    }
}