using System.Net;
using Application.ApiClients.RecaptchaValidator;
using Newtonsoft.Json.Linq;

namespace Infrastructure.ApiClients.RecaptchaValidation;

public class GoogleRecaptchaValidatorApiClient : IGoogleRecaptchaValidatorApiClient
{
    public bool IsValid(string gRecaptchaResponse)
    {
        HttpClient httpClient = new HttpClient();

        string secretKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Recaptcha")["SecretKey"];
        HttpResponseMessage res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={gRecaptchaResponse}").Result;

        if (res.StatusCode != HttpStatusCode.OK)
        {
            return false;
        }
        string JSONres = res.Content.ReadAsStringAsync().Result;
        dynamic JSONdata = JObject.Parse(JSONres);

        if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
        {
            return false;
        }

        return true;
    }

    public string GetSiteKey()
    {
        return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Recaptcha")["SiteKey"];
    }
}