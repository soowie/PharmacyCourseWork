using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyWindows.mainWindow = this;
            GenderBox.ItemsSource = Enum.GetValues(typeof(Gender));
            DataContext = this;
            PatientsList.ItemsSource = Patients.PatientsList;
            MyWindows.symptomsWindow = new SymptomsWindow();
            RenderSymptomList();
            MyWindows.medicinesWindow = new MedicinesWindow();
            MyWindows.prescriptionWindow = new PrescriptionWindow();
            if (!File.Exists("Patients.xml"))
            {
                File.Create("Patients.xml");
            }
            else
            {
                Deserialize();
            }
        }

        #region Patient Interractions
        public bool ArePatientChangesUnsaved { get; set; } = false;

        #region Serialization
        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArePatientChangesUnsaved)
            {
                SerializePatients();
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        public void SerializePatients()
        {
            SerializeXML();
            ArePatientChangesUnsaved = false;
            ActionInfo.Text = "Changes saved to a file";
        }

        public void SerializeXML()
        {
            if (File.Exists("Patients.xml"))
            {
                File.Delete("Patients.xml");
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Patient>));
            using (FileStream fs = new FileStream("Patients.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, Patients.PatientsList);
            }
        }

        #endregion

        #region Deserialization
        private void DeserializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArePatientChangesUnsaved)
            {
                Deserialize();
                ArePatientChangesUnsaved = false;
                ActionInfo.Text = "Changes has been undo";
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        private void Deserialize()
        {
            ClearInput();
            UpdatePatientsFromXml("Patients.xml");
        }
        #endregion

        #region Helper Functions

        public void DeleteSelectedPatient()
        {
            Patients.PatientsList.RemoveAt(PatientsList.SelectedIndex);
            MyWindows.prescriptionWindow.UpdatePatients();
            ArePatientChangesUnsaved = true;
            ActionInfo.Text = "Item Deleted";
        }

        public void DeleteSelectedPatients()
        {
            List<Patient> selectedItems = new List<Patient>();
            foreach (Patient patient in PatientsList.SelectedItems)
            {
                selectedItems.Add(patient);
            }
            foreach (Patient patient in selectedItems)
            {
                Patients.PatientsList.Remove(patient);
            }
            MyWindows.prescriptionWindow.UpdatePatients();
            ArePatientChangesUnsaved = true;
            ActionInfo.Text = "Items Deleted";
        }

        public void AddPatientToListView(Patient patient)
        {
            Patients.PatientsList.Add(patient);
        }

        public void UpdatePatientsFromXml(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Patient>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                ObservableCollection<Patient> patients = new ObservableCollection<Patient>((ObservableCollection<Patient>)xmlSerializer.Deserialize(fs));
                Patients.PatientsList.Clear();
                Patients.PatientsList = patients;
                PatientsList.ItemsSource = Patients.PatientsList;
                MyWindows.prescriptionWindow.UpdatePatients();
            }
        }

        public void UpdateTextBoxes(object sender)
        {
            Patient patient = Patients.PatientsList[PatientsList.SelectedIndex];
            PatientNameBox.Text = patient.Name;
            GenderBox.SelectedItem = patient.Gender;
            AgeBox.Text = patient.Age.ToString();
            BirthDateBox.SelectedDate = patient.BirthdayDate;
            UpdateSymptoms(patient);
        }

        public void UpdateSymptoms(Patient patient)
        {
            foreach (ListViewItem lvi in SymptomBox.Items)
            {
                var checkbox = (CheckBox)lvi.Content;
                checkbox.IsChecked = false;
            }
            for (int i = 0; i < patient.Symptoms.Count; i++)
            {
                string str = patient.Symptoms[i];
                for (int j = 0; j < SymptomBox.Items.Count; j++)
                {
                    ListViewItem lvi = (ListViewItem)SymptomBox.Items[j];
                    var checkbox = (CheckBox)lvi.Content;
                    if (str == (string)checkbox.Content)
                    {
                        checkbox.IsChecked = true;
                        break;
                    }
                }
            }
        }

        public void ClearInput()
        {
            PatientNameBox.Text = string.Empty;
            GenderBox.SelectedIndex = 0;
            BirthDateBox.SelectedDate = null;
            ClearSymptomsList();
            AgeBox.Text = string.Empty;

        }

        public void ClearSymptomsList()
        {
            foreach (ListViewItem lvi in SymptomBox.Items)
            {
                var checkbox = (CheckBox)lvi.Content;
                checkbox.IsChecked = false;
            }
        }

        private bool AreInputsCorrect()
        {
            if (string.IsNullOrEmpty(PatientNameBox.Text))
            {
                MessageBox.Show("Wrong patient name!");
                return false;
            }
            else if (BirthDateBox.SelectedDate == null)
            {
                MessageBox.Show("Wrong birthday date!");
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CallDialog(object sender, CancelEventArgs e)
        {
            if (!ArePatientChangesUnsaved)
            {
                return;
            }
            else
            {
                string sMessageBoxText = "Patients changes were not saved. Do you want to save changes into file?";
                string sCaption = "Patients changes are not saved";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                if (rsltMessageBox == MessageBoxResult.Yes)
                {
                    SerializePatients();
                }
            }
        }

        public int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //Excel documentation says "COMPLETE calendar years in between dates"
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month &&// if the start month and the end month are the same
                endDate.Day < startDate.Day// AND the end day is less than the start day
                || endDate.Month < startDate.Month)// OR if the end month is less than the start month
            {
                years--;
            }
            return years;
        }

        public void RenderSymptomList()
        {
            SymptomBox.Items.Clear();
            foreach (string str in Symptoms.SymptomsList)
            {
                CheckBox cb = new CheckBox();
                cb.Content = str;
                ListViewItem lvi = new ListViewItem();
                lvi.Content = cb;
                SymptomBox.Items.Add(lvi);
            }
        }

        public List<string> GetSymptomsFromList()
        {
            List<string> symptomsList = new List<string>();
            foreach (ListViewItem lvi in SymptomBox.Items)
            {
                var checkbox = (CheckBox)lvi.Content;
                if ((bool)checkbox.IsChecked)
                {
                    symptomsList.Add((string)checkbox.Content);
                }
            }
            return symptomsList;
        }
        #endregion

        #region Event Handlers
        private void GoSymptomsButton_Click(object sender, RoutedEventArgs e)
        {
            MyWindows.symptomsWindow.Show();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreInputsCorrect())
            {
                int age = GetDifferenceInYears((DateTime)BirthDateBox.SelectedDate, DateTime.Now);

                Patient patient = new Patient(PatientNameBox.Text, (DateTime)BirthDateBox.SelectedDate, (Gender)GenderBox.SelectedItem, age, GetSymptomsFromList());
                AddPatientToListView(patient);
                ClearInput();
                ArePatientChangesUnsaved = true;
                ActionInfo.Text = "Object added to a list";
                MyWindows.medicinesWindow.ActionInfo.Text = "Patient changed u!"; ;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item first!");
            }
            else if (AreInputsCorrect())
            {
                int age = GetDifferenceInYears((DateTime)BirthDateBox.SelectedDate, DateTime.Now);

                Patient patient = Patients.PatientsList[PatientsList.SelectedIndex];
                patient.Name = PatientNameBox.Text;
                patient.Age = age;
                patient.BirthdayDate = (DateTime)BirthDateBox.SelectedDate;
                patient.Gender = (Gender)GenderBox.SelectedItem;
                patient.Symptoms = GetSymptomsFromList();

                //((ListViewItem)PatientsList.SelectedItems[0]).Tag = patient;
                AgeBox.Text = patient.Age.ToString();
                ClearInput();
                PatientsList.SelectedItem = null;
                ArePatientChangesUnsaved = true;
                ActionInfo.Text = "Object settings saved!";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item first!");
            }
            else if (PatientsList.SelectedItems.Count == 1)
            {
                DeleteSelectedPatient();
            }
            else
            {
                DeleteSelectedPatients();
            }
        }

        private void GoMedicinesButton_Click(object sender, RoutedEventArgs e)
        {
            MyWindows.medicinesWindow.Show();
        }

        private void GoPrescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            MyWindows.prescriptionWindow.Show();
        }

        private void PatientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientsList.SelectedItems.Count == 1)
            {
                UpdateTextBoxes(sender);
            }
            else if (PatientsList.SelectedItems.Count == 0)
            {
                ClearInput();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CallDialog(null, null);
            if (MyWindows.medicinesWindow.IsVisible || MyWindows.prescriptionWindow.IsVisible)
            {
                e.Cancel = true;  // cancels the window close
                this.Hide();      // Programmatically hides the window
            }
            else
            {
                App.Current.Shutdown();
            }
        }
        #endregion
        #endregion
    }
}