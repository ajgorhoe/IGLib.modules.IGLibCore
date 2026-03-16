using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    /// <summary>Stores results of validation of a value of a single object, usually provided externally 
    /// (throuh user input via console or GUI, from input devices, from remote system, etc.).</summary>
    /// <remarks>This class is not thread safe. It is not excepted that a single validation would be 
    /// handled by multiple threads.
    /// <para>The class is designed to be used by validators (interface <see cref="IValidator{T}"/>)
    /// to store reults of their validation.</para>
    /// <para>Class can be reused for successive validations. Cal the <see cref="Clear"/> method before
    /// each reuse.</para></remarks>
    public class ValidationResults : IValidationIssueSink
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

        public virtual void Reset() => _issues.Clear();

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

    /// <summary>Stores results of validation of an object.
    /// <para>This class inherits from the non-generic <see cref="ValidationResult"/> and adds the 
    /// <see cref="ValidatedValue"/> property to optionally store the value whose validation results
    /// are stored. See documentation of this class for more information on usage and behavior.</para></summary>
    /// <typeparam name="ValidatedType">Type of the object for which validation results are stored.</typeparam>
    /// <remarks></remarks>
    public class ValidationResults<T> : ValidationResults
    {

        /// <summary>Default constructor. It does not set the <see cref="ValidatedValue"/>, and the value
        /// can be set later by calling <see cref="Reset(T?)"/>.</summary>
        public ValidationResults()
        { }

        /// <summary>Constructor used also to set (store) the value to be validated.</summary>
        /// <param name="validatedValue">Value to be validated, stored to <see cref="ValidatedValue"/>.</param>
        public ValidationResults(T? validatedValue)
        {
            ValidatedValue = validatedValue;
        }

        public T? ValidatedValue { get; private set; }

        public override void Reset()
        {
            Reset(default);

        }

        public virtual void Reset(T? validatedValue)
        {
            base.Reset();
            ValidatedValue = validatedValue;
        }
    }

}
