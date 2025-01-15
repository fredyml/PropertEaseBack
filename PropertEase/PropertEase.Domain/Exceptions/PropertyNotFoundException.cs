namespace PropertEase.Domain.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(string message)
            : base(message)
        {
        }

        public PropertyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
