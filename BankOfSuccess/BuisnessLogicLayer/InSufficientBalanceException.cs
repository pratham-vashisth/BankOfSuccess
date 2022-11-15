using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class InSufficientBalanceException : Exception
    {
        public InSufficientBalanceException()
        {
        }

        public InSufficientBalanceException(string? message) : base(message)
        {
        }

        public InSufficientBalanceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InSufficientBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}