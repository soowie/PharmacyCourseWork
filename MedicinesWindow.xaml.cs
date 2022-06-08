using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MedicinesWindow : Window
    {

        public MedicinesWindow()
        {
            InitializeComponent();
            RenderSymptomList();
            CreateOrDeserializeXML();
            DataContext = this;
            MyDataGrid.CellEditEnding += myDG_CellEditEnding;
            MyDataGrid.ItemsSource = Medicines.MedicinesList;
        }

        #region Medicine Interractions
        public bool AreMedicineChangesUnsaved { get; set; } = false;

        public const string FilePath = "Medicines.xml";

        #region Serialization
        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreMedicineChangesUnsaved)
            {
                SerializeMedicines();
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        public void SerializeMedicines()
        {
            SerializeXML();
            AreMedicineChangesUnsaved = false;
            ActionInfo.Text = "Changes saved to a file";
        }

        public void SerializeXML()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Medicine>));
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, Medicines.MedicinesList);
            }
        }

        #endregion

        #region Deserialization
        private void DeserializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreMedicineChangesUnsaved)
            {
                Deserialize();
                MyWindows.prescriptionWindow.RenderMedicinesList();
                AreMedicineChangesUnsaved = false;
                ActionInfo.Text = "Changes has been undo";
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        private void Deserialize()
        {
            GetMedicinesFromXml(FilePath);
            MyDataGrid.ItemsSource = Medicines.MedicinesList;
            ClearInput();
        }
        #endregion

        #region Helper Functions

        public void DeleteSelectedMedicine()
        {
            Medicines.MedicinesList.Remove((Medicine)MyDataGrid.SelectedItems[0]);
            MyWindows.prescriptionWindow.RenderMedicinesList();
            AreMedicineChangesUnsaved = true;
            ActionInfo.Text = "Item Deleted";
        }

        public void DeleteSelectedMedicines()
        {
            ObservableCollection<Medicine> selectedItems = new ObservableCollection<Medicine>();
            foreach (Medicine eachItem in MyDataGrid.SelectedItems)
            {
                selectedItems.Add(eachItem);
            }
            foreach (Medicine eachItem in selectedItems)
            {
                Medicines.MedicinesList.Remove(eachItem);
            }
            MyWindows.prescriptionWindow.RenderMedicinesList();
            AreMedicineChangesUnsaved = true;
            ActionInfo.Text = "Items Deleted";
        }

        public void AddMedicineToMedicinesList(Medicine medicine)
        {
            Medicines.MedicinesList.Add(medicine);
            MyWindows.prescriptionWindow.RenderMedicinesList();
        }

        public void GetMedicinesFromXml(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Medicine>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                ObservableCollection<Medicine> medicines = new ObservableCollection<Medicine>((ObservableCollection<Medicine>)xmlSerializer.Deserialize(fs));
                Medicines.MedicinesList.Clear();
                Medicines.MedicinesList = medicines;
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

        public void FillMedicinesListFromGrid()
        {
            foreach (Medicine item in MyDataGrid.Items)
            {
                Medicines.MedicinesList.Add(item);
            }
        }

        public void UpdateTextBoxes()
        {
            Medicine medicine = (Medicine)MyDataGrid.SelectedItems[0];
            MedicineNameBox.Text = medicine.Name;
            MedicineDescriptionBox.Text = medicine.Description;
            MedicineAmountBox.Text = medicine.Amount.ToString();
            MedicinePriceBox.Text = medicine.Price.ToString();
            UpdateSymptoms(medicine);
        }

        public void UpdateSymptoms(Medicine medicine)
        {
            ClearSymptomsList();
            for (int i = 0; i < medicine.SymptomsCovering.Count; i++)
            {
                string str = medicine.SymptomsCovering[i];
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
                    AreMedicineChangesUnsaved = true;
                    ClearInput();

                    // rowIndex has the row index
                    // bindingPath has the column's binding
                    // el.Text has the new, user-entered value
                }
            }
        }

        public void ClearInput()
        {
            MedicineNameBox.Text = string.Empty;
            MedicineDescriptionBox.Text = string.Empty;
            ClearSymptomsList();
            MedicinePriceBox.Text = string.Empty;
            MedicineAmountBox.Text = string.Empty;
            MyDataGrid.SelectedItem = null;
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
            if (string.IsNullOrEmpty(MedicineNameBox.Text))
            {
                MessageBox.Show("Wrong medicine name!");
                return false;
            }
            else if (string.IsNullOrEmpty(MedicineDescriptionBox.Text))
            {
                MessageBox.Show("Wrong medicine name!");
                return false;
            }
            else if (!int.TryParse(MedicinePriceBox.Text, out _) || Convert.ToInt32(MedicinePriceBox.Text) < 0)
            {
                MessageBox.Show("Wrong price!");
                return false;
            }
            else if (!int.TryParse(MedicineAmountBox.Text, out _) || Convert.ToInt32(MedicineAmountBox.Text) < 0)
            {
                MessageBox.Show("Wrong price!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CallDialog(object sender, CancelEventArgs e)
        {
            if (!AreMedicineChangesUnsaved)
            {
                return;
            }
            else
            {
                string sMessageBoxText = "Medicines changes were not saved. Do you want to save changes into file?";
                string sCaption = "Medicines changes are not saved";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                if (rsltMessageBox == MessageBoxResult.Yes)
                {
                    SerializeMedicines();
                }
            }
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

        #endregion

        #region Event Handlers
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreInputsCorrect())
            {
                Medicine medicine = new Medicine(MedicineNameBox.Text, MedicineDescriptionBox.Text, Convert.ToInt32(MedicinePriceBox.Text), Convert.ToInt32(MedicineAmountBox.Text), GetSymptomsFromList());
                AddMedicineToMedicinesList(medicine);
                ClearInput();
                AreMedicineChangesUnsaved = true;
                ActionInfo.Text = $"Object \"{medicine.Name}\" added to a list";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item first!");
            }
            else if (AreInputsCorrect())
            {
                Medicine item = (Medicine)MyDataGrid.SelectedItems[0];
                item.Name = MedicineNameBox.Text;
                item.Description = MedicineDescriptionBox.Text;
                item.SymptomsCovering = GetSymptomsFromList();
                item.Price = Convert.ToInt32(MedicinePriceBox.Text);
                item.Amount = Convert.ToInt32(MedicineAmountBox.Text);
                ActionInfo.Text = $"Object \"{item.Name}\" saved";
                ClearInput();
                MyWindows.prescriptionWindow.RenderMedicinesList();
                AreMedicineChangesUnsaved = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyDataGrid.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item first!");
            }
            else if (MyDataGrid.SelectedItems.Count == 1)
            {
                DeleteSelectedMedicine();
            }
            else
            {
                DeleteSelectedMedicines();
            }
        }

        private void MyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyDataGrid.SelectedItems.Count == 1)
            {
                UpdateTextBoxes();
            }
            else if (MyDataGrid.SelectedItems.Count == 0)
            {
                ClearInput();
            }
        }

        private void GoPatientsButton_Click(object sender, RoutedEventArgs e)
        {
            //CallDialog(null, null);
            MyWindows.mainWindow.Show();
        }

        private void GoPrescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            //CallDialog(null, null);
            MyWindows.prescriptionWindow.Show();
        }

        private void GoSymptomsButton_Click(object sender, RoutedEventArgs e)
        {
            MyWindows.symptomsWindow.Show();
        }

        private void PriceIncrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MedicinePriceBox.Text, out _) && Convert.ToInt32(MedicinePriceBox.Text) > 0 && MyDataGrid.SelectedItems.Count > 0)
            {
                MedicinePriceBox.Text = (Convert.ToInt32(MedicinePriceBox.Text) + 1).ToString();
                ((Medicine)MyDataGrid.SelectedItems[0]).Price++;
                AreMedicineChangesUnsaved = true;
            }
        }

        private void PriceDecrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MedicinePriceBox.Text, out _) && Convert.ToInt32(MedicinePriceBox.Text) > 0 && MyDataGrid.SelectedItems.Count > 0)
            {
                MedicinePriceBox.Text = (Convert.ToInt32(MedicinePriceBox.Text) - 1).ToString();
                ((Medicine)MyDataGrid.SelectedItems[0]).Price--;
                AreMedicineChangesUnsaved = true;
            }
        }

        private void AmountIncrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MedicineAmountBox.Text, out _) && Convert.ToInt32(MedicineAmountBox.Text) > 0 && MyDataGrid.SelectedItems.Count > 0)
            {
                MedicineAmountBox.Text = (Convert.ToInt32(MedicineAmountBox.Text) + 1).ToString();
                ((Medicine)MyDataGrid.SelectedItems[0]).Amount++;
                AreMedicineChangesUnsaved = true;
            }
        }

        private void AmountDecrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MedicineAmountBox.Text, out _) && Convert.ToInt32(MedicineAmountBox.Text) > 0 && MyDataGrid.SelectedItems.Count > 0)
            {
                MedicineAmountBox.Text = (Convert.ToInt32(MedicineAmountBox.Text) - 1).ToString();
                ((Medicine)MyDataGrid.SelectedItems[0]).Amount--;
                AreMedicineChangesUnsaved = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CallDialog(null, null);
            if (MyWindows.mainWindow.IsVisible || MyWindows.prescriptionWindow.IsVisible)
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

        private void SearchByNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchByNameBox.Text))
            {
                SearchByDescriptionBox.Text = string.Empty;
                var foundList = Medicines.MedicinesList.Where(m => m.Name.ToLower().Contains(SearchByNameBox.Text.ToLower())).ToList();
                MyDataGrid.ItemsSource = foundList;
            }
            else
            {
                MyDataGrid.ItemsSource = Medicines.MedicinesList;
            }
            
        }

        private void SearchByDescriptionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchByDescriptionBox.Text))
            {
                SearchByNameBox.Text = string.Empty;
                var foundList = Medicines.MedicinesList.Where(m => m.Description.ToLower().Contains(SearchByDescriptionBox.Text.ToLower())).ToList();
                MyDataGrid.ItemsSource = foundList;
            }
            else
            {
                MyDataGrid.ItemsSource = Medicines.MedicinesList;
            }
        }
    }
}