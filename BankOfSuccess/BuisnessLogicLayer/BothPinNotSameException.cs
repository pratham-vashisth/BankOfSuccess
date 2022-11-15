using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class BothPinNotSameException : Exception
    {
        public BothPinNotSameException()
        {
        }

        public BothPinNotSameException(string? message) : base(message)
        {
        }

        public BothPinNotSameException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BothPinNotSameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}