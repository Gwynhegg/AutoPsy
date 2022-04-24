using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class DiaryHandler
    {
        private DiaryPage page;
        private ObservableCollection<Symptom> symptoms;
        public byte stateMode { get; set; } = 0;

        public DiaryHandler(int userId)
        {
            page = new DiaryPage();
            page.UserId = userId;
            page.Topic = "Не указано";
            symptoms = new ObservableCollection<Symptom>();
        }

        public void CopyDiaryPage(DiaryPage page)
        {
            this.page = (DiaryPage)page.Clone();
        }

        public void AddTopic(string topic)
        {
            page.Topic = topic;
        }
        public string GetTopic()
        {
            if (page.Topic != null)
                return page.Topic;
            else
                return AutoPsy.Resources.AuxiliaryResources.NotMentioned;
        }

        public void SetMainText(string text)
        {
            page.MainText = text;
        }

        public string GetMainText()
        {
            return page.MainText;
        }

        public void AddSymptom(Symptom symptom)
        {
            this.symptoms.Add(symptom);
        }

        public ObservableCollection<Symptom> GetSymptoms()
        {
            return symptoms;
        }
        public bool ContainsSymptom(string symptomName)
        {
            var req = this.symptoms.FirstOrDefault(x => x.SymptomeName.Equals(symptomName));
            if (req != null) return true; else return false;
        }

        public void SetDate(DateTime dateTime)
        {
            page.DateOfRecord = dateTime;
        }

        public DateTime GetDate()
        {
            return page.DateOfRecord;
        }

        public bool CheckCorrectness()
        {
            if (page.MainText != null && page.MainText != "" && page.DateOfRecord != null) return true; else return false;
        }

        public void CreateDiaryPageInfo()
        {
            CodifySymptomsData();

            if (stateMode == 0)
                App.Connector.CreateAndInsertData<DiaryPage>(page);
            else
                App.Connector.UpdateData<DiaryPage>(page);
        }

        private void CodifySymptomsData()
        {
            string codifiedSymptoms = "";
            foreach (Symptom symp in symptoms)
                codifiedSymptoms += String.Concat(symp.SymptomeName, '\\');
            page.AttachedSymptoms = codifiedSymptoms;
        }

        public void RecreateSymptomData(DiaryPage page)
        {
            if (page.AttachedSymptoms == null) return;

            string[] codifiedSymptoms = page.AttachedSymptoms.Split('\\');
            foreach(string symp in codifiedSymptoms)
                symptoms.Add(new Symptom() { SymptomeName = symp });
        }

        public DiaryPage GetDiaryPage()
        {
            return page;
        }
    }
}
