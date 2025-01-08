using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Record_System.DATA
{
    public class AppointmentGS
    {
        [Key]
        public int AppointmentId { get; set; }
        public string Fullname { get; set; }
        public DateTime DOB { get; set; }
        public string Prescription { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Healthcarepractitioner { get; set; }
        public string Medicalhistory { get; set; }
        public DateTime DatemedicalhistorySaved { get; set; }



        [ForeignKey("Patient")]

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        





    }


}
