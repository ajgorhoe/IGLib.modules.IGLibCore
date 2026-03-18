
using System;

namespace IGLib
{

    /// <summary>Provides the default format provider (object of type <see cref="IFormatProvider"/>) 
    /// for cases where the format provider is not explicitly specified; e.g., in methods where
    /// <see cref="IFormatProvider"/> is a parameter, but is not provided by the caller (parameter
    /// is set to null).</summary>
    /// <remarks><para>Usage example:</para>
    /// <remarks>In constructors or methods that have <see cref="IFormatProvider"/> as parameter, also 
    /// provide <see cref="IDefaultFormatProviderSelector?"/> as parameter, and default it to null.
    /// Then, within the constructor, check if fhe format provider parameter is null, and if yes,
    /// set it to what the default format provider selector's 
    /// <see cref="DefaulltFormatProvider"/> returns. In applications, you can arrange for specific 
    /// <see cref="IDefaultFormatProviderSelector"/> to be injected in all such constructors.</remarks>
    /// </remarks>
    public interface IDefaultFormatProviderSelector
    {

        /// <summary>Returns the <see cref="IFormatProvider"/> object that should be used as default,
        /// when the <see cref="IFormatProvider"/> is not proveded for a specific purpose.</summary>
        IFormatProvider DefaultFormatProvider { get; }

    }


}
