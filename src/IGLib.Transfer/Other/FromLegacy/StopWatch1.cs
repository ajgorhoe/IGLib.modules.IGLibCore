// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
// using System.Threading;

namespace IG.Lib
{

    //[Obsolete("Replaced by StopWatch1.")]
    //public class Timer : StopWatch1 { }


    /// <summary>Timer for measuring execution times 
    /// and other intervals of time elapsed between successive events.
    /// $A Igor xx Apr10 Jun15;</summary>
    public class StopWatch1: ILockable, IIdentifiable
    {

        #region Construction

        protected static int lastId = 0;

        private static object _staticLock;

        /// <summary>Static lock object accessible only from the current class.
        /// <para>Global lock object <see cref="Util.LockGlobal"/> is used to synchronize creation of this lock.</para></summary>
        protected static object StaticLock
        {
            get
            {
                if (_staticLock == null)
                {
                    lock (Util.LockGlobal)
                    {
                        _staticLock = new object();
                    }
                }
                return _staticLock;
            }
        }

        public StopWatch1()
        { Init(); }

        public StopWatch1(string label)
        { Init(label); }

        protected void Init()
        {
            lock (StaticLock)
            {
                _ID = ++lastId;
            }
            SetTimeStamp();
        }

        protected void Init(string label)
        {
            Init();
            SetLabel(label);
        }

        public static StopWatch1 Create()
        {
            StopWatch1 ret = new StopWatch1();
            return ret;
        }

        public static StopWatch1 Create(string label)
        {
            StopWatch1 ret = new StopWatch1(label);
            return ret;
        }

        #endregion Construction



        #region ThreadLocking
        
        /// <summary>Lock object to be used for locking the current object.</summary>
        protected readonly object _lock = new object();

        /// <summary>Lock object to be used for locking the current object.</summary>
        public object Lock
        {
            get { return _lock; }
        }

        #endregion ThreadLocking

        // protected object lockobj = new object();


        protected string _label = null;
        protected int _ID = 0;


        protected double
            _creationcCpuTime, // CPU time of creation
            _totalCpuTime,  // total CPU time measured by the timer
            _startCpuTime,   // CPU time when timer was last started
            _stopCpuTime,   // CPU time when timer was last stopped
            _firstStartCpuTime;  // CPU time when timer was FIRST started

        protected DateTime
            _creationTime,    // time of creation (absolute time) 
            _startTime,     // absolute time when the timer was last started 
            _stopTime,      // absolute time when timer was last stopped 
            _firstStartTime;     // absolute time when the timer was FIRST started 

        protected double
            _totalTime;     // total wallclock time measured by the timer, in seconds 

        protected bool
            _running = false,   // tells whether timer is running or not 
            _used = false,      // tells whether timer has been used after creation or after last  reset
            _measureTime = true,     // tells whether wallclock time is measured by the current stopwatch
            _measureCpuTime = true;  // tells whether CPU time is measured by the current stopwatch

        #region Auxiliary

        #endregion  // Auxiliary

        protected static int numWarnCpu = 0;

        public static int MaxWarnCpu = 0;

        /// <summary>Returns the total CPU time sent up to this moment by the current process.</summary>
        public double ThreadCpuTime()
        {
            if (numWarnCpu < MaxWarnCpu)
            {
                Warning("Measurement of CPU time is implemented only for the whole process, not for the current thread.");
                ++numWarnCpu;
            }
            return Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds;
        }

        /// <summary>Prints out a warning related to timer use.</summary>
        /// <param name="warningstr"></param>
        protected void Warning(string warningstr)
        {
            Console.WriteLine(Environment.NewLine + Environment.NewLine
                + "Warning - timer, ID = " + Id + ": " + warningstr
                + Environment.NewLine + Environment.NewLine);
        }

        #region Operations

        /// <summary>Returns a flag indicating whether the stopwatch is currently running.</summary>
        public bool IsRunning
        {
            get { lock (_lock) { return _running; } }
        }

        /// <summary>Starts the timer (the elapsed time and CPU time are measured since this moment). 
        /// If the timer is already running then exception is thrown.</summary>
        public void Start()
        {
            lock (_lock)
            {
                if (_running)
                    Warning("Timer can not be started, since it is already running.");
                else
                {
                    _startTime = DateTime.Now;
                    _startCpuTime = ThreadCpuTime();
                    _measureTime = _measureCpuTime = true;
                    _running = true;
                    if (!_used)
                    {
                        _used = true;
                        _firstStartTime = _startTime;
                        _firstStartCpuTime = _startCpuTime;
                    }
                }
            }
        }

        /// <summary>Stops the timer and adds the time and CPU time difference measured in the 
        /// last round to the total time. If the timer is not running then exception is thrown.</summary>
        public void Stop()
        {
            lock (_lock)
            {
                if (!_running)
                    Warning("timer can not be stopped, it is not running.");
                else
                {
                    _running = false;
                    if (_measureCpuTime)
                    {
                        _stopCpuTime = ThreadCpuTime();
                        _totalCpuTime += _stopCpuTime - _startCpuTime;
                    }
                    if (_measureTime)
                    {
                        _stopTime = DateTime.Now;
                        _totalTime += (_stopTime - _startTime).TotalSeconds;
                    }
                }
            }
        }

        /// <summary>Resets the timer. Its state becomes identical to the state right after creation 
        /// (as it has not been used before).</summary>
        public void Reset()
        {
            lock (_lock)
            {
                _running = _used = false;
                _totalTime = _totalCpuTime = 0.0;
                // _measureTime = _measureCpuTime = false;
                SetTimeStamp();
            }
        }

        /// <summary>Sets the timestamp on the timer (marks the current time). This is done automatically 
        /// by Reset(), Create() and constructors.</summary>
        public void SetTimeStamp()
        {
            lock (_lock)
            {
                _creationTime = DateTime.Now;
                _creationcCpuTime = ThreadCpuTime();
            }
        }

        public void SetLabel(string label)
        {
            lock (_lock)
            {
                _label = label;
            }
        }


        public override string ToString()
        {
            lock (_lock)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_label))
                    sb.AppendLine("Timer \"" + _label + "\":");
                if (_running)
                {
                    sb.AppendLine("Total time (intermediate): " + TotalTime +
                        " s (CPU " + TotalCpuTime + " s)");
                    sb.AppendLine("Last round (intermediate): " + Time +
                        " s (CPU " + CpuTime + " s)");
                }
                else
                {
                    sb.AppendLine("Total elapsed time: " + TotalTime +
                        " s (CPU " + TotalCpuTime + " s)");
                    sb.AppendLine("        Last round: " + Time +
                        " s (CPU " + CpuTime + " s)");
                }
                return sb.ToString();
            }
        }

        public string ToStringLong()
        {
            lock (_lock)
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_label))
                    sb.AppendLine("Timer \"" + _label + "\":");
                else
                    sb.AppendLine(Environment.NewLine + "Timer data:");
                sb.AppendLine(ToString());
                sb.AppendLine("First start (absolute time): ");
                sb.AppendLine("First start (CPU time): ");
                sb.AppendLine("Start of the last round (absolute time): ");
                sb.AppendLine("Start of the last round (CPU time): ");
                sb.AppendLine("Stop of the last round (absolute time): ");
                sb.AppendLine("Stop of the last round (CPU time): ");
                if (_running)
                    sb.AppendLine("Timer is running.");
                else
                    sb.AppendLine("Timer is not running.");
                if (_used)
                    sb.AppendLine("Timer has been used already.");
                else
                    sb.AppendLine("Timer has not been used yet.");
                sb.AppendLine("TimeStamp: " + TimeStamp.ToString());
                return sb.ToString();
            }
        }

        #endregion  // Operations


        #region Properties

        /// <summary>Returns counter's ID.</summary>
        public int Id
        { get { return _ID; } }

        /// <summary>Gets the data from time stamp contained in the timer.</summary>
        public DateTime TimeStamp
        { get { lock (_lock) { return _creationTime; } } }

        public string Label
        { get { return _label; } }

        /// <summary>Gets the total time in seconds, measured by the timer up to the current moment. 
        /// If the timer is running then the current absolute time is calculated, and difference with 
        /// the last starting time added to the total time accumulated in previous rounds.</summary>
        public double TotalTime
        {
            get
            {
                lock (_lock)
                {
                    if (!_measureTime)
                        return -1.0e-9;
                    double ret = _totalTime;
                    if (_running)
                    {
                        ret += (DateTime.Now - _startTime).TotalSeconds;
                    }
                    return ret;
                }
            }
        }

        /// <summary>Gets the total CPU time in seconds, measured by the timer up to the current moment. 
        /// If the timer is running then the current CPU time is calculated, and difference with 
        /// the last starting CPU time added to the total CPU time accumulated in previous rounds.</summary>
        public double TotalCpuTime
        {
            get
            {
                lock (_lock)
                {
                    if (!_measureCpuTime)
                        return -1.0e-9;
                    double ret = _totalCpuTime;
                    if (_running)
                    {
                        ret += ThreadCpuTime() - _startCpuTime;
                    }
                return ret;
                }
            }
        }

        /// <summary>Returns the elapsed time measured by the timer in the last round. 
        /// If the timer  is running then the current time is calculated and its difference with 
        /// the starting time returned.</summary>
        public double Time
        {
            get
            {
                lock (_lock)
                {
                    if (!_measureTime)
                        return -1.0e-9;
                    if (_running)
                        return (DateTime.Now - _startTime).TotalSeconds;
                    else
                        return (_stopTime - _startTime).TotalSeconds;
                }
            }
        }


        /// <summary>Returns the elapsed CPU time measured by the timer in the last round. 
        /// If the timer  is running then the current CPU time is calculated and its difference with 
        /// the starting time returned.</summary>
        public double CpuTime
        {
            get
            {
                lock (_lock)
                {
                    if (!_measureCpuTime)
                        return -1.0e-9;
                    if (_running)
                        return (ThreadCpuTime() - _startCpuTime);
                    else
                        return (_stopCpuTime - _startCpuTime);
                }
            }
        }

        /// <summary>Gets a timespan object that is equivalent to the specified time in seconds, and returns it.
        /// <para>There need not be whole number of seconds.</para></summary>
        /// <param name="seconds">Time in seconds for which the equivalent <see cref="TimeSpan"/> object is returned.</param>
        public static TimeSpan GetTimeSpan(double seconds)
        {
            double wholeSeconds = Math.Floor(seconds);
            double remainingMilliSeconds = 1000.0 * (seconds-wholeSeconds);
            return new TimeSpan(0, 0, 0, (int) wholeSeconds, (int) remainingMilliSeconds); 
        }

        public TimeSpan TimeSpan
        {
            get { return GetTimeSpan(this.Time); }
        }

        public TimeSpan CpuTimeSpan
        {
            get { return GetTimeSpan(this.CpuTime); }
        }

        public TimeSpan TotalTimeSpan
        {
            get { return GetTimeSpan(this.TotalTime); }
        }

        public TimeSpan TotalCpuTimeSpan
        {
            get { return GetTimeSpan(this.TotalCpuTime); }
        }

        #endregion  // Properties


        #region Examples

        /// <summary>Example of using the stopwatch.</summary>
        public static void Example()
        {
            Console.WriteLine("Stopwatch test...");
            double tSleep;
            StopWatch1 t = new StopWatch1();
            t.Start();
            Console.WriteLine("> Stopwatch created and started.");
            tSleep = 0.62;
            Console.Write("> Sleeping " + tSleep + " s ... ");
            Util.SleepSeconds(tSleep);
            Console.WriteLine("  ... sleep finished.");
            Console.WriteLine("Stopwatch state: " + Environment.NewLine
                + "  Time: " + t.Time + " (span: " + t.TimeSpan + "). " + Environment.NewLine
                + "  Total time: " + t.TotalTime + " ( span: " + t.TotalTimeSpan + ").");
            t.Stop();
            Console.WriteLine(">Stopwatch stopped.");
            Console.WriteLine("  Time: " + t.Time + " (span: " + t.TimeSpan
                + ").");
            Console.WriteLine("  Total time: " + t.TotalTime + " ( span: " + t.TotalTimeSpan + ").");
            t.Start();
            Console.WriteLine(">Stopwatch started.");

            tSleep = 1.39;
            Console.Write("> Sleeping " + tSleep + " s ... ");
            Util.SleepSeconds(tSleep);
            Console.WriteLine("  ... sleep finished.");
            Console.WriteLine("Stopwatch state: " + Environment.NewLine
                + "  Time: " + t.Time + " (span: " + t.TimeSpan + "). " + Environment.NewLine
                + "  Total time: " + t.TotalTime + " ( span: " + t.TotalTimeSpan + ").");
            t.Stop();
            Console.WriteLine(">Stopwatch stopped.");
            Console.WriteLine("  Time: " + t.Time + " (span: " + t.TimeSpan
                + ").");
            Console.WriteLine("  Total time: " + t.TotalTime + " ( span: " + t.TotalTimeSpan + ").");
            t.Start();
            Console.WriteLine(">Stopwatch started.");
        }

        #endregion Examples



        #region Static.MeasureExecutionTimes

        public delegate void VoidDelegate();


        public static int TestExecutionTime(VoidDelegate work, out double averageExecutionTime, out double averageCpuTime, 
            double targetedTime = 0.1,  int outpuLevel = 0, int numInitialExecutions = 1)
        {
            int batchSize = numInitialExecutions;
            if (batchSize < 1)
                batchSize = 1;
            IG.Lib.StopWatch1 t = new IG.Lib.StopWatch1();
            int numExecutions = 0;
            double totalTime = 0.0, totalCpuTime = 0;
            double timeRatio = 0.0;
            bool stop = false;
            if (outpuLevel >= 1)
            {
                Console.WriteLine("Starting the time performance test...");
            }
            while (!stop)
            {
                t.Start();
                for (int i = 0; i < batchSize; ++i)
                {
                    work();
                    ++ numExecutions;
                }
                t.Stop();
                totalTime += t.Time;
                totalCpuTime += t.CpuTime;
                timeRatio = totalTime / targetedTime;
                if (outpuLevel >= 2)
                {
                    Console.WriteLine("  " + batchSize + " executions performed in " + t.Time + " s (" + t.CpuTime + "CPU) " + Environment.NewLine
                        + "    total time: " + totalTime + " s (CPU: " + totalCpuTime + "), time ratio: " + timeRatio );
                }
                if (timeRatio >= 1.0)
                    stop = true;
                else if (timeRatio < 0.4)
                {
                    if (timeRatio < 0.01)
                    {
                        if (numExecutions > 100)
                            batchSize = (int)(batchSize * 10);
                        else
                            batchSize *= 4;
                    } else if (timeRatio < 0.1)
                    {
                        if ((numExecutions > 20))
                            batchSize *= 6;
                        else
                            batchSize *= 3;
                    } else if (timeRatio < 0.1)
                    {
                        if (numExecutions > 5)
                            batchSize *= 4;
                        else
                            batchSize *= 2;
                    } else
                    {
                        if (numExecutions > 5)
                        {
                            batchSize = (int)(0.6 * (1.0 - timeRatio) * (double)numExecutions / (timeRatio));
                            if (batchSize < 1)
                                batchSize = 1;
                        }
                        else
                        {
                            batchSize = (int) (1.5 * (double) batchSize);
                            if (batchSize < 1)
                                batchSize = 1;
                        }
                    }
                } else if (timeRatio < 0.8)
                {
                    if (numExecutions > 5)
                    {
                        batchSize = (int)(0.8 * (1.0 - timeRatio) * (double)numExecutions / (timeRatio));
                        if (batchSize < 1)
                            batchSize = 1;
                    } else 
                    {
                        batchSize = (int)(0.6 * (1.0 - timeRatio) * (double)numExecutions / (timeRatio));
                        if (batchSize < 1)
                            batchSize = 1;
                    }
                } else if (timeRatio < 0.9)
                {
                    batchSize = (int)(0.9 * (1.0 - timeRatio) * (double)numExecutions / (timeRatio));
                    if (batchSize < 1)
                        batchSize = 1;
                } else if (timeRatio < 0.99)
                {
                    batchSize = (int)(1.1 * (1.0 - timeRatio) * (double)numExecutions / (timeRatio));
                    if (batchSize < 1)
                        batchSize = 1;
                } else
                {
                    batchSize = (int)(1.5 * (1.0 - timeRatio) * (double)numExecutions / (timeRatio));
                    if (batchSize < 1)
                        batchSize = 1;
                }
                if (batchSize < 1)
                    batchSize = 1;
            }
            averageExecutionTime = totalTime / (double) numExecutions;
            averageCpuTime = totalCpuTime / (double) numExecutions;
            if (outpuLevel >=1)
            {
                Console.WriteLine("  ... test finished.");
                Console.WriteLine(numExecutions + " executions performed in " + totalTime + " s (" + totalCpuTime + " s CPU).");
                Console.WriteLine("Executions per second: " + ((double)numExecutions / totalTime) + " (per second of CPU time: "
                    + ((double)numExecutions / totalCpuTime) + ").");
                Console.WriteLine("Average execution time: " + averageExecutionTime + " s (CPU: " + averageCpuTime + " s).");
            }
            return numExecutions;
        }





        #endregion Static.MeasureExecutionTimes






    }  // StopWatch1


}
