using System;

namespace Domain.Entities
{
    public class DocketCaseEntry
    {
        public DateTime FiledDate { get; set; }
        public string Entry { get; set; }
        public string Filer { get; set; }
    }
}
