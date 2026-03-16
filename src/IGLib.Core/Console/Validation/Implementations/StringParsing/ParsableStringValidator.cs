using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    public class ParsableStringValidator<TTarget> : IValidator<string>
    {
        private readonly IStringParser<TTarget> _parser;

        public ParsableStringValidator(
            IFormatProvider? formatProvider = null,
            string? errorMessage = null)
        {
            _parser = StringParserProvider.GetParser<TTarget>(formatProvider);
            ErrorMessage = errorMessage ?? $"Value is not a valid {typeof(TTarget).Name}.";
        }

        public string ErrorMessage { get; }

        public virtual void Validate(string value, ValidationResults results)
        {
            // ToDo ArgumentNullException.ThrowIfNull(results);

            if (value == null)
            {
                results.AddError(ErrorMessage);
                return;
            }

            if (!_parser.TryParse(value, out _))
                results.AddError(ErrorMessage);
        }
    }
}
