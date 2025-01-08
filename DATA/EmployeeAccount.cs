using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Record_System.DATA
{


    public class EmployeeAccount
    {
        [Key]
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Jobrole { get; set; }

        public bool IsEnabled {  get; set; }

    }

}

