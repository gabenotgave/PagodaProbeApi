namespace Domain.Entities;

public class Area
{
    public required string State { get; set; }
    
    public required List<string> Counties { get; set; }
}