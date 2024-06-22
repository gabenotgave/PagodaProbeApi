using System;

namespace Domain.Entities
{
    public class DocketCaseCharge
    {
        public int Number { get; set; }
        public string Charge { get; set; }
        public string Grade { get; set; }
        public string Description { get; set; }
        public DateTime OffenseDate { get; set; }
    }
}
