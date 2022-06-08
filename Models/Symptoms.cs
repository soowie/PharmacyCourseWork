using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkPharmacy
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class Symptoms
    {
        private static ObservableCollection<string> symptomsList = new ObservableCollection<string>();

        public static ObservableCollection<string> SymptomsList { get => symptomsList; set => symptomsList = value; }
    }
}
