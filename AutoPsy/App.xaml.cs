using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy
{
    public partial class App : Application
    {
        private static Database.DatabaseConnector connector;
        public static Database.DatabaseConnector Connector
        {
            get
            {
                if (connector == null)
                    connector = new Database.DatabaseConnector();
                return connector;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Pages.WelcomePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
