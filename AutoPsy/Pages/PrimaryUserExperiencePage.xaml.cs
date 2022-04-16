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
            var addedExperience = experiencePanel.experienceHandler.GetUserExperience();
            int indexOfElement = experiencePages.IndexOf(experiencePages.Where(x => x.Id == addedExperience.Id).FirstOrDefault());

            if (indexOfElement == -1)
                experiencePages.Add(experiencePanel.experienceHandler.GetUserExperience());
            else
                experiencePages[indexOfElement] = addedExperience;

            ExperienceCarouselView.ItemsSource = experiencePages;
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {

            if (experiencePages.Count == 0)
            {
                await DisplayAlert("Упс!", "Пока у вас нет записей для редактирования", "OK");
                return;
            }
            var temp = ExperienceCarouselView.CurrentItem as AutoPsy.Database.Entities.UserExperience;

            if (temp != null) await Navigation.PushModalAsync(new UserExperienceEditorPage(this, temp));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (experiencePages.Count == 0)
            {
                await DisplayAlert("Упс!", "Пока у вас нет записей для удаления", "OK");
                return;
            }

            var temp = ExperienceCarouselView.CurrentItem as AutoPsy.Database.Entities.UserExperience;

            experiencePages.Remove(temp);
            ExperienceCarouselView.ItemsSource = experiencePages;

            if (temp != null) Database.DatabaseConnector.GetDatabaseConnector().DeleteData(temp);
        }
    }
}