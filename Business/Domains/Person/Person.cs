using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PersonGender Gender { get; set; }
        public BirthData Birth { get; set; }
        public List<Contact> Contacts { get; private set; }

        public class BirthData
        {
            public DateTime? Date { get; set; }
            public string Country { get; set; }
        }

        public enum PersonGender
        {
            Male,
            Female
        }
    }
}
