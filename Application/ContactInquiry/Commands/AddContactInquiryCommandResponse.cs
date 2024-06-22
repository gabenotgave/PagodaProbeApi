namespace Application.ContactInquiry.Commands;

public class AddContactInquiryCommandResponse
{
    public required bool Successful { get; set; }
    
    public string? ReasonForFailure { get; set; }
}