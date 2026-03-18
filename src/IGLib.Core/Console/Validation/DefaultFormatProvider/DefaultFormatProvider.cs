using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib;


public class DefaultFormatProviderSelector
{

    /// <summary>Returns the global <see cref="IDefaultFormatProviderSelector"/> that is used to provide
    /// the <see cref="IFormatProvider"/> when one is not specified, and also the the
    /// <see cref="IDefaultFormatProviderSelector"/> is not specified.</summary>
    public static IDefaultFormatProviderSelector Global { get; private set; }
        = new DefaultFormatProviderSelectorInvariantCulture();

    /// <summary>Sets (changes) the global <see cref="IDefaultFormatProviderSelector"/>, which will be
    /// used by default to select the default <see cref="IFormatProvider"/> object when one is not
    /// specified, and the <see cref="IDefaultFormatProviderSelector"/> to obtain the default value
    /// is also not specified.</summary>
    /// <param name="selector">The <see cref="IDefaultFormatProviderSelector"/> that will be returned
    /// by the <see cref="DefaultFormatProviderSelector.Global"/> property after this call.
    /// The marameter may not be null (otherwise, this method throws an exception).</param>
    /// <exception cref="ArgumentNullException">When the <paramref name="selector"/> parameter is null.</exception>
    public static void SetGlobal(IDefaultFormatProviderSelector selector) 
    {
        if (selector == null)
        {
            throw new ArgumentNullException(nameof(selector), "The global selector of default format provider cannot be set to null.");
        }
        Global = selector;
    }

    /// <summary>Returns the appropriate <see cref="IFormatProvider"/> according to the specified (suggested)
    /// <paramref name="specifiedFormatProvider"/> (that can be null) and the specified <paramref name="defaultSelector"/>,
    /// which provides the default <see cref="IFormatProvider"/> when the <paramref name="specifiedFormatProvider"/>
    /// is null If both parameters are null then the default <see cref="IFormatProvider"/> is provided by the
    /// <see cref="DefaultFormatProviderSelector.Global"/> property.</summary>
    /// <param name="specifiedFormatProvider">The specified <see cref="IFormatProvider"/>. If not null then this
    /// object is returned immediately, otherwise the <paramref name="defaultSelector"/> parameter is used to 
    /// provide the object that is returned.</param>
    /// <param name="defaultSelector">Provides the default <see cref="IFormatProvider"/> (via the 
    /// <see cref="IDefaultFormatProviderSelector.DefaultFormatProvider"/> property) in case that the <paramref name="specifiedFormatProvider"/>
    /// is null. In that case, if this parameter is also null, the <see cref="DefaultFormatProviderSelector.Global"/> provides the
    /// default object to be returned.</param>
    /// <returns>The <see cref="IFormatProvider"/> returned according to method parameters:
    /// <para>* If <paramref name="specifiedFormatProvider"/> is not null, this object is returned.</para>
    /// <para>* Otherwise, if <paramref name="defaultSelector"/> is not null then the object obtained by its 
    /// <see cref="IDefaultFormatProviderSelector.DefaultFormatProvider"/> property is returned.</para>
    /// <para>Otherwise, the format provider obtained by the <see cref="DefaultFormatProviderSelector.Global"/>'s 
    /// <see cref="IDefaultFormatProviderSelector.DefaultFormatProvider"/> property is returned.</para></returns>
    public static IFormatProvider GetFormatProvider(IFormatProvider? specifiedFormatProvider,
        IDefaultFormatProviderSelector? defaultSelector)
    {
        return (specifiedFormatProvider ?? (defaultSelector ?? Global).DefaultFormatProvider);
    }

}
