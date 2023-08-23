using System.ComponentModel.DataAnnotations;

namespace urlgoatbackend.Dto
{
    public class CreateShortUrlDto
    {
        [Required]
        [Url]
        public string LongUrl { get; set; }
    }
}