using System;
using System.Collections.Generic;
namespace patient.demography
{
    public class patient
    {
        public int patientid { get; set; }
        public string forenames { get; set; }
        public string surname { get; set; }
        public DateTime dateofbirth { get; set; }
        public int gender { get; set; }
        public gendertype gendertype { get { return (gendertype)this.gender; } }
        public bool isdeleted { get; set; }
        public IEnumerable<patientphonenumber> patientphonenumbers { get; set; }

    }
}
