using Application.DocketCase.Queries;
using Application.DocketCaseSearch.Commands;
using Domain.DataModels;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PagodaProbeApi.Controllers;

public class DocketCaseController : BaseController
{
    [HttpGet]
    [Route("getcasebydocketnumber")]
    public async Task<DocketCase> GetDocketCaseByNumber(string docketNumber)
    {
        var docketCase = await Mediator.Send(new GetDocketCaseByNumberQuery()
        {
            DocketNumber = docketNumber
        });
        
        // Persist docket case search
        await Mediator.Send(new AddDocketCaseSearchCommand()
        {
            DocketCase = docketCase,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        });

        return docketCase;
    }
}