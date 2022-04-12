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
        private PrimaryUserExperiencePage parentPage;
        public UserExperienceEditorPage(PrimaryUserExperiencePage parentPage)
        {
            InitializeComponent();
            this.parentPage = parentPage;
            experiencePanel = new CustomComponents.UserExperiencePanel(enabled: true);
            CurrentItem.Children.Insert(0, experiencePanel); 
        }

        private async void SaveAndReturn_Clicked(object sender, EventArgs e)
        {
            try
            {
                experiencePanel.TrySave();
                parentPage.SynchronizeContentPages(experiencePanel);
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(AutoPsy.Resources.AuxiliaryResources.AlertMessage, AutoPsy.Resources.AuxiliaryResources.ExperienceAlertMessage, AutoPsy.Resources.AuxiliaryResources.ButtonOK);
            }


        }
    }
}