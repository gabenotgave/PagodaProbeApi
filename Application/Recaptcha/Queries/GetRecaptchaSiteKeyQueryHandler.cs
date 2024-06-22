using Application.ApiClients.RecaptchaValidator;
using MediatR;

namespace Application.Recaptcha.Queries;

public class GetRecaptchaSiteKeyQueryHandler : IRequestHandler<GetRecaptchaSiteKeyQuery, string>
{
    private readonly IGoogleRecaptchaValidatorApiClient _googleRecaptchaValidatorApiClient;
    
    public GetRecaptchaSiteKeyQueryHandler(IGoogleRecaptchaValidatorApiClient googleRecaptchaValidatorApiClient)
    {
        _googleRecaptchaValidatorApiClient = googleRecaptchaValidatorApiClient;
    }

    public async Task<string> Handle(GetRecaptchaSiteKeyQuery request, CancellationToken cancellationToken)
    {
        var siteKey = _googleRecaptchaValidatorApiClient.GetSiteKey();
        
        return await Task.FromResult(siteKey);
    }
}