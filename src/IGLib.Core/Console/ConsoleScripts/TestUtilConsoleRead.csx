
// This script contains some snippets to manually test some console utilities.
//   * Open console in the directory to output direcory where binaries are generated.
//   * Run dotnet script.
//   * Copy the reference and using parts of the script
//   * Copy snippets of code to be tested
// Don't forget to update the dotnet-script before running:
//   dotnet tool update -g dotnet-script

// Reference the library:
#r "./IGLib.Core.dll"

using System;
using IGLib.ConsoleAbstractions;
using static IGLib.ConsoleAbstractions.ConsoleUtilities;

// Reading values from console:

void TestReadBool(bool value = true)
{
    var initialValue = value;
    Console.WriteLine($"\nTest - reading a value of type {value.GetType().Name}:\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, <Enter> to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"\nInitial value: {initialValue}; was provided: {wasRead}; new value: {value}\n");
}

void TestReadLong(long value = 987)
{
    var initialValue = value;
    Console.WriteLine($"\nTest - reading a value of type {value.GetType().Name}:\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, <Enter> to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"\nInitial value: {initialValue}; was provided: {wasRead}; new value: {value}\n");
}


void TestReadDouble(double value = 4.5)
{
    var initialValue = value;
    Console.WriteLine($"\nTest - reading a value of type {value.GetType().Name}:\n");
    Console.Write($"Insert a number of type {value.GetType().Name} (? for help, <Enter> to keep {value}): ");
    bool wasRead = ConsoleUtilities.Read(ref value);
    Console.WriteLine($"\nInitial value: {initialValue}; was provided: {wasRead}; new value: {value}\n");
}



/*
How to run test examples:

#load "TestUtilConsoleRead.csx"

TestReadBool(false);

TestReadLong(22);

TestReadDouble(2.3);




*/

