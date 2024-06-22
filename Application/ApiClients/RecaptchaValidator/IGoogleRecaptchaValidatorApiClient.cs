namespace Application.ApiClients.RecaptchaValidator;

public interface IGoogleRecaptchaValidatorApiClient
{
    public bool IsValid(string gRecaptchaResponse);

    public string GetSiteKey();
}