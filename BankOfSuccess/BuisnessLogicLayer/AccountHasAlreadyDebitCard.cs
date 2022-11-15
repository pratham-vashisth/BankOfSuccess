using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class AccountHasAlreadyDebitCard : Exception
    {
        public AccountHasAlreadyDebitCard()
        {
        }

        public AccountHasAlreadyDebitCard(string? message) : base(message)
        {
        }

        public AccountHasAlreadyDebitCard(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccountHasAlreadyDebitCard(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}