namespace Otus.SocNet.WebApi.Models
{
    public class PostCreateRequest
    {
        public int AuthorId { get; set; }
        public string Content { get; set; } = null!;
    }
}
