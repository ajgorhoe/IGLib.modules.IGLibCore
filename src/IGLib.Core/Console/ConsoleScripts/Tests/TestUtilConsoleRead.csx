
// This script contains some snippets to manually test some console utilities.
//   * Open console in the directory to output direcory where binaries are generated.
//   * Run dotnet script.
//   * Copy the reference and using parts of the script
//   * Copy snippets of code to be tested
// Don't forget to update the dotnet-script before running:
//   dotnet tool update -g dotnet-script

// Reference the library:
#r "IGLib.Core.dll"

using System;
using IGLib.ConsoleUtils;
using static IGLib.ConsoleUtils.ConsoleUtilities;


// Reading values from console:

void TestReadBool(bool value = true)
{
    Console.Write($"\nTest - reading a value of type {value.GetType().Name}.\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, < Enter > to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"Initial value: {value}; was read: {wasRead}; new value: {value}");
}



void TestReadLong(long value = 0l)
{
    Console.Write($"\nTest - reading a value of type {value.GetType().Name}.\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, < Enter > to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"Initial value: {value}; was read: {wasRead}; new value: {value}");
}


void TestReadDoubleOld(double value = 0.0)
{
    Console.Write($"\nTest - reading a value of type {value.GetType().Name}.\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, < Enter > to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"Initial value: {value}; was read: {wasRead}; new value: {value}");
}

void TestReadDouble(double value = 0.0)
{
    Console.Write($"\nTest - reading a value of type {value.GetType().Name}.\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, < Enter > to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"Initial value: {value}; was read: {wasRead}; new value: {value}");
}



/*

TestReadBool(false);

TestReadLong(22);

TestReadDouble(2.3);

TestReadDoubleOld(105e-2);



*/

