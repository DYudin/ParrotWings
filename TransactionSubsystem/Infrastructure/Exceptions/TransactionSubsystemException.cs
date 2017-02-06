using System;

namespace TransactionSubsystem.Infrastructure.Exceptions
{
    public class TransactionSubsystemException : Exception
    {
        public TransactionSubsystemException(string message) : this(message, null)
        { }

        public TransactionSubsystemException(string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}
