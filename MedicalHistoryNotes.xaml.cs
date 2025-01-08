using Patient_Record_System.DATA;
using System;
using System.Collections.Generic;
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
using System.Linq;
using System.Collections.ObjectModel;

namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for MedicalHistoryNotes.xaml
    /// </summary>
    /// 

    public partial class MedicalHistoryNotes : Window
    {
        public MedicalHistoryNotes()
        {
            InitializeComponent();
            LoadMedicalHistoryNotes();
        }

        public ObservableCollection<AppointmentGS> Appointments { get; set; } = new ObservableCollection<AppointmentGS>();
        private void LoadMedicalHistoryNotes()
        {
            using (var db = new MainDatabaseContext())
            {
                var appointmentsFromDb = db.Appointments.ToList();
                foreach (var appointment in appointmentsFromDb)
                {
                    Appointments.Add(new AppointmentGS
                    {
                        PatientId = appointment.PatientId,
                        Fullname = appointment.Fullname,
                        DOB = appointment.DOB,
                        AppointmentDate = appointment.AppointmentDate,
                        AppointmentTime = appointment.AppointmentTime,
                        Healthcarepractitioner = appointment.Healthcarepractitioner,
                        Medicalhistory = appointment.Medicalhistory,
                        DatemedicalhistorySaved = appointment.DatemedicalhistorySaved
                    });
                }
            }

            datagridmedicalhistorynotes.ItemsSource = null;
            datagridmedicalhistorynotes.ItemsSource = Appointments;





        }

        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
            string search = txtmedicalsearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(search))
            {
                MessageBox.Show("Enter text to search");
                return;
            }
            using (var db = new MainDatabaseContext())
            {
                var appointmentsFromDb = db.Appointments
                    //performs evaluation locally
                     .AsEnumerable()
                        .Where(a => a.Fullname.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                        a.Medicalhistory.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                        a.PatientId.ToString().Contains(search)) 
                        .OrderByDescending(a => a.DatemedicalhistorySaved) 
                        .ToList();

                if (appointmentsFromDb.Count == 0)
                {
                    MessageBox.Show("No matching record found");
                    datagridmedicalhistorynotes.ItemsSource = null;
                    return;
                }
                else
                {
                    MessageBox.Show($"Found {appointmentsFromDb.Count} matching appointment(s)");
                }

                
                datagridmedicalhistorynotes.ItemsSource = null;
                datagridmedicalhistorynotes.ItemsSource = appointmentsFromDb;
            }



        }
    }
        
}
    

