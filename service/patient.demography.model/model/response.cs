using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace patient.demography
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("response")]
    public class response
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class.
        /// </summary>
        public response()
        {
            errormessage = new List<string>();
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        ///

        [XmlElement("data")]
        [XmlType(]
        public object data { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ResponseMessage" /> is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if error; otherwise, <c>false</c>.
        /// </value>
        ///

        [XmlElement("error")]
        public bool error { get { return (errormessage ?? new List<string>()).Count > 0; } }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        ///
        [XmlArray("errormessage")]
        [XmlArrayItem("errormessage", typeof(string))]
        public List<string> errormessage { get; set; }
    }
}