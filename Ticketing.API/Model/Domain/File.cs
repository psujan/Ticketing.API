namespace Ticketing.API.Model.Domain
{
    public class File : Base<int>
    {
        public string Name { get; set; } // Display Name For File

        public string? OriginalName { get; set; }

        public string? MimeType { get; set; }

        public double? Size { get; set; }

        public string? Path { get; set; }

        public string? Model { get; set; }

        public int? ModelId { get; set; }

        // Navigation property to TicketFile
        public TicketFile? TicketFile { get; set; }

        public SolutionGuide? SolutionGuide { get; set; }
    }
}
