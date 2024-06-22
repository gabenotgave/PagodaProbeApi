using Application.ContactInquiry.Commands;
using Microsoft.AspNetCore.Mvc;

namespace PagodaProbeApi.Controllers;

public class ContactController : BaseController
{
    [HttpPost]
    [Route("submitcontactinquiry")]
    public async Task<AddContactInquiryCommandResponse> SubmitContactInquiry(AddContactInquiryCommand addContactInquiryCommand)
    {
        addContactInquiryCommand.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
        var response = await Mediator.Send(addContactInquiryCommand);

        return response;
    }
}