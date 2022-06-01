using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPagePanel : Grid, IСustomComponent     // Панель для отображения данных дневника пользователя
    {
        private string[] symptomNames;

        public DiaryHandler diaryHandler { get; private set; }      // инициализируем обработчик записей дневника
        private readonly ContentPage parent;     // Указываем родительский элемент данной панели
        public DiaryPagePanel(bool enabled, ContentPage parent)
        {
            InitializeComponent();

            // Поскольку панель применяется сразу в двух местах (пассивно и активно), было принято
            // решение активировать и дезактивировать составные части с помощью простого параметра
            this.Children.All(x => x.IsEnabled = enabled);
            this.parent = parent;

            var currentUser = App.Connector.currentConnectedUser;       // получаем текущего подключенного пользователя
            this.diaryHandler = new DiaryHandler(currentUser) { stateMode = 0 };      // создаем обработчик записей и указываем его режим работы. 0 - создание записи, 1 - обновление       
            this.DateOfRecord.Date = DateTime.Now;
            this.diaryHandler.SetDate(this.DateOfRecord.Date);
            GetSymptomCollection();
        }

        public DiaryPagePanel(bool enabled, ContentPage parent, DiaryPage diaryPage)
        {
            InitializeComponent();

            // Поскольку панель применяется сразу в двух местах (пассивно и активно), было принято
            // решение активировать и дезактивировать составные части с помощью простого параметра
            this.Children.All(x => x.IsEnabled = enabled);
            this.parent = parent;

            var currentUser = App.Connector.currentConnectedUser;       // получаем текущего подключенного пользователя
            this.diaryHandler = new DiaryHandler(currentUser) { stateMode = 1 };      // создаем обработчик записей и указываем его режим работы. 0 - создание записи, 1 - обновление     

            SynchronizeData(diaryPage);
            GetSymptomCollection();
        }

        private void SynchronizeData(Database.Entities.DiaryPage diaryPage)     // Метод синхронизации элементов формы с данными страницы
        {
            this.diaryHandler.CopyDiaryPage(diaryPage);      // создаем копию страницы для гарантирования безопасности данных в оригинале
            this.DateOfRecord.Date = this.diaryHandler.GetDate();     // устанавливаем дату
            this.TopicEntry.Text = this.diaryHandler.GetTopic();      // получаем тему
            this.TextEditor.Text = this.diaryHandler.GetMainText();       // получаем основной текст
            this.diaryHandler.RecreateSymptomData(diaryPage);        // воссоздаем список симптомов, привязанных к записи
            this.ListOfSymptoms.ItemsSource = this.diaryHandler.GetSymptoms();        // отображаем список симптомов
        }

        private void GetSymptomCollection()     // метод для получения коллекции симптомов
        {
            INode[] temp = App.Graph.GetSymptomNodes();     // получаем все узлы симптомов
            var iterator = 0;
            this.symptomNames = new string[temp.Length];     // инициализируем список названий симптомов

            foreach (INode symp in temp)      // перебираем значения...
                this.symptomNames[iterator++] = symp.Value;      // и помещаем их в массив
        }

        private async void AddTag_Clicked(object sender, EventArgs e)
        {
            var result = await this.parent.DisplayActionSheet(DiaryPageDefault.SelectSymptom, AuxiliaryResources.Cancel, null, this.symptomNames);

            if (result != null)
            {
                if (!this.diaryHandler.ContainsSymptom(result))
                {
                    var symptom = new Symptom() { SymptomeName = result };
                    this.diaryHandler.AddSymptom(symptom);
                    this.ListOfSymptoms.ItemsSource = this.diaryHandler.GetSymptoms();
                }
            }
        }

        public void TrySave()
        {
            this.TopicEntry.Unfocus(); this.TextEditor.Unfocus();

            if (this.diaryHandler.CheckCorrectness())
                this.diaryHandler.CreateDiaryPageInfo();
            else throw new Exception();
        }

        private void DateOfRecord_DateSelected(object sender, DateChangedEventArgs e) => this.diaryHandler.SetDate(this.DateOfRecord.Date);

        private void TopicEntry_Focused(object sender, FocusEventArgs e)
        {
            if (this.TopicEntry.Text.Equals(DiaryPageDefault.Topic)) this.TopicEntry.Text = string.Empty;
        }

        private void TopicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (!this.TopicEntry.Text.Equals(string.Empty) && !this.TopicEntry.Text.Equals(DiaryPageDefault.Topic))
                this.diaryHandler.AddTopic(this.TopicEntry.Text);
            else
                this.TopicEntry.Text = DiaryPageDefault.Topic;
        }

        private void TextEditor_Focused(object sender, FocusEventArgs e)
        {
            if (this.TextEditor.Text.Equals(DiaryPageDefault.MainText)) this.TextEditor.Text = string.Empty;
        }

        private void TextEditor_Unfocused(object sender, FocusEventArgs e)
        {
            if (!this.TextEditor.Text.Equals(string.Empty) && !this.TextEditor.Text.Equals(DiaryPageDefault.MainText))
                this.diaryHandler.SetMainText(this.TextEditor.Text);
            else
                this.TextEditor.Text = DiaryPageDefault.MainText;
        }
    }
}