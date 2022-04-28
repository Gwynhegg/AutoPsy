using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AutoPsy.Logic
{
    public class DiaryPagesCalc
    {
        Structures.INode[] nodes;
        private Dictionary<string, Structures.DiaryResultRecords> statRecords;
        private List<Database.Entities.DiaryPage> pages;
        private List<Structures.DiaryPagesCut> pageCuts;
        public void ProcessRecords(List<Database.Entities.DiaryPage> pages)
        {
            nodes = App.Graph.GetAllItems();
            statRecords = new Dictionary<string, Structures.DiaryResultRecords>();
            pageCuts = new List<Structures.DiaryPagesCut>();
            this.pages = pages.OrderBy(x => x.DateOfRecord).ToList();
        }


        public void RecursiveFilling()
        {
            foreach (var page in pages)
            {
                Structures.DiaryPagesCut diaryCut = new Structures.DiaryPagesCut() { Date = page.DateOfRecord, Nodes = new Dictionary<string, int>() };
                var symptoms = PartiallyRecreateSymptoms(page);
                if (symptoms != null)
                    foreach (var symptom in symptoms)
                        diaryCut.FillSymptomTree(symptom);
                pageCuts.Add(diaryCut);
            }
        }
        public void StatisticCalculation()
        {
            statRecords = new Dictionary<string, Structures.DiaryResultRecords>();
            while (pageCuts.Count > 0)
            {
                var node = pageCuts.First().Nodes.First();

                var cutContainers = pageCuts.Where(x => x.Nodes.ContainsKey(node.Key)).ToList();
                Structures.DiaryResultRecords diaryResult = new Structures.DiaryResultRecords() { Node = node.Key };
                diaryResult.Calculate(cutContainers.Select(x => x.Date).ToArray(), cutContainers.Select(x => x.Nodes[node.Key]).ToArray());
                statRecords.Add(node.Key, diaryResult);

                foreach (var page in pageCuts)
                    page.Nodes.Remove(node.Key);

                if (pageCuts.First().Nodes.Count == 0) pageCuts.RemoveAt(0);
            }
        }

        public Dictionary<string, Structures.DiaryResultRecords> GetStatisticResults()
        {
            return this.statRecords;
        }

        public Dictionary<string, Structures.DiaryResultRecords> GetOnlySymptoms()
        {
            var symptoms = App.Graph.GetSymptomNodes().Select(x => x.Id);
            return statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, Structures.DiaryResultRecords> GetOnlyDisplays()
        {
            var symptoms = App.Graph.GetDiseasesNodes().Select(x => x.Id);
            return statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, Structures.DiaryResultRecords> GetOnlyCategories()
        {
            var symptoms = App.Graph.GetCategoryNodes().Select(x => x.Id);
            return statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }


        private string[] PartiallyRecreateSymptoms(Database.Entities.DiaryPage page)
        {
            if (page.AttachedSymptoms == null) return null;

            var splitted = page.AttachedSymptoms.Split('\\');
            Array.Resize(ref splitted, splitted.Length - 1);
            return splitted;
        }
    }
}
