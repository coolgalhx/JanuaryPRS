using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.NativeInterop;
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
    /// Interaction logic for AllAccounts.xaml
    /// </summary>
    public partial class AllAccounts : Window
    {
        public ObservableCollection<EmployeeAccount> Accounts { get; set; }
        public AllAccounts()
        {
            InitializeComponent();
            LoadAccounts();
        }
        private void LoadAccounts()
        {

            //using block makes sure the database is closed after query
            using (var db = new MainDatabaseContext())
            {
                var accountlist = db.EmployeeAccounts?.ToList();

                if (accountlist != null)
                {  

                    Accounts = new ObservableCollection<EmployeeAccount>(accountlist);
                }
                else
                {   
                    //if no data datagrid shows as empty
                    Accounts = new ObservableCollection<EmployeeAccount>();
                }
                datagridallaccounts.ItemsSource = Accounts;

            }
            
              
        }
    }
}
