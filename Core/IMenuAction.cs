namespace Core
{
    public interface IMenuAction
    {
        void Execute();
        string Description { get; }
    }
}