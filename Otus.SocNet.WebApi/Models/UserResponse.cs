namespace Otus.SocNet.WebApi.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string First_Name { get; set; } = null!;
        public string Second_Name { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string Biography { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
