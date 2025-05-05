using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IGLib.Tests
{

    /// <summary>Provides information about speed test execution, including the eventual  results.</summary>
    public class SpeedTestInfo
    {

        public SpeedTestInfo(string testId): this()
        {
            TestId = testId;
        }

        public SpeedTestInfo() { }

        public const string DefultTestId = "[[Unspecidied test.]]";

        /// <summary>A distinctive string ID of the test. Its value should be defined as a public constant 
        /// somewhere close to test definition.
        /// <para>Default is <see cref="DefultTestId"/>, such that it can be distinguished when the property
        /// has not been set.</para></summary>
        public virtual string TestId { get; set; } = DefultTestId; 

        /// <summary>Number of executions or iterations performed in a speed test.</summary>
        public virtual int NumExecutions { get; set; } = 1;

        /// <summary>Dimensions of the speed test (e.g. matrix dimensions {d1, d2} in case of matrix tests,
        /// {0} in case of scalar tets).</summary>
        public virtual int[] Dimensions { get; set; } = { 0 };

        public const double DefaultExecutionTimeSeconds = 1e9;

        /// <summary>Total execution time of the test, in seconds.
        /// <para>Initially set to <see cref="DefaultNumExecutionTimeSeconds"/>, such that the
        /// <see cref="SpeedFactor"/> becomes infinity if the <see cref="ReferenceExecutionsPerSecond"/>
        /// is not specified, or a very large number if it is specified but the <see cref="NumExecutionsPerSecond"/>
        /// is not specified.</para></summary>
        public virtual double ExecutionTimeSeconds { get; set; } = DefaultExecutionTimeSeconds;

        /// <summary>Reference result (number of executions per second), average of the same test
        /// performef on a particular rference machine.
        /// <para>Initialized to 0.0, such that <see cref="SpeedFactor"/> becomes infinity
        /// when this property is not set.</para></summary>
        public virtual double ReferenceExecutionsPerSecond { get; set; }

        /// <summary>Result: number of executions (or iterations) per second measured in the 
        /// test, the most relevant result of te test (the larger the number, the faster the host 
        /// computer is).</summary>
        public virtual double NumExecutionsPerSecond => (double)NumExecutions / ExecutionTimeSeconds;

        /// <summary>Result: number of millions of executions per second measured in the test, easier
        /// to read for most tests than <see cref="NumExecutionsPerSecond"/>.</summary>
        public virtual double MegaExecutionsPerSecond => NumExecutionsPerSecond / 1.0e6;


        /// <summary>Result: the normalized speed factor (number of executions per second divided by
        /// the result of the same test achieved in an reference environment). The larger the number,
        /// the faster the host computer.</summary>
        public virtual double SpeedFactor => NumExecutionsPerSecond / ReferenceExecutionsPerSecond;

    }

}
