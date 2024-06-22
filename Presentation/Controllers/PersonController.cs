using System.ComponentModel.DataAnnotations;
using Application.Person.Queries;
using Application.Search.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PagodaProbeApi.Controllers;

public class PersonController : BaseController
{
    [HttpGet]
    [Route("getpersonbyname")]
    public async Task<Person> GetPersonByName(string firstName, string lastName, string state, string county, string? middleInitial = null, DateTime? birthDate = null)
    {
        if (firstName.Trim().Length < 2 || lastName.Trim().Length < 2)
        {
            throw new ValidationException("First name and last name must contain at least two characters each");
        }
        
        var person = await Mediator.Send(new GetPersonByNameQuery()
        {
            FirstName = firstName,
            MiddleInitial = middleInitial,
            LastName = lastName,
            BirthDate = birthDate,
            State = state,
            County = county
        });
        
        // Persist search for person
        await Mediator.Send(new AddSearchCommand()
        {
            Person = person,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        });
        
        return person;
    }
}