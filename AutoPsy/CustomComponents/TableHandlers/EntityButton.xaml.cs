using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents.TableHandlers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityButton : StackLayout
    {
        private Database.Entities.ITableEntity entity;
        public EntityButton(Database.Entities.ITableEntity entity)
        {
            InitializeComponent();
            this.entity = entity;
        }

        public string Value
        {
            set { this.ValueLabel.Text = value.ToString(); }
        }

        private async void ValueLabel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Pages.TablePages.ValueSetterPage(entity));
        }
    }
}