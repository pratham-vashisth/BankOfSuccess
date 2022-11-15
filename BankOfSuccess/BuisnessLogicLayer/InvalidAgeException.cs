using System.Runtime.Serialization;

namespace BankOfSuccess.BuisnessLogicLayer
{
    [Serializable]
    internal class InvalidAgeException : Exception
    {
        public InvalidAgeException()
        {
        }

        public InvalidAgeException(string message) : base(message)
        {
        }

        public InvalidAgeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidAgeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}