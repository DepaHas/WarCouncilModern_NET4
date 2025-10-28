using System;

namespace WarCouncilModern.Utilities.Interfaces
{
    public interface IModLogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(string message, Exception ex);
    }
}