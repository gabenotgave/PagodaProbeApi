using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DataModels
{
    public class ContactInquiry
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required string Comment { get; set; }

        public DateTime DateSubmitted { get; set; }

        public required string IpAddress { get; set; }

        public bool IsArchived { get; set; }

        [NotMapped]
        public string RecaptchaToken { get; set; }
    }
}
