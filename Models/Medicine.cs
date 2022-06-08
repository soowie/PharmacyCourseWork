using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class Medicine
    {
        public Medicine()
        {

        }

        public Medicine(string name, string description, double price, int amount, List<string> symptomsCovering)
        {
            Name = name;
            Description = description;
            Price = price;
            Amount = amount;
            SymptomsCovering = symptomsCovering;

        }

        public double Price { get; set; }

        public int Amount { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> SymptomsCovering { get; set; }

        [XmlIgnore]
        public string SymptomsString
        {
            get
            {
                return String.Join(", ", SymptomsCovering.ToArray());
            }
            private set
            {

            }
        }
    }
}
