using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using System;

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
        public TestBase(ITestOutputHelper output, bool assignConsole= true)
        {
            Output = output;
            Console = output;
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new XUnitLoggerProvider(output));
        }

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
