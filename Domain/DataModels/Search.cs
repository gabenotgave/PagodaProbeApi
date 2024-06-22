namespace Domain.DataModels
{
    public class Search
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public string? MiddleInitial { get; set; }

        public required string LastName { get; set; }

        public DateTime? Birthdate { get; set; }

        public required string County { get; set; }

        public int ParcelCount { get; set; }

        public int DocketCaseCount { get; set; }

        public DateTime DateSearched { get; set; }

        public required string IpAddress { get; set; }
    }
}
