// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

using IG.Lib;


namespace IG.Forms
{

        /// <summary>
        /// Manages a fading message window. 
        /// Windows containing a message are launched in separate threads, closing after a specified time.
        /// </summary>
        /// <remarks>This class' constructors call virtual methods. This should in general be avoides and
        /// should be considered exceptional case.
        /// <para>Because of this, in derived classes, constructors should just directly call constructors of base
        /// class with the same arguments!</para></remarks>
        /// $A Igor jul08;
	public partial class FadingMessage : System.Windows.Forms.Form, IIdentifiable, ILockable
    {

        #region Initialization


        #region Initialization.Begin


        // BEGINNING OF INITIALIZATION:

        /// <summary>Begins initialization of the fading message (it does not launch it).
        /// <para>If necessary, this method can be </para></summary>
        protected virtual void InitFadingMessageBegin()
        {
            InitializeComponent();
            this.TopMost = IsTopMostWindow;
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="msgtext">Fading message's text (the message).</param>
        protected void InitFadingMessageBegin(string msgtext)
        {
            InitFadingMessageBegin();
            this.MsgText = msgtext;
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        protected void InitFadingMessageBegin(string msgtext, int showtime)
        {
            InitFadingMessageBegin();
            this.MsgText = msgtext;
            this.ShowTime = showtime;
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        /// <param name="fadeportion">Portion of the show time in shich teh message background fades.</param>
        protected void InitFadingMessageBegin(string msgtext, int showtime, double fadeportion)
        {
            InitFadingMessageBegin();
            this.MsgText = msgtext;
            this.ShowTime = showtime;
            this.FadingTimePortion = fadeportion;
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="title">Fading message's title.</param>
        /// <param name="msgtext">Fading message's text (the message).</param>
        protected void InitFadingMessageBegin(string title, string msgtext)
        {
            InitFadingMessageBegin();
            this.MsgTitle = title;
            this.MsgText = msgtext;
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="title">Fading message's title.</param>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        protected void InitFadingMessageBegin(string title, string msgtext, int showtime)
        {
            InitFadingMessageBegin();
            this.MsgTitle = title;
            this.MsgText = msgtext;
            this.ShowTime = showtime;
        }


        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="title">Fading message's title.</param>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        /// <param name="fadeportion">Portion of the show time in shich teh message background fades.</param>
        protected void InitFadingMessageBegin(string title, string msgtext, int showtime, double fadeportion)
        {
            InitFadingMessageBegin();
            this.MsgTitle = title;
            this.MsgText = msgtext;
            this.ShowTime = showtime;
            this.FadingTimePortion = fadeportion;
        }


        #endregion Initialization.Begin


        #region Initialization.End

        // END OF INITIALIZATION:



        /// <summary>Ends initialization of the fading message as specified by parameters.
        /// <para>This method can be overridden in derived classes.</para></summary>
        /// <param name="doLaunch">If true then fading message is automatically launched when 
        /// initialization is done.</param>
        /// <param name="inParallelThread">If true then mafing message is launched in a paralleel
        /// thread.</param>
        protected virtual void InitFadingMessageEndBase(bool doLaunch, bool inParallelThread)
        {
            if (doLaunch)
            {
                Launch(inParallelThread);
            }
        }


        /// <summary>Finalizes initialization of the fading message, which may include launching the message.</summary>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected virtual void InitFadingMessageEnd(bool doLaunch)
        {
            InitFadingMessageEndBase(doLaunch, true /* inParallelThread */);
        }


        #endregion Initialization.End

        

        #region Initialization.Complete

        // COMPLETE INITIALIZATION:


        /// <summary>Begins initialization of the fading message (it does not launch it).
        /// <para>If necessary, this method can be overridden in derived classes.</para></summary>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(bool doLaunch = true)
        {
            InitFadingMessageBegin();
            InitFadingMessageEnd(doLaunch);
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(string msgtext, bool doLaunch = true)
        {
            InitFadingMessageBegin(msgtext);
            InitFadingMessageEnd(doLaunch);
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(string msgtext, int showtime, bool doLaunch = true)
        {
            InitFadingMessageBegin(msgtext, showtime);
            InitFadingMessageEnd(doLaunch);
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        /// <param name="fadeportion">Portion of the show time in shich teh message background fades.</param>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(string msgtext, int showtime, double fadeportion, bool doLaunch = true)
        {
            InitFadingMessageBegin(msgtext, showtime, fadeportion);
            InitFadingMessageEnd(doLaunch);
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="title">Fading message's title.</param>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(string title, string msgtext, bool doLaunch = true)
        {
            InitFadingMessageBegin(title, msgtext);
            InitFadingMessageEnd(doLaunch);
        }

        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="title">Fading message's title.</param>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(string title, string msgtext, int showtime, bool doLaunch = true)
        {
            InitFadingMessageBegin(title, msgtext, showtime);
            InitFadingMessageEnd(doLaunch);
        }


        /// <summary>Begins initialization of the fading message (it does not launch it).</summary>
        /// <param name="title">Fading message's title.</param>
        /// <param name="msgtext">Fading message's text (the message).</param>
        /// <param name="showtime">Time (in miilliiseconds) in which the message is shown.</param>
        /// <param name="fadeportion">Portion of the show time in shich teh message background fades.</param>
        /// <param name="doLaunch">Whether the message should also be launched.</param>
        protected void InitFadingMessage(string title, string msgtext, int showtime, double fadeportion, bool doLaunch = true)
        {
            InitFadingMessageBegin(title, msgtext, showtime, fadeportion);
            InitFadingMessageEnd(doLaunch);
        }

        #endregion Initialization.Complete

        #region Constructors

        // CONSTRUCTORS:

        // REMARK: Constructors in derived classes must just directly call these constructors. 
        // Different behavior is handled in the overriden initialization methods which the constructors
        // call.

        public FadingMessage(bool doLaunch = true)
        /// Argument-less constructor, does not launch the window in a parallel thread.
		{
            InitFadingMessage(doLaunch);
        }


        public FadingMessage(string msgtext, bool doLaunch = true)
        /// Shows a fading message in a new thread, with message text equal to mshtext and without a title;
        { InitFadingMessage(msgtext, doLaunch); }

        public FadingMessage(string msgtext, int showtime, bool doLaunch = true)
        /// Shows a fading message in a new thread, with message text equal to  msgtext and with specified showing time in ms.
        { InitFadingMessage(msgtext, showtime, doLaunch); }

        public FadingMessage(string msgtext, int showtime, double fadeportion, bool doLaunch = true)
        /// Shows a fading message in a new thread, with message text equal to  msgtext and with specified showing time in ms and fading time portion.
        { InitFadingMessage(msgtext, showtime, fadeportion, doLaunch); }


        public FadingMessage(string title, string msgtext, bool doLaunch = true)
        /// Shows the consform in a new thread, with a title and with message text equal to msgtext.
        { InitFadingMessage(title, msgtext, doLaunch); }

        public FadingMessage(string title, string msgtext, int showtime, bool doLaunch = true)
        /// Shows a fading message in a new thread, with a title and with message text equal to  msgtext, with specified showing time in ms.
        { InitFadingMessage(title, msgtext, showtime, doLaunch); }

        public FadingMessage(string title, string msgtext, int showtime, double fadeportion, bool doLaunch = true)
        /// Shows a fading message in a new thread, with a title and with message text equal to  msgtext, with specified showing time in ms and fading time portion.
        {
            InitFadingMessage(title, msgtext, showtime, fadeportion, doLaunch);
        }

        #endregion Constructors


        #endregion Initialization


        #region Locking

        private static object _lockStatic = null;

        /// <summary>Static lock object that can be used by all fadiing message objects.</summary>
        public static object LockStatic
        {
            get {
                if (_lockStatic == null)
                {
                    lock(UtilForms.Lock)
                    {
                        if (_lockStatic == null)
                            _lockStatic = new object();
                    }
                }
                return _lockStatic;
            }
        }

        private readonly object _lock = new object();

        /// <summary><see cref="ILockable"/>'s lock object.</summary>
        public object Lock { get { return _lock; } }

        #endregion Locking


        #region IIdentifiable

        protected static int _idStatic = -1;

        protected static int GetNextId()
        {
            lock(LockStatic)
            {
                ++_idStatic;
                return _idStatic;
            }
        }

        private volatile int _id = GetNextId();

        /// <summary>ID of the current fading message object.</summary>
        public int Id
        {
            get {  return _id; } }

        private static volatile int _numLaunched = 0;

        private static volatile int _numShown = 0;

        /// <summary>Gets the total number of launched fading messages.</summary>
        public static int NumLaunched
        { get { return _numLaunched; } }

        /// <summary>Gets the total number of fading messages that aree currently active (displayed).</summary>
        public static int NumShown
        { get { return _numShown; } }

        /// <summary>Increments the total number of launched fading messages.</summary>
        protected void IncrementNumLaunched()
        { lock(LockStatic) { ++_numLaunched; } }

        /// <summary>Increments the number of currently active (displayed) fading messages.</summary>
        protected void IncrementNumShown()
        { lock(LockStatic) { ++_numShown; } }

        /// <summary>Decrements the number of currently active (displayed) fading messages.</summary>
        protected void DecrementNumShown()
        { lock(LockStatic) { --_numShown; } }

        private static int _outputLevel = 0;

        /// <summary>Level of output printed to console (for debugging).</summary>
        protected static int OutputLevel { get { return _outputLevel; } set { _outputLevel = value; }  }


        #endregion

        #region Behavior


        private bool _isTopMostWindow = true;

        /// <summary>Whether or not the launched fading message is a topmost window.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsTopMostWindow
        {
            get { return _isTopMostWindow; }
            set {
                _isTopMostWindow = value;
                this.TopMost = value;
            }
        }

        private bool _launchesAtMouseCursor = false;

        /// <summary>Sets position of the message window on the screen.
        /// <para>If position arguments are less than 0 then mouse cursor position is taken.</para></summary>
        /// <param name="left">Distance of window from the left edge of the screen.
        /// <para>If less than 0, it is set at mouse cursor horizontal position.</para></param>
        /// <param name="top">Distance of window from the top edge of the screen.
        /// <para>If less than 0, it is set at mouse cursor vertical position.</para></param>
        public void SetScreenPosition(int left = -1, int top = -1)
        {
            try
            {
                Point mousePos = Cursor.Position;
                if (left < 0)
                    left = mousePos.X;
                if (top < 0)
                    top = mousePos.Y;
                    this.Left = left;
                    this.Top = top;
            }
            catch(Exception ex)
            {
                if (OutputLevel >= 1)
                    Console.WriteLine(Environment.NewLine + Environment.NewLine
                        + "ERROR: Could not set fading message screen position. " + Environment.NewLine
                        + "  Details: " + ex.Message + Environment.NewLine);
            }
        }

        /// <summary>Whether the message should be launched at the position of the mouse cursor.
        /// <para>If set when the message is displayed, the form moves to the position of mouse cursor.</para></summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool LaunchedAtMouseCursor
        {
            get { return _launchesAtMouseCursor; }
            set {
                _launchesAtMouseCursor = true;
                if (value == true && WasLaunched && ! IsFormClosedOrClosing)
                {
                    SetScreenPosition(-1, -1);
                }
            }
        }

        #endregion Behavior


        #region Data

        public static readonly Color DefaultForeGroundColorTitle = Color.Blue;

        public static readonly Color DefaultForeGroundColorMessage = Color.Black;

        public static readonly Color DefaultBackGroundColor = Color.LightYellow;

        public static readonly Color DefaultInfoBackgroundColor = DefaultBackGroundColor;

        public static readonly Color DefaultWarningBackgroundColor = Color.Orange;

        public static readonly Color DefaultErrorBackgroundColor = Color.Red;

        public static readonly Color DefaultFadeColor = Color.DarkGray;

        // public static readonly color 

        /// <summary>Color of the title (if shown in fading message).</summary>
        public Color ForeGroundColorTitle = DefaultForeGroundColorTitle;

        /// <summary>Color of the message text shown in the fading message.</summary>
        public Color ForeGroundColorMsg = Color.Black;

        /// <summary>Active background color of the fading message.</summary>
        public Color BackGroundColor = DefaultBackGroundColor;

        /// <summary>Final (faded) color of the fading message.</summary>
        public Color FadeColor = DefaultFadeColor;

        public string MsgTitle = null, MsgText = null;

        public bool IsReady = false; /// Returns true if control has been created and is ready to work with.

        protected bool IsClosable = true;  /// If false then the consform can not be closed or dispoded.

        public static int MaxShown = 50; /// Indicates how many messages can be shown simultaneously.

        public static bool BlockMaxShown = false;  /// Indicates to block launching new messges when too many are processed.

        public bool BlockMaxShownCurrent = false; /// Instructs just for the current index to block launching the messge when too many are processed.

        //[Obsolete("This should be removed in the future (not needed because there is a common point when teh number of shown messagesis increased or decreased).")]
        //private bool Counted = false;  // true indicates that Numshown has been incremented for this instance
                                       // but not yet decremented 

        // Default properties:
        private static int defaultShowtime = 3000;
        private static double defaultFadingTimePortion = 0.3;

        // Properties:
        private int _showTime = defaultShowtime;
        private double _fadingTimePortion = defaultFadingTimePortion;

        /// <summary>Total time, in milliseconds, for which the message is displayed.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int ShowTime
        {
            set { if (value < 500) value = 500; _showTime = value; }
            get { return _showTime; }
        }

        /// <summary>Portion of display time in which the message background color fades at the end of 
        /// its display time.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public double FadingTimePortion
        {
            set { if (value < 0) value = 0; if (value > 1) value = 1; _fadingTimePortion = value; }
            get { return _fadingTimePortion; }
        }


        #endregion Data




        #region Launching


        private Thread _formthread = null, _manipulationthread = null;

        /// <summary>A parallel thread that takes care of launching the thread that manages the form.
        /// This thread also takes care that the form is disposed after the prescribed time elapses.</summary>
        private void ManipulationThreadFunc()
        {
            int WasShown=NumShown, SkipLimit=(int)((double) MaxShown*1.2);
            if (NumShown > SkipLimit)
            {
                // A limit is exceeded when we start considering skipping some messages:
                Thread.Sleep(200); // give some time to improve the situation

                if (OutputLevel >=2)
                {
                    Console.WriteLine(Environment.NewLine + "FM " + this.Id + " too many open: " + NumShown 
                        + ", waiting with launch... ");
                }

                if (NumShown >= WasShown && 
                            NumShown > (int) (double) SkipLimit*1.1)  // additional chance to recover
                {
                    // The number of open forms has not been decreased, therefore we skip showing the current message:
                    // Undo increment of the number of launched forms because this one is not launched:
                    // if ( this.Counted ) { this.Counted=false; }

                    if (OutputLevel >= 2)
                    {
                        Console.WriteLine(Environment.NewLine + "FM " + this.Id + " skipped, too many open: " + NumShown 
                            + ", limit: " + MaxShown + Environment.NewLine);
                    }
                    return;
                }
            }
            // Perform some waiting if the number of launching forms exceeds the maximum allowed number:
            int count=0, waitinterval = 200, maxwaittime = 2000;
            while (NumShown >= MaxShown && count * waitinterval <= maxwaittime)
            {
                if (OutputLevel >=2)
                {
                    Console.WriteLine(Environment.NewLine + "FM " + this.Id + " postponing launch, too many open: " + NumShown 
                        + ", limit: " + MaxShown + "...");
                }
                Thread.Sleep(waitinterval); // Take care that not too many forms are shown in parallel threads
            }
            try
            {
                _formthread = new Thread(new ThreadStart(LauncherNewThread));
                _formthread.IsBackground = true;
                _formthread.Start();
                Thread.Sleep((int) RemainingTimeSpan.TotalMilliseconds);

                if (OutputLevel >= 2)
                {
                    Console.WriteLine(Environment.NewLine + "FM " + this.Id + " after show interval expired,  " 
                        + " remaining time: " + RemainingTimeSpan.TotalSeconds + " s.");
                }

                // Account for the possibility that total display time has changed in between:
                while (RemainingTimeSpan.TotalMilliseconds > 2.0)
                {
                    Thread.Sleep((int)RemainingTimeSpan.TotalMilliseconds);
                    if (OutputLevel >= 2)
                    {
                        Console.WriteLine(Environment.NewLine + "FM " + this.Id + " additional remaining time: " 
                            + RemainingTimeSpan.TotalSeconds + " s.");
                    }
                }
                this.CloseForm();
                Thread.Sleep(100);
                // formthread.Abort();
                _formthread.Join();
            }
            catch (Exception e) { Exception ee = e; }
        }


        private bool _wasLaunched = false;

        /// <summary>Specifies whether the current fading message has already been launched.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool WasLaunched
        {
            get { return _wasLaunched; }
            protected set { _wasLaunched = value; }
        }

        private bool _launchedInParallelThread = false;

        /// <summary>Specifies whether the current fading message has been launched in a parallel thread.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool LaunchedInParallelThread
        {
            get { return _launchedInParallelThread; }
            set { _launchedInParallelThread = value; }
        }



        /// <summary>Launches the fading messsage as specified by parameters.</summary>
        /// <param name="inParallelThread">If true then fading message is launched in a parallel thread.
        /// Otherwise, it is launched in the current thread.</param>
        /// <returns>The current fading message form that has been launched.</returns>
        public virtual FadingMessage Launch(bool inParallelThread)
        {
            if (WasLaunched)
            {
                throw new InvalidOperationException("The current fading message has already been launched.");
            }
            else
            {
                WasLaunched = true;
                if (inParallelThread)
                {
                    this.LaunchInNewThread();
                }
                else
                {
                    this.LauncherCurrentThread();
                }
            }
            return this;
        }


        /// <summary>Shows a fading message in a new thread.
        /// <para>It first launches a new thread that launches the thread managing the form.
        /// In this way, the calling thread returns quickly and robust manipulation of the consform can be done.</para></summary>
        public void LaunchInNewThread()
        {
            if (BlockMaxShown || this.BlockMaxShownCurrent)
            {
                // If specified, the calling thread blocks until the number of launched messages gets down to the allowed number.
                while (NumShown >= MaxShown)
                    Thread.Sleep(20);
            }
            else
            {

                // In the launcing thread, perform some waiting if the number of launching forms heavily exceeds the maximum allowed number:
                int count = 0, waitinterval = 10, maxwaittime = 200;
                while (NumShown >= 2 * MaxShown && count * waitinterval <= maxwaittime)
                {
                    Thread.Sleep(waitinterval); // Take care that not too many threads are open
                    if (OutputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "FM " + this.Id + ": sleep " + waitinterval + " ms in 'LaunchInNewThread()'.");
                    }
                }
                while (NumShown >= 3 * MaxShown && count * waitinterval <= 1000)
                {
                    if (OutputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "FM " + this.Id + ": sleep " + waitinterval + " ms in 'LaunchInNewThread()'.");
                    }
                    Thread.Sleep(waitinterval);
                }
            }
            // if (!this.Counted) { this.Counted = true; }  // add this instance to the counter.
            _manipulationthread = new Thread(new ThreadStart(ManipulationThreadFunc));
            _manipulationthread.IsBackground = true;
            _manipulationthread.Start();
        }

        

        /// <summary>Launches the current fading message in a parallel thread.</summary>
        [STAThread]
        protected void LauncherNewThread()
        {
             LauncherMethod(true /* launchInNewThread */);
        }

        /// <summary>Launches the current fading message in the current thread, which should be a GUI thread
        /// (with a message loop running).</summary>
        protected void LauncherCurrentThread()
        {
            LauncherMethod(false /* launchedInNewThread */);
        }


        /// <summary>This method is used to launch the fading message, either in a parallel thread to the caller thread
        /// or on the caller thread. Parameter specified in which way the message is launched.</summary>
        /// <param name="launchedInNewThread">If true then the message is launched in a new parallel thread, which means
        /// that the message loop must be run by calling <see cref="ShowDialog()"/> method.</param>
        protected virtual void LauncherMethod(bool launchedInNewThread)
        /// Launches the message.
        {
            this.LaunchedInParallelThread = launchedInNewThread;
            this.WasLaunched = true;
            this.StartTime = DateTime.Now;
            try
            {
                timer1.Enabled = false;
                if (LaunchedAtMouseCursor)
                    SetScreenPosition(-1, -1);
                // Set up timer parameters:
                FirstTickInterval = (int)((1.0 - FadingTimePortion) * (double)ShowTime);
                // Set the total number of ticks such that fading is smooth but there are not too much timer intervals:
                TickInterval = MinTickinterval;
                TotalTicks = (int)(FadingTimePortion * (double)ShowTime / (double)TickInterval);
                if (TotalTicks > MaxTicks)
                {
                    TotalTicks = MaxTicks;
                    TickInterval = (int)(FadingTimePortion * (double)ShowTime / (double)TotalTicks);
                }
                timer1.Interval = FirstTickInterval;
            }
            catch (Exception) {
                if (OutputLevel >= 1)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR: Exception was thrown in FadingMessage.FormThreadFunc()."
                        + Environment.NewLine);
                }
            }
            IncrementNumLaunched();
            IncrementNumShown();
            try
            {
                if (OutputLevel >= 2)
                {
                    Console.WriteLine(Environment.NewLine + "FM " + this.Id + " LAUNCHED, timer enabled. "
                        + " rem. time: " + RemainingTimeSpan.TotalSeconds + " s.");
                }

                timer1.Enabled = true;
                if (launchedInNewThread)
                {
                    this.ShowDialog();
                } else
                {
                    this.Show();
                }
            }
            catch (Exception) {
                if (OutputLevel >= 1)
                {
                    string message = Environment.NewLine + "ERROR: Exception was thrown when launching fading message. " + Environment.NewLine;
                    if (launchedInNewThread)
                        message += "  Message was launched in a new thread." + Environment.NewLine;
                    else
                        message += "  Message was launched in the current thread." + Environment.NewLine;
                    message += "Original message of the fading message (which failed to be displayed): " + Environment.NewLine
                        + "  " + MsgText + Environment.NewLine;
                    Console.WriteLine(message);
                }
            }
        }


        #endregion Launching


        #region TimeHandling


        private DateTime _startTime;

        /// <summary>Time when the fading message was launched.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public DateTime StartTime
        { get { return _startTime; } protected set { _startTime = value; } }

        /// <summary>Gets the amount of time during which the message has been displayed until now.
        /// <para>Zero time span is returned if the message has not yett been displayed.</para></summary>
        protected TimeSpan ElapsedTimeSpan {
            get { if (!WasLaunched)
                    return TimeSpan.Zero;
                else
                    return DateTime.Now - StartTime;
            } }

        /// <summary>Gets or sets the total time span for which the message is to be displayed.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TimeSpan ShowTimeSpan { get { return new TimeSpan(0, 0, 0, 0, ShowTime); }
             set { ShowTime = (int)(1000 * value.TotalSeconds); } }

        /// <summary>Gets the remaining time span for which the message will be displayed.
        /// <para>Can be negative.</para></summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public TimeSpan RemainingTimeSpan { get { return ShowTimeSpan - ElapsedTimeSpan; }
            protected set { ShowTimeSpan = ShowTimeSpan + (value - RemainingTimeSpan); }
        }

        // Timer controled fade/out:
        private int _minTickInterval = 40;

        /// <summary>Minimal tick interval in milliseconds.</summary>
        int MinTickinterval { get { return _minTickInterval; } set { _minTickInterval = value; } }

        private int _maxTicks = 50;  // parameters to enable smooth fading

        /// <summary>Maximal number of timer ticks before the message fades out
        /// (this refers to the second part of the message).</summary>
        int MaxTicks { get { return _maxTicks; } set { _maxTicks = value; } }

        private int _numTicks = 0; 

        /// <summary>Number of timer ticks that occurred after the fading message has been launched.
        /// <para>This onlly counts ticks in the fading portion of display time.</para></summary>
        int NumTicks { get { return _numTicks; } set { _numTicks = value; } }

        private int _totalTicks = 20;

        /// <summary>Total number of timerr ticks in the fading portion of the message display time.</summary>
        int TotalTicks { get { return _totalTicks; } set { _totalTicks = value; } }

        private int _firstTick = 1000;

        /// <summary>Length of the first timer tick interval in milliseconds.
        /// <pra>The first tick marks start of the fading portion of the display time. After the first
        /// tick, tick interval is reduced to enable smooth color transition.</pra></summary>
        int FirstTickInterval { get { return _firstTick; } set { _firstTick = value; } }

        private int _tickInterval = 100;

        /// <summary>Current tick interval in milliseconds.</summary>
        int TickInterval { get { return _tickInterval; } set { _tickInterval = value; } }

        private double _fadingFactor = 0;

        /// <summary>Current fading factor - extent to which the active bacground color has changed to the faded color.</summary>
        protected double FadingFactor { get { return _fadingFactor; } set { _fadingFactor = value; } }


        /// <summary>Timer event, handles color change, closing after display time is passed, etc.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ++NumTicks;
                if (OutputLevel >= 2 && NumTicks <= 2)
                {
                    Console.WriteLine(Environment.NewLine + "FM " + this.Id + " tick No. "
                        + NumTicks + ", prev. interval: " + timer1.Interval);
                }
                timer1.Interval = TickInterval; // change time interval after the first tick
                FadingFactor = (double)(NumTicks - 1) / ((double)TotalTicks);
                if (FadingFactor >= 1.0)
                {
                    if (RemainingTimeSpan.TotalMilliseconds < 10)
                    {
                        if (OutputLevel >= 2 && RemainingTimeSpan > TimeSpan.Zero)
                        {
                            Console.WriteLine(Environment.NewLine  + "FM "+ this.Id + " timer tries to close, FadingFactor = " 
                                + FadingFactor + ", rem. time: " + RemainingTimeSpan.TotalSeconds + " s.");
                        }
                        if (RemainingTimeSpan <= TimeSpan.Zero)
                        {
                            if (OutputLevel >= 2)
                            {
                                Console.WriteLine(Environment.NewLine  + "FM "+ this.Id + " timer CLOSES message, FadingFactor = " 
                                    + FadingFactor + ", rem. time: " + RemainingTimeSpan.TotalSeconds + " s.");
                            }
                            this.Hide();
                            timer1.Enabled = false;
                            if (!IsFormClosedOrClosing)
                            {
                                CloseForm();
                            }
                        }
                    }
                }
                // Report fading progress (uncomment this only for testing):
                // StatusLbl.Text = "Fading: " + numticks.ToString() + "/" + totalticks.ToString();
                if (FadingFactor <= 1)
                    SetFadeLevel(FadingFactor);
            }
            catch { }
        }

        #endregion TimeHandling



        #region Closing


        volatile bool _isFormClosing = false;

        volatile bool _isFormClosed = false;
        
        /// <summary>Gets a flag indicating whether the fading message is currently being closed.</summary>
        public bool IsFormClosing
        { get { return _isFormClosing; } }

        /// <summary>Gets aa flag indicating whether the form has been closed or is currently being closed.</summary>
        public bool IsFormClosedOrClosing
        { get { return _isFormClosing || _isFormClosed; } }


        /// <summary>Closes the fading message control by properly (i.e. thread-safe) calling the Close() and Dispose().</summary>
        public void CloseForm()
        {
            lock (Lock)
            {
                if (!(_isFormClosing || _isFormClosed))
                {
                    _isFormClosing = true;
                    DecrementNumShown();

                    if (OutputLevel >= 2)
                    {
                        Console.WriteLine(Environment.NewLine +
                            "FM " + this.Id + ": CLOSING. Remaining display time: " 
                            + RemainingTimeSpan.TotalSeconds + " s." + Environment.NewLine
                            + "                 Num. shown: " + NumShown + " / " + NumLaunched + Environment.NewLine);
                    }

                    try
                    {
                        if (this.InvokeRequired)
                        {
                            // Delegate the method when called consform a thread not owning the consform.
                            //this.Invoke(new MethodInvoker(CloseForm));
                            //this.Invoke(new MethodInvoker(() => CloseForm()) );

                            this.Invoke((MethodInvoker) delegate { CloseForm(); });

                        }
                        else
                        {
                            this.IsReady = false;
                            // Stop the timer first:
                            if (timer1 != null)
                                timer1.Enabled = false;
                            // Call appropriate system methods to close the consform:
                            this.Close();
                            if (!this.IsDisposed)
                                this.Dispose();
                        }
                    }
                    catch (Exception e) { Exception ee = e; }
                    finally
                    {
                        _isFormClosing = false;
                        _isFormClosed = true;
                    }
                } else
                {
                    if (OutputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine +
                            "FM " + this.Id + ": attempt to close when already closing.");
                    }
                }
            }
        }


        #endregion Closing




        #region UserInteraction



        int m_PrevX;
        int m_PrevY;

        /// <summary>Enables dragging by mouse.</summary>
        private void FadeMessage_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            m_PrevX = e.X;
            m_PrevY = e.Y;
        }

        /// <summary>Enables dragging by mouse.</summary>
        private void FadeMessage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            Left = Left + (e.X - m_PrevX);
            Top = Top + (e.Y - m_PrevY);
        }




        /// <summary>Sets control's background accordig to the fading factor.</summary>
        /// <param name="fadefactor"></param>
        private void SetFadeLevel(double fadefactor)
        {
            Color bkcolor;
            double R, G, B;
            if (fadefactor < 0.0)
                fadefactor = 0.0;
            R = (1.0 - fadefactor) * (double)BackGroundColor.R + fadefactor * FadeColor.R;
            G = (1.0 - fadefactor) * (double)BackGroundColor.G + fadefactor * FadeColor.G;
            B = (1.0 - fadefactor) * (double)BackGroundColor.B + fadefactor * FadeColor.B;
            if (R < 0) R = 0; if (R > 255) R = 255;
            if (G < 0) G = 0; if (G > 255) G = 255;
            if (B < 0) B = 0; if (B > 255) B = 255;
            bkcolor = Color.FromArgb((int)R, (int)G, (int)B);
            this.BackGroundColor = bkcolor;
            UtilForms.SetBackColorRec(this, bkcolor);
        }


        /// <summary>This method is used to set  the common event handlers for the curren fading message control and its subcontrols.</summary>
        /// <param name="f">Control on which common event handlers are set.</param>
        void SetCommonEvents(Control f)
        {
            {
                try
                {
                    // This will enable to kill the windowby clicking the mouse button 3 
                    // while pressing Ctrl, launcging context menu, etc.
                    f.MouseClick += new MouseEventHandler(this.FadeMessage_MouseClick);
                }
                catch (Exception e) { Exception ee = e; }
                try
                {
                    // This will enable to kill the window by clicking the mouse button 3
                    f.KeyPress += new KeyPressEventHandler(this.FadeMessage_KeyPress);
                }
                catch (Exception e) { Exception ee = e; }
                try
                {
                    // This will allow dragging the window by mouse:
                    f.MouseDown += new MouseEventHandler(this.FadeMessage_MouseDown);
                    f.MouseMove += new MouseEventHandler(this.FadeMessage_MouseMove);
                }
                catch (Exception e) { Exception ee = e; }
            }
        }


		private void FadeMessage_Load(object sender, System.EventArgs e)
		{
            //if (!this.Counted) {  this.Counted = true;  }
            try
            {
                // Set common event handlers recursively:
                UtilForms.RecursiveControlDelegate(this, new ControlDelegate(SetCommonEvents));

                // Set up timer parameters:
                FirstTickInterval = (int)((1.0 - FadingTimePortion) * (double)ShowTime);
                // Set the total number of ticks such that fading is smooth but there are not too much timer intervals:
                TickInterval = MinTickinterval;
                TotalTicks = (int)(FadingTimePortion * (double)ShowTime / (double)TickInterval);
                if (TotalTicks > MaxTicks)
                {
                    TotalTicks = MaxTicks;
                    TickInterval = (int)(FadingTimePortion * (double)ShowTime / (double)TotalTicks);
                }
                timer1.Interval = FirstTickInterval;
                timer1.Enabled = true;
            }
            catch (Exception ex) {
                Console.WriteLine(Environment.NewLine + "ERROR: exception thrown in fading message's Load event handler. " + Environment.NewLine
                    + "  Details: " + ex.Message + Environment.NewLine 
                    + "  Original message: " + this.MsgText + Environment.NewLine);
            }
            finally
            {
                try
                {
                    lblTitle.ForeColor = ForeGroundColorTitle;
                    lblMEssage.ForeColor = ForeGroundColorMsg;
                    if (MsgTitle == null)
                        MsgTitle = "";
                    lblTitle.Text = MsgTitle;
                    if (MsgTitle.Length < 1)
                    {
                        lblTitle.Visible = false;
                        // lblMEssage.Location.Y = 4;
                    }

                    this.lblMEssage.Text = MsgText;

                    UtilForms.SetBackColorRec(this, BackGroundColor);
                    //if (0 != 1)
                    //{
                    //    this.StatusLbl.Text = "< Right-Click to close >";
                    //    // report fading parameters in the status line (uncomment this only for testing)
                    //    //this.StatusLbl.Text = "firsttick = " + firsttick.ToString() + "  tickinterval = " + tickinterval
                    //    //    + "  totalticks = " + totalticks.ToString();
                    //}
                    // this.ShowDialog();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR: exception thrown in fading message's Load event handler. " + Environment.NewLine
                        + "  Details: " + ex.Message + Environment.NewLine
                        + "  Original message: " + this.MsgText + Environment.NewLine);
                }
                finally
                {
                    this.IsReady = true;
                    try
                    {
                        lblStatus.Visible = false;  // do not show the status label
                        btnCancel.Visible = false;
                        // PnlStatus.Visible = false;

                        // Ensure proper size of controls:
                        lblMEssage.Top = lblTitle.Location.Y;
                        if (lblTitle.Text != null && lblTitle.Visible == true)
                            lblMEssage.Top = lblTitle.Location.Y + lblTitle.Size.Height;
                        // Place the distance label such that its bottom edge will be at the same position as the bottom edge of
                        // the bottom Panel:
                        labelBottom.Text = "";
                        if (PnlStatus.Visible)
                            labelBottom.Top = BodyPnl.Location.Y + BodyPnl.Size.Height + PnlStatus.Size.Height - labelBottom.Size.Height;
                        else // otherwise we do not need a distance keeping label.
                            labelBottom.Top = BodyPnl.Top;

                        //PnlStatus.Top = BodyPnl.Location.Y + BodyPnl.Size.Height;

                        //PnlStatus.Right = 0;
                        //PnlStatus.Size.Width = BodyPnl.Size.Width ;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Environment.NewLine + "ERROR: exception thrown in fading message's Load event handler. " + Environment.NewLine
                            + "  Details: " + ex.Message + Environment.NewLine
                            + "  Original message: " + this.MsgText + Environment.NewLine);
                    }
                }
            }
		}



        private void FadeMessage_Disposed(object sender, System.EventArgs e)
        {
            // if (this.Counted) { this.Counted = false;  }
            this.IsReady = false;
        }

        private void FadeMessage_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (OutputLevel >= 2)
            {
                Console.WriteLine(Environment.NewLine + "FM " + this.Id + ": FormClosing event."
                    + " remainning show time: " + RemainingTimeSpan.TotalSeconds + " s." 
                    + Environment.NewLine);
            }

            // If the consform is tagged as non-closable, then cancel the closing of the consform:
            if (!this.IsClosable)
            {
                e.Cancel = true;
            }
            else
            {
                this.IsReady = false;
            }
        }





        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CloseForm();
        }


        /// <summary>Handles mouse click events for the current fading message control.</summary>
        private void FadeMessage_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    bool ControlPressed = false;
                    try
                    {
                        Control SenderControl = sender as Control;
                        if (SenderControl != null)
                        {
                            if (Control.ModifierKeys == Keys.Control)
                                ControlPressed = true;
                        }
                    }
                    catch { }
                    if (ControlPressed)
                        CloseForm();
                    else
                        menuMain.Show(Cursor.Position);
                }
            }
            catch { }
        }

        /// <summary>Handles mouse click events for the current fading message control.</summary>
        private void FadeMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ControlPressed = false;
                try
                {
                    Control SenderControl = sender as Control;
                    if (SenderControl != null)
                    {
                        if (Control.ModifierKeys == Keys.Control)
                            ControlPressed = true;
                    }
                }
                catch { }

                if (ControlPressed && e.KeyChar == 'c' || e.KeyChar == 'C')
                {
                    CopyMessageToClipboard();
                }
            }
            catch { }
        }





        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /// <summary>Copies message text to clipboard.</summary>
        private void menuCopyMessage_Click(object sender, EventArgs e)
        {
            CopyMessageToClipboard();
        }

        public void CopyMessageToClipboard()
        {

            FadingMessage fm = new FadingMessage("Copying Message...", "Message text is being copied to clipboard.", 1500, false);
            fm.Launch(false /* inParallelThread */);
            

            Clipboard.SetText(lblMEssage.Text);
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private double _minRemainingSecMenu = 8.0;

        /// <summary>Minimal remaining show time, in seconds, when user launches a context menu.
        /// <para>If in that case the remaining display time of the message is lesser than the minimum
        /// time, the display time is extented accordingly.</para></summary>
        protected double MinimalRemainingSecondsWithMenu
        { get { return _minRemainingSecMenu; } set { _minRemainingSecMenu = value; } }

        /// <summary>Executed when the context menue is opening.
        /// <para>If necessary, display time of the fading message is extended,
        /// such that the user has time to select items from the menu.</para></summary>
        private void menuMain_Opening(object sender, CancelEventArgs e)
        {
            // If the contxt menu is opened, take care that the remaiining show time is
            // increased, if necessary (to at least match the specified minimal time):

            if (OutputLevel >= 2)
            {
                Console.WriteLine(Environment.NewLine +
                    "Message menu launched, remaining display time: " + RemainingTimeSpan.TotalSeconds + " s.");
            }

            if (RemainingTimeSpan.TotalSeconds <= MinimalRemainingSecondsWithMenu)
            {
                RemainingTimeSpan = new TimeSpan(0, 0, 0, 0,
                    (int)(MinimalRemainingSecondsWithMenu * (double)1000));

                if (OutputLevel >= 3)
                {
                    Console.WriteLine("  Remaining display time increased to " + RemainingTimeSpan.TotalSeconds + " s." + Environment.NewLine);
                }

            }
            else
                Console.WriteLine("  Remaining display time was NOT increased." + Environment.NewLine);

        }

        private void menuIdentifyThread_Click(object sender, EventArgs e)
        {
            // UtilForms.IdentifyCurrentThread(4000);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            string title = "Fading Message " + this.Id + ", Thread " + threadId;

            string msg = UtilForms.GetCurrentThreadInfo() + Environment.NewLine;

            if (!this.WasLaunched)
                msg += "Message HAS NOT BEEN LAUNCHED!." + Environment.NewLine + Environment.NewLine;

            if (this.LaunchedInParallelThread)
                msg += "The fading message was launched in a NEW THREAD." + Environment.NewLine + Environment.NewLine;
            else
                msg += "Message was launched in the calling thread." + Environment.NewLine + Environment.NewLine;

            msg += "Remaining time span: " + RemainingTimeSpan.ToString() + Environment.NewLine + Environment.NewLine;

            FadingMessage message = new FadingMessage(title, msg, 4000, 0.3, false);
            message.Launch(true /* inParallelThread */);

        }

        private void menuLaunchSameThread_Click(object sender, EventArgs e)
        {
            FadingMessage msg = new FadingMessage("Test Message (same thread)", "Launched from another fading message.",
                20000, 0.3, false /* launchImmediately */);
            msg.Launch(false /* parallelThread */);
        }

        private void menuLaunchMessageParallel_Click(object sender, EventArgs e)
        {
            FadingMessage msg = new FadingMessage("Test Message", "Launched from another fading message.",
                20000, 0.3);
        }


        /// <summary>Launches a demonstrative example message.</summary>
        private void menuExample_Click(object sender, EventArgs e)
        {
            FadingMessage.Example();
        }

        /// <summary>Runs a massive launch demo.</summary>
        private void menuExampleMassive_Click(object sender, EventArgs e)
        {
            FadingMessage.ExampleLargeNum();
        }


        #endregion UserInteraction

        


        #region Examples

        /// <summary>Launches a sample fading message, with some instructions of how to use it.</summary>
        public static void Example()
        {
            //Creating a fading message in this thread, which must be canceled explicitly (e.g. by pressing mouse button 3:)
            string msgtitle = "Sample Fading Message with instructions";
            string msgtext = "Message text.\nLine 2.\nLine 3.\nLine 4.\nLine 5.\n\n" 
                + "Right-click this message to launch context menu!\n\n"
                + "Drag with mouse to move around!\n\n"
                + "Ctrl-right-click to close!\n";
            FadingMessage fm = new FadingMessage(msgtitle, msgtext, 8000, false);
            fm.Launch(true /* parallelThread */);
        }  // Example1()


        /// <summary>Launches a larger number of fading messages to check that the system is capable of 
        /// correct display.</summary>
        public static void ExampleLargeNum(double maxDurationInSeconds = 5.0, double launchIntervalInSeconds = 0.05,
            bool launchInCurrentThreadWhenPossible = true)
        {
            string info = "Masive message launch demo will start in a second..." + Environment.NewLine + Environment.NewLine
                + "Move the mouse cursor around to see the effect!" + Environment.NewLine + Environment.NewLine
                + "";
            FadingMessage fmInfo = new FadingMessage("  Instructions: ", info, 4000, false);
            fmInfo.BackGroundColor = Color.Red;
            fmInfo.IsTopMostWindow = true;
            fmInfo.Launch(true /* parallelThread */);
            //Thread.Sleep(1000);
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            DateTime demoLaunchTime = DateTime.Now;
            TimeSpan maxTimeSpan = new TimeSpan(0, 0, 0, 0 /* seconds */, 
                (int)(maxDurationInSeconds * (double) 1000) /* milliseconds */);
            bool inCurrentThread = false;
            if (launchInCurrentThreadWhenPossible
                && UtilForms.IsMainGuiThread())
                inCurrentThread = true;
            int which = 0;
            t.Tick += new EventHandler((sender, eventArgs) => 
            {
                ++which;
                FadingMessage message = new FadingMessage("Message No. " + which,
                    "This message belongs to the massive message launch example." 
                    + Environment.NewLine + Environment.NewLine
                    + "Move mouse pointer around to observe the effect!", 
                    3000, 0.3, false /* launch immediately */);
                message.IsTopMostWindow = false;
                message.Launch(!inCurrentThread /* launchInParallelThread */);
                message.LaunchedAtMouseCursor = true;
                if (DateTime.Now - demoLaunchTime > maxTimeSpan)
                {
                    FadingMessage fmEnd = new FadingMessage("Demo finished", 
                        "This demo is finished, messages will not be launched any more.", 3000, 0.3);
                    fmEnd.IsTopMostWindow = true;
                    fmEnd.BackGroundColor = Color.Green;
                    t.Enabled = false;
                }
            });  // event handler
            t.Interval = (int)(launchIntervalInSeconds * (double)1000);
            // new FadingMessage("Sleeping for 2 seconds...", 2000).BackGroundColor = Color.LightBlue;
            t.Enabled = true;
            Thread.Sleep(2000);
        }




        /// <summary>A less relevant example.</summary>
        public static void ExampleInferior()
        {
            // Creating a fading message in this thread, which must be canceled explicitly (e.g. by pressing mouse button 3:)
            FadingMessage fm = new FadingMessage();
            fm.MsgTitle = "Test message";
            fm.MsgText = "My label.";
            fm.ShowDialog();

            // Fading message tests:
            new FadingMessage("Fading message", "Fade message, 9 s  this is a very long fade message in order to see how the window size is adapted\r\nline2\n\rline 3\n\rline4\r\nline5\r\n\r\n\r\n\r\n\r\n\r\nBottom line",
                9000);
            new FadingMessage("Fade message, 4 s", 4000);
            new FadingMessage("Fade message, 1 s", 1000);


            // Test launching many fading messages:
            int averagetime = 5000, deviation = 2000;
            Random rnd = new Random(0);
            for (int i = 1; i <= 800; ++i)
            {
                int showtime = averagetime + rnd.Next(deviation);
                float f = (float)showtime / (float)1000;
                new FadingMessage("Fading message " + i.ToString() + ", " + f.ToString() + " s",
                        averagetime + rnd.Next(deviation),
                        0.5);
            }

        } // FadeMessageExample2()


        #endregion Examples



	}  // class FadeMessage : System.Windows.Forms.Form

}  // namespace IG.Forms
