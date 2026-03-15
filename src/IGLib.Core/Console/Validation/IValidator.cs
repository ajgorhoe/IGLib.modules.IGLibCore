using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib
{

    /// <summary>Represent an object that performs validation of the value of the specified type (<typeparamref name="T"/>) 
    /// and stores validation results in a <see cref="ValidationResults"/> objects.</summary>
    /// <typeparam name="T">Type of values that the currentt validator validates.</typeparam>
    public interface IValidator<in T>
    {
        void Validate(T value, ValidationResults results);
    }

}
