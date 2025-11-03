using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace IGLib.Tests.Base
{

    /// <summary><para>Importent:</para>
    /// <para>Do not use this class as base class for test classes. Use <see cref="TestBase{TestClassType}"/> instead.</para>
    /// <para>This class is used only as base class of <see cref="TestBase{TestClassType}"/>, such
    /// that the class <see cref="TestBase{TestClassType}.TestOutputWrapper"/> can access to the
    /// object that handles standard output of the test </summary>
    /// <param name="output"></para></param>
    [Obsolete("This base class is not needed any more and will be removed in the future.")]
    public abstract class TestBase
    {

        internal TestBase(ITestOutputHelper output)
        {
            OutputInternal = output;
        }

        /// <summary>Object that handles writing to tests' output; this property is wrepped by the
        /// <see cref="TestBase{TestClassType}.TestOutputWrapper"/> class in properties
        /// <see cref="TestBase{TestClassType}.Output"/> and <see cref="TestBase{TestClassType}.Console"/>,
        /// which are accessible in test classes that inherit in test classes that inherit from
        /// <see cref="TestBase{TestClassType}.TestOutputWrapper"/>.</summary>
        protected internal ITestOutputHelper OutputInternal { get; private init; }


        /// <summary>Provides a thread-safe and failure-safe wrapper around test output object.
        /// <para>This class provides the equivalent output functions as <see cref="TestBase{TestClassType}.Output"/>,
        /// which is of type <see cref="ITestOutputHelper"/>, namely:</para></summary>
        public class TestOutputWrapper
        {

            public TestOutputWrapper(TestBase testClass)
            {
                _testClass = testClass;
            }


            TestBase _testClass = null;

            ITestOutputHelper Output => _testClass == null ? null : _testClass.OutputInternal;

            protected object Lock { get; } = new object();

            public void WriteLine(string message)
            {
                try
                {
                    lock (Lock)
                    {
                        Output.WriteLine(message);
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        lock (Lock)
                        {
                            Output.WriteLine("\nERROR when using test output:\n  " + ex.Message);
                        }
                    }
                    catch { }
                }
            }

            public void WriteLine(string format, params object[] args)
            {
                try
                {
                    lock (Lock)
                    {
                        Output.WriteLine(format, args);
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        lock (Lock)
                        {
                            Output.WriteLine("\nERROR when using test output:\n  " + ex.Message);
                        }
                    }
                    catch { }
                }
            }

        }


#if false
        protected void Test_Misc()
        {

            int[] a = [1, 2, 3];
            OutputInternal.WriteLine("String");

            OutputInternal.WriteLine("Formatted: {0}, {1}, {2}", [1, 2, 3]);

        }
#endif


    }



}
