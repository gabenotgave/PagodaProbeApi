using MediatR;

namespace Application.Recaptcha.Queries;

public class GetRecaptchaSiteKeyQuery : IRequest<string>
{
}