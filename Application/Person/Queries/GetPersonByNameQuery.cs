using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using MediatR;

namespace Application.Person.Queries;

public class GetPersonByNameQuery : IRequest<Domain.Entities.Person>
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    public required string State { get; set; }
    
    public required string County { get; set; }
    
    public string? MiddleInitial { get; set; }
}