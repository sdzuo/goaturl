namespace urlgoatbackend.Models
{
    public class CreateShortUrl
    {
        public string LongUrl { get; set; } = string.Empty;
        public string shortKey { get; set; } = string.Empty;
        public int id { get; set; }
    }
}
