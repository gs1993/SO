using Dawn;

namespace Entities
{
    public class ConnectionString
    {
        public string Value { get; set; }

        public ConnectionString(string value)
        {
            Guard.Argument(value, nameof(value)).NotWhiteSpace();

            Value = value;
        }
    }
}
