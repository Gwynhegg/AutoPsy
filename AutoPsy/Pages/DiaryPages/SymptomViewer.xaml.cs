using AutoPsy.Database.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SymptomViewer : ContentPage
    {
        private readonly INode[] guideInfo;     // массив для хранения всех узлов графа
        private readonly ObservableCollection<string> searchResults;     // коллекция, в которой будут отображены результаты поиска
        public SymptomViewer()
        {
            InitializeComponent();
            this.guideInfo = App.Graph.GetAllItems();        // получаем все элементы графа и сохраняем в массив
            this.searchResults = new ObservableCollection<string>();
        }

        // Событие, возникающее при изменении текста в строке поиска
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (this.searchBar.Text != string.Empty)
            {
                SetInfoPanelInto(false);        // Изменяем состояние панели (подробнее ниже)

                // Ищем все возможные совпадения введенного текста по массиву и выдаем их в виде списка совпадений
                IEnumerable<INode> query = this.guideInfo.Where(x => x.Value.ToLower().Contains(this.searchBar.Text.ToLower()));
                this.searchResults.Clear();
                foreach (INode item in query)
                    this.searchResults.Add(item.Value);

                this.SearchResults.ItemsSource = this.searchResults;
            }
        }

        // Метод, вызываемый при выборе конкретного симптома
        private void SearchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SetInfoPanelInto(true);     // Изменяем состояние панели (подробнее ниже)

            // Отображаем информацию о выбранном объекте
            var selectedItem = this.SearchResults.SelectedItem as string;
            this.NameOfEntity.Text = selectedItem;
            this.DescriptionOfEntity.Text = "ОПИСАНИЕ"; // --------------------TODO: ЕСЛИ БУДЕТ ВРЕМЯ, ДОБАВИТЬ ОПИСАНИЕ СИМПТОМАМ

            var itemId = this.guideInfo.First(x => x.Value == selectedItem).Id;       // Получаем Id выбранного объекта

            // Ищем всех родителей данного узла, и если находим - отображаем их в коллекции и на экране
            var ancestors = App.Graph.SearchAncestorsLink(itemId);
            if (ancestors != null) this.AncestorsList.ItemsSource = ancestors; else this.AncestorsList.ItemsSource = AutoPsy.Resources.SymptomHelperResources.HasNoParents;

            // Ищем всех детей данного узла, и если находим - отображаем их в коллекции и на экране
            var descenders = App.Graph.SearchDescendersLink(itemId);
            if (descenders != null) this.DescendersList.ItemsSource = descenders; else this.DescendersList.ItemsSource = AutoPsy.Resources.SymptomHelperResources.HasNoChilds;
        }

        private void SetInfoPanelInto(bool type)        // Метод для изменения состояния панели
        {
            // При значениях true отображается поле описания, а также список родителей и детей. Остальные поля скрываются
            this.NameOfEntity.IsVisible = type;
            this.DescriptionOfEntity.IsVisible = type;
            this.DescendersList.IsVisible = type;
            this.AncestorsList.IsVisible = type;
            this.NameOfEntity.IsEnabled = type;
            this.DescriptionOfEntity.IsEnabled = type;
            this.DescendersList.IsEnabled = type;
            this.AncestorsList.IsEnabled = type;
            this.ChildsLabel.IsVisible = type;
            this.ParentsLabel.IsVisible = type;

            // В противном случае - отображаем строку и панель поиска, скрывая остальное
            this.SearchResults.IsVisible = !type;
            this.SearchResults.IsEnabled = !type;
        }
    }
}