using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class File : Base<int>
    {
        public string Name { get; set; } // Display Name For File

        public string? OriginalName { get; set; }

        public string? MimeType { get; set; }

        public double? Size { get; set; }

        public string? Path { get; set; }
    }
}
