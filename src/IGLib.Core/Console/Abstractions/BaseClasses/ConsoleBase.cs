
namespace IGLib.ConsoleAbstractions;

public abstract class ConsoleBase : IConsole
{
    public abstract string? ReadLine();

    // Core output primitives
    public abstract void Write(string? value);
    public abstract void WriteLine(string? value = null);

    // Optional: helpers you want all implementations to inherit
    protected static string NormalizeWrite(string? value) => value ?? string.Empty;
}
