using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib
{

    /// <summary>Contains information on a specific issue found during validation of a certain value.</summary>
    public sealed class ValidationIssue
    {
        public ValidationIssue(ValidationSeverity severity, string message, Exception? exception = null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                if (message is null)
                    throw new ArgumentNullException(nameof(message));
                throw new ArgumentException("Validation message must not be empty or whitespace.", nameof(message));
            }

            Severity = severity;
            Message = message;
            Exception = exception;
        }

        public ValidationSeverity Severity { get; }

        public string Message { get; }

        public Exception? Exception { get; }

        public override string ToString() => $"{Severity}: {Message}";
    }

}
