using Models;
using Services;

namespace Core
{
    public class LoginAction : IMenuAction
    {
        private readonly UserService _userService;
        private readonly GameService _gameService;

        public LoginAction(UserService userService, GameService gameService)
        {
            _userService = userService;
            _gameService = gameService;
        }

        public string Description => "Login";

        public void Execute()
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine()!;

            Console.Write("Enter password: ");
            var password = Console.ReadLine()!;

            var user = _userService.Login(username, password);

            if (user == null)
            {
                Console.WriteLine("Invalid credentials.");
                return;
            }

            Console.WriteLine($"Welcome, {user.Username}! Your rating: {user.Rating}");
            new UserMenu(_gameService, user).Run();
        }
    }
}