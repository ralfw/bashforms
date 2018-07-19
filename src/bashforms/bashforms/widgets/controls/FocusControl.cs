using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public abstract class FocusControl : Control
    {
        private bool _hasFocus;
        protected ConsoleColor _focusBackgroundColor;
        protected ConsoleColor _focusForegroundColor;

        public FocusControl(int left, int top, int width, int height) : base(left, top, width, height) {
            this.TabIndex = 0;

            _focusBackgroundColor = Console.BackgroundColor;
            _focusForegroundColor = Console.ForegroundColor;
        }
        
        public int TabIndex { get; set; }
        public bool CanHaveFocus => this.TabIndex >= 0;

        
        public ConsoleColor FocusBackgroundColor {
            get => _focusBackgroundColor;
            set { _focusBackgroundColor = value; OnUpdated(this,new EventArgs()); }
        }
        public ConsoleColor FocusForegroundColor { 
            get => _focusForegroundColor;
            set { _focusForegroundColor = value; OnUpdated(this,new EventArgs()); }
        }

        
        public bool HasFocus {
            get => _hasFocus;
            set {
                if (value && !this.CanHaveFocus) throw new InvalidOperationException("Control cannot accept focus!");
                _hasFocus = value;
            }
        }
    }
}