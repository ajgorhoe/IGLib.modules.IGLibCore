
using System;
using System.Globalization;

namespace IGLib.Old;


/// <summary>Provides the default format provider (object of type <see cref="IFormatProvider"/>) 
/// for cases where the format provider is not explicitly specified; e.g., in methods where
/// <see cref="IFormatProvider"/> is a parameter, but is not provided by the caller (parameter
/// is set to null).</summary>
/// <remarks>See also documentation for IDefaultFormatProviderSelector.</remarks>
public class DefaultFormatProviderSelectorCurrentCulture : IDefaultFormatProviderSelector
{

    /// <inheritdoc/>
    public IFormatProvider DefaultFormatProvider { get; } = CultureInfo.CurrentCulture;

}
