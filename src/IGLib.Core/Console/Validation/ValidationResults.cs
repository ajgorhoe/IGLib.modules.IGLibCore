using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    /// <summary>Stores results of validation of a value of an object, usually provided externally (via user input, input/output,
    /// via remote system, etc.).</summary>
    public class ValidationResults
    {
        private readonly List<ValidationIssue> _issues = new();

        public bool IsValid => !_issues.Any(issue => issue.Severity == ValidationSeverity.Error);

        public bool HasWarnings => _issues.Any(issue => issue.Severity == ValidationSeverity.Warning);

        public int Count => _issues.Count;

        public IReadOnlyList<ValidationIssue> Issues => new ReadOnlyCollection<ValidationIssue>(_issues);

        public IEnumerable<ValidationIssue> Errors =>
            _issues.Where(issue => issue.Severity == ValidationSeverity.Error);

        public IEnumerable<ValidationIssue> Warnings =>
            _issues.Where(issue => issue.Severity == ValidationSeverity.Warning);

        public virtual void AddIssue(ValidationIssue issue)
        {
            if (issue == null)
            {
                throw new ArgumentNullException(nameof(issue), "The validation issue to be added may not be null.");
            }
            _issues.Add(issue);
        }

        public virtual void AddError(string message, Exception? exception = null) =>
            AddIssue(new ValidationIssue(ValidationSeverity.Error, message, exception));

        public virtual void AddWarning(string message, Exception? exception = null) =>
            AddIssue(new ValidationIssue(ValidationSeverity.Warning, message, exception));

        public virtual void Merge(ValidationResults other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other), "Validation results to be merged into current results may not be null.");
            }
            _issues.AddRange(other.Issues);
        }

        public virtual void Clear() => _issues.Clear();

        public virtual string? GetMessagesString(ValidationSeverity severity, string separator)
        {
            if (separator is null)
                throw new ArgumentNullException(nameof(separator));

            var messages = _issues
                .Where(issue => issue.Severity == severity)
                .Select(issue => issue.Message)
                .ToArray();

            return messages.Length == 0 ? null : string.Join(separator, messages);
        }

        public virtual string? GetErrorsString(string separator) =>
            GetMessagesString(ValidationSeverity.Error, separator);

        public virtual string? GetErrorsString() =>
            GetErrorsString(Environment.NewLine);

        public virtual string? GetWarningsString(string separator) =>
            GetMessagesString(ValidationSeverity.Warning, separator);

        public virtual string? GetWarningsString() =>
            GetWarningsString(Environment.NewLine);

        public override string ToString()
        {
            if (_issues.Count == 0)
                return "Validation passed.";

            return string.Join(Environment.NewLine, _issues.Select(i => i.ToString()));
        }
    }

    /// <summary>Stores results of validation of an object provided externally (via user input, input/output,
    /// via remote system, etc.).</summary>
    /// <typeparam name="ValidatedType">Type of the object for which validation results are provided.</typeparam>
    /// <remarks>This class is not thread safe. It is not excepted that a single validation would be handled by multiple threads.</remarks>
    public class ValidationResults<T> : ValidationResults
    {
    }
}
