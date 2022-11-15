using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class AccountAlreadyExistException : Exception
    {
        public AccountAlreadyExistException()
        {
        }

        public AccountAlreadyExistException(string? message) : base(message)
        {
        }

        public AccountAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}