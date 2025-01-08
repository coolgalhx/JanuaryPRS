using Patient_Record_System.DATA;
using System.Collections.ObjectModel;
using System.Net.Cache;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Patient_Record_System.MainWindow;
using Microsoft.EntityFrameworkCore;



namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public ObservableCollection<Patient> Patients { get; set; }
        public MainWindow()
        {
            //var Context = new ObservableCollection<Patient>();
            InitializeComponent();

            LoadPatient();
            using (var context = new MainDatabaseContext())
            {
                //context.Database.Migrate();
                context.Database.EnsureCreated();
            }
            LoadPatient();
        }





        private void LoadPatient()
        {
            // FoundUser.Content = $"{LogIn.VerifiedUser.Jobrole}";
            using (var db = new MainDatabaseContext())
            {
                db.Database.EnsureCreated();
                var patientList = db.Patients?.ToList();
                if (patientList != null)
                {
                    Patients = new ObservableCollection<Patient>(patientList);
                }
                else
                {
                    Patients = new ObservableCollection<Patient>();
                }

                datagridpatients.ItemsSource = Patients;
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtpatientname.Text))
            {
                MessageBox.Show("Patient name cannot be empty.");
                return false;
            }

            if (!int.TryParse(txtage.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Invalid age.");
                return false;
            }

            if (!DateTime.TryParse(txtdob.Text, out DateTime dob))
            {
                MessageBox.Show("Invalid date of birth. Please use the format yyyy-MM-dd.");
                return false;
            }

            return true;
        }


        private Patient selectedPatient = null;
        private void btnadd_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;



            using (var db = new MainDatabaseContext())
            {
                if (selectedPatient == null)
                {
                    var newPatient = new Patient()
                    {

                        //uses tryparse if that doesnt work, default value is 0
                        Name = txtpatientname.Text,
                        patientID = int.TryParse(txtpatientid.Text, out int id) ? id : 0,
                        Age = int.TryParse(txtage.Text, out int age) ? age : 0,
                        Gender = txtgender.Text,
                        DOB = DateTime.TryParse(txtdob.Text, out DateTime dob) ? dob : DateTime.MinValue,
                        Height = double.TryParse(txtheight.Text, out double height) ? height : 0.0,
                        Weight = double.TryParse(txtweigth.Text, out double weight) ? weight : 0.0,
                        Address = txtaddress.Text,
                        Number = txtnumber.Text,
                        Hospitalnumber = GenerateHospitalNumber(DateTime.Now),
                        NHSnumber = txtnhsnumber.Text,


                       

                    };
                    db.Patients.Add(newPatient);
                    db.SaveChanges();

                    Patients.Add(newPatient);
                    MessageBox.Show("New Patient Added! Please delete old record if applicable.");
                }
                else
                {
                    txtpatientname.Text = selectedPatient.Name;
                    txtpatientid.Text = selectedPatient.patientID.ToString();
                    txtage.Text = selectedPatient.Age.ToString();
                    txtgender.Text = selectedPatient.Gender;
                    txtdob.Text = selectedPatient.DOB.ToString("yyyy-MM-dd");
                    txtweigth.Text = selectedPatient.Weight.ToString();
                    txtnumber.Text = selectedPatient.Number;
                    txtnhsnumber.Text = selectedPatient.NHSnumber;

                    db.Patients.Update(selectedPatient);
                    db.SaveChanges();

                    MessageBox.Show("Patint Record Saved!");

                }
                datagridpatients.Items.Refresh();
                ClearFields();


                selectedPatient = null;
            }



        }

        private void ClearFields()
        {
            txtaddress.Clear();
            txtpatientname.Clear();
            txtage.Clear();
            txtdob.Clear();
            txtnumber.Clear();
            txtweigth.Clear();
            txtaddress.Clear();
            txtgender.Clear();
            txtheight.Clear();
        }

        private void btnupdate_Click(object sender, RoutedEventArgs e)
        {
            if (datagridpatients.SelectedItem is Patient selectedPatient)
            {

                populatefieldsagain(selectedPatient);
                MessageBox.Show("Here is the patient data, to save any updates click on the add button");
            }
            else
            {
                MessageBox.Show("Please select a patient record to update");
            }



        }
        private void populatefieldsagain(Patient selectedPatient)
        {
            txtpatientname.Text = selectedPatient.Name;
            txtpatientid.Text = selectedPatient.patientID.ToString();
            txtage.Text = selectedPatient.Age.ToString();
            txtgender.Text = selectedPatient.Gender;
            txtdob.Text = selectedPatient.DOB.ToString("yyyy-MM-dd");
            txtweigth.Text = selectedPatient.Weight.ToString();
            txtnumber.Text = selectedPatient.Number;
            txtnhsnumber.Text=selectedPatient.NHSnumber.ToString();

        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            //validation for whether patient is selected on the datagrid
            if (datagridpatients.SelectedItem is Patient selectedPatient)
            {

                var confirmResult = MessageBox.Show("Do you want to delete this patient record?",
                                         "Confirm Deletion",
                                          MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    using (var db = new MainDatabaseContext())
                    {

                        db.Patients.Attach(selectedPatient);
                        db.Patients.Remove(selectedPatient);
                        db.SaveChanges();
                    }


                    Patients.Remove(selectedPatient);

                    MessageBox.Show("Patient record deleted");
                }
            }
            else
            {

                MessageBox.Show("Please select a patient record to delete");
            }

        }



       

        //track number of patient added for ech specific date
        private static Dictionary<string, int> dailyPatientCounter = new Dictionary<string, int>();

        private string GenerateHospitalNumber(DateTime dateAdded)
        {
            string datePart = dateAdded.ToString("ddMMyy");

            //checks if current date(datePart already exists in dictionary)
            if (!dailyPatientCounter.ContainsKey(datePart))
            {
                dailyPatientCounter[datePart] = 0;//initialise count for that day to 0
            }
            //increment for unique seq number
            dailyPatientCounter[datePart]++;

            int sequencenumber = dailyPatientCounter[datePart];
            //format hospital number
            string hospitalnumber = $"PRS-{datePart}-{sequencenumber:D2}";

            return hospitalnumber;

        }

        private void btnautogenerate_Click(object sender, RoutedEventArgs e)
        {
            //checks if patient is in the datagrid
            if (datagridpatients.SelectedItem is Patient selectedPatient)
            {
                string hospitalnumber = GenerateHospitalNumber(DateTime.Now);

                selectedPatient.Hospitalnumber = hospitalnumber;

                //opens database and updates hospital number
                using (var db = new MainDatabaseContext())
                {
                    //find patient record that has same name
                    var patientInDb = db.Patients.FirstOrDefault(p => p.Name == selectedPatient.Name);
                    //if patient exists update and save 
                    if (patientInDb != null)
                    {
                        patientInDb.Hospitalnumber = hospitalnumber;
                        db.SaveChanges();
                    }
                }

                datagridpatients.Items.Refresh();

                MessageBox.Show($"Hospital Number Generated: {hospitalnumber}");
            }
            else
            {
                MessageBox.Show("Please select a patient record");
            }

        }
    }  
}



