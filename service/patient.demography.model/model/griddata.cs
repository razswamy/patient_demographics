namespace patient.demography
{
    public class griddata
    {
        public griddata() => searchkey = "";

        public int limit { get; set; }

        public int offset
        {
            get; set;
        }

        public string otherparameters { get; set; }
        public object rows { get; set; }
        public bool search { get { return (searchkey ?? "").Trim().Length > 0; } }
        public string searchkey { get; set; }
        public string sort { get; set; }
        public string sortorder { get; set; }
        public int total { get; set; }
    }
}