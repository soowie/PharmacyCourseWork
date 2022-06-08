using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public static class Patients
    {
        private static ObservableCollection<Patient> patientsList = new ObservableCollection<Patient>();
        public static ObservableCollection<Patient> PatientsList
        {
            set
            {
                patientsList = value;

            }
            get
            {
                return patientsList;
            }
        }
    }
}
