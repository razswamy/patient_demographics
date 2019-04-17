using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace patient.demography
{
    [Serializable()]
    [XmlRoot(ElementName = "patient")]
    public class patient
    {
        [XmlElement(ElementName = "patientid")]
        public int patientid { get; set; }

        [XmlElement(ElementName = "forenames")]
        public string forenames { get; set; }

        [XmlElement(ElementName = "surname")]
        public string surname { get; set; }

        [XmlElement(ElementName = "dateofbirth")]
        public DateTime dateofbirth { get; set; }

        [XmlElement(ElementName = "gender")]
        public int gender { get; set; }

        [XmlElement(ElementName = "gendertype", Type = typeof(gendertype))]
        public gendertype gendertype { get { return (gendertype)this.gender; } }

        [XmlElement(ElementName = "isdeleted")]
        public bool isdeleted { get; set; }

        [XmlArray(ElementName = "patientphonenumbers")]
        [XmlArrayItem("patientphonenumbers", typeof(patientphonenumber))]
        [XmlElement(ElementName = "patientphonenumber", Type = typeof(List<patientphonenumber>))]
        public List<patientphonenumber> patientphonenumbers { get; set; }
    }
}