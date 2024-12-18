
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class SolutionGuide :Base<int>
    {
        public string Title { get; set; }

        public string Details { get; set; }

        public string Status { get; set; }

       
        public string UserId { get; set; } // Added By

        //Navigational Properties

        public ICollection<File>? Files { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
