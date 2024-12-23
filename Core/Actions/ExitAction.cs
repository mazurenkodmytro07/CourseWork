namespace Core
{
    public class ExitAction : IMenuAction
    {
        public string Description => "Exit";

        public void Execute()
        {
            Console.WriteLine("Goodbye!");
            Environment.Exit(0);
        }
    }
}