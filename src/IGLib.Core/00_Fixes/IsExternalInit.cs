// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

#nullable disable


// This file is necessary to enable properties' init accessors when .NET 4.8 is targeted
// This is a fix for the following compiler error in .NET 4.8 targets:
// "Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported"

// #if NET48  // conditinal compilation is not needed
namespace System.Runtime.CompilerServices
{
    public class IsExternalInit {}
}
// #endif