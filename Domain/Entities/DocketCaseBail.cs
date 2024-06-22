using System;

namespace Domain.Entities
{
    public class DocketCaseBail
    {
        public string BailActionType { get; set; }
        public DateTime BailActionDate { get; set; }
        public string BailType { get; set; }
        public double Amount { get; set; }
    }
}
