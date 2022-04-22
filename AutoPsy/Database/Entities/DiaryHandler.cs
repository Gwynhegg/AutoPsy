using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace AutoPsy.Database.Entities
{
    public class DiaryHandler
    {
        private DiaryPage page;
        private ObservableCollection<Symptom> symptoms;

        public DiaryHandler()
        {
            page = new DiaryPage();
            page.Topic = "Не указано";
            symptoms = new ObservableCollection<Symptom>();
        }

        public void AddTopic(string topic)
        {
            page.Topic = topic;
        }
        public string GetTopic()
        {
            return page.Topic;
        }

        public void SetMainText(string text)
        {
            page.MainText = text;
        }

        public string GetMainText()
        {
            return page.MainText;
        }

        public void AddSymptom(string symptomName)
        {
            var symptom = new Symptom();
            symptom.SymptomeName = symptomName;
            this.symptoms.Add(symptom);
        }

        public ObservableCollection<Symptom> GetSymptoms()
        {
            return symptoms;
        }

        public bool CheckCorrectness()
        {
            if (page.MainText != null && page.MainText != "") return true; else return false;
        }

        public void CreateDiaryPageInfo()
        {
            App.Connector.CreateAndInsertData<DiaryPage>(page);
        }
    }
}
