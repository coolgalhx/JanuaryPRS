using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;



namespace Patient_Record_System.DATA
{
   
        public class Patient
        {
            
            public string Name { get; set; }

            [Key]
             public int patientID { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public DateTime DOB { get; set; }
            public double Height { get; set; }
            public double Weight { get; set; }
            public string Address { get; set; }
            public string Number { get; set; }
            public string Hospitalnumber {  get; set; }
            
           public string NHSnumber {  get; set; }

          








        }

       


}



