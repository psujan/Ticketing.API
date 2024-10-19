using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model.Domain
{
    public class Category:Base<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
    }
}
