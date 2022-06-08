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
    public class Medicines
    {

        private static ObservableCollection<Medicine> medicinesList = new ObservableCollection<Medicine>();
        public static ObservableCollection<Medicine> MedicinesList
        {
            set
            {
                medicinesList = value;

            }
            get
            {
                return medicinesList;
            }
        }
    }
}
