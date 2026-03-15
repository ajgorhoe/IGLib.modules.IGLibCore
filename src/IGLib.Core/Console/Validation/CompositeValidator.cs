using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGLib
{

    public sealed class CompositeValidator<T> // : IValidator<T>
    {
        private readonly IReadOnlyList<IValidator<T>> _validators;

    }

}
