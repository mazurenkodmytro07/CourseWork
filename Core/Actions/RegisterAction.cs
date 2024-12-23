using Services;

namespace Core
{
    public class RegisterAction : IMenuAction
    {
        private readonly UserService _userService;

        public RegisterAction(UserService userService)
        {
            _userService = userService;
        }

        public string Description => "Register";

        public void Execute()
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine()!;

            Console.Write("Enter password: ");
            var password = Console.ReadLine()!;

            try
            {
                _userService.Register(username, password);
                Console.WriteLine("User registered successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}