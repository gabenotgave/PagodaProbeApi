namespace Domain.Entities
{
    public class Person
    {
        public required string FirstName { get; init; }
        public string? MiddleInitial { get; init; }
        public required string LastName { get; init; }
        public DateTime Birthdate { get; init; }
        public required string County { get; init; }
        public List<Parcel> Parcels { get; set; }
        public List<DocketCase> DocketCases { get; set; }
        
        public required List<Area> SupportedAreas { get; init; } 
    }
}
