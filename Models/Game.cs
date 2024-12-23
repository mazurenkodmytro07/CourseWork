namespace Models
{
    public abstract class Game
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public string Result { get; set; } = string.Empty; 
        public DateTime PlayedAt { get; set; }
        public User Player1 { get; set; } = null!;
        public User? Player2 { get; set; }

        public abstract void UpdateRatings(User player1, User? player2, InMemoryDbContext context);
    }

    public class StandardGame : Game
{
    public override void UpdateRatings(User player1, User? player2, InMemoryDbContext context)
    {
        if (Result.Contains("AI Wins"))
        {
            player1.Rating -= 5; 
        }
        else if (Result.Contains(player1.Username))
        {
            player1.Rating += 10;

            int wins = context.GetWinCount(player1);
            if (wins >= 3)
            {
                player1.Rating += 5; 
            }

            context.IncrementWinCount(player1);
        }
    }
}

public class SaveLossGame : Game
{
    public override void UpdateRatings(User player1, User? player2, InMemoryDbContext context)
    {
        if (Result.Contains("AI Wins"))
        {
            player1.Rating -= 2; 
        }
        else if (Result.Contains(player1.Username))
        {
            player1.Rating += 5;

            int wins = context.GetWinCount(player1);
            if (wins >= 3)
            {
                player1.Rating += 5; 
            }

            context.IncrementWinCount(player1);
        }
    }
}
}