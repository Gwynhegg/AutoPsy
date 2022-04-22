using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy
{
    public partial class App : Application
    {
        private static Database.DatabaseConnector connector;        // переменная-коннектор для доступа к базе данных
        public static Database.DatabaseConnector Connector      // свойство-коннектор на основе синглтона
        {
            get     // геттер для коннектора
            {
                if (connector == null)
                    connector = new Database.DatabaseConnector();
                return connector;
            }
        }

        private static AutoPsy.Logic.Structures.DiseaseGraph graph;
        public static AutoPsy.Logic.Structures.DiseaseGraph Graph
        {
            get
            {
                if (graph == null)
                    graph = new AutoPsy.Logic.Structures.DiseaseGraph();
                return graph;
            }
        }

        public App()
        {
            InitializeComponent();

            var graph = App.Graph;
            MainPage = new NavigationPage(new Pages.WelcomePage());     // Загрузка начальной страницы
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
