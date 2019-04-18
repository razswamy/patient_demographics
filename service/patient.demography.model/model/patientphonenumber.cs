namespace patient.demography
{
    public class patientphonenumber
    {
        public int patientid { get; set; }

        public int patientphonenumberid { get; set; }

        public phonenumbertype phonenumbertypeenum { get { return (phonenumbertype)this.phonenumbertype; } }

        public int phonenumbertype { get; set; }

        public string phonenumber { get; set; }

        public bool isdeleted { get; set; }
    }
}