using Models;
using Services;

namespace Core
{
    public class PlayGameAction : IMenuAction
    {
        private readonly GameService _gameService;
        private readonly User _user;

        public PlayGameAction(GameService gameService, User user)
        {
            _gameService = gameService;
            _user = user;
        }

        public string Description => "Play Game";

        public void Execute()
        {
            Console.WriteLine("\nChoose a game mode:");
            Console.WriteLine("1. Standard");
            Console.WriteLine("2. SaveLoss");
            Console.Write("Enter option: ");
            var modeChoice = Console.ReadLine();

            string gameMode = modeChoice switch
            {
                "1" => "Standard",
                "2" => "SaveLoss",
                _ => throw new Exception("Invalid game mode.")
            };

            Console.WriteLine("\nChoose an opponent:");
            Console.WriteLine("1. Play with AI");
            Console.WriteLine("2. Play with a Friend");
            Console.Write("Enter option: ");
            var playChoice = Console.ReadLine();

            if (playChoice == "1")
            {
                _gameService.PlayGame(_user, string.Empty, true, gameMode);
            }
            else if (playChoice == "2")
            {
                Console.Write("Enter your friend's username: ");
                string friendUsername = Console.ReadLine()!;
                _gameService.PlayGame(_user, friendUsername, false, gameMode);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }
    }
}