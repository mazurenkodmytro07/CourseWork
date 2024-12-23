namespace Core
{
    public class LogoutAction : IMenuAction
    {
        public string Description => "Logout";

        public void Execute()
        {
            Console.WriteLine("Logging out...");
            // Логіка виходу з меню користувача. Ніяких додаткових дій тут не потрібно.
        }
    }
}