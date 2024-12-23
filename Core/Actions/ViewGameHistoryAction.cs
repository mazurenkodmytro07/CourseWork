using Services;

namespace Core
{
    public class ViewGameHistoryAction : IMenuAction
    {
        private readonly GameService _gameService;

        public ViewGameHistoryAction(GameService gameService)
        {
            _gameService = gameService;
        }

        public string Description => "View Game History";

        public void Execute()
        {
            _gameService.ViewGameHistory();
        }
    }
}