using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    /// <summary>
    /// Логика взаимодействия для PrescriptionWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class PrescriptionWindow : Window
    {
        public PrescriptionWindow()
        {
            InitializeComponent();
            RenderMedicinesList();
            CreateOrDeserializeXML();
            DataContext = this;
            PatientComboBox.ItemsSource = Patients.PatientsList;
            PrescriptionsDataGrid.ItemsSource = Prescriptions.PrescriptionsList;
        }

        public Patient SelectedPatient
        {
            get
            {
                return (Patient)PatientComboBox.SelectedItem;
            }
        }

        public string FilePath = "Prescriptions.xml";

        public bool ArePrescriptionsChangesUnsaved { get; set; } = false;

        #region Serialization
        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArePrescriptionsChangesUnsaved)
            {
                SerializePrescriptions();
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        public void SerializePrescriptions()
        {
            SerializeXML();
            ArePrescriptionsChangesUnsaved = false;
            ActionInfo.Text = "Changes saved to a file";
        }

        public void SerializeXML()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Prescription>));
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, Prescriptions.PrescriptionsList);
            }
        }
        #endregion

        #region Deserialization
        private void DeserializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArePrescriptionsChangesUnsaved)
            {
                Deserialize();
                ArePrescriptionsChangesUnsaved = false;
                ActionInfo.Text = "Changes has been undo";
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        private void Deserialize()
        {
            GetPrescriptionsFromXml(FilePath);
            //ClearInput();
        }

        public void GetPrescriptionsFromXml(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Prescription>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                ObservableCollection<Prescription> prescriptions = new ObservableCollection<Prescription>((ObservableCollection<Prescription>)xmlSerializer.Deserialize(fs));
                Prescriptions.PrescriptionsList.Clear();
                Prescriptions.PrescriptionsList = prescriptions;
                PrescriptionsDataGrid.ItemsSource = Prescriptions.PrescriptionsList;
            }
        }
        #endregion

        #region Helper Methods

        public void CreateOrDeserializeXML()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }
            else
            {
                Deserialize();
            }
        }

        public void DeleteSelectedPrescription()
        {
            Prescriptions.PrescriptionsList.Remove((Prescription)PrescriptionsDataGrid.SelectedItems[0]);
            ArePrescriptionsChangesUnsaved = true;
            ActionInfo.Text = "Item Deleted";
        }

        public void DeleteSelectedPrescriptions()
        {
            ObservableCollection<Prescription> selectedItems = new ObservableCollection<Prescription>();
            foreach (Prescription eachItem in PrescriptionsDataGrid.SelectedItems)
            {
                selectedItems.Add(eachItem);
            }
            foreach (Prescription eachItem in selectedItems)
            {
                Prescriptions.PrescriptionsList.Remove(eachItem);
            }
            ArePrescriptionsChangesUnsaved = true;
            ActionInfo.Text = "Items Deleted";
        }

        public void ClearInput()
        {
            PatientComboBox.SelectedIndex = 0;
            ClearMedicinesList();
            ReccomendationsBox.Text = string.Empty;
        }

        public void RenderMedicinesList()
        {
            MedicinesBox.Items.Clear();
            foreach (Medicine med in Medicines.MedicinesList)
            {
                CheckBox cb = new CheckBox();
                cb.Content = med.Name;
                cb.Tag = med;
                cb.Click += CheckBox_Click;
                ListViewItem lvi = new ListViewItem();
                lvi.Content = cb;
                MedicinesBox.Items.Add(lvi);
            }
        }

        public ObservableCollection<Medicine> GetMedicinesFromList()
        {
            ObservableCollection<Medicine> med = new ObservableCollection<Medicine>();
            foreach (ListViewItem lvi in MedicinesBox.Items)
            {
                var checkbox = (CheckBox)lvi.Content;
                if ((bool)checkbox.IsChecked)
                {
                    med.Add((Medicine)checkbox.Tag);
                }
            }
            return med;
        }

        public void CallDialog(object sender, CancelEventArgs e)
        {
            if (!ArePrescriptionsChangesUnsaved)
            {
                return;
            }
            else
            {
                string sMessageBoxText = "Prescriptions changes were not saved. Do you want to save changes into file?";
                string sCaption = "Prescriptions changes are not saved";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                if (rsltMessageBox == MessageBoxResult.Yes)
                {
                    SerializePrescriptions();
                }
            }
        }

        public bool SaveWindowsChangesPrompt()
        {
            string sMessageBoxText = "Before generating, you must have Medicines and Patients data saved. Would you like to save them now?";
            string sCaption = "Confirmation of data saving";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            if (rsltMessageBox == MessageBoxResult.Yes)
            {
                MyWindows.medicinesWindow.SerializeMedicines();
                MyWindows.mainWindow.SerializePatients();
                return true;
            }
            return false;
        }

        public ObservableCollection<Medicine> FindSuitableMedicines(Patient patient)
        {
            ObservableCollection<Medicine> suitableMedicines = new ObservableCollection<Medicine>();
            foreach (string symptom in patient.Symptoms)
            {
                foreach (Medicine med in Medicines.MedicinesList)
                {
                    if (med.SymptomsCovering.Contains(symptom) && !suitableMedicines.Contains(med))
                    {
                        suitableMedicines.Add(med);
                    }
                }
            }
            return suitableMedicines;
        }

        public void ClearMedicinesList()
        {
            foreach (ListViewItem lvi in MedicinesBox.Items)
            {
                var checkbox = (CheckBox)lvi.Content;
                checkbox.IsChecked = false;
            }
        }

        public void UpdateMedicines()
        {
            ClearMedicinesList();
            ObservableCollection<Medicine> ocm = ((Prescription)PrescriptionsDataGrid.SelectedItem).PrescriptedMedicines;
            List<string> strs = new List<string>();
            foreach (var item in ocm)
            {
                strs.Add(item.Name);
            }
            foreach (ListViewItem item in MedicinesBox.Items)
            {
                CheckBox ch = (CheckBox)item.Content;
                if (strs.Contains(((Medicine)ch.Tag).Name))
                {
                    ch.IsChecked = true;
                }
            }
        }

        public void AutoGeneratePrescriptions()
        {
            Prescriptions.PrescriptionsList.Clear();
            foreach (Patient patient in Patients.PatientsList)
            {
                Prescription prescription = new Prescription();
                prescription.Patient = patient;
                prescription.PrescriptedMedicines = FindSuitableMedicines(patient);
                Prescriptions.PrescriptionsList.Add(prescription);
            }
            ArePrescriptionsChangesUnsaved = true;
        }

        public void UpdatePatients()
        {
            PatientComboBox.ItemsSource = Patients.PatientsList;
            PatientComboBox.SelectedIndex = 0;
        }

        public void UpdateTextBoxes()
        {
            PatientComboBox.SelectedIndex = Prescriptions.PrescriptionsList.IndexOf((Prescription)PrescriptionsDataGrid.SelectedItem); // works
            UpdateMedicines();
            ReccomendationsBox.Text = ((Prescription)PrescriptionsDataGrid.SelectedItem).Reccomendations;
        }

        void myDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    int rowIndex = e.Row.GetIndex();
                    var el = e.EditingElement as TextBox;
                    ArePrescriptionsChangesUnsaved = true;
                    ClearInput();

                    // rowIndex has the row index
                    // bindingPath has the column's binding
                    // el.Text has the new, user-entered value
                }
            }
        }
        #endregion

        #region Event Handlers

        private void GoPatientsButton_Click(object sender, RoutedEventArgs e)
        {
            CallDialog(null, null);
            MyWindows.mainWindow.Show();
        }

        private void GoMedicinesButton_Click(object sender, RoutedEventArgs e)
        {
            CallDialog(null, null);
            MyWindows.medicinesWindow.Show();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CallDialog(null, null);
            if (MyWindows.mainWindow.IsVisible || MyWindows.medicinesWindow.IsVisible)
            {
                e.Cancel = true;  // cancels the window close
                this.Hide();      // Programmatically hides the window
            }
            else
            {
                App.Current.Shutdown();
            }
        }

        private void PrescriptionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Prescriptions.PrescriptionsList.Add(new Prescription() { Patient = SelectedPatient, PrescriptedMedicines = GetMedicinesFromList(), Reccomendations = ReccomendationsBox.Text });
            ArePrescriptionsChangesUnsaved = true;
            ActionInfo.Text = "Prescription added to a list";
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItems.Count > 0)
            {
                Prescription pr = (Prescription)PrescriptionsDataGrid.SelectedItems[0];
                pr.PrescriptedMedicines = GetMedicinesFromList();
                ArePrescriptionsChangesUnsaved = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item first!");
            }
            else if (PrescriptionsDataGrid.SelectedItems.Count == 1)
            {
                DeleteSelectedPrescription();
            }
            else
            {
                DeleteSelectedPrescriptions();
            }
        }

        private void GeneratePrescription_Click(object sender, RoutedEventArgs e)
        {
            if (MyWindows.mainWindow.ArePatientChangesUnsaved || MyWindows.medicinesWindow.AreMedicineChangesUnsaved)
            {
                if (SaveWindowsChangesPrompt())
                {
                    AutoGeneratePrescriptions();
                }
            }
            else
            {
                AutoGeneratePrescriptions();
            }
        }

        private void PrescriptionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItems.Count == 1)
            {
                UpdateTextBoxes();
            }
            else if (PrescriptionsDataGrid.SelectedItems.Count == 0)
            {
                ClearInput();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItem != null)
            {
                ((Prescription)PrescriptionsDataGrid.SelectedItem).Reccomendations = ReccomendationsBox.Text;
                ArePrescriptionsChangesUnsaved = true;
            }
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (PrescriptionsDataGrid.SelectedItems.Count > 0)
            {
                PrintDialog printDialog = new PrintDialog();
                if ((bool)printDialog.ShowDialog().GetValueOrDefault())
                {
                    FlowDocument flowDocument = new FlowDocument();
                    for (int i = 0; i < PrescriptionsDataGrid.SelectedItems.Count; i++)
                    {
                        foreach (string line in ((Prescription)PrescriptionsDataGrid.SelectedItems[i]).Reccomendations.Split('\n'))
                        {
                            Paragraph name = new Paragraph();
                            name.FontWeight = FontWeights.Bold;
                            name.Foreground = Brushes.Red;
                            name.Margin = new Thickness(0);
                            name.Inlines.Add(((Prescription)PrescriptionsDataGrid.SelectedItems[i]).Patient.Name + ":");
                            flowDocument.Blocks.Add(name);
                            Paragraph myParagraph2 = new Paragraph();
                            Paragraph myParagraph2_2 = new Paragraph();
                            myParagraph2_2.FontWeight = FontWeights.Bold;
                            myParagraph2_2.Margin = new Thickness(0);
                            myParagraph2.Margin = new Thickness(0);
                            myParagraph2_2.Inlines.Add("Medicament prescripted: ");
                            myParagraph2.Inlines.Add(((Prescription)PrescriptionsDataGrid.SelectedItems[i]).PrescriptedMedicinesString);
                            flowDocument.Blocks.Add(myParagraph2_2);
                            flowDocument.Blocks.Add(myParagraph2);
                            Paragraph myParagraph3 = new Paragraph();
                            Paragraph myParagraph3_2 = new Paragraph();
                            myParagraph3_2.FontWeight = FontWeights.Bold;
                            myParagraph3_2.Margin = new Thickness(0);
                            myParagraph3.Margin = new Thickness(0);
                            myParagraph3_2.Inlines.Add("Reccomendated actions: ");
                            myParagraph3.Inlines.Add(new Run(line));
                            flowDocument.Blocks.Add(myParagraph3_2);
                            flowDocument.Blocks.Add(myParagraph3);
                            flowDocument.Blocks.Add(new Paragraph());
                        }
                    }
                    DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                    printDialog.PrintDocument(paginator, /*((Prescription)PrescriptionsDataGrid.SelectedItems[0]).Patient.Name*/ "Reciepts");
                }
            }
            else
            {
                MessageBox.Show("Before printing, selects prescriptions for a print");
            }
        }
        #endregion
    }
}
