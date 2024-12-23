using Core;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the Tic-Tac-Toe Game!");
        var menuHandler = new MenuHandler();
        menuHandler.Run();
    }
}