using AutoPsy.Database.Entities;
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
    public partial class TableStatisticsPage : ContentPage
    {
        private List<(string, ITableEntity)> entityValues;
        public TableStatisticsPage(List<(string, ITableEntity)> entityValues)
        {
            InitializeComponent();
            this.entityValues = entityValues;
        }
    }
}