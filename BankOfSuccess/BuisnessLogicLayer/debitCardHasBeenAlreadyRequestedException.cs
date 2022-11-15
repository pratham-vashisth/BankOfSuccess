using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class DebitCardHasBeenAlreadyRequestedException : Exception
    {
        public DebitCardHasBeenAlreadyRequestedException()
        {
        }

        public DebitCardHasBeenAlreadyRequestedException(string? message) : base(message)
        {
        }

        public DebitCardHasBeenAlreadyRequestedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DebitCardHasBeenAlreadyRequestedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}