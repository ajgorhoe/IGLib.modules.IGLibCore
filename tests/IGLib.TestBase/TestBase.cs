using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace IGLib.Tests.Base
{
    public class TestBase<TestClass>
    {

        /// <summaryConstructor of the common test classes' base class.</summary>
        /// <param name="output">Object that provides access to tests' output by the XUnit testing test famework.</param>
        /// <param name="assignConsole">If true then output is also assigned to Console property, such that Console
        /// can be used to write to the test output, too. The API is of type <see cref="ITestOutputHelper"/>
        /// and is more limited than consol, but it still enables some code that uses output to Consol to be directly
        /// copied into test methods.</param>
        public TestBase(ITestOutputHelper output, bool assignConsole = true)
        {
            Output = output;
            Console = output;
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new XUnitLoggerProvider(output));
        }

        /// <summary>Default export path for tests that generate output. Specified relative to the
        /// currennt directory of test execution, which is usually a subdirectory of the [ProjectDir]/bin/  .</summary>
        protected const string DefaultExportPathIGLib = "../_IGLib_TestExports/";

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
        protected string ExportPathIGLib { get; } = DefaultExportPathIGLib;

        protected string ExportPathIGLibTemporary => Path.Combine(ExportPathIGLib, "temp");

        /// <summary>Enables writing to tests' output. Provided by the framework (injected via constructor).</summary>
        public ITestOutputHelper Output { get; set; }

        /// <summary>This property makes possible to use the name Console instead of Output in
        /// test method of this class, such that code can be directly copied to console 
        /// applications while output is still visible in tests' output. Unfortunately, you 
        /// cannot use Console.Write(...) because the ITestOutputHelper does not have it.</summary>
        protected static ITestOutputHelper Console { get; set; }

        private LoggerFactory LoggerFactory{ get; set; }

        // ToDo: provide a logger!

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

        public IDisposable BeginScope<TState>(TState state)
            => NoopDisposable.Instance;

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _testOutputHelper.WriteLine($"{_categoryName} [{eventId}] {formatter(state, exception)}");
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
