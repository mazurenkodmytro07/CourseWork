using Models;

namespace Services
{
    public interface IUserService
    {
        void Register(string username, string password);
        User? Login(string username, string password);
        int GetRating(string username);
    }
}
