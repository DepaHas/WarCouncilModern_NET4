namespace WarCouncilModern.Utilities.Interfaces
{
    public interface IModLogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, System.Exception? ex);
        void Debug(string message);
    }
}