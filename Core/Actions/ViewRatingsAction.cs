using Services;

namespace Core
{
    public class ViewRatingsAction : IMenuAction
    {
        private readonly GameService _gameService;

        public ViewRatingsAction(GameService gameService)
        {
            _gameService = gameService;
        }

        public string Description => "View All Ratings";

        public void Execute()
        {
            _gameService.ViewAllRatings();
        }
    }
}