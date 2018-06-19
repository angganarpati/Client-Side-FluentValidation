using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Internal;
using FluentValidation.Resources;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Reflection;

namespace ClientSideFluentValidation.PropertyValidators
{
    public class LessThanOrEqualPropertyValidator : ClientValidatorBase
    {
        public LessThanOrEqualPropertyValidator(PropertyRule rule, IPropertyValidator validator) : base(rule, validator)
        {
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            var validator = (GreaterThanValidator)Validator;
            var memberInfo = (validator).MemberToCompare;
            var compareValue = (validator).ValueToCompare;
            if (memberInfo != null)
            {
                var type = ((PropertyInfo)memberInfo).PropertyType;

                MergeAttribute(context.Attributes, "lessthanorequalto", GetErrorMessage(context, validator, memberInfo.Name.ToString()));
                MergeAttribute(context.Attributes, "field", memberInfo.Name);
                MergeAttribute(context.Attributes, "isnullable", (Nullable.GetUnderlyingType(type) == null).ToString());

                return;
            }
            else if (compareValue != null)
            {
                MergeAttribute(context.Attributes, "data-val", "true");
                MergeAttribute(context.Attributes, "data-val-range", GetErrorMessage(context, validator, validator.ValueToCompare.ToString()));
                MergeAttribute(context.Attributes, "data-val-range-max", compareValue.ToString());
                MergeAttribute(context.Attributes, "data-val-range-min", "0");
            }
        }

        private string GetErrorMessage<T>(ClientModelValidationContext context, T validator, string comparevalue)
          where T : AbstractComparisonValidator
        {
            var formatter = ValidatorOptions.MessageFormatterFactory()
                .AppendPropertyName(Rule.GetDisplayName())
                .AppendArgument("ComparisonValue", comparevalue);

            var messageNeedsSplitting = validator.ErrorMessageSource.ResourceType == typeof(LanguageManager);

            string message;

            try
            {
                message = validator.ErrorMessageSource.GetString(null);
            }
            catch (FluentValidationMessageFormatException)
            {
                message = ValidatorOptions.LanguageManager.GetStringForValidator<T>();
                messageNeedsSplitting = true;
            }

            if (messageNeedsSplitting && message.Contains(".") && message.Contains("{ComparisonValue}"))
            {
                // If we're using the default resources then the mesage for length errors will have two parts, eg:
                // '{PropertyName}' must be between {From} and {To}. You entered {Value}.
                // We can't include the "Value" part of the message because this information isn't available at the time the message is constructed.
                // Instead, we'll just strip this off by finding the index of the period that separates the two parts of the message.

                message = message.Substring(0, message.IndexOf(".") + 1);
            }
            message = formatter.BuildMessage(message);

            return message;
        }
    }
}