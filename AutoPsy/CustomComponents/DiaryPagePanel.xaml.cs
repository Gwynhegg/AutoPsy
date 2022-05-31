using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoPsy.Database.Entities;
using AutoPsy.Resources;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.CustomComponents
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPagePanel : Grid, IСustomComponent     // Панель для отображения данных дневника пользователя
    {
        private string[] symptomNames;

        public DiaryHandler diaryHandler { get; private set; }      // инициализируем обработчик записей дневника
        private ContentPage parent;     // Указываем родительский элемент данной панели
        public DiaryPagePanel(bool enabled, ContentPage parent)
        {
            InitializeComponent();

            // Поскольку панель применяется сразу в двух местах (пассивно и активно), было принято
            // решение активировать и дезактивировать составные части с помощью простого параметра
            this.Children.All(x => x.IsEnabled = enabled);
            this.parent = parent;

            var currentUser = App.Connector.currentConnectedUser;       // получаем текущего подключенного пользователя
            diaryHandler = new DiaryHandler(currentUser) { stateMode = 0};      // создаем обработчик записей и указываем его режим работы. 0 - создание записи, 1 - обновление       
            DateOfRecord.Date = DateTime.Now;
            diaryHandler.SetDate(DateOfRecord.Date);
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
            diaryHandler = new DiaryHandler(currentUser) { stateMode = 1};      // создаем обработчик записей и указываем его режим работы. 0 - создание записи, 1 - обновление     

            SynchronizeData(diaryPage);
            GetSymptomCollection();
        }

        private void SynchronizeData(Database.Entities.DiaryPage diaryPage)     // Метод синхронизации элементов формы с данными страницы
        {
            diaryHandler.CopyDiaryPage(diaryPage);      // создаем копию страницы для гарантирования безопасности данных в оригинале
            DateOfRecord.Date = diaryHandler.GetDate();     // устанавливаем дату
            TopicEntry.Text = diaryHandler.GetTopic();      // получаем тему
            TextEditor.Text = diaryHandler.GetMainText();       // получаем основной текст
            diaryHandler.RecreateSymptomData(diaryPage);        // воссоздаем список симптомов, привязанных к записи
            ListOfSymptoms.ItemsSource = diaryHandler.GetSymptoms();        // отображаем список симптомов
        }

        private void GetSymptomCollection()     // метод для получения коллекции симптомов
        {
            var temp = App.Graph.GetSymptomNodes();     // получаем все узлы симптомов
            var iterator = 0;
            symptomNames = new string[temp.Length];     // инициализируем список названий симптомов

            foreach (var symp in temp)      // перебираем значения...
                symptomNames[iterator++] = symp.Value;      // и помещаем их в массив
        }

        private async void AddTag_Clicked(object sender, EventArgs e)
        {
            var result = await parent.DisplayActionSheet(DiaryPageDefault.SelectSymptom, AuxiliaryResources.Cancel, null, symptomNames);

            if (result != null)
            {
                if (!diaryHandler.ContainsSymptom(result))
                {
                    var symptom = new Symptom() { SymptomeName = result };
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
            if (TopicEntry.Text.Equals(DiaryPageDefault.Topic)) TopicEntry.Text = String.Empty;
        }

        private void TopicEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (!TopicEntry.Text.Equals(String.Empty) && !TopicEntry.Text.Equals(DiaryPageDefault.Topic))
                diaryHandler.AddTopic(TopicEntry.Text);
            else
                TopicEntry.Text = DiaryPageDefault.Topic;
        }

        private void TextEditor_Focused(object sender, FocusEventArgs e)
        {
            if (TextEditor.Text.Equals(DiaryPageDefault.MainText)) TextEditor.Text = String.Empty;
        }

        private void TextEditor_Unfocused(object sender, FocusEventArgs e)
        {
            if (!TextEditor.Text.Equals(String.Empty) && !TextEditor.Text.Equals(DiaryPageDefault.MainText))
                diaryHandler.SetMainText(TextEditor.Text);
            else
                TextEditor.Text = DiaryPageDefault.MainText;
        }
    }
}