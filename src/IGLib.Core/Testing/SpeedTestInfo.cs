// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IGLib.Tests
{


    /// <summary>Provides information about speed test execution, including the eventual results.
    /// <para>Type of the result of calculation executed during test is <see cref="double"/>.</para></summary>
    public class SpeedTestInfo: SpeedTestInfo<double>
    {

        public SpeedTestInfo(string testId, string referenceMachineId = null) :
            base(testId, referenceMachineId) 
        { }

        public SpeedTestInfo() : base()
        { }

        /// <inheritdoc/>
        /// <remarks>This property evaluates to true for the current class.</remarks>
        public override bool CanCalculateDiscrepancy { get; } = true;

        /// <inheritdoc/>
        public override double Discrepancy => Result - AnalyticalResult;

    }


    /// <summary>Provides information about speed test execution, including the eventual results.</summary>
    public class SpeedTestInfo<ResultType>
    {

        public SpeedTestInfo(string testId, string referenceMachineId = null): this()
        {
            TestId = testId;
            if (!string.IsNullOrEmpty(referenceMachineId))
            {
                ReferenceMachineId = referenceMachineId;
            }
        }

        public SpeedTestInfo() { }

        public const string DefultTestId = "{{Unspecidied test.}}";

        /// <summary>A distinctive string ID of the test. Its value should be defined as a public constant 
        /// somewhere close to test definition.
        /// <para>Default is <see cref="DefultTestId"/>, such that it can be distinguished when the property
        /// has not been set.</para></summary>
        public virtual string TestId { get; set; } = DefultTestId; 

        /// <summary>Number of executions or iterations performed in a speed test.</summary>
        public virtual int NumExecutions { get; set; } = 1;

        /// <summary>Dimensions of the speed test (e.g. matrix dimensions {d1, d2} in case of matrix tests,
        /// {0} in case of scalar tets).</summary>
        /// <remarks>This is currently not used. Please elaborate on the behavior later.</remarks>
        public virtual int[] Dimensions { get; set; } = { 0 };

        /// <summary>List of parameters of the test (if any).
        /// <pra>To Consider: should this be repalced by ModelParameterSet when it is ready?</pra></summary>
        public List<(string ParameterName, object ParameterValue)> Parameters { get; }
            = new List<(string ParameterName, object ParameterValue)>();

        /// <summary>Whether the current object contains a valid result.
        /// <para>This implementation provides a heuristic estimation, which shuld produce correct results in
        /// most cases (e.g. in different concretized instances of this generic class). False is returned 
        /// if either the <see cref="Exception"/> property is set (meaning that exception was thrown when 
        /// performing the test) or when the <see cref="Result"/> property has default value, but this
        /// is not equal to what is contained in the <see cref="AnalyticalResult"/>. When the result type
        /// is a class, default value is null and if result is null this almost certainly means that
        /// the result has not been calculated and assigned. When the result type is a value type such
        /// as <see cref="double"/>, default value (0.0 for double) might easily be a result actually
        /// produced by the test, but in such cases the <see cref="AnalyticalResult"/> would lekely have
        /// the same value.</para></summary>
        public virtual bool HasValidResult =>
            Exception == null && (!Result.Equals(default(ResultType)) || Result.Equals(AnalyticalResult));

        /// <summary>Whether the current object has the execution time assigned.
        /// <para>This implements a heuristic tests: the <see cref="ExecutionTimeSeconds"/> must be
        /// greater than 0 to return true. No speed test should be designed in such a way that execution
        /// time is below the resolution of the system's clock, therefore execution time being 0 can
        /// only mean that the execution time has not been assigned.</para></summary>
        public virtual bool HasExecutionTime => 
            Exception == null && ExecutionTimeSeconds != DefaultExecutionTimeSeconds;

        /// <summary>Result calculated by the test.</summary>
        public virtual ResultType Result { get; set; } = default(ResultType);

        /// <summary>Expected result of the test, calculated by some other method (e.g., using an
        /// analytical formula in case it exists).</summary>
        public virtual ResultType AnalyticalResult { get; set; } = default(ResultType);

        /// <summary>Tolerance for the result of the calculation performed by the test to be considered correct.</summary>
        public virtual ResultType Tolerance { get; set; } = default(ResultType);

        /// <summary>True if discrepancy between <see cref="Result"/> and <see cref="AnalyticalResult"/>
        /// can be calculated by the current object, false if not.</summary>
        public virtual bool CanCalculateDiscrepancy { get; } = false;
        
        /// <summary>Calculated discrepancy between the calculated result of the test (<see cref="Result"/>) 
        /// and the expected result of the test (<see cref="AnalyticalResult"/>).
        /// <para>Before referencing this property, check the <see cref="CanCalculateDiscrepancy"/> to see
        /// whether the discrepancy can at all be provided with the current class.</para>
        /// <para>The value of this property is meaningful only when the <see cref="Result"/> and
        /// <see cref="AnalyticalResult"/> were both assigned.</para></summary>
        public virtual ResultType Discrepancy => 
            throw new NotImplementedException($"Calculation of discrepancy is not implemented for this class ({GetType().Name})");

        public const double DefaultExecutionTimeSeconds = 0.0;

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


        /// <summary>Default value for <see cref="ReferenceMachineId"/></summary>
        public const string DefaultRefeenceMachineId = "[[UnknownMachine]]";

        public virtual string ReferenceMachineId { get; set; } = DefaultRefeenceMachineId;

        /// <summary>Result: number of executions (or iterations) per second measured in the 
        /// test, the most relevant result of te test (the larger the number, the faster the host 
        /// computer is).</summary>
        public virtual double NumExecutionsPerSecond => (double)NumExecutions / ExecutionTimeSeconds;

        /// <summary>Result: number of millions of executions per second measured in the test, easier
        /// to read for most tests than <see cref="NumExecutionsPerSecond"/>.</summary>
        public virtual double MegaExecutionsPerSecond => NumExecutionsPerSecond / 1.0e6;

        /// <summary>Result: the normalized speed factor (number of executions per second divided by
        /// the result of the same test achieved in an reference environment, identified by <see cref="ReferenceMachineId"/>).
        /// The larger the number, the faster the host computer.</summary>
        public virtual double SpeedFactor => NumExecutionsPerSecond / ReferenceExecutionsPerSecond;

        /// <summary>Eventual exception thrown during the test.</summary>
        public virtual Exception Exception { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Speed test \"{TestId}\":");
            sb.AppendLine($"  Number of executions / iterations: {NumExecutions}");
            if (Parameters != null && Parameters.Count > 0)
            {
                sb.AppendLine($"  Parameters:");
                foreach (var parameter in Parameters)
                {
                    sb.AppendLine($"    {parameter.ParameterName} = {parameter.ParameterValue}");
                }
            }
            if (Dimensions != null && Dimensions.Length > 1 || Dimensions.Length == 1 && Dimensions[0] != 0)
            {
                sb.Append($"  Dimensions: [");
                for ( int i = 0; i < Dimensions.Length; i++ )
                {
                    sb.Append($"{Dimensions[i]}");
                    if (i < Dimensions.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine($"]");
            }
            sb.AppendLine($"  Test result:   {Result}");
            if (CanCalculateDiscrepancy)
            {
                sb.AppendLine($"    Analytical:  {AnalyticalResult}");
                sb.AppendLine($"    Discrepancy: {Discrepancy}, Tolerance: {Tolerance}");
            }
            sb.AppendLine($"  Reference executions per second: {ReferenceExecutionsPerSecond} ({ReferenceExecutionsPerSecond / 1e6} M)");
            sb.AppendLine($"  Reference machine ID:  {ReferenceMachineId}");
            sb.AppendLine($"  Results:");
            sb.AppendLine($"  Execution time:        {ExecutionTimeSeconds} s");
            sb.AppendLine($"  Executions per second: {NumExecutionsPerSecond}");
            sb.AppendLine($"    Millions per second: {MegaExecutionsPerSecond}");
            sb.AppendLine($"           Speed factor: {SpeedFactor}");
            if (Exception != null)
            {
                sb.AppendLine($"  EXCEPTION ({Exception.GetType().Name}) thrown:");
                sb.AppendLine($"     \"{Exception.Message}\"");
                Exception innerException = Exception.InnerException;
                if (innerException != null)
                {
                    sb.AppendLine($"    Inner exception ({innerException.GetType().Name}):");
                    sb.AppendLine($"       \"{innerException.Message}\"");
                }
            }
            return sb.ToString();
        }

    }

}
