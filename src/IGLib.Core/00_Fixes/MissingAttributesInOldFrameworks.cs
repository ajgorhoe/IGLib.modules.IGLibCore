
// In .NET Standard, .NET Framework, and earlier versions of .NET (prior to 5.0),
// the definition of attribute RequiresUnreferencedCode is missing. Code block below
// solves this:

#if !NET5_0_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
    internal sealed class RequiresUnreferencedCodeAttribute : Attribute
    {
        public RequiresUnreferencedCodeAttribute(string message) => Message = message;
        public string Message { get; }
        public string? Url { get; set; }
    }
}
#endif
