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
        private Logic.Structures.INode[] guideInfo;
        ObservableCollection<string> searchResults;
        public SymptomViewer()
        {
            InitializeComponent();
            guideInfo = App.Graph.GetAllItems();
            searchResults = new ObservableCollection<string>();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (searchBar.Text != "")
            {
                SetInfoPanelInto(false);

                var query = guideInfo.Where(x => x.Value.ToLower().Contains(searchBar.Text.ToLower()));
                searchResults.Clear();
                foreach(var item in query)
                    searchResults.Add(item.Value);

                SearchResults.ItemsSource = searchResults;                
            }
        }

        private void SearchResults_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SetInfoPanelInto(true);

            

            var selectedItem = SearchResults.SelectedItem as string;
            NameOfEntity.Text = selectedItem;
            DescriptionOfEntity.Text = "ОПИСАНИЕ";

            string itemId = guideInfo.First(x => x.Value == selectedItem).Id;

            var ancestors = App.Graph.SearchAncestorsLink(itemId);
            if (ancestors != null) AncestorsList.ItemsSource = ancestors; else AncestorsList.ItemsSource = "Не родителей";

            var descenders = App.Graph.SearchDescendersLink(itemId);
            if (descenders != null) DescendersList.ItemsSource = descenders; else DescendersList.ItemsSource = "Нет детей";
        }

        private void SetInfoPanelInto(bool type)
        {
            NameOfEntity.IsVisible = type;
            DescriptionOfEntity.IsVisible = type;
            DescendersList.IsVisible = type;
            AncestorsList.IsVisible = type;
            NameOfEntity.IsEnabled = type;
            DescriptionOfEntity.IsEnabled = type;
            DescendersList.IsEnabled = type;
            AncestorsList.IsEnabled = type;
            SearchResults.IsVisible = !type;
            SearchResults.IsEnabled = !type;
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}