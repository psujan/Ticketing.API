using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class SolutionGuideFile : File
    {
        public int SolutionGuideId { get; set; }
        public SolutionGuide SolutionGuide { get; set; }
    }
}
