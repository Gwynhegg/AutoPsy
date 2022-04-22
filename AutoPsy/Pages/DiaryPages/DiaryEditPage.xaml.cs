using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryEditPage : ContentPage
    {
        private CustomComponents.DiaryPagePanel pagePanel;
        public DiaryEditPage()
        {
            InitializeComponent();
            pagePanel = new CustomComponents.DiaryPagePanel(this);      // создаем панель для ввода
            CurrentItem.Children.Insert(0, pagePanel);
        }
    }
}