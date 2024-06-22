using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.ContactInquiry.Commands;

public class AddContactInquiryCommand : IRequest<AddContactInquiryCommandResponse>
{
    public required string Name { get; set; }
    
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public required string Email { get; set; }

    public required string Comment { get; set; }

    public required string RecaptchaToken { get; set; }
    
    public string? IpAddress { get; set; }
}