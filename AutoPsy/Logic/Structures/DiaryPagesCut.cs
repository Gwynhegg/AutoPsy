using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Logic.Structures
{
    public class DiaryPagesCut
    {
        public DateTime Date { get; set; }
        public Dictionary<string, int> Nodes { get; set; }

        public void FillSymptomTree(string symptom)
        {
            var sympId = App.Graph.GetNodeId(symptom);
            if (!Nodes.ContainsKey(sympId))
            {
                Nodes.Add(sympId, 1);
                var diseases = App.Graph.SearchAncestorsLinkId(sympId);
                foreach(var disease in diseases)
                {
                    if (Nodes.ContainsKey(disease)) 
                        Nodes[disease]++; 
                    else
                        Nodes.Add(disease, 0);

                    var categories = App.Graph.SearchAncestorsLinkId(disease);
                    foreach(var category in categories)
                    {
                        if (Nodes.ContainsKey(category))
                            Nodes[category]++;
                        else
                            Nodes.Add(category, 0);
                    }
                }
            }
            else return;
        }
    }
}
