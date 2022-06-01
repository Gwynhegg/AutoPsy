using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : Xamarin.Forms.TabbedPage      // Главная страница навигации приложения
    {
        public MainPage() => InitializeComponent();

        protected override bool OnBackButtonPressed() => true;
    }
}