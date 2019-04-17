using System;
using System.Xml.Serialization;

namespace patient.demography
{
    [Serializable()]
    [XmlRoot(ElementName = "patientphonenumber")]

    public class patientphonenumber
    {

        [XmlElement(ElementName = "patientid")]
        public int patientid { get; set; }


        [XmlElement(ElementName = "patientphonenumberid")]
        public int patientphonenumberid { get; set; }


        [XmlElement(ElementName = "phonenumbertypeenum")]
        public phonenumbertype phonenumbertypeenum { get { return (phonenumbertype)this.phonenumbertype; } }


        [XmlElement(ElementName = "phonenumbertype")]
        public int phonenumbertype { get; set; }


        [XmlElement(ElementName = "phonenumber")]
        public string phonenumber { get; set; }


        [XmlElement(ElementName = "isdeleted")]
        public bool isdeleted { get; set; }
    }
}
