namespace Ticketing.API.Model.Dto
{
    public class FileResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }    

        public string OriginalName {  get; set; }

        public string Path {  get; set; }

        public double? Size { get; set; }   
    }
}
