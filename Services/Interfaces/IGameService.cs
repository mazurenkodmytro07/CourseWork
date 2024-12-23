using Models;

namespace Services
{
    public interface IGameService
    {
        void PlayGame(User player1, string player2Username, bool isAgainstAI, string gameMode);
        void ViewAllRatings();
        void ViewGameHistory();
    }
}