using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPagePanel : StackLayout
    {
        private List<string> symptoms;
        private string[] symptomNames;

        private Database.Entities.DiaryHandler diaryHandler;
        private ContentPage parent;
        public DiaryPagePanel(ContentPage parent)
        {
            InitializeComponent();
            this.parent = parent;

            SwipeItem deleteSwipeItem = new SwipeItem() { Text = "Delete" };
            deleteSwipeItem.Invoked += OnDeleteSwipeItemInvoked;
            TagsView.LeftItems.Add(deleteSwipeItem);

            symptoms = new List<string>();
            diaryHandler = new Database.Entities.DiaryHandler();
            GetSymptomCollection();
        }

        private void GetSymptomCollection()
        {
            var temp = App.Graph.GetSymptomNodes();
            var iterator = 0;
            symptomNames = new string[temp.Length];

            foreach (var symp in temp)
                symptomNames[iterator++] = symp;
        }

        private void TopicEntry_Focused(object sender, FocusEventArgs e)
        {
            UpperFocus();
        }

        private void TextEditor_Focused(object sender, FocusEventArgs e)
        {
            UpperFocus();
        }

        private void TagsView_Focused(object sender, FocusEventArgs e)
        {
            LowerFocus();
        }

        private async void AddTag_Clicked(object sender, EventArgs e)
        {
            var result = await parent.DisplayActionSheet("Выберите симптом", "Отмена", null, symptomNames);

            if (result != null)
            {
                if (!symptoms.Contains(result))
                {
                    symptoms.Add(result);
                    diaryHandler.AddSymptom(result);
                    TagsContainer.Children.Add(new Label() { Text = result, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center });
                }
            }
            LowerFocus();
        }

        private void UpperFocus()
        {
            TagsRow.Height = new GridLength(2, GridUnitType.Star);
            TopicRow.Height = new GridLength(2, GridUnitType.Star);
            TextRow.Height = new GridLength(5, GridUnitType.Star);
        }

        private void LowerFocus()
        {
            TagsRow.Height = new GridLength(5, GridUnitType.Star);
            TopicRow.Height = new GridLength(2, GridUnitType.Star);
            TextRow.Height = new GridLength(2, GridUnitType.Star);
        }

        private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
        {

        }
    }
}