
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

bool answer = true;
bool wasRead = ConsoleUtilities.Read(ref answer);

(answer, wasRead)

