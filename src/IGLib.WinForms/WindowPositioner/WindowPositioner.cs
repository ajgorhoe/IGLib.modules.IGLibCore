#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using IG.Lib;
using IG.Num;

namespace IG.Forms
{


    /// <summary>Takes care of windows positioning, relative of absolute to the screen and/or other windows.</summary>
    /// $A Igor xx;
    public class WindowPositioner
    {

        
        /// <summary>Constructs a new window positioner where the positioned and the master window are not yet defined.</summary>
        public WindowPositioner(): this(null, null)
        { }

       /// <summary>Constructs a new window positioner with the specified window to be positioned and unspecified 
        /// master window.</summary>
        /// <param name="positionedWindow">Window to be positioned. Can be null.</param>
        /// <param name="masterWindow">Master window with respect to which the window is also positioned. Can be null.</param>
        public WindowPositioner(Form positionedWindow): this(positionedWindow, null)
        {  }

        /// <summary>Constructs a new window positioner with the specified window to be positioned and the 
        /// specified master window.</summary>
        /// <param name="positionedWindow">Window to be positioned. Can be null.</param>
        /// <param name="masterWindow">Master window with respect to which the window is also positioned. Can be null.</param>
        public WindowPositioner(Form positionedWindow, Form masterWindow)
        {
            if (positionedWindow != null)
                this.Window = positionedWindow;
            if (masterWindow != null)
                this.MasterWindow = masterWindow;
        }


        #region ILockable

        private object _lock = new object();

        public object Lock
        {
            get{ return _lock; }
        }

        #endregion ILockable


        #region Operation

        protected bool _positionWindowExecuted = false;

        /// <summary>Positions the positioned window according to the <see cref="CalculatedPosition"/></summary>
        public void PositionWindow()
        {
            lock (Lock)
            {
                _positionWindowExecuted = true;
                try
                {
                    Form win = Window;
                    if (win != null)
                    {
                        int x = (int)CalculatedPosition.x;
                        int y = (int)CalculatedPosition.y;
                        win.Location = new Point(x, y);
                    }
                }
                finally
                {
                    _positionWindowExecuted = false;
                }
            }
        }

        /// <summary>Shanges the shift of window position such that it corresponds to actual current position of the window.
        /// <para>This is usually used when user is allowed to adjust relative positinon to the master of a sticking window.</para></summary>
        public void SetToCurrentPosition()
        {
            lock (Lock)
            {
                if (Window != null)
                {
                    vec2 calculated = CalculatedPosition;
                    Point actual = Window.Location;
                    vec2 dif;
                    dif.x = actual.X-calculated.x;
                    dif.y = actual.Y-calculated.y;
                    ShiftAbsolute = ShiftAbsolute + dif;
                }
            }
        }


        protected DateTime _lastMoved = DateTime.Now;

        /// <summary>Minimal acceptable value for the pause  between consecutive movement events that are processed
        /// in implementation of sticking window and remembering relative position.</summary>
        public static double MinimalMovementPauseSeconds = 0.001;

        protected double _movementPauseSeconds = 0.01;

        /// <summary>Minimal pause, in seconds, between two successive movements of the window due to sticking.</summary>
        public double MovementPauseSeconds
        {
            get { lock (Lock) { return _movementPauseSeconds; } }
            set
            {
                lock (Lock)
                {
                    if (value < MinimalMovementPauseSeconds)
                        _movementPauseSeconds = MinimalMovementPauseSeconds;
                    else
                        _movementPauseSeconds = value;
                }
            }
        }


        protected bool _isStickedToMaster = false;

        /// <summary>Whether or not the position window is sticked to the master window such that it follows its movement.</summary>
        public bool IsStickedToMaster
        {
            get { lock (Lock) { return _isStickedToMaster; } }
            set { lock (Lock) { _isStickedToMaster = value; } }
        }

        protected bool _isRememberPosition = false;

        /// <summary>Whether or not relative position with respect to master is remembered when movement is not caused by 
        /// sticking to the master window.
        /// <para>Positions are only remembered if sticking to master is also performed.</para></summary>
        public bool IsRememberPosition
        {
            get { lock (Lock) { return _isRememberPosition; } }
            set { lock (Lock) { _isRememberPosition = value; } }
        }


        /// <summary>This event handler is added to the master window so that its position and size changes can be tracked.</summary>
        protected void StickEventHandler(object Sender, EventArgs args)
        {
            lock (Lock)
            {
                if (IsStickedToMaster && Window != null && MasterWindow != null
                    && !Window.IsDisposed && !MasterWindow.IsDisposed)
                {
                    try
                    {
                        if ((DateTime.Now - _lastMoved).TotalSeconds >= MovementPauseSeconds)
                        {
                            PositionWindow();
                            _lastMoved = DateTime.Now;
                        }
                    }
                    catch { }
                }
            }
        }


        /// <summary>This event handler is added to the positioned window so that its position and size changes can be remembered
        /// when not caused by sticking to the master.</summary>
        protected void RememberPositionEventHandler(object Sender, EventArgs args)
        {
            lock (Lock)
            {
                if (IsRememberPosition && IsStickedToMaster && !_positionWindowExecuted && Window != null && MasterWindow != null
                    && !Window.IsDisposed && !MasterWindow.IsDisposed)
                {
                    try
                    {
                        if ((DateTime.Now - _lastMoved).TotalSeconds >= MovementPauseSeconds)
                        {
                            SetToCurrentPosition();
                            _lastMoved = DateTime.Now;
                        }
                    }
                    catch { }
                }
            }
        }


        /// <summary>Sticks the positioned window to master window, if possible.</summary>
        public void StickToMaster()
        {
            lock (Lock)
            {
                IsStickedToMaster = true;
                Form win = Window;
                Form master = MasterWindow;
                if (win != null && master != null)
                {
                    master.SizeChanged += StickEventHandler;
                    master.Move += StickEventHandler;
                }
            }
        }



        /// <summary>Stichs the positioned window to master window (), if possible.</summary>
        public void UnStickFromMaster()
        {
            lock (Lock)
            {
                IsStickedToMaster = false;
                Form win = Window;
                Form master = MasterWindow;
                if (win != null && master != null)
                {
                    try
                    {
                        master.SizeChanged -= StickEventHandler;
                    }
                    catch { }
                    try
                    {
                        master.Move -= StickEventHandler;
                    }
                    catch { }
                }
            }
        }

        /// <summary>Starts remembering potitions relative to the master window when the position is 
        /// changed and this is not due to sticking to the master window.</summary>
        public void RememberPosition()
        {
            lock (Lock)
            {
                IsRememberPosition = true;
                Form win = Window;
                Form master = MasterWindow;
                if (win != null && master != null)
                {
                    win.SizeChanged += RememberPositionEventHandler;
                    win.Move += RememberPositionEventHandler;
                }
            }
        }


        /// <summary>Stops remembering potitions relative to the master window when the position is 
        /// changed and this is not due to sticking to the master window.</summary>
        public void StopRememberPosition()
        {
            lock (Lock)
            {
                IsRememberPosition = false;
                Form win = Window;
                Form master = MasterWindow;
                if (win != null && master != null)
                {
                    win.SizeChanged -= RememberPositionEventHandler;
                    win.Move -= RememberPositionEventHandler;
                }
            }
        }


        #endregion Operation

        #region PositionedWindowGeneral

        protected vec2 _shiftAbsolute = new vec2(0, 0);

        /// <summary>Additional shift of positioned window in pixels.
        /// <para>{0,0} means no shift, otherwise shift is in pixels and menas difference to otherwise calculated position.</para></summary>
        public virtual vec2 ShiftAbsolute
        {
            get { lock (Lock) { return _shiftAbsolute; } }
            set { lock (Lock) { _shiftAbsolute = value; } }
        }

        protected double _masterWeight = 1.0;


        /// <summary>Relative weight (importance) of positioning relative to master window.</summary>
        public virtual double MasterWeight
        {
            get
            {
                lock (Lock)
                {
                    if (_masterWindow == null)
                        return 0.0;
                    else
                        return _masterWeight;
                }
            }
            set
            {
                lock (Lock)
                {
                    if (value < 0)
                        throw new ArgumentException("Mastet window position weight must be between 0 and 1 inclusive. " + Environment.NewLine
                        + "  Attempted to set to " + value + ".");
                    _masterWeight = value;
                }
            }
        }

        /// <summary>Relative weight (importance) of positioning relative to screen window.</summary>
        public virtual double ScreenWeight
        {
            get { return (1.0 - MasterWeight); }
            set { MasterWeight = 1.0 - value; }
        }


        /// <summary>Calculated position of the top-left corner of the positioned window.</summary>
        public virtual vec2 CalculatedPosition
        {
            get
            {
                return ShiftAbsolute + WindowShift + MasterWeight * MasterWindowShift + ScreenWeight * ScreenShift;
            }
        }


        #endregion PositionedWindowGeneral


        #region PositionedWindow

        protected System.Windows.Forms.Form _window;

        /// <summary>Window to be positioned.</summary>
        public virtual System.Windows.Forms.Form Window
        {
            get { lock (Lock) { return _window; } }
            set
            {
                lock (Lock)
                {
                    if (value != _window)
                    {
                        _window = value;
                        if (IsStickedToMaster)
                            StickToMaster();
                        else
                            UnStickFromMaster();
                        if (IsRememberPosition)
                            RememberPosition();
                        else
                            StopRememberPosition();
                    }
                }
            }
        }

        protected Alignment _windowAlignment = new Alignment(AlignmentHorizontal.Left, AlignmentVertical.Top);

        /// <summary>Alignment of the positioned window with respect to point of positioning.</summary>
        public virtual Alignment WindowAlignment
        {
            get { lock (Lock) { return _windowAlignment; } }
            set { lock (Lock) { _windowAlignment = value; } }
        }



        protected vec2 _windowShiftRelative = new vec2(0, 0);

        /// <summary>Additional shift of positioned window relative to Window size.</summary>
        public virtual vec2 WindowShiftRelative
        {
            get { lock (Lock) { return _windowShiftRelative; } }
            set { lock (Lock) { _windowShiftRelative = value; } }
        }

        /// <summary>Bounds of the window to be positioned.</summary>
        public virtual Rectangle WindowBounds
        {
            get
            {
                lock (Lock)
                {
                    if (Window == null)
                        return new Rectangle(0, 0, 0, 0);
                    else
                    {
                        Size size = Window.Size;
                        Point position = Window.Location;
                        return new Rectangle(position.X, position.Y, size.Width, size.Height);
                    }
                }
            }
        }


        /// <summary>calculated additional shift of the positioned window that depends on that window.</summary>
        public virtual vec2 WindowShift
        {
            get
            {
                lock (Lock)
                {
                    vec2 shift = new vec2(0, 0);
                    Alignment alignment = WindowAlignment;
                    switch (alignment.Horizontal)
                    {
                        case AlignmentHorizontal.Right:
                            shift.x = -(int)(WindowBounds.Width);
                            break;
                        case AlignmentHorizontal.Centered:
                            shift.x = -(int)(0.5 * (WindowBounds.Width));
                            break;
                    }
                    switch (alignment.Vertical)
                    {
                        case AlignmentVertical.Bottom:
                            shift.y = -(int)(WindowBounds.Height);
                            break;
                        case AlignmentVertical.Middle:
                            shift.y = -(int)(0.5 * (WindowBounds.Height));
                            break;
                    }
                    shift.x += (int)((double)WindowBounds.Width * _windowShiftRelative.x);
                    shift.y += (int)((double)WindowBounds.Height * _windowShiftRelative.y);
                    return shift;
                }
            }
        }

        #endregion PositionedWindow;


        #region MasterWindow


        protected System.Windows.Forms.Form _masterWindow;

        /// <summary>Master window relative to which positioning is calculated.</summary>
        public virtual System.Windows.Forms.Form MasterWindow
        {
            get { lock (Lock) { return _masterWindow; } }
            set
            {
                lock (Lock)
                {
                    lock (Lock)
                    {
                        if (value != _masterWindow)
                        {
                            _masterWindow = value;
                            if (IsStickedToMaster)
                                StickToMaster();
                            else
                                UnStickFromMaster();
                            if (IsRememberPosition)
                                RememberPosition();
                            else
                                StopRememberPosition();
                        }
                    }
                }
            }
        }




        protected Alignment _masterWindowAlignment = new Alignment(AlignmentHorizontal.Left, AlignmentVertical.Top);

        /// <summary>Alignment of the positioned window with respect to point of positioning.</summary>
        public virtual Alignment MasterWindowAlignment
        {
            get { lock (Lock) { return _masterWindowAlignment; } }
            set { lock (Lock) { _masterWindowAlignment = value; } }
        }



        protected vec2 _masterWindowShiftRelative = new vec2(0, 0);

        /// <summary>Additional shift of positioned window relative to Window size.</summary>
        public vec2 MasterWindowShiftRelative
        {
            get { lock (Lock) { return _masterWindowShiftRelative; } }
            set { lock (Lock) { _masterWindowShiftRelative = value; } }
        }

        /// <summary>Bounds of the window to be positioned.</summary>
        public virtual Rectangle MasterWindowBounds
        {
            get
            {
                lock (Lock)
                {
                    if (MasterWindow == null)
                        return new Rectangle(0, 0, 0, 0);
                    else
                    {
                        Size size = MasterWindow.Size;
                        Point position = MasterWindow.Location;
                        return new Rectangle(position.X, position.Y, size.Width, size.Height);
                    }
                }
            }
        }


        /// <summary>calculated additional shift of the positioned window that depends on that window.</summary>
        public virtual vec2 MasterWindowShift
        {
            get
            {
                lock (Lock)
                {
                    if (MasterWindow == null)
                    {
                        MasterWeight = 0.0;
                        return new vec2(0.0, 0.0);
                    } else
                    {
                        Point location = MasterWindow.Location;
                        vec2 shift = new vec2(location.X, location.Y);
                        Alignment alignment = MasterWindowAlignment;
                        switch (alignment.Horizontal)
                        {
                            case AlignmentHorizontal.Right:
                                shift.x = (int)(MasterWindowBounds.Right);
                                break;
                            case AlignmentHorizontal.Centered:
                                shift.x = (int)(0.5 * (MasterWindowBounds.Left + MasterWindowBounds.Right));
                                break;
                        }
                        switch (alignment.Vertical)
                        {
                            case AlignmentVertical.Bottom:
                                shift.y = (int)(MasterWindowBounds.Bottom);
                                break;
                            case AlignmentVertical.Middle:
                                shift.y = (int)(0.5 * (MasterWindowBounds.Bottom + MasterWindowBounds.Top));
                                break;
                        }
                        shift.x += (int)((double)MasterWindowBounds.Width * _masterWindowShiftRelative.x);
                        shift.y += (int)((double)MasterWindowBounds.Height * _masterWindowShiftRelative.y);
                        return shift;
                    }
                }
            }
        }


        #endregion MasterWindow


        #region Screen

        protected bool _isScreenDataInitialized = false;

        /// <summary>Whwtheer screen data has been obtained or not.</summary>
        protected virtual bool IsScreenDataInitialized
        {
            get { lock (Lock) { return _isScreenDataInitialized; } }
            set { lock (Lock) { _isScreenDataInitialized = value; } }
        }

        protected Rectangle _screenBounds;

        /// <summary>Gets screen bounds.</summary>
        public Rectangle ScreenBounds
        {
            get
            {
                InitScreenData();
                return _screenBounds;
            }
        }

        /// <summary>Obtains the screen resolution etc. (if data hasen't been obtained yet), and sets the 
        /// <see cref="IsScreenDataInitialized"/> property to true.</summary>
        public virtual void InitScreenData()
        {
            lock (Lock)
            {
                if (!IsScreenDataInitialized)
                    InitScreenDataForced();
            }
        }

        /// <summary>Obtains the screen resolution etc., and sets the <see cref="IsScreenDataInitialized"/> property to true.</summary>
        public virtual void InitScreenDataForced()
        {
            lock (Lock)
            {
                if (Window != null)
                {
                    _screenBounds = Screen.FromControl(Window).Bounds;
                }
                else
                {
                    _screenBounds = Screen.PrimaryScreen.Bounds;
                }
                IsScreenDataInitialized = true;
            }
        }

        protected Alignment _screenAlignment = new Alignment(AlignmentHorizontal.Left, AlignmentVertical.Top);

        public Alignment ScreenAlignment
        {
            get { lock (Lock) { return _screenAlignment; } }
            set { lock (Lock) { _screenAlignment = value; } }
        }

        protected vec2 _screenShiftRelative = new vec2(0, 0);

        /// <summary>Additional shift of positioned window relative to screen size.</summary>
        public vec2 ScreenShiftRelative
        {
            get { lock (Lock) { return _screenShiftRelative; } }
            set { lock (Lock) { _screenShiftRelative = value; } }
        }


        /// <summary>calculated additional shift of the position window that is bound to screen.</summary>
        public vec2 ScreenShift
        {
            get
            {
                lock (Lock)
                {
                    vec2 shift = new vec2(0, 0);
                    InitScreenData();
                    Alignment alignment = ScreenAlignment;
                    switch (alignment.Horizontal)
                    {
                        case AlignmentHorizontal.Right:
                            shift.x = (int)(_screenBounds.Right);
                            break;
                        case AlignmentHorizontal.Centered:
                            shift.x = (int)(0.5 * (_screenBounds.Left + _screenBounds.Right));
                            break;
                    }
                    switch (alignment.Vertical)
                    {
                        case AlignmentVertical.Bottom:
                            shift.y = (int)(_screenBounds.Bottom);
                            break;
                        case AlignmentVertical.Middle:
                            shift.y = (int)(0.5 * (_screenBounds.Bottom + _screenBounds.Top));
                            break;
                    }
                    shift.x += (int)((double)_screenBounds.Width * _screenShiftRelative.x + _shiftAbsolute.x);
                    shift.y += (int)((double)_screenBounds.Height * _screenShiftRelative.y + _shiftAbsolute.y);
                    return shift;
                }
            }
        }





        #endregion Screen



    }

}
