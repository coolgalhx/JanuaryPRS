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
    /// Interaction logic for ListAllAppointments.xaml
    /// </summary>
    public partial class ListAllAppointments : Window
    {
        public ObservableCollection<AppointmentGS> Appointments { get; set; } =  new ObservableCollection<AppointmentGS>();
        public ListAllAppointments()
        {
            InitializeComponent();
            ListAllAppointmentsonDG();

        }
        private void ListAllAppointmentsonDG()
        {
            using (var db = new MainDatabaseContext())
            {
                var appointmentsFromDb = db.Appointments.ToList();
                foreach (var appointment in appointmentsFromDb)
                {
                    Appointments.Add(appointment);
                }
            }

            datagridlistappointments.ItemsSource = null; 
            datagridlistappointments.ItemsSource = Appointments;



        }
    }
}
