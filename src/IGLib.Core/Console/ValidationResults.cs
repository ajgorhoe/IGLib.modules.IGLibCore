using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib
{
    
    /// <summary>Stores results of validation of an object provided externally (via user input, input/output,
    /// via remote system, etc.).</summary>
    /// <typeparam name="ValidatedType">Type of the object for which validation results are provided.</typeparam>
    public class ValidationResults<ValidatedType>
    {

        public bool IsValid { get; protected set; }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

    }

    public class StringValidationResults: ValidationResults<string>
    {  }

}
