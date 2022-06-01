using AutoPsy.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPsy.Logic
{
    public class DiaryPagesCalc
    {
        private INode[] nodes;
        private Dictionary<string, Structures.DiaryResultRecords> statRecords;
        private List<DiaryPage> pages;
        private List<Structures.DiaryPagesCut> pageCuts;

        // Метод инициализации работы с блоком стат. обработки.
        public void ProcessRecords(List<DiaryPage> pages)     // В блок передается набор выбранных записей из дневника
        {
            this.nodes = App.Graph.GetAllItems();        // Поулчаем все узлы из графа
            this.statRecords = new Dictionary<string, Structures.DiaryResultRecords>();      // Инициализируем библиотеку для хранения результатов
            this.pageCuts = new List<Structures.DiaryPagesCut>();        // Инициализируем "срезы" - набор активных узлов на конкретную дату
            this.pages = pages.OrderBy(x => x.DateOfRecord).ToList();       // Сортируем данные записей по дате написания
        }

        // Метод обхода графа с целью заполнения структур - срезов
        public void RecursiveFilling()
        {
            TryToMergeData();

            foreach (DiaryPage page in this.pages)
            {
                // Создаем срез с конкретно указанной датой
                var diaryCut = new Structures.DiaryPagesCut() { Date = page.DateOfRecord, Nodes = new Dictionary<string, int>() };
                var symptoms = PartiallyRecreateSymptoms(page);     // Восстанавливаем симптомы  по выбранной странице
                if (symptoms != null)
                {
                    foreach (var symptom in symptoms)       // Перебираем кажыдй из симптомов
                        diaryCut.FillSymptomTree(symptom);      // Переходим в метод обхода дерева-графа и заполнения структуры
                }

                this.pageCuts.Add(diaryCut);     // Добавляем заполненный срез в коллекцию
            }
        }

        // Метод высчитывания статистических параметров
        public void StatisticCalculation()
        {
            // Создаем новый объект - словарь. Ключем будет являться имя узла, значением - набор связанных с ним стат. параметров
            this.statRecords = new Dictionary<string, Structures.DiaryResultRecords>();
            while (this.pageCuts.Count > 0)      // Пока у нас есть срезы для обработки...
            {
                KeyValuePair<string, int> node = this.pageCuts.First().Nodes.First();      // Поулчаем самый первый элемент (узел)

                // Выбираем все вхождения выбранного элемента в каждый из срезов
                var cutContainers = this.pageCuts.Where(x => x.Nodes.ContainsKey(node.Key)).ToList();
                // Создаем объект для хранения стат. параметров на указанный узел
                var diaryResult = new Structures.DiaryResultRecords() { Node = node.Key };
                // Передаем в метод для вычисления набор связанных с узлом дат вхождений и количество наложений (вхождений)
                diaryResult.Calculate(cutContainers.Select(x => x.Date).ToArray(), cutContainers.Select(x => x.Nodes[node.Key]).ToArray());
                // Добавляем получившуюся структуру в коллекцию результатов
                this.statRecords.Add(node.Key, diaryResult);

                foreach (Structures.DiaryPagesCut page in this.pageCuts)      // Удаляем обработанный элемент из каждого из срезов
                    page.Nodes.Remove(node.Key);

                for (var i = this.pageCuts.Count - 1; i >= 0; i--)
                    if (this.pageCuts[i].Nodes.Count == 0) this.pageCuts.RemoveAt(i);     // Если элементов в текущем срезе не осталось - удаляем его и переходим к следующему
            }
        }

        public Dictionary<string, Structures.DiaryResultRecords> GetStatisticResults() => this.statRecords;

        // Метод для выделения исключительно симптомов из набора узлов
        public Dictionary<string, Structures.DiaryResultRecords> GetOnlySymptoms()
        {
            IEnumerable<string> symptoms = App.Graph.GetSymptomNodes().Select(x => x.Id);
            return this.statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        // Метод для выделения исключительно проявлений из набора узлов
        public Dictionary<string, Structures.DiaryResultRecords> GetOnlyDisplays()
        {
            IEnumerable<string> symptoms = App.Graph.GetDiseasesNodes().Select(x => x.Id);
            return this.statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        // Метод для выделения исключительно категорий из набора узлов
        public Dictionary<string, Structures.DiaryResultRecords> GetOnlyCategories()
        {
            IEnumerable<string> symptoms = App.Graph.GetCategoryNodes().Select(x => x.Id);
            return this.statRecords.Where(x => symptoms.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }


        // Поскольку данные в базе хранятся в виде цельной строки с разделителями, необходимо преобразовать запись в удобообрабатываемый вид
        private string[] PartiallyRecreateSymptoms(DiaryPage page)
        {
            if (page.AttachedSymptoms == null) return null;

            var splitted = page.AttachedSymptoms.Split('\n');
            Array.Resize(ref splitted, splitted.Length - 1);
            return splitted;
        }

        private string CodifySymptoms(string[] symptoms) => string.Join("\n", symptoms);

        private void TryToMergeData()       // алгоритм слияния записей
        {
            var iterator = 0;
            while (iterator < this.pages.Count - 1)      // пока итератор не дошел до конца списка...
            {
                if (DateTime.Compare(this.pages[iterator].DateOfRecord.Date, this.pages[iterator + 1].DateOfRecord.Date) == 0)        // если даты текущего листа совпадают со следующим
                {
                    var firstSymptoms = PartiallyRecreateSymptoms(this.pages[iterator]);     // получаем симптомы первого листа
                    var secondSymptoms = PartiallyRecreateSymptoms(this.pages[iterator + 1]);        // получаем симптомы второго листа
                    var resultSymptoms = firstSymptoms.Union(secondSymptoms).ToArray();     // объединяем симптомы логической функцией
                    this.pages[iterator].AttachedSymptoms = CodifySymptoms(resultSymptoms);      // сохраняем изменения
                    this.pages.RemoveAt(iterator + 1);       // удаляем дублирующий лист
                }
                else
                {
                    iterator++;
                }
            }
        }
    }
}
