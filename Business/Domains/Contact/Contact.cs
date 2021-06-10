using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class Contact
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
        public string Comments { get; set; }
    }

    public enum ContactType
    {
        Telephone,
        Email
    }

}
