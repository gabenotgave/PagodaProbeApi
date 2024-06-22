using Application.Recaptcha.Queries;
using Microsoft.AspNetCore.Mvc;

namespace PagodaProbeApi.Controllers;

public class RecaptchaController : BaseController
{
    [HttpGet]
    [Route("getracaptchasitekey")]
    public async Task<string> GetRecaptchaSiteKey()
    {
        var siteKey = await Mediator.Send(new GetRecaptchaSiteKeyQuery());

        return siteKey;
    }
}