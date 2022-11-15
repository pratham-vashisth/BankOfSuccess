using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class OrderAlreadyPlaced : Exception
    {
        public OrderAlreadyPlaced()
        {
        }

        public OrderAlreadyPlaced(string? message) : base(message)
        {
        }

        public OrderAlreadyPlaced(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderAlreadyPlaced(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}