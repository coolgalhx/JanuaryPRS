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
using System.Runtime.InteropServices;
using System.Speech.Synthesis;



namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        static SpeechSynthesizer synth = new SpeechSynthesizer();
        public static EmployeeAccount VerifiedUser;
        
        public MainMenu(EmployeeAccount LoggedInUser)
        {
            InitializeComponent();
            VerifiedUser = LoggedInUser;


        }

        private void addpatient(object sender, RoutedEventArgs e)
        {
            if (MainMenu.VerifiedUser != null && MainMenu.VerifiedUser.Jobrole != null &&
                MainMenu.VerifiedUser.Jobrole.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied! Doctors are not allowed to access this page.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }
            MainWindow mw=new MainWindow();
            mw.Show();
        }

        private void btnsearchpatient(object sender, RoutedEventArgs e)
        {
            SearchPatient sp=new SearchPatient();
            sp.Show();
        }

       

        private void btnadministration(object sender, RoutedEventArgs e)
        {
            if (MainMenu.VerifiedUser != null && MainMenu.VerifiedUser.Jobrole != null &&
               (MainMenu.VerifiedUser.Jobrole.Equals("Doctor", StringComparison.OrdinalIgnoreCase) ||
                MainMenu.VerifiedUser.Jobrole.Equals("Nurse", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Access Denied: Doctors and Nurses are not allowed access to this page.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            AdminPage ap = new AdminPage();
            ap.Show();
        }

        private void btnallaccounts(object sender, RoutedEventArgs e)
        {
            if (MainMenu.VerifiedUser != null && MainMenu.VerifiedUser.Jobrole != null &&
                (MainMenu.VerifiedUser.Jobrole.Equals("Doctor", StringComparison.OrdinalIgnoreCase) ||
                 MainMenu.VerifiedUser.Jobrole.Equals("Nurse", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Access Denied: Doctors and Nurses are not allowed access to this page.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            AllAccounts aa= new AllAccounts();
            aa.Show();
        }

        private void btnbookapptm(object sender, RoutedEventArgs e)
        {
            if (MainMenu.VerifiedUser != null && MainMenu.VerifiedUser.Jobrole != null &&
                MainMenu.VerifiedUser.Jobrole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied: Admins are not allowed access to this page.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Appointment ap = new Appointment();
            ap.Show();

        }

       

        private void btnlistallapptm(object sender, RoutedEventArgs e)
        {
            ListAllAppointments laa=new ListAllAppointments();
            laa.Show();
        }

        private void btntexttospeech(object sender, RoutedEventArgs e)
        {
            string textToRead = "";

            foreach (UIElement element in ButtonsContainer.Children)
            {
                if (element is Button button)
                {
                   
                    if (button.Content != null)
                    {
                        textToRead += button.Content.ToString() + ". ";
                    }
                }
                else if (element is Label label)
                {
                   
                    if (label.Content != null)
                    {
                        textToRead += label.Content.ToString() + ". ";
                    }
                }
            }


            SpeakText(textToRead);
        }
        private void SpeakText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            try
            {
                using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
                {
                    synthesizer.Speak(text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has occurred whilst trying to read aloud: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnlogout_Click(object sender, RoutedEventArgs e)
        {
            
            var result = MessageBox.Show("Would You Like To LogOut ?", "Logout Confirmation",
                                         MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                
                LogIn loginPage = new LogIn();
                loginPage.Show();

               
                this.Close();
            }
        }

        private void btnmedicalhistorynotes(object sender, RoutedEventArgs e)
        {
            if (MainMenu.VerifiedUser != null && MainMenu.VerifiedUser.Jobrole != null &&
               MainMenu.VerifiedUser.Jobrole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access Denied: Admins are not allowed access to this page.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            MedicalHistoryNotes mhn=new MedicalHistoryNotes();
            mhn.Show();
        }
    }
}
