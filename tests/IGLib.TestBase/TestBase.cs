using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

using IGLib.ConsoleAbstractions;

namespace IGLib.Tests.Base
{
    
    /// <summary>Base class for test classes. Provides utilities such as output (via the properties 
    /// <see cref="Output"/> of type <see cref="ITestOutput"/> and <see cref="Console"/> of type <see cref="IConsole"/>.</summary>
    /// <typeparam name="TestClassType">Actual type of the test class, or at least its base class. Can be abstract.</typeparam>
    public abstract class TestBase<TestClassType>   // not needed any more: : TestBase
    {

        /// <summary>Constructor of the common test classes' base class.</summary>
        /// <param name="output">Object that provides access to tests' standard output by the XUnit testing test famework.</param>
        /// <param name="isConsoleLineBuffered">If true then output of <see cref="Console"/> is line buffered, i.e., output 
        /// of <see cref="IConsole.Write(string?)"/> calls will not be written until the first call to a method that adds the
        /// newline, such as <see cref="IConsole.WriteLine(string?)"/>.</param>
        public TestBase(ITestOutputHelper output, bool isConsoleLineBuffered = true) // : base(output)
        {
            Output = output;
            Console = new XUnitOutputConsole(output, isConsoleLineBuffered);
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new XUnitLoggerProvider(output));
        }

        /// <summary>Default export path for tests that generate output. Specified relative to the
        /// currennt directory of test execution, which is usually a subdirectory of the [ProjectDir]/bin/  .</summary>
        protected const string DefaultExportPathIGLib = "../../_IGLib_TestExports/";

        /// <summary>Export path for tests that generate output files. This is set to <see cref="IGLibDefaultExportPath"/>
        /// by default (good for most cases), but this can be overridden in test class's constructor.
        /// <para>When default is used, directory and file paths in subdirectories can be created by string
        /// addition.</para></summary>
        /// <remarks>There are several reasons one would like to generatte file output in tests.
        /// <para>Some tests will generate temporary intermediate output, e.g. when their aim is specifically to test 
        /// correctness of file exports or imports.</para>
        /// <para>There are also tests that serve as examples, or they generate output that isintended for manual 
        /// inspection, possibly with help off 3rd party software. Tyical example are some tests / examples for 
        /// 3D graphics, which generate output that can be manually imported and inspected in software like Blender.
        /// User can immediately give rough estimate of correctness of generated surfaces, or even correctness of
        /// generated surface normals (since incorrect normals would reflect visually in strange rendering of coarse
        /// meshes ).</para></remarks>
        protected string ExportPathIGLib { get; set; } = DefaultExportPathIGLib;

        protected string ExportPathIGLibTemporary => Path.Combine(ExportPathIGLib, "temp");

        /// <summary>Enables writing to tests' standard output via its <see cref="ITestOutputHelper.WriteLine(string)"/>
        /// and <see cref="ITestOutputHelper.WriteLine(string, object[])"/> methods. Provided by the framework (injected 
        /// via constructor).</summary>
        protected ITestOutputHelper Output { get; init; }

        /// <summary>This property provides an <see cref="IConsole"/> object that can be used to write to the standard
        /// output of tests in inherited classes. The object of actual type <see cref="XUnitOutputConsole"/> adapts the
        /// raw <see cref="ITestOutputHelper"/> object accessible via the <see cref="Output"/> property.
        /// <para>If the actual type of this object is <see cref="XUnitOutputConsole"/> then the <see cref="IsConsoleOutputLineBuffered"/>
        /// property can be used in order to set this console's output to line buffered (when set to true) or unbuffered (when set to false).</para>
        /// <para>In order to use the <see cref="IConsole"/> extension methods on this properrty, include the using statement 
        /// with the <see cref="IGLib.ConsoleAbstractions.Extensions"/> namespace.</para></summary>
        protected IConsole Console { get; init; }

        /// <summary>Gets or sets a value indicating whether the console output is line buffered for the XUnit output console.</summary>
        /// <remarks>This property is applicable only when the console is of type <see cref="XUnitOutputConsole"/>.
        /// Setting this property to a non-null value will throw an <see cref="InvalidOperationException"/> if the console is not of
        /// the appropriate type. This can be verified by getting this property value: if the value is null then the above
        /// conditions are not met, and the property cannot be set to a non-null value.</remarks>
        protected bool? IsConsoleOutputLineBuffered
        {
            get
            {
                if (Console is XUnitOutputConsole xUnitConsole)
                {
                    return xUnitConsole.IsLineBuffered;
                }
                return null; // Not applicable for other console types
            }
            set
            {
                if (Console is XUnitOutputConsole xUnitConsole && xUnitConsole is not null && value != null)
                {
                    xUnitConsole.IsLineBuffered = value.Value;
                }
                throw new InvalidOperationException("IsConsoleOutputLineBuffered can only be set if Console is of type XUnitOutputConsole and value is not null.");
            } 
        }

        private LoggerFactory LoggerFactory{ get; set; }

        // ToBeDone: provide a logger!
        //public ILogger GetLogger()
        //{
        //    throw new NotImplementedException();
        //    // return LoggerFactory.CreateLogger();
        //}


    }

    // For logging, see:
    // https://www.meziantou.net/how-to-get-asp-net-core-logs-in-the-output-of-xunit-tests.htm#how-to-create-an-ins
    // https://stackoverflow.com/questions/46169169/net-core-2-0-configurelogging-xunit-test





    public class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
            => new XUnitLogger(_testOutputHelper, categoryName);

        public ILogger CreateLoger<T>() => new XUnitLogger(_testOutputHelper, typeof(T).Name);

        public void Dispose()
        { }
    }

    public class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly string _categoryName;

        public XUnitLogger(ITestOutputHelper testOutputHelper, string categoryName)
        {
            _testOutputHelper = testOutputHelper;
            _categoryName = categoryName;
        }


#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;
#pragma warning restore CS8633 

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception??new())}");
            if (exception != null)
                _testOutputHelper.WriteLine(exception.ToString());
        }

        private class NoopDisposable : IDisposable
        {
            public static readonly NoopDisposable Instance = new NoopDisposable();
            public void Dispose()
            { }
        }

    }

}
