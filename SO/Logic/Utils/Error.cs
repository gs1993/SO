using CSharpFunctionalExtensions;
using System;
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

        public static implicit operator Result(Error error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            return Result.Failure(error.Message);
        }
    }
}
