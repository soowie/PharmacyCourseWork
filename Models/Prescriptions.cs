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
    public static class Prescriptions
    {
        private static ObservableCollection<Prescription> prescriptionsList = new ObservableCollection<Prescription>();
        public static ObservableCollection<Prescription> PrescriptionsList
        {
            set
            {
                prescriptionsList = value;

            }
            get
            {
                return prescriptionsList;
            }
        }
    }
}
