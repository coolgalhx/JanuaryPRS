using Microsoft.EntityFrameworkCore;
using Patient_Record_System.DATA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for SearchPatient.xaml
    /// </summary>
    public partial class SearchPatient : Window
    {
        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        public SearchPatient()
        {
            InitializeComponent();
            ListAllPatientson();
        }
        private void ListAllPatientson()
        {
            using (var db = new MainDatabaseContext())
            {
              
                var patientsFromDb = db.Patients.ToList();

               
                foreach (var patient in patientsFromDb)
                {
                    Patients.Add(patient);
                }
            }


            datagridsearchpatient.ItemsSource = null;
            datagridsearchpatient.ItemsSource = Patients;
        }

        private void btngo_Click(object sender, RoutedEventArgs e)
        {   
            string search=txtsearchpatient.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                MessageBox.Show("Enter text to search");
                return;
            }
           using(var db=new MainDatabaseContext())
           {
             var viewedpatients=db.Patients
                    .Where(p => p.Name.Contains(search)||p.Address.Contains(search))
                    .ToList();
                if (viewedpatients.Count == 0)
                {
                    MessageBox.Show("Patient not found");
                }
                else
                {
                    MessageBox.Show($" Found {viewedpatients.Count} patient record(s)");
                }
                datagridsearchpatient.ItemsSource = viewedpatients;
           }
            

        }
        
    }
}
