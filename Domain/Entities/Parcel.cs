using System;

namespace Domain.Entities
{
    public class Parcel
    {
        public string OwnerName { get; set; }
        public string StreetAddress { get; set; }
        public string Municipality { get; set; }
        public string ZipCode { get; set; }
        public string SchoolDistrict { get; set; }
        public string PropertyId { get; set; }
        public string MapPin { get; set; }
        public string DeedId { get; set; }
        public DateTime DeedDate { get; set; }
        public int DeedAmount { get; set; }
        public string ReportPdf { get; set; }
        public Confidence Confidence { get; set; } = Confidence.NotConfident; // Not confident is default
    }
}
