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
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            var usersExisted = App.Connector.IsTableExisted<Database.Entities.User>();

            if (usersExisted)
                Navigation.PushModalAsync(new CheckPasswordPage());
            else SetPrivacyPoliceView();
        }

        private async void AcceptPolicy_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Pages.RegisterPage());
        private void SetPrivacyPoliceView()
        {
            LoadingView.IsVisible = false;
            PolicyView.IsVisible = true;
        }

    }
}