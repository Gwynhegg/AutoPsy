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

        // Метод инициализации работы с блоком стат. обработки.
        public void ProcessRecords(List<Database.Entities.DiaryPage> pages)     // В блок передается набор выбранных записей из дневника
        {
            nodes = App.Graph.GetAllItems();        // Поулчаем все узлы из графа
            statRecords = new Dictionary<string, Structures.DiaryResultRecords>();      // Инициализируем библиотеку для хранения результатов
            pageCuts = new List<Structures.DiaryPagesCut>();        // Инициализируем "срезы" - набор активных узлов на конкретную дату
            this.pages = pages.OrderBy(x => x.DateOfRecord).ToList();       // Сортируем данные записей по дате написания
        }

        // Метод обхода графа с целью заполнения структур - срезов
        public void RecursiveFilling()
        {
            foreach (var page in pages)
            {
                // Создаем срез с конкретно указанной датой
                Structures.DiaryPagesCut diaryCut = new Structures.DiaryPagesCut() { Date = page.DateOfRecord, Nodes = new Dictionary<string, int>() };
                var symptoms = PartiallyRecreateSymptoms(page);     // Восстанавливаем симптомы  по выбранной странице
                if (symptoms != null)
                    foreach (var symptom in symptoms)       // Перебираем кажыдй из симптомов
                        diaryCut.FillSymptomTree(symptom);      // Переходим в метод обхода дерева-графа и заполнения структуры
                pageCuts.Add(diaryCut);     // Добавляем заполненный срез в коллекцию
            }
        }

        // Метод высчитывания статистических параметров
        public void StatisticCalculation()
        {
            // Создаем новый объект - словарь. Ключем будет являться имя узла, значением - набор связанных с ним стат. параметров
            statRecords = new Dictionary<string, Structures.DiaryResultRecords>();
            while (pageCuts.Count > 0)      // Пока у нас есть срезы для обработки...
            {
                var node = pageCuts.First().Nodes.First();      // Поулчаем самый первый элемент (узел)

                // Выбираем все вхождения выбранного элемента в каждый из срезов
                var cutContainers = pageCuts.Where(x => x.Nodes.ContainsKey(node.Key)).ToList();
                // Создаем объект для хранения стат. параметров на указанный узел
                Structures.DiaryResultRecords diaryResult = new Structures.DiaryResultRecords() { Node = node.Key };
                // Передаем в метод для вычисления набор связанных с узлом дат вхождений и количество наложений (вхождений)
                diaryResult.Calculate(cutContainers.Select(x => x.Date).ToArray(), cutContainers.Select(x => x.Nodes[node.Key]).ToArray());
                // Добавляем получившуюся структуру в коллекцию результатов
                statRecords.Add(node.Key, diaryResult);

                foreach (var page in pageCuts)      // Удаляем обработанный элемент из каждого из срезов
                    page.Nodes.Remove(node.Key);

                for (int i = pageCuts.Count - 1; i >= 0; i--)
                    if (pageCuts[i].Nodes.Count == 0) pageCuts.RemoveAt(i);     // Если элементов в текущем срезе не осталось - удаляем его и переходим к следующему
                //if (pageCuts.First().Nodes.Count == 0) pageCuts.RemoveAt(0);        
            }
        }

        public Dictionary<string, Structures.DiaryResultRecords> GetStatisticResults()
        {
            return this.statRecords;
        }

        // Метод для выделения исключительно симптомов из набора узлов
        public Dictionary<string, Structures.DiaryResultRecords> GetOnlySymptoms()
        {
            var symptoms = App.Graph.GetSymptomNodes().Select(x => x.Id);
            return statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        // Метод для выделения исключительно проявлений из набора узлов
        public Dictionary<string, Structures.DiaryResultRecords> GetOnlyDisplays()
        {
            var symptoms = App.Graph.GetDiseasesNodes().Select(x => x.Id);
            return statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        // Метод для выделения исключительно категорий из набора узлов
        public Dictionary<string, Structures.DiaryResultRecords> GetOnlyCategories()
        {
            var symptoms = App.Graph.GetCategoryNodes().Select(x => x.Id);
            return statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }


        // Поскольку данные в базе хранятся в виде цельной строки с разделителями, необходимо преобразовать запись в удобообрабатываемый вид
        private string[] PartiallyRecreateSymptoms(Database.Entities.DiaryPage page)
        {
            if (page.AttachedSymptoms == null) return null;

            var splitted = page.AttachedSymptoms.Split('\\');
            Array.Resize(ref splitted, splitted.Length - 1);
            return splitted;
        }
    }
}
