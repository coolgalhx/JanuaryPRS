using Microsoft.EntityFrameworkCore;
using Patient_Record_System.DATA;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for Appointment.xaml
    /// </summary>
    public partial class Appointment : Window
    {
        public static EmployeeAccount VerifiedUser { get; set; }


        public Appointment()
        {

            InitializeComponent();
            VerifiedUser = MainMenu.VerifiedUser;
            using (var context = new MainDatabaseContext())
            {

                context.Database.EnsureCreated();
            }
            Loadappointments();
            bool isDoctor = VerifiedUser.Jobrole != null && VerifiedUser.Jobrole.Equals("Doctor", StringComparison.OrdinalIgnoreCase);
            if (isDoctor)
            {
                txthealthcareprac.IsEnabled = false;
            }

        }

        public ObservableCollection<AppointmentGS> appointments { get; set; }
        private void Loadappointments()
        {

            using (var db = new MainDatabaseContext())
            {

                db.Database.EnsureCreated();
                var appointmentList = db.Appointments?.ToList();
                if (appointmentList != null)
                {
                    appointments = new ObservableCollection<AppointmentGS>(appointmentList);
                }
                else
                {
                    appointments = new ObservableCollection<AppointmentGS>();
                }

                datagridappointments.ItemsSource = appointments;
            }
        }

        DateTime CurrentDate = DateTime.Now;






        private void btnaddaptmnt(object sender, RoutedEventArgs e)
        {
            if (VerifiedUser == null)
            {
                MessageBox.Show("Error: No logged-in user detected. Please log in again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isDoctor = VerifiedUser.Jobrole != null && VerifiedUser.Jobrole.Equals("Doctor", StringComparison.OrdinalIgnoreCase);

            using (var db = new MainDatabaseContext())
            {   
                //validations
                if (!int.TryParse(txtpatientid.Text, out int patientId))
                {
                    MessageBox.Show("Invalid Patient ID. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!DateTime.TryParse(txtdob.Text, out DateTime dob))
                {
                    MessageBox.Show("Invalid Patient DOB. Please enter a valid Date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingPatient = db.Patients.FirstOrDefault(p => p.patientID == patientId && p.Name == txtfullname.Text && p.DOB == dob);
                if (existingPatient == null)
                {
                    MessageBox.Show("Patient ID or Name or DOB not found. Please ensure the patient is registered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!DateTime.TryParse(txtappointmentdate.Text, out DateTime appointmentDate))
                {
                    MessageBox.Show("Invalid Appointment Date. Please enter a valid date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int timeCheck = DateTime.Compare(CurrentDate, appointmentDate);
                if (timeCheck > 0)
                {
                    MessageBox.Show("Invalid Date: Date is in the past");
                    return;
                }

                if (!TimeSpan.TryParse(txtappointmenttime.Text, out TimeSpan appointmentTime))
                {
                    MessageBox.Show("Invalid Appointment Time. Please enter a valid time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string selectedPractitioner = txthealthcareprac.Text.Trim();

                if (isDoctor)
                {
                    // doctor logged in can only book appointment for themselves
                    selectedPractitioner = VerifiedUser.Name;

                    // patient conflict; same patient same time
                    var patientConflict = db.Appointments.FirstOrDefault(a =>
                        a.PatientId == patientId &&
                        a.AppointmentDate.Date == appointmentDate.Date &&
                        a.AppointmentTime == appointmentTime);

                    if (patientConflict != null)
                    {
                        MessageBox.Show($"Scheduling conflict detected! Patient {existingPatient.Name} already has an appointment at this time.",
                            "Scheduling Conflict!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // doctor conflict;same doctor same time
                    var doctorConflict = db.Appointments.FirstOrDefault(a =>
                        a.Healthcarepractitioner == selectedPractitioner &&
                        a.AppointmentDate.Date == appointmentDate.Date &&
                        a.AppointmentTime == appointmentTime);

                    if (doctorConflict != null)
                    {
                        MessageBox.Show($"Scheduling conflict detected! {selectedPractitioner} already has an appointment at this time.",
                            "Conflict Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    MessageBox.Show($"Appointment has been created on behalf of Doctor {VerifiedUser.Name}.");
                }
                else
                {
                    // patient conflict; same patient same time
                    var patientConflict = db.Appointments.FirstOrDefault(a =>
                        a.PatientId == patientId &&
                        a.AppointmentDate.Date == appointmentDate.Date &&
                        a.AppointmentTime == appointmentTime);

                    if (patientConflict != null)
                    {
                        MessageBox.Show($"Scheduling conflict detected! Patient {existingPatient.Name} already has an appointment at this time.",
                            "Scheduling Conflict!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Healthcare Prac conflict; same prac same time
                    var practitionerConflict = db.Appointments.FirstOrDefault(a =>
                        a.Healthcarepractitioner == selectedPractitioner &&
                        a.AppointmentDate.Date == appointmentDate.Date &&
                        a.AppointmentTime == appointmentTime);

                    if (practitionerConflict != null)
                    {
                        MessageBox.Show($"Scheduling conflict detected! {selectedPractitioner} already has an appointment at this time.",
                            "Scheduling Conflict!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Create a new appointment
                var newAppointment = new AppointmentGS
                {
                    PatientId = patientId,
                    Fullname = existingPatient.Name,
                    DOB = existingPatient.DOB,
                    Prescription = txtprescription.Text.Trim(),
                    Medicalhistory = txtmedhis.Text.Trim(),
                    AppointmentDate = appointmentDate,
                    AppointmentTime = appointmentTime,
                    Healthcarepractitioner = selectedPractitioner,
                    DatemedicalhistorySaved = CurrentDate
                };

                try
                {
                    db.Appointments.Add(newAppointment);
                    db.SaveChanges();

                    MessageBox.Show("Appointment added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

           

        }
        private void RefreshDataGrid()
        {
            using (var db = new MainDatabaseContext())
            {
                var appointments = db.Appointments
                    .Select(a => new
                    {
                        a.PatientId,
                        a.Fullname,
                        DOB = a.DOB.Date.ToShortDateString(),//ToString("dd/MM/yyyy"),
                        a.Prescription,
                        a.Medicalhistory,
                        AppointmentDate = a.AppointmentDate.Date.ToString("dd/MM/yyyy"),
                        a.AppointmentTime,
                        a.Healthcarepractitioner,
                        DateMedicalHistorySaved = a.DatemedicalhistorySaved.ToString("dd/MM/yyyy")
                    })
                    .ToList();


                var appointmentList = db.Appointments.Include(a => a.Patient).ToList();
                datagridappointments.ItemsSource = appointmentList;
            }
        }

        private void btnupdateapptmnt(object sender, RoutedEventArgs e)
        {
            if (datagridappointments.SelectedItem is AppointmentGS selectedAppointment)
            {
                PopulateAppointmentFields(selectedAppointment);
                MessageBox.Show("Fields have been populated again, to save these changes selct the 'ADD' button again");
            }
            else
            {
                MessageBox.Show("Please select an appointment record from the grid to update");
            }
        }
        private void PopulateAppointmentFields(AppointmentGS selectedAppointment)
        {
            txtpatientid.Text = selectedAppointment.PatientId.ToString();
            txtfullname.Text = selectedAppointment.Fullname;
            txtdob.Text = selectedAppointment.DOB.ToString("yyyy-MM-dd");
            txtprescription.Text = selectedAppointment.Prescription;
            txtappointmentdate.Text = selectedAppointment.AppointmentDate.ToString("yyyy-MM-dd");
            txtappointmenttime.Text = Convert.ToString(selectedAppointment.AppointmentTime);
            txthealthcareprac.Text = selectedAppointment.Healthcarepractitioner;
            txtmedhis.Text = selectedAppointment.Medicalhistory;
            txtdatemedicalhistorysaved.Text = selectedAppointment.DatemedicalhistorySaved.ToString("yyyy-MM-dd");
        }

        private void btndeleteapptmnt(object sender, RoutedEventArgs e)
        {

            if (datagridappointments.SelectedItem is AppointmentGS selectedAppointment)
            {
                var confirmResult = MessageBox.Show("Please confirm if you would like to delete this record?", "Confirm Deletion", MessageBoxButton.YesNo);

                if (confirmResult == MessageBoxResult.Yes)
                {
                    using (var db = new MainDatabaseContext())
                    {
                        var appointmentToDelete = db.Appointments.SingleOrDefault(a => a.AppointmentId == selectedAppointment.AppointmentId);
                        if (appointmentToDelete != null)
                        {
                            db.Appointments.Remove(appointmentToDelete);
                            db.SaveChanges();

                            
                            RefreshDataGrid();

                            MessageBox.Show("Selected appointment has been deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Appointment has not been found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an appointment record to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            
        }
    }
}


