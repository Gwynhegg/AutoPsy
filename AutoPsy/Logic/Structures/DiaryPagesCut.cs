using System;
using System.Collections.Generic;

namespace AutoPsy.Logic.Structures
{
    public class DiaryPagesCut      // Класс среза, содержащий данный об узлах и наложениях (вхождениях) на указанную дату
    {
        public DateTime Date { get; set; }      // Дата снятия среза
        public Dictionary<string, int> Nodes { get; set; }      // Словарь узлов и связанного с ними количества вхождений

        public void FillSymptomTree(string symptom)     // Метод для обхода структуры графа и заполнения среза по симптому
        {
            var sympId = App.Graph.GetNodeId(symptom);      // Получаем Id узла
            if (!this.Nodes.ContainsKey(sympId))     // ЕСли такой симптом уже есть в наборе узлов (чего не должно быть)...
            {
                this.Nodes.Add(sympId, 1);       // Добавляем узел
                var diseases = App.Graph.SearchAncestorsLinkId(sympId);     // Получаем всех связанных с симптомом родителей-проявлений
                foreach (var disease in diseases)        // Для каждого из проявлений...
                {
                    if (this.Nodes.ContainsKey(disease))         // Есди проявление уже добавлено...
                        this.Nodes[disease]++;       // Увеличиваем количество вхождений данного проявления
                    else
                        this.Nodes.Add(disease, 1);      // Иначе добавляем его в словарь

                    var categories = App.Graph.SearchAncestorsLinkId(disease);      // Получаем связанные с проявлением категории
                    foreach (var category in categories)     // Для каждой из категорий...
                    {
                        if (this.Nodes.ContainsKey(category))        // Если категория уже есть в словаре...
                            this.Nodes[category]++;      // Увеличиваем количество вхождений данной категории
                        else
                            this.Nodes.Add(category, 1);     // Иначе добавляем ее в словарь
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
