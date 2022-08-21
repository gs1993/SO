using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Logic.Utils
{
    public sealed class Error : ValueObject
    {
        public string Message { get; }

        internal Error(string message)
        {
            Message = message;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Message;
        }
    }
}
