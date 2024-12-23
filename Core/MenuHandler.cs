using Services;

namespace Core
{
    public class MenuHandler
    {
        private readonly List<IMenuAction> _actions;

        public MenuHandler()
        {
            var context = new InMemoryDbContext();
            var userService = new UserService(context);
            var gameService = new GameService(context);

            _actions = new List<IMenuAction>
            {
                new RegisterAction(userService),
                new LoginAction(userService, gameService),
                new ViewRatingsAction(gameService),
                new ExitAction()
            };
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                for (int i = 0; i < _actions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_actions[i].Description}");
                }
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _actions.Count)
                {
                    _actions[choice - 1].Execute();
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }
    }
}