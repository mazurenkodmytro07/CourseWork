using Models;
public static class GameFactory
{
    public static Game CreateGame(string gameType)
    {
        return gameType.ToLower() switch
        {
            "standard" => new StandardGame(),
            "safeloss" => new SaveLossGame(),
            _ => throw new ArgumentException($"Unknown game type: {gameType}")
        };
    }
}