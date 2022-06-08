using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWorkPharmacy
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class Patient
    {
        public Patient()
        {
        }

        public Patient(string name, DateTime birthdayDate, Gender gender, int age, List<string> symptoms)
        {
            Name = name;
            BirthdayDate = birthdayDate;
            Gender = gender;
            Age = age;
            Symptoms = symptoms;
        }
        public string Name { get; set; }

        public DateTime BirthdayDate { get; set; }

        public Gender Gender { get; set; }

        public int Age { get; set; }

        //private List<Symptom> symptoms;

        public List<string> Symptoms { get; set; }
    }
}