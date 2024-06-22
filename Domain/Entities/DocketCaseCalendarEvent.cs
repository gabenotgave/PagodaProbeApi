using System;

namespace Domain.Entities
{
    public class DocketCaseCalendarEvent
    {
        public string CaseCalendarEventType { get; set; }
        public DateTime ScheduleStartDate { get; set; }
        public string JudgeName { get; set; }
        public string ScheduleStatus { get; set; }
    }
}
