using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    public class ParsableStringValidator<TTarget> : IValidator<string>
    {
        private static readonly IStringParser<TTarget> Parser = StringParserProvider.GetParser<TTarget>();

        public ParsableStringValidator(string? errorMessage = null)
        {
            ErrorMessage = errorMessage ?? $"Value is not a valid {typeof(TTarget).Name}.";
        }

        public string ErrorMessage { get; }

        public void Validate(string value, ValidationResults results)
        {
            // ArgumentNullException.ThrowIfNull(results);
            if (results == null)
            { throw new ArgumentNullException(nameof(results), "Object to store validation results may not be null."); }

            if (value is null || !Parser.TryParse(value, out _))
                results.AddError(ErrorMessage);
        }
    }

}
