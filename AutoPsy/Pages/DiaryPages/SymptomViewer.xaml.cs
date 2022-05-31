using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages.DiaryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SymptomViewer : ContentPage
    {
        private INode[] guideInfo;     // массив для хранения всех узлов графа
        ObservableCollection<string> searchResults;     // коллекция, в которой будут отображены результаты поиска
        public SymptomViewer()
        {
            InitializeComponent();
            guideInfo = App.Graph.GetAllItems();        // получаем все элементы графа и сохраняем в массив
            searchResults = new ObservableCollection<string>();
        }

        // Событие, возникающее при изменении текста в строке поиска
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (searchBar.Text != String.Empty)
            {
                SetInfoPanelInto(false);        // Изменяем состояние панели (подробнее ниже)

                // Ищем все возможные совпадения введенного текста по массиву и выдаем их в виде списка совпадений
                var query = guideInfo.Where(x => x.Value.ToLower().Contains(searchBar.Text.ToLower()));
                searchResults.Clear();
                foreach(var item in query)
                    searchResults.Add(item.Value);

                SearchResults.ItemsSource = searchResults;                
            }
        }

        // Метод, вызываемый при выборе конкретного симптома
        private void SearchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SetInfoPanelInto(true);     // Изменяем состояние панели (подробнее ниже)

            // Отображаем информацию о выбранном объекте
            var selectedItem = SearchResults.SelectedItem as string;
            NameOfEntity.Text = selectedItem; 
            DescriptionOfEntity.Text = "ОПИСАНИЕ"; // --------------------TODO: ЕСЛИ БУДЕТ ВРЕМЯ, ДОБАВИТЬ ОПИСАНИЕ СИМПТОМАМ

            string itemId = guideInfo.First(x => x.Value == selectedItem).Id;       // Получаем Id выбранного объекта

            // Ищем всех родителей данного узла, и если находим - отображаем их в коллекции и на экране
            var ancestors = App.Graph.SearchAncestorsLink(itemId);
            if (ancestors != null) AncestorsList.ItemsSource = ancestors; else AncestorsList.ItemsSource = AutoPsy.Resources.SymptomHelperResources.HasNoParents;

            // Ищем всех детей данного узла, и если находим - отображаем их в коллекции и на экране
            var descenders = App.Graph.SearchDescendersLink(itemId);
            if (descenders != null) DescendersList.ItemsSource = descenders; else DescendersList.ItemsSource = AutoPsy.Resources.SymptomHelperResources.HasNoChilds;
        }

        private void SetInfoPanelInto(bool type)        // Метод для изменения состояния панели
        {
            // При значениях true отображается поле описания, а также список родителей и детей. Остальные поля скрываются
            NameOfEntity.IsVisible = type;
            DescriptionOfEntity.IsVisible = type;
            DescendersList.IsVisible = type;
            AncestorsList.IsVisible = type;
            NameOfEntity.IsEnabled = type;
            DescriptionOfEntity.IsEnabled = type;
            DescendersList.IsEnabled = type;
            AncestorsList.IsEnabled = type;
            ChildsLabel.IsVisible = type;
            ParentsLabel.IsVisible = type;

            // В противном случае - отображаем строку и панель поиска, скрывая остальное
            SearchResults.IsVisible = !type;
            SearchResults.IsEnabled = !type;
        }
    }
}