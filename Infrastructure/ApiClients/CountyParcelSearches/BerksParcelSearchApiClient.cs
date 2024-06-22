using System.Net;
using System.Text.RegularExpressions;
using Application.ApiClients.CountyParcelSearches;
using Application.Person.Queries;
using Domain.Entities;
using HtmlAgilityPack;
using Application.Extensions;

namespace Infrastructure.ApiClients.CountyParcelSearches;

public class BerksParcelSearchApiClient : IBerksParcelSearchApiClient
{
    public async Task<List<Parcel>> GetParcelsByPersonName(GetPersonByNameQuery person)
    {
            // Generate name
            string name = $"{person.LastName} {person.FirstName}";

            List<Parcel> prcls = new List<Parcel>();

            // Scrape Berks County Parcel Search website
            Uri baseAddress = new Uri("https://gis.co.berks.pa.us/parcelsearch/Results.aspx");
            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get };
            CookieContainer cookieContainer = new CookieContainer();
            using (HttpClientHandler handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    // Add headers
                    //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
                    // Add cookie with name (this is what Berks County Parcel Search wants (for
                    // whatever reason)
                    cookieContainer.Add(baseAddress, new Cookie("Field", "NAME"));
                    cookieContainer.Add(baseAddress, new Cookie("Value", name));
                    using (HttpResponseMessage response = await client.SendAsync(requestMessage))
                    {
                        using (HttpContent content = response.Content)
                        {
                            string htmlResult = await content.ReadAsStringAsync();
                            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                            document.LoadHtml(htmlResult);
                            IEnumerable<HtmlNode> htmlRecords = document.DocumentNode.Descendants("tr").Skip(9); // Parcel records start at 9th row
                            foreach (HtmlNode htmlRecord in htmlRecords)
                            {
                                List<string> record = htmlRecord
                                    .Descendants("td")
                                    .Select(n => System.Web.HttpUtility.HtmlDecode(n.InnerText))
                                    .Take(5) // Only first 5 are relevant
                                    .ToList();
                                Parcel parc = new Parcel
                                {
                                    OwnerName = record[0],
                                    StreetAddress = record[1].Trim(),
                                    Municipality = record[2].Trim(),
                                    PropertyId = record[3].Trim(),
                                    MapPin = record[4].Trim(),
                                    ReportPdf = $"https://gis.co.berks.pa.us/parcelsearch/Details.aspx?PropID={record[3]}"
                                };
                                prcls.Add(parc);
                            }
                        }
                    }
                }
            }

            // Weed out unrelated parcels
            int parcelCount = prcls.Count();
            List<Parcel> parcels = new List<Parcel>();
            if (!string.IsNullOrEmpty(person.MiddleInitial))
            {
                foreach (Parcel parcel in prcls)
                {
                    // Check if parcel record has different middle initial (meaning parcel owner is definitively not the target)
                    if (Regex.IsMatch(parcel.OwnerName, $"{person.FirstName}[^\\s]* [^{person.MiddleInitial}][^\\s]* ", RegexOptions.IgnoreCase))
                    {
                        continue; // Skip iteration (do nothing)
                    }
                    parcel.Confidence = Confidence.Confident;
                    parcels.Add(parcel);
                }
            }
            else
            {
                foreach (Parcel parcel in prcls)
                {
                    // Mark parcel as high confidence if its owner has no middle initial (as the provided person has no middle initial either)
                    if (Regex.IsMatch(parcel.OwnerName, $"{person.FirstName}[^\\s]*((\\s$)|\\/|\\s&)", RegexOptions.IgnoreCase))
                    {
                        parcel.Confidence = Confidence.Confident;
                    }
                    parcels.Add(parcel);
                }
            }

            // Indicate high confidence if only one parcel record remains
            if (parcels.Count() == 1)
            {
                parcels[0].Confidence = Confidence.Confident;
            }

            // Sort list so that confident parcels fall first, then the most alphabetically first
            parcels = parcels
                .OrderBy(p => p.Confidence == Confidence.Confident ? 0 : 1)
                .ThenBy(p => p.OwnerName)
                .ToList();

            return parcels;
        }
}