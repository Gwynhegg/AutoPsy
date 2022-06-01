using Xamarin.Forms;

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

        // Был избран именно такой вариант доступа к графам с данными, так как поднятие
        // и загрузку данных лучше осуществлять единоразово при запуске приложения, после чего
        // возможен доступ к ним из любой точки приложения

        private static AutoPsy.Logic.Structures.DiseaseGraph graph;     // переменная-коннектор для доступа к структуре графа для дневника
        public static AutoPsy.Logic.Structures.DiseaseGraph Graph       // свойство-коннектор на основе синглтона
        {
            get     // геттер для графа
            {
                if (graph == null)
                    graph = new AutoPsy.Logic.Structures.DiseaseGraph();
                return graph;
            }
        }

        private static AutoPsy.Logic.Structures.TableEntitiesGraph tableGraph;      // переменная-коннектор для доступа к структуре данных для таблицы
        public static AutoPsy.Logic.Structures.TableEntitiesGraph TableGraph        // свойство-коннектор на основе синглтона
        {
            get     // геттер для графа с таблицей
            {
                if (tableGraph == null)
                    tableGraph = new AutoPsy.Logic.Structures.TableEntitiesGraph();
                return tableGraph;
            }
        }

        public App()
        {
            InitializeComponent();

            // поднимаем графы
            Logic.Structures.DiseaseGraph graph = App.Graph;
            Logic.Structures.TableEntitiesGraph table = App.TableGraph;

            this.MainPage = new NavigationPage(new Pages.WelcomePage());     // Загрузка начальной страницы
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume() => this.MainPage = new NavigationPage(new Pages.WelcomePage());
    }
}
