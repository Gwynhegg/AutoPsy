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
    public partial class EntityLabel : StackLayout
    {
        private Database.Entities.ITableEntity entity;
        public EntityLabel(Database.Entities.ITableEntity entity)
        {
            InitializeComponent();
            this.entity = entity;
        }

        public string Label
        {
            set { this.LabelEntity.Text = value; }
        }

        private async void LabelEntity_Clicked(object sender, EventArgs e)
        {
            var page = new Pages.TablePages.ParameterUpdatePage(entity);
            await Navigation.PushModalAsync(page);
        }
    }
}