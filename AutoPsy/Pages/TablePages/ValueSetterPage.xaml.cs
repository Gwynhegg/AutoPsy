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
        private Database.Entities.ITableEntity entity;
        public ValueSetterPage(Database.Entities.ITableEntity entity)
        {
            InitializeComponent();
            this.entity = entity;
        }
    }
}