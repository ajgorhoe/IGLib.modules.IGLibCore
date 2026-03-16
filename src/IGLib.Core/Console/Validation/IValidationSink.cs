using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    /// <summary>The interface implemented by the <see cref="ValidationResult"/> class. It contains a very
    /// reduced API from that class, by <see cref="IValidatableObject"/> to file a validation issue
    /// discovered during validation.</summary>
    public interface IValidationIssueSink
    {
        void AddIssue(ValidationIssue issue);
    }

}
