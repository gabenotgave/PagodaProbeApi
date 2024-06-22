using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class DocketCase
    {
        public string DocketNumber { get; set; }
        public string CourtType { get; set; }
        public string CaseCaption { get; set; }
        public string CaseStatus { get; set; }
        public DateTime FilingDate { get; set; }
        public List<string> PrimaryParticipants { get; set; }
        public string County { get; set; }
        public string CourtOffice { get; set; }
        public string CaseInfoHref { get; set; }
        public Confidence Confidence { get; set; } = Confidence.NotConfident; // Not confident is default
        
        // Other details
        public double ClaimAmount { get; set; }
        public string AssignedJudge { get; set; }
        public string ArrestingAgency { get; set; }
        public double TotalAssessments { get; set; }
        public DocketCaseStatusInformation CaseStatusInformation { get; set; }
        public List<DocketCaseParticipant> CaseParticipants { get; set; }
        public List<DocketCaseEntry> CaseEntries { get; set; }
        public List<DocketCaseCalendarEvent> CaseCalendarEvents { get; set; }
        public List<DocketCaseBail> CaseBails { get; set; }
        public List<DocketCaseCharge> CaseCharges { get; set; }
        public List<DocketCaseAssessment> CaseAssessments { get; set; }
    }
}
