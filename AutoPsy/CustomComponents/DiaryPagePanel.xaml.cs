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
    public partial class DiaryPagePanel : StackLayout, AutoPsy.CustomComponents.IСustomComponent
    {
        private string[] symptomNames;

        public Database.Entities.DiaryHandler diaryHandler { get; private set; }
        private ContentPage parent;
        public DiaryPagePanel(bool enabled, ContentPage parent)
        {
            InitializeComponent();

            this.Children.All(x => x.IsEnabled = enabled);
            this.parent = parent;

            var currentUser = App.Connector.currentConnectedUser;
            diaryHandler = new Database.Entities.DiaryHandler(currentUser) { stateMode = 0};
            DateOfRecord.Date = DateTime.Now;
            diaryHandler.SetDate(DateOfRecord.Date);
            GetSymptomCollection();
        }

        public DiaryPagePanel(bool enabled, ContentPage parent, Database.Entities.DiaryPage diaryPage)
        {
            InitializeComponent();

            this.Children.All(x => x.IsEnabled = enabled);
            this.parent = parent;

            var currentUser = App.Connector.currentConnectedUser;
            diaryHandler = new Database.Entities.DiaryHandler(currentUser) { stateMode = 1};

            SynchronizeData(diaryPage);
            GetSymptomCollection();
        }

        private void SynchronizeData(Database.Entities.DiaryPage diaryPage)
        {
            diaryHandler.CopyDiaryPage(diaryPage);
            DateOfRecord.Date = diaryHandler.GetDate();
            TopicEntry.Text = diaryHandler.GetTopic();
            TextEditor.Text = diaryHandler.GetMainText();
            diaryHandler.RecreateSymptomData(diaryPage);
            ListOfSymptoms.ItemsSource = diaryHandler.GetSymptoms();
        }

        private void GetSymptomCollection()
        {
            var temp = App.Graph.GetSymptomNodes();
            var iterator = 0;
            symptomNames = new string[temp.Length];

            foreach (var symp in temp)
                symptomNames[iterator++] = symp;
        }

        private async void AddTag_Clicked(object sender, EventArgs e)
        {
            var result = await parent.DisplayActionSheet("Выберите симптом", "Отмена", null, symptomNames);

            if (result != null)
            {
                if (!diaryHandler.ContainsSymptom(result))
                {
                    var symptom = new Database.Entities.Symptom() { SymptomeName = result };
                    diaryHandler.AddSymptom(symptom);
                    ListOfSymptoms.ItemsSource = diaryHandler.GetSymptoms();
                }
            }
        }

        public void TrySave()
        {
            TopicEntry.Unfocus(); TextEditor.Unfocus();
           
            if (diaryHandler.CheckCorrectness())
                diaryHandler.CreateDiaryPageInfo();
            else throw new Exception();
        }

        private void DateOfRecord_DateSelected(object sender, DateChangedEventArgs e)
        {
            diaryHandler.SetDate(DateOfRecord.Date);
        }

        private void TopicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (TopicEntry.Text.Equals(AutoPsy.Resources.DiaryPageDefault.Topic)) TopicEntry.Text = "";
        }

        private void TopicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (!TopicEntry.Text.Equals("") && !TopicEntry.Text.Equals(AutoPsy.Resources.DiaryPageDefault.Topic))
                diaryHandler.AddTopic(TopicEntry.Text);
            else
                TopicEntry.Text = AutoPsy.Resources.DiaryPageDefault.Topic;
        }

        private void TextEditor_Focused(object sender, FocusEventArgs e)
        {
            if (TextEditor.Text.Equals(AutoPsy.Resources.DiaryPageDefault.MainText)) TextEditor.Text = "";
        }

        private void TextEditor_Unfocused(object sender, FocusEventArgs e)
        {
            if (!TextEditor.Text.Equals("") && !TextEditor.Text.Equals(AutoPsy.Resources.DiaryPageDefault.MainText))
                diaryHandler.SetMainText(TextEditor.Text);
            else
                TextEditor.Text = AutoPsy.Resources.DiaryPageDefault.MainText;
        }
    }
}