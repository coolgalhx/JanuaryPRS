using Microsoft.VisualBasic.Logging;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Patient_Record_System
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ProgramStart(string[] args)
        {
            //string username;
            //string password;


            //var login = new LogIn();

            //if (username == txtusername.Text && password == txtpassword.Text)
            //{
            //    var Admin = new AdminPage();
            //       Admin.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Incorrect details");
            //}


            
                var Admin = new AdminPage();
                Admin.Show();
           

            //username==txtusername.Text&&password==txtpassword.Text




        }
    }

}
