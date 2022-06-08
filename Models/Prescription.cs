using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class Prescription
    {
        public Patient Patient { get; set; }
        public string Reccomendations { get; set; } = string.Empty;

        private ObservableCollection<Medicine> _prescriptedMedicines = new ObservableCollection<Medicine>();
        public ObservableCollection<Medicine> PrescriptedMedicines
        {
            set
            {
                _prescriptedMedicines = value;

            }
            get
            {
                return _prescriptedMedicines;
            }
        }

        [XmlIgnore]
        public string PrescriptedMedicinesString
        {
            
            get
            {
                if(PrescriptedMedicines.Count == 0)
                {
                    return "-";
                }
                List<string> strl = new List<string>();
                foreach (Medicine med in PrescriptedMedicines)
                {
                    strl.Add(med.Name);
                }
                return String.Join(", ", strl.ToArray());
            }
            private set
            {

            }
        }
    }
}
