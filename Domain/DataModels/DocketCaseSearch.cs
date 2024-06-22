namespace Domain.DataModels
{
    public class DocketCaseSearch
    {
        public int Id { get; set; }

        public required string DocketNumber { get; set; }

        public DateTime DateSearched { get; set; }

        public required string IpAddress { get; set; }
    }
}
