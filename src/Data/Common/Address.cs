using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common
{
    public class Address
    {
        public string HouseNumber { get; set; }
        public string FloorNumber { get; set; }
        public string BuildingName { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string Purok { get; set; }
        public string SubdivisionName { get; set; }
        public string Barangay { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
