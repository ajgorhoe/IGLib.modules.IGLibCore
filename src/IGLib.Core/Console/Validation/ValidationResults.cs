using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IGLib
{
    
    /// <summary>Stores results of validation of a value of an object, usually provided externally (via user input, input/output,
    /// via remote system, etc.).</summary>
    public class ValidationResults
    {

        public virtual bool IsValid
        {
            get
            {
                return ErrorMessages.Count == 0;
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>Contains validation error messages.</summary>
        protected List<string> ErrorMessages { get; } = new();

        protected List<string> WarningMessages { get; } = new();

        /// <summary>Returns an aggregate string containing recorded validation error messages.
        /// null is returned if there are no messages.</summary>
        /// <param name="separator">A seprator string inserted between individual error messages; can be null.</param>
        public virtual string? GetErrorsString(string separator) =>
            ErrorMessages.Count == 0 ? null : string.Join(separator, ErrorMessages);

        /// <summary>Returns an aggregate string containing recorded validation error messages, with individual
        /// messages separated by new lines. null is returned if there are no messages.</summary>
        public virtual string? GetErrorsString() => GetErrorsString(Environment.NewLine);

        /// <summary>Returns an aggregate string containing recorded validation warning messages.
        /// null is returned if there are no messages.</summary>
        /// <param name="separator">A seprator string inserted between individual warning messages; can be null.</param>
        public virtual string? GetWarningsString(string separator) =>
            WarningMessages.Count == 0 ? null : string.Join(separator, WarningMessages);


        /// <summary>Returns an aggregate string containing recorded validation warning messages, with individual
        /// messages separated by new lines. null is returned if there are no messages.</summary>
        public virtual string? GetWarningsString() => GetWarningsString(Environment.NewLine);

        public void AddError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                if (errorMessage == null)
                {
                    throw new ArgumentNullException(nameof(errorMessage), "Validation error message may not be null.");
                }
                throw new ArgumentException("Validation error message may not consist purely of whitespace.", nameof(errorMessage));
            }
            ErrorMessages.Add(errorMessage);
        }

        public void AddWarning(string warningMessage)
        {
            if (string.IsNullOrWhiteSpace(warningMessage))
            {
                if (warningMessage == null)
                {
                    throw new ArgumentNullException(nameof(warningMessage), "Validation warning message may not be null.");
                }
                throw new ArgumentException("Validation warning message may not consist purely of whitespace.", nameof(warningMessage));
            }
            WarningMessages.Add(warningMessage);
        }


    }

    /// <summary>Stores results of validation of an object provided externally (via user input, input/output,
    /// via remote system, etc.).</summary>
    /// <typeparam name="ValidatedType">Type of the object for which validation results are provided.</typeparam>
    /// <remarks>This class is not thread safe. It is not excepted that a single validation would be handled by multiple threads.</remarks>
    public class ValidationResults<ValidatedType> : ValidationResults
    {  }

}
