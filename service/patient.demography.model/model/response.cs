using System.Collections.Generic;

namespace patient.demography
{
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

        public object data { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ResponseMessage" /> is error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if error; otherwise, <c>false</c>.
        /// </value>
        ///

        public bool error { get { return (errormessage ?? new List<string>()).Count > 0; } }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        ///
        public List<string> errormessage { get; set; }
    }
}