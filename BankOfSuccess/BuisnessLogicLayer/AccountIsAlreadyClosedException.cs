using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class AccountIsAlreadyClosedException : Exception
    {
        public AccountIsAlreadyClosedException()
        {
        }

        public AccountIsAlreadyClosedException(string? message) : base(message)
        {
        }

        public AccountIsAlreadyClosedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountIsAlreadyClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}