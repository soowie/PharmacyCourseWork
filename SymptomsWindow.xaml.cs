using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace CourseWorkPharmacy
{
    /// <summary>
    /// Логика взаимодействия для SymptomsWindow.xaml
    /// </summary>
    /// 
    [AddINotifyPropertyChangedInterface]
    public partial class SymptomsWindow : Window
    {
        public bool AreSymptomsChangesUnsaved { get; set; } = false;

        public const string FilePath = "Symptoms.xml";

        public SymptomsWindow()
        {
            InitializeComponent();
            CreateOrDeserializeXML();
            DataContext = this;
            SymptomsList.ItemsSource = Symptoms.SymptomsList;
        }

        #region Serialization
        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreSymptomsChangesUnsaved)
            {
                SerializeSymptoms();
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        public void SerializeSymptoms()
        {
            SerializeXML();
            AreSymptomsChangesUnsaved = false;
            ActionInfo.Text = "Changes saved to a file";
            MyWindows.mainWindow.RenderSymptomList();
            MyWindows.medicinesWindow.RenderSymptomList();
        }

        public void SerializeXML()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<string>));
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, Symptoms.SymptomsList);
            }
        }
        #endregion

        #region Deserialize
        private void DeserializeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreSymptomsChangesUnsaved)
            {
                Deserialize();
                //MyWindows.prescriptionWindow.RenderMedicinesList();
                AreSymptomsChangesUnsaved = false;
                ActionInfo.Text = "Changes has been undo";
                MyWindows.mainWindow.RenderSymptomList();
                MyWindows.medicinesWindow.RenderSymptomList();
            }
            else
            {
                ActionInfo.Text = "File is already saved";
            }
        }

        private void Deserialize()
        {
            GetSymptomsFromXml();
            SymptomsList.ItemsSource = Symptoms.SymptomsList;
            ClearInput();
        }
        #endregion

        #region Helper Methods
        private void ClearInput()
        {
            SymptomBox.Text = string.Empty;
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

        private void GetSymptomsFromXml()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<string>));
            using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                ObservableCollection<string> symptoms = new ObservableCollection<string>((ObservableCollection<string>)xmlSerializer.Deserialize(fs));
                Symptoms.SymptomsList.Clear();
                Symptoms.SymptomsList = symptoms;
            }
        }

        public void CallDialog(object sender, CancelEventArgs e)
        {
            if (!AreSymptomsChangesUnsaved)
            {
                return;
            }
            else
            {
                string sMessageBoxText = "Symptoms changes were not saved. Do you want to save changes into file?";
                string sCaption = "Symptoms changes are not saved";

                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                if (rsltMessageBox == MessageBoxResult.Yes)
                {
                    SerializeSymptoms();
                }
            }
        }

        public bool IsInputCorrect()
        {
            if (string.IsNullOrEmpty(SymptomBox.Text))
            {
                MessageBox.Show("Can't add empty symptom");
                return false;
            }
            return true;
        }

        public void DeleteSelectedSymptom()
        {
            Symptoms.SymptomsList.Remove((string)SymptomsList.SelectedItems[0]);
            //MyWindows.prescriptionWindow.RenderMedicinesList();
            AreSymptomsChangesUnsaved = true;
            ActionInfo.Text = "Item Deleted";
        }

        public void DeleteSelectedSymptoms()
        {
            ObservableCollection<string> selectedItems = new ObservableCollection<string>();
            foreach (string eachItem in SymptomsList.SelectedItems)
            {
                selectedItems.Add(eachItem);
            }
            foreach (string eachItem in selectedItems)
            {
                Symptoms.SymptomsList.Remove(eachItem);
            }
            //MyWindows.prescriptionWindow.RenderMedicinesList();
            AreSymptomsChangesUnsaved = true;
            ActionInfo.Text = "Items Deleted";
        }
        #endregion

        #region Event Handlers
        protected override void OnClosing(CancelEventArgs e)
        {
            CallDialog(null, null);
            if (MyWindows.medicinesWindow.IsVisible || MyWindows.prescriptionWindow.IsVisible || MyWindows.mainWindow.IsVisible)
            {
                e.Cancel = true;  // cancels the window close
                this.Hide();      // Programmatically hides the window
            }
            else
            {
                App.Current.Shutdown();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SymptomsList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item first!");
            }
            else if (SymptomsList.SelectedItems.Count == 1)
            {
                DeleteSelectedSymptom();
            }
            else
            {
                DeleteSelectedSymptoms();
            }
            ActionInfo.Text = $"Symptom deleted from list!";
            AreSymptomsChangesUnsaved = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsInputCorrect())
            {
                Symptoms.SymptomsList.Add(SymptomBox.Text);
                ActionInfo.Text = $"Symptom \"{SymptomBox.Text}\" added to list!";
                ClearInput();
                AreSymptomsChangesUnsaved = true;
            }
        }

        private void SymptomsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SymptomsList.SelectedItems.Count == 0)
            {
                SymptomBox.Text = string.Empty;
                SymptomBox.IsReadOnly = false;
                SymptomBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");
                SymptomBox.Background = Brushes.White;
            }
            else
            {
                SymptomBox.Text = (string)SymptomsList.SelectedItem;
                SymptomBox.IsReadOnly = true;
                SymptomBox.BorderBrush = null;
                SymptomBox.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFE7E7E7");
            }
        }
        #endregion
    }
}