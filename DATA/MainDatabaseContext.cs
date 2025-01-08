using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Record_System.DATA
{
    public class MainDatabaseContext: DbContext
    {
            public DbSet<Patient> Patients { get; set; }

            public DbSet<EmployeeAccount> EmployeeAccounts { get; set; }

            public DbSet<AppointmentGS> Appointments { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlite("Data Source=MainDB.db");
            }

            public MainDatabaseContext()
            {
                Database.EnsureCreated();
        }
    }
   
}
