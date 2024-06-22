using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DataModels
{
    public class DesktopAppRelease
    {
        public int Id { get; set; }

        public required string WindowsDownloadLink { get; set; }

        public required string MacDownloadLink { get; set; }

        public required string LinuxDownloadLink { get; set; }

        public required string Version { get; set; }

        public int Downloads { get; set; }

        public string? Comment { get; set; }

        public required DateTime DateReleased { get; set; }

        public required string ReleasedById { get; set; }

        [NotMapped]
        public required IFormFile WindowsFile { get; set; }

        [NotMapped]
        public required IFormFile LinuxFile { get; set; }

        [NotMapped]
        public required IFormFile MacFile { get; set; }
    }
}
