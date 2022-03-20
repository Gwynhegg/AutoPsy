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
    public partial class PrimaryUserExperiencePage : ContentPage
    {
        private List<Database.Entities.UserExperience> experienceList;
        private List<CustomComponents.UserExperiencePanel> experiencePages;
        public PrimaryUserExperiencePage()
        {
            InitializeComponent();

            experienceList = new List<Database.Entities.UserExperience>();
            experiencePages = new List<CustomComponents.UserExperiencePanel>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserExperienceEditorPage());
        }
    }
}