using Models;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly InMemoryDbContext _context;

        public UserService(InMemoryDbContext context)
        {
            _context = context;
        }

        public void Register(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
                throw new Exception("Username already exists.");

            if (password.Length < 8)
                throw new Exception("Password must be at least 8 characters long.");

            var user = new User
            {
                Id = _context.Users.Count + 1,
                Username = username,
                Password = password,  
                Rating = 1000
            };

            _context.Users.Add(user);
        }

        public User? Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user == null || user.Password != password)
            {
                return null; 
            }

            return user; 
        }

        public int GetRating(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) throw new Exception("User not found.");
            return user.Rating;
        }
    }
}
