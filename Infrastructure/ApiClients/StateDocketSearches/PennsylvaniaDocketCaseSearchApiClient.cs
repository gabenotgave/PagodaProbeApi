using System.Text;
using System.Text.RegularExpressions;
using Application.ApiClients.CountyParcelSearches;
using Application.ApiClients.StateDocketSearches;
using Domain.Entities;
using Newtonsoft.Json.Linq;

namespace Infrastructure.ApiClients.StateDocketSearches;

public class PennsylvaniaDocketCaseSearchApiClient : IPennsylvaniaDocketCaseSearchApiClient
{
    public async Task<List<DocketCase>> GetDocketCasesByPerson(Person person)
        {
            List<DocketCase> dcktCases = new List<DocketCase>();

            // Hit pacourts API based on person's object properties
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://services.pacourts.us");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "public/v1/cases/search");
                if (person.Birthdate == DateTime.MinValue)
                {
                    // Omit birthdate value (it wasn't provided)
                    request.Content = new StringContent($"{{\"participantFirstName\":\"{person.FirstName}\",\"participantLastName\":\"{person.LastName}\", \"startIndex\": \"0\", \"endIndex\": \"99\"}}", Encoding.UTF8, "application/json");
                }
                else
                {
                    // Otherwise include it
                    request.Content = new StringContent($"{{\"participantFirstName\":\"{person.FirstName}\",\"participantLastName\":\"{person.LastName}\", \"dateOfBirth\": \"{person.Birthdate.ToString("M-d-yyyy")}\", \"startIndex\": \"0\", \"endIndex\": \"99\"}}", Encoding.UTF8, "application/json");
                }
                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                JToken jCases = JObject.Parse(responseBody)["results"];

                foreach (JToken jCase in jCases)
                {
                    DocketCase dcktCase = new DocketCase
                    {
                        DocketNumber = jCase["docketNumber"].ToString(),
                        CourtType = jCase["courtOffice"]["courtOfficeType"].ToString(),
                        CaseCaption = jCase["shortCaption"].ToString(),
                        CaseStatus = jCase["statusName"].ToString(),
                        FilingDate = DateTime.Parse(jCase["filingDate"].ToString()),
                        PrimaryParticipants = jCase["shortCaption"].ToString().Split("v.").Select(x => x.Trim()).ToList(),
                        County = jCase["courtOffice"]["county"]["name"].ToString(),
                        CourtOffice = jCase["courtOffice"]["name"].ToString(),
                        CaseInfoHref = jCase["href"].ToString()
                    };
                    dcktCases.Add(dcktCase);
                }
            }

            // Weed out unrelated cases
            string name = $"{person.LastName} {person.FirstName}";
            List<DocketCase> docketCases = new List<DocketCase>();
            foreach (DocketCase docketCase in dcktCases)
            {
                // Automatically mark as confident if birthdate was provided (as only docket records with matching birthdates will be retrieved)
                if (person.Birthdate != DateTime.MinValue)
                {
                    docketCase.Confidence = Confidence.Confident;
                }
                if (!string.IsNullOrEmpty(person.MiddleInitial))
                {
                    if (docketCase.PrimaryParticipants.Any(p => Regex.IsMatch(p, $"^{person.LastName}, {person.FirstName}[a-zA-Z]*$", RegexOptions.IgnoreCase)))
                    {
                        docketCases.Add(docketCase);
                    }
                    else if (docketCase.PrimaryParticipants.Any(p => Regex.IsMatch(p, $"^{person.LastName}, {person.FirstName}[a-zA-Z]* {person.MiddleInitial}", RegexOptions.IgnoreCase)))
                    {
                        if (docketCase.County == person.County)
                        {
                            docketCase.Confidence = Confidence.Confident;
                        }
                        docketCases.Add(docketCase);
                    }
                }
                else
                {
                    if (docketCase.PrimaryParticipants.Any(p => Regex.IsMatch(p, $"^{person.LastName}, {person.FirstName}[a-zA-Z]*$", RegexOptions.IgnoreCase)))
                    {
                        if (docketCase.County == person.County)
                        {
                            docketCase.Confidence = Confidence.Confident;
                        }
                        docketCases.Add(docketCase);
                    }
                    else if (docketCase.PrimaryParticipants.Any(p => Regex.IsMatch(p, $"^{person.LastName}, {person.FirstName}", RegexOptions.IgnoreCase)))
                    {
                        docketCases.Add(docketCase);
                    }
                }
            }

            // Sort list so that confident cases fall first, then Berks cases, then most recent cases
            docketCases = docketCases
                .OrderBy(c => c.Confidence == Confidence.Confident ? 0 : 1)
                .ThenBy(c => c.County == person.County ? 0 : 1)
                .ThenByDescending(c => c.FilingDate)
                .ToList();

            return docketCases;
        }

        public async Task<DocketCase> GetDocketCaseByNumber(string docketNumber)
        {
            // Get docket case data via pacourts API
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://services.pacourts.us");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"public/v1/cases/{docketNumber}");
                HttpResponseMessage response = await client.SendAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jCase = JObject.Parse(responseBody);

                DocketCase docketCase = new DocketCase
                {
                    DocketNumber = jCase["docketNumber"].ToString(),
                    CourtType = jCase["courtOffice"]["courtOfficeType"].ToString(),
                    CaseCaption = jCase["shortCaption"].ToString(),
                    CaseStatus = jCase["statusName"].ToString(),
                    FilingDate = DateTime.Parse(jCase["filingDate"].ToString()),
                    PrimaryParticipants = jCase["shortCaption"].ToString().Split("v.").Select(x => x.Trim()).ToList(),
                    County = jCase["courtOffice"]["county"]["name"].ToString(),
                    CourtOffice = jCase["courtOffice"]["name"].ToString(),
                    CaseInfoHref = jCase["href"].ToString()
                };

                // Get other case information details
                if (jCase["claimAmount"].Type != JTokenType.Null)
                    docketCase.ClaimAmount = Double.Parse(jCase["claimAmount"].ToString());
                if (jCase["nameOfAssignedJudge"]["documentName"].Type != JTokenType.Null)
                    docketCase.AssignedJudge = jCase["nameOfAssignedJudge"]["documentName"].ToString();
                if (jCase["rootOriginatingOrganizationName"]["displayName"].Type != JTokenType.Null)
                    docketCase.ArrestingAgency = jCase["rootOriginatingOrganizationName"]["displayName"].ToString();
                if (jCase["currentProcessingStatus"].Type != JTokenType.Null)
                {
                    docketCase.CaseStatusInformation = new DocketCaseStatusInformation
                    {
                        StatusDate = DateTime.Parse(jCase["currentProcessingStatus"]["statusChangeDateTime"].ToString()),
                        ProcessingStatus = jCase["currentProcessingStatus"]["processingStatus"].ToString()
                    };
                }
                if (jCase["caseParticipants"].Count() > 0) {
                    docketCase.CaseParticipants = new List<DocketCaseParticipant>();
                    foreach (JToken jEntry in jCase["caseParticipants"])
                    {
                        DocketCaseParticipant docketCaseParticipant = new DocketCaseParticipant
                        {
                            ParticipantName = jEntry["participantName"]["displayName"].ToString(),
                            ParticipantType = jEntry["role"]["name"].ToString()
                        };
                        if (jEntry["caseMemberAddresses"].Count() > 0)
                        {
                            docketCaseParticipant.Address = jEntry["caseMemberAddresses"][0]["addressLines"][0].ToString();
                        }
                        if (jEntry["primaryDateOfBirth"].Type != JTokenType.Null)
                        {
                            docketCaseParticipant.Birthdate = DateTime.Parse(jEntry["primaryDateOfBirth"].ToString());
                        }
                        if (jEntry["gender"].Type != JTokenType.Null)
                        {
                            docketCaseParticipant.Gender = jEntry["gender"].ToString();
                        }
                        if (jEntry["race"].Type != JTokenType.Null)
                        {
                            docketCaseParticipant.Race = jEntry["race"].ToString();
                        }
                        docketCase.CaseParticipants.Add(docketCaseParticipant);
                    }
                }
                if (jCase["registryEntries"].Count() > 0)
                {
                    docketCase.CaseEntries = new List<DocketCaseEntry>();
                    foreach (JToken jEntry in jCase["registryEntries"])
                    {
                        DocketCaseEntry docketCaseEntry = new DocketCaseEntry
                        {
                            Entry = jEntry["registryEntryCode"].ToString(),
                            FiledDate = DateTime.Parse(jEntry["filedDateTime"].ToString())
                        };
                        if (jEntry["filers"].Count() > 0)
                        {
                            docketCaseEntry.Filer = jEntry["filers"][0]["participantName"]["displayName"].ToString();
                        }
                        docketCase.CaseEntries.Add(docketCaseEntry);
                    }
                }
                if (jCase["caseCalendarEvents"].Count() > 0)
                {
                    docketCase.CaseCalendarEvents = new List<DocketCaseCalendarEvent>();
                    foreach (JToken jEntry in jCase["caseCalendarEvents"])
                    {
                        DocketCaseCalendarEvent docketCaseCalendarEvent = new DocketCaseCalendarEvent
                        {
                            CaseCalendarEventType = jEntry["caseCalendarEventType"].ToString(),
                            ScheduleStatus = jEntry["scheduleStatus"].ToString(),
                            ScheduleStartDate = DateTime.Parse(jEntry["startDateTime"].ToString()),
                            JudgeName = jEntry["presidingAuthorityName"]["displayName"].ToString()
                        };
                        docketCase.CaseCalendarEvents.Add(docketCaseCalendarEvent);
                    }
                }
                if (jCase["caseBails"].Count() > 0)
                {
                    docketCase.CaseBails = new List<DocketCaseBail>();
                    foreach (JToken jEntry in jCase["caseBails"][0]["bailRecords"])
                    {
                        DocketCaseBail docketCaseBail = new DocketCaseBail
                        {
                            BailActionType = jEntry["actionTypeName"].ToString(),
                            BailType = jEntry["typeName"].ToString(),
                            BailActionDate = DateTime.Parse(jEntry["actionDate"].ToString()),
                            Amount = Double.Parse(jEntry["totalAmount"].ToString())
                        };
                        docketCase.CaseBails.Add(docketCaseBail);
                    }
                }
                if (jCase["offenses"].Count() > 0)
                {
                    docketCase.CaseCharges = new List<DocketCaseCharge>();
                    foreach (JToken jEntry in jCase["offenses"])
                    {
                        DocketCaseCharge docketCaseCharge = new DocketCaseCharge
                        {
                            Number = Int32.Parse(jEntry["sequenceNumber"].ToString()),
                            Grade = jEntry["grade"].ToString(),
                            Charge = jEntry["statuteName"].ToString(),
                            Description = jEntry["description"].ToString(),
                            OffenseDate = DateTime.Parse(jEntry["offenseDate"].ToString())
                        };
                        docketCase.CaseCharges.Add(docketCaseCharge);
                    }
                }
                if (jCase["assessmentGrandTotals"].Type != JTokenType.Null)
                {
                    docketCase.TotalAssessments = Double.Parse(jCase["assessmentGrandTotals"]["totalAssessments"].ToString());
                    //if (jCase["assessmentGrandTotals"]["assessmentCategoryTotals"].Count() > 0)
                    //{
                    //    docketCase.CaseAssessments = new List<DocketCaseAssessment>();
                    //    foreach (JToken jEntry in jCase["assessmentCategoryTotals"]["assessments"])
                    //    {
                    //        DocketCaseAssessment docketCaseAssessment = new DocketCaseAssessment
                    //        {
                    //            Type = jEntry[]
                    //        };
                    //        docketCase.CaseAssessments.Add(docketCaseAssessment);
                    //    }
                    //}
                }

                return docketCase;
            }
        }
}