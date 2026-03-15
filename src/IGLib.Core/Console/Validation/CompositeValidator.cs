using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    public sealed class CompositeValidator<T> : IValidator<T>
    {
        private readonly IReadOnlyList<IValidator<T>> _validators;

        public CompositeValidator(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators?.ToList()
                ?? throw new ArgumentNullException(nameof(validators));
        }

        public void Validate(T value, ValidationResults results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }
            foreach (var validator in _validators)
            {
                validator.Validate(value, results);
            }
        }
    }

}
