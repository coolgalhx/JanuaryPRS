using Microsoft.VisualBasic.ApplicationServices;
using Patient_Record_System.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public static EmployeeAccount VerifiedUser;

        public LogIn()
        {
            InitializeComponent();

        }

        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            string username = txtusername.Text;
            string password = passbox.Password;
            string hashedPassword = HashingHelper.HashPassword(password);
            
            using (var db = new MainDatabaseContext())
            {
                var employee = db.EmployeeAccounts.FirstOrDefault(e => e.Username == username && e.Password == password);
                if (employee != null)
                {
                    if (!employee.IsEnabled)
                    {
                        MessageBox.Show("This account has been disabled");
                        return;
                    }

                    // global variable storage
                    VerifiedUser = employee;

                    MessageBox.Show($"Welcome, {employee.Jobrole}!");


                    MainMenu mm = new MainMenu(employee);
                    mm.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect Credentials");
                }
            }

        }
        private void HashExistingPasswords()
        {
            using (var db = new MainDatabaseContext())
            {
                foreach (var employee in db.EmployeeAccounts)
                {
                   
                    if (!IsPasswordHashed(employee.Password))
                    {
                        employee.Password = HashingHelper.HashPassword(employee.Password);
                    }
                }
                db.SaveChanges();
            }
        }

        
        private bool IsPasswordHashed(string password)
        {
            
            return password.Length == 64 && password.All(c => Uri.IsHexDigit(c));
        }
        private void btnforgotpassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please raise a ticket with our IT Department");

        }
    }
   

} 






        

   




