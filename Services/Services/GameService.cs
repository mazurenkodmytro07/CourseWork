using Models;
using Services;

public class GameService : IGameService
{
    private readonly InMemoryDbContext _context;

    public GameService(InMemoryDbContext context)
    {
        _context = context;
    }

    public void PlayGame(User player1, string player2Username, bool isAgainstAI, string gameMode)
    {
        Game game = GameFactory.CreateGame(gameMode);

        var board = InitializeBoard();
        bool playerTurn = true;
        User? player2 = null;

        if (!isAgainstAI && !string.IsNullOrWhiteSpace(player2Username))
        {
            player2 = _context.Users.FirstOrDefault(u => u.Username == player2Username) 
                    ?? new User { Username = player2Username, Rating = 0 };
        }

        while (true)
        {
            DrawBoard(board);

            if (playerTurn)
            {
                MakePlayerMove(player1, board, 'X');
            }
            else if (!isAgainstAI && player2 != null)
            {
                MakePlayerMove(player2, board, 'O');
            }
            else
            {
                MakeAIMove(board, 'O');
            }

            if (CheckWin(board, 'X') || CheckWin(board, 'O') || CheckDraw(board))
                break;

            playerTurn = !playerTurn;
        }

        DrawBoard(board);
        string result = EvaluateResult(board, player1, player2);

        game.Player1 = player1;
        game.Player2 = player2;
        game.Result = result;
        game.PlayedAt = DateTime.Now;

        if (isAgainstAI || player2 == null) 
        {
            game.UpdateRatings(player1, null, _context);
        }

        _context.Games.Add(game);
        _context.SaveChanges();
    }

    private static char[,] InitializeBoard()
    {
        return new char[3, 3]
        {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' }
        };
    }

    private static void DrawBoard(char[,] board)
    {
        Console.Clear();
        Console.WriteLine("   0   1   2");
        Console.WriteLine("  ┌───┬───┬───┐");
        for (int i = 0; i < 3; i++)
        {
            Console.Write(i + " │ ");
            for (int j = 0; j < 3; j++)
            {
                Console.Write(board[i, j]);
                if (j < 2) Console.Write(" │ ");
            }
            Console.WriteLine(" │");
            if (i < 2) Console.WriteLine("  ├───┼───┼───┤");
        }
        Console.WriteLine("  └───┴───┴───┘");
    }

    private static void MakePlayerMove(User player, char[,] board, char symbol)
    {
        Console.WriteLine($"{player.Username}'s turn ({symbol}). Enter row and column (e.g., '1 2'):");
        var move = GetValidMove(board);
        board[move.Item1, move.Item2] = symbol;
    }

    private static void MakeAIMove(char[,] board, char symbol)
    {
        var aiMove = GetBestMove(board, symbol);
        Console.WriteLine("AI's move:");
        board[aiMove.Item1, aiMove.Item2] = symbol;
    }

    private static (int, int) GetValidMove(char[,] board)
    {
        while (true)
        {
            var input = Console.ReadLine()?.Split();
            if (input == null || input.Length != 2 ||
                !int.TryParse(input[0], out int row) ||
                !int.TryParse(input[1], out int col) ||
                row < 0 || row > 2 || col < 0 || col > 2 ||
                board[row, col] != ' ')
            {
                Console.WriteLine("Invalid move. Enter row and column numbers (e.g., '1 2'):");
                continue;
            }
            return (row, col);
        }
    }

    private static (int, int) GetBestMove(char[,] board, char player)
    {
        Random random = new Random();
        if (random.Next(0, 100) < 30) 
        {
            var availableMoves = new List<(int, int)>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        availableMoves.Add((i, j));
                    }
                }
            }
            return availableMoves[random.Next(availableMoves.Count)];
        }

        return CalculateBestMove(board, player);
    }

    private static (int, int) CalculateBestMove(char[,] board, char player)
    {
        int bestValue = int.MinValue;
        (int, int) bestMove = (-1, -1);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == ' ')
                {
                    board[i, j] = player;
                    int moveValue = Minimax(board, 0, false, player);
                    board[i, j] = ' ';

                    if (moveValue > bestValue)
                    {
                        bestMove = (i, j);
                        bestValue = moveValue;
                    }
                }
            }
        }
        return bestMove;
    }

    private static int Minimax(char[,] board, int depth, bool isMaximizing, char player)
    {
        char opponent = player == 'X' ? 'O' : 'X';

        if (CheckWin(board, player)) return 10 - depth;
        if (CheckWin(board, opponent)) return depth - 10;
        if (CheckDraw(board)) return 0;
        
        if (depth >= 3) return 0;
        if (isMaximizing)
        {
            int best = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = player;
                        best = Math.Max(best, Minimax(board, depth + 1, false, player));
                        board[i, j] = ' ';
                    }
                }
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = opponent;
                        best = Math.Min(best, Minimax(board, depth + 1, true, player));
                        board[i, j] = ' ';
                    }
                }
            }
            return best;
        }
    }

    private static bool CheckWin(char[,] board, char player)
    {
        return Enumerable.Range(0, 3).Any(i => board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
               Enumerable.Range(0, 3).Any(i => board[0, i] == player && board[1, i] == player && board[2, i] == player) ||
               (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
               (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player);
    }

    private static bool CheckDraw(char[,] board)
    {
        return board.Cast<char>().All(c => c != ' ');
    }

    private static string EvaluateResult(char[,] board, User player1, User? player2)
    {
        if (CheckWin(board, 'X')) return player1.Username + " Wins";
        if (CheckWin(board, 'O')) return player2 == null ? "AI Wins" : player2.Username + " Wins";
        return "Draw";
    }

    public void ViewAllRatings()
    {
        var users = _context.Users.ToList();
        if (users.Any())
        {
            Console.WriteLine("\n--- User Ratings ---");
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Username}: {user.Rating} rating");
            }
        }
        else
        {
            Console.WriteLine("No users found.");
        }
    }

    public void ViewGameHistory()
    {
        var games = _context.Games
            .OrderByDescending(g => g.PlayedAt)
            .ToList();

        if (games.Any())
        {
            Console.WriteLine("\n--- Game History ---");
            foreach (var game in games)
            {
                var player1 = _context.Users.FirstOrDefault(u => u.Id == game.Player1Id)?.Username ?? "Unknown Player";
                string player2Name;

                if (game.Player2Id.HasValue)
                {
                    player2Name = _context.Users.FirstOrDefault(u => u.Id == game.Player2Id)?.Username ?? "Unknown Player";
                }
                else
                {
                    player2Name = game.Result.Contains("AI") ? "AI" : "Guest Player";
                }

                Console.WriteLine($"Played At: {game.PlayedAt:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Player 1: {player1}, Player 2: {player2Name}");
                Console.WriteLine($"Game Result: {game.Result}");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("No game history found.");
        }
    }
}
