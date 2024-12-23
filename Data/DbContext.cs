using System.Security.Cryptography;
using Models;

public class InMemoryDbContext
{
    public List<User> Users { get; set; }
    public List<Game> Games { get; set; }
    private Dictionary<int, int> userWinCounts; 

    public InMemoryDbContext()
    {
        Users = new List<User>
        {
            new User { Id = 1, Username = "player1", Password = "password1", Rating = 1200 },
            new User { Id = 2, Username = "player2", Password = "password1", Rating = 1100 },
            new User { Id = 3, Username = "player3", Password = "password1", Rating = 1000 },
            new User { Id = 4, Username = "player4", Password = "password1", Rating = 950 },
            new User { Id = 5, Username = "player5", Password = "password1", Rating = 1300 }
        };

        Games = new List<Game>();
        userWinCounts = new Dictionary<int, int>();
    }

    public int GetWinCount(User user)
    {
        if (userWinCounts.TryGetValue(user.Id, out int count))
        {
            return count;
        }
        return 0;
    }

    public void IncrementWinCount(User user)
    {
        if (userWinCounts.ContainsKey(user.Id))
        {
            userWinCounts[user.Id]++;
        }
        else
        {
            userWinCounts[user.Id] = 1;
        }
    }

    public void SaveChanges()
    {
        Console.WriteLine("Changes saved to in-memory database.");
    }
}