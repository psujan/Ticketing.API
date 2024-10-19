using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.API.Model
{
    public class Base<T>
    {
        public T Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
