using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Database.Entities.UserExperience> experiencePages;
        public PrimaryUserExperiencePage()
        {
            InitializeComponent();
            experiencePages = new ObservableCollection<Database.Entities.UserExperience>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserExperienceEditorPage(this));
        }

        public void SynchronizeContentPages(CustomComponents.UserExperiencePanel experiencePanel)
        {
            experiencePages.Add(experiencePanel.experienceHandler.GetUserExperience());
            ExperienceCarouselView.ItemsSource = experiencePages;
        }
    }
}