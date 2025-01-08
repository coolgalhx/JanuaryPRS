using Patient_Record_System.DATA;
using System.Windows;
using static Patient_Record_System.DATA.MainDatabaseContext;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private bool AreFieldsValid()
        {
            
            return !string.IsNullOrWhiteSpace(txtemployeeid.Text) &&
                   !string.IsNullOrWhiteSpace(txtfullname.Text) &&
                   !string.IsNullOrWhiteSpace(txtusername.Text) &&
                   !string.IsNullOrWhiteSpace(txtpassword.Text) &&
                   !string.IsNullOrWhiteSpace(txtjobrole.Text);
        }


        private bool IsDataValid()
        {
            if (!int.TryParse(txtemployeeid.Text, out _))
            {
                MessageBox.Show("Employee ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (string.IsNullOrWhiteSpace(txtfullname.Text) ||
                string.IsNullOrWhiteSpace(txtusername.Text) ||
                string.IsNullOrWhiteSpace(txtpassword.Text) ||
                string.IsNullOrWhiteSpace(txtjobrole.Text))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (!txtusername.Text.EndsWith("@gmail.com"))
            {
                MessageBox.Show("Username must end with '@gmail.com'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            if (txtpassword.Text.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters long.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string[] validJobroles = { "Doctor", "Nurse", "Admin" };
            //OrdinalIgnoreCase-case-insensitive validation
            if (!validJobroles.Contains(txtjobrole.Text, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Job role must be 'Doctor', 'Nurse', or 'Admin'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            using (var db = new MainDatabaseContext())
            {
                string usernameCheck = txtusername.Text.ToLower();
                bool usernameTaken = db.EmployeeAccounts
                 .Any(u => u.Username.ToLower() == usernameCheck);
                if (usernameTaken)
                {
                    MessageBox.Show("Username already exixts. Please enter a different one.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }



                return true;



            }


            

        }
        private void btnaddaccount(object sender, RoutedEventArgs e)
        {
            if (!IsDataValid())
            {
                return;
            }


            //creates new instance of employee accounts model cls
            var newEmployee = new EmployeeAccount
            {
                EmployeeID = Convert.ToInt16(txtemployeeid.Text),
                Name = txtfullname.Text,
                Username = txtusername.Text,
                Password = txtpassword.Text,
                Jobrole = txtjobrole.Text,
                IsEnabled = true,
            };
            using (var db = new MainDatabaseContext())
            {
                db.EmployeeAccounts.Add(newEmployee);
                db.Database.EnsureCreated();
                db.SaveChanges();
            }
            MessageBox.Show("Account created");
            //ClearFields();



        }
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            txtfullname.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            txtjobrole.Clear();
        }
       
        private void btndisableaccount(object sender, RoutedEventArgs e)
            {
                if (!IsDataValid())
                {
                    return;
                }


                int employeeid = Convert.ToInt16(txtemployeeid.Text);

                if (LogIn.VerifiedUser != null && LogIn.VerifiedUser.Jobrole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    if (LogIn.VerifiedUser.EmployeeID == employeeid)
                    {
                        MessageBox.Show("As admin, you cannot disable your own account.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                using (var db = new MainDatabaseContext())
                {
                    var employee = db.EmployeeAccounts.SingleOrDefault(emp => emp.EmployeeID == employeeid);

                    if (employee != null)
                    {
                        employee.IsEnabled = false;
                        db.SaveChanges();
                        MessageBox.Show("This employee account has been disabled.");
                    }
                    else
                    {
                        MessageBox.Show("Employee not found. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }



            }

        private void btnenableaccount(object sender, RoutedEventArgs e)
        {
            if (!IsDataValid())
            {
                return;
            }


            int employeeid = Convert.ToInt16(txtemployeeid.Text);

            using (var db = new MainDatabaseContext())
            {
                var employee = db.EmployeeAccounts.SingleOrDefault(emp => emp.EmployeeID == employeeid);
                if (employee != null)
                {
                    employee.IsEnabled = true;
                    db.SaveChanges();
                    MessageBox.Show("This employee account has been enabled");
                }
            }
        }



        private void btnchangepassword(object sender, RoutedEventArgs e)
        {
            int employeeid = Convert.ToInt16(txtemployeeid.Text);
            string newPassword = txtpassword.Text;

            using (var db = new MainDatabaseContext())
            {
                var employee = db.EmployeeAccounts.SingleOrDefault(emp => emp.EmployeeID == employeeid);
                if (employee != null)
                {
                    employee.Password = newPassword;
                    db.SaveChanges();
                    MessageBox.Show("Password has now been changed");
                }
                else
                {
                    MessageBox.Show("Employee not found.Please try again.");
                }

            }
        }
    }
}

