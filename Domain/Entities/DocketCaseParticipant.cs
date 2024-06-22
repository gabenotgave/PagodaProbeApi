using System;

namespace Domain.Entities
{
    public class DocketCaseParticipant
    {
        public string ParticipantType { get; set; }
        public string ParticipantName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Address { get; set; }
    }
}
