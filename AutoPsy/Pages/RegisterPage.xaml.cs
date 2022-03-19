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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Clicked(object sender, EventArgs e)
        {
            LoginOptions.IsVisible = false;
            RegisterForm.IsVisible = true;
        }

        private void HideRegisterForm_Clicked(object sender, EventArgs e)
        {
            LoginOptions.IsVisible = true;
            RegisterForm.IsVisible = false;
        }

        private void Continue_Clicked(object sender, EventArgs e)
        {

        }

        private void LoginVK_Clicked(object sender, EventArgs e)
        {

        }
    }
}