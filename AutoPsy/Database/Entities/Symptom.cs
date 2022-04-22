using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.Database.Entities
{
    public class Symptom
    {
        private string symptomeName;
        public string SymptomeName
        {
            get { return symptomeName; }
            set { symptomeName = value; }
        }
    }
}
