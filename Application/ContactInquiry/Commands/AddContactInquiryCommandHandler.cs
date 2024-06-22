using Application.ApiClients.RecaptchaValidator;
using Application.Data;
using MediatR;

namespace Application.ContactInquiry.Commands;

public class AddContactInquiryCommandHandler : IRequestHandler<AddContactInquiryCommand, AddContactInquiryCommandResponse>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly IGoogleRecaptchaValidatorApiClient _googleRecaptchaValidatorApiClient;
    
    public AddContactInquiryCommandHandler(IApplicationDbContext applicationDbContext,
        IGoogleRecaptchaValidatorApiClient googleRecaptchaValidatorApiClient)
    {
        _applicationDbContext = applicationDbContext;
        _googleRecaptchaValidatorApiClient = googleRecaptchaValidatorApiClient;
    }

    public async Task<AddContactInquiryCommandResponse> Handle(AddContactInquiryCommand request,
        CancellationToken cancellationToken)
    {
        // Validate Google Recaptcha token
        if (!_googleRecaptchaValidatorApiClient.IsValid(request.RecaptchaToken))
        {
            return new AddContactInquiryCommandResponse()
            {
                Successful = false,
                ReasonForFailure = "Google Recaptcha failed"
            };
        }
        
        _applicationDbContext.ContactInquiries.Add(new Domain.DataModels.ContactInquiry()
        {
            Name = request.Name,
            Email = request.Email,
            Comment = request.Comment,
            DateSubmitted = DateTime.Now,
            IpAddress = request.IpAddress
        });
        await _applicationDbContext.SaveChangesAsync();

        return new AddContactInquiryCommandResponse()
        {
            Successful = true
        };
    }
}