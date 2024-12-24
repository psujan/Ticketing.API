namespace Ticketing.API.Model.Dto.Category
{
    public class CategoryDto
    {
        // For Now this is same as Category Domain
        public new int Id { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
    }
}
