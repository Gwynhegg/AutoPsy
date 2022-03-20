using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserExperienceEditorPage : ContentPage
    {
        CustomComponents.UserExperiencePanel experiencePanel;
        public UserExperienceEditorPage()
        {
            InitializeComponent();
            experiencePanel = new CustomComponents.UserExperiencePanel(enabled: true);
            CurrentItem.Children.Insert(0, experiencePanel); 

        }

        private void SaveAndReturn_Clicked(object sender, EventArgs e)
        {
            experiencePanel.TrySave();

        }
    }
}