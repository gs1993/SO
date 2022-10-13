using CSharpFunctionalExtensions;
using FluentValidation;
using Logic.Utils;
using Logic.Utils.Db;
using System;

namespace Api.Validation
{
    internal static class CustomValidators
    {
        internal static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoryMethod)
            where TValueObject : BaseEntity
        {
            return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsFailure)
                    context.AddFailure(result.Error.Message);
            });
        }
    }
}
