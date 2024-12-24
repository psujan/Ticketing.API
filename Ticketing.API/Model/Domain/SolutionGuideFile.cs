using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class SolutionGuideFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SolutionGuideId { get; set; }
        public SolutionGuide SolutionGuide { get; set; }

        public int FileId { get; set; }
        public File File { get; set; }
    }
}
