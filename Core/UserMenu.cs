using Models;
using Services;

namespace Core
{
    public class UserMenu
    {
        private readonly List<IMenuAction> _actions;

        public UserMenu(GameService gameService, User user)
        {
            _actions = new List<IMenuAction>
            {
                new PlayGameAction(gameService, user),
                new ViewGameHistoryAction(gameService),
                new ViewRatingsAction(gameService),
                new LogoutAction()
            };
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nUser Menu:");
                for (int i = 0; i < _actions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_actions[i].Description}");
                }
                Console.Write("Choose an option: ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _actions.Count)
                {
                    _actions[choice - 1].Execute();
                    if (_actions[choice - 1] is LogoutAction)
                        break;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }
    }
}