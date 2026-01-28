using System;
using System.ComponentModel;
using System.Threading;


string GetModifierString(ConsoleModifiers modifiers, bool appendSeparator = true, string separator = "+")
{
    List<string> modifierStrings = new();
    if ((modifiers & ConsoleModifiers.Control) != 0)
    {
        modifierStrings.Add("Ctrl");
    }
    if ((modifiers & ConsoleModifiers.Alt) != 0)
    {
        modifierStrings.Add("Alt");
    }
    if ((modifiers & ConsoleModifiers.Shift) != 0)
    {
        modifierStrings.Add("Shift");
    }
    if (modifierStrings.Count == 0)
    { appendSeparator = false; }
    return string.Join("+", modifierStrings.ToArray()) + (appendSeparator ? separator : ""); 
}

string GetModifierString(ConsoleKeyInfo keyInfo, bool appendSeparator = true, string separator = "+")
{
    return GetModifierString(keyInfo.Modifiers, appendSeparator, separator);
}

ConsoleKeyInfo cki = default;

while(cki.Key != ConsoleKey.X || (cki.Modifiers & ConsoleModifiers.Control) == 0) 
{
    Console.WriteLine("\nPress a key to display; press the 'Ctrl+x' key to quit.");

    // Your code could perform some useful task in the following loop. However,
    // for the sake of this example we'll merely pause for a quarter second.

    while (!Console.KeyAvailable)
        Thread.Sleep(250); // Loop until input is entered.

    cki = Console.ReadKey(true);
    Console.WriteLine($"\nYou pressed the '{cki.Key}' key.");
    Console.WriteLine($"      You pressed key:   {GetModifierString(cki)}{cki.Key}");
    Console.WriteLine($"      You pressed:       {GetModifierString(cki)}{cki.KeyChar}");

}

class Sample
{
    public static void Main()
    {

    }
}
/*
This example produces results similar to the following:

Press a key to display; press the 'x' key to quit.
You pressed the 'H' key.

Press a key to display; press the 'x' key to quit.
You pressed the 'E' key.

Press a key to display; press the 'x' key to quit.
You pressed the 'PageUp' key.

Press a key to display; press the 'x' key to quit.
You pressed the 'DownArrow' key.

Press a key to display; press the 'x' key to quit.
You pressed the 'X' key.
*/