using AutoPsy.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoPsy.Database.Entities
{
    public class DiaryHandler
    {
        private DiaryPage page;
        private readonly ObservableCollection<Symptom> symptoms;
        public byte stateMode { get; set; } = 0;

        public DiaryHandler(int userId)
        {
            this.page = new DiaryPage
            {
                UserId = userId,
                Topic = AuxiliaryResources.NotMentioned
            };
            this.symptoms = new ObservableCollection<Symptom>();
        }

        public void CopyDiaryPage(DiaryPage page) => this.page = (DiaryPage)page.Clone();

        public void AddTopic(string topic) => this.page.Topic = topic;
        public string GetTopic()
        {
            if (this.page.Topic != null)
                return this.page.Topic;
            else
                return AuxiliaryResources.NotMentioned;
        }

        public void SetMainText(string text) => this.page.MainText = text;

        public string GetMainText() => this.page.MainText;

        public void AddSymptom(Symptom symptom) => this.symptoms.Add(symptom);

        public ObservableCollection<Symptom> GetSymptoms() => this.symptoms;
        public bool ContainsSymptom(string symptomName)
        {
            Symptom req = this.symptoms.FirstOrDefault(x => x.SymptomeName.Equals(symptomName));
            if (req != null) return true; else return false;
        }

        public void SetDate(DateTime dateTime) => this.page.DateOfRecord = dateTime.Date;

        public DateTime GetDate() => this.page.DateOfRecord;

        public bool CheckCorrectness()
        {
            if (this.page.MainText != null && this.page.MainText != string.Empty && this.page.DateOfRecord != null) return true; else return false;
        }

        public void CreateDiaryPageInfo()
        {
            CodifySymptomsData();

            if (this.stateMode == 0)
                App.Connector.CreateAndInsertData<DiaryPage>(this.page);
            else
                App.Connector.UpdateData<DiaryPage>(this.page);
        }

        private void CodifySymptomsData()
        {
            var codifiedSymptoms = string.Empty;
            foreach (Symptom symp in this.symptoms)
                codifiedSymptoms += string.Concat(symp.SymptomeName, '\n');
            this.page.AttachedSymptoms = codifiedSymptoms;
        }

        public void RecreateSymptomData(DiaryPage page)
        {
            if (page.AttachedSymptoms == null) return;

            var codifiedSymptoms = page.AttachedSymptoms.Split('\n');
            Array.Resize(ref codifiedSymptoms, codifiedSymptoms.Length - 1);
            foreach (var symp in codifiedSymptoms)
                this.symptoms.Add(new Symptom() { SymptomeName = symp });
        }

        public DiaryPage GetDiaryPage() => this.page;
    }
}
