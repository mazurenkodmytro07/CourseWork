namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Rating { get; set; }
        public List<Game> Games { get; set; } = new();
    }
}
