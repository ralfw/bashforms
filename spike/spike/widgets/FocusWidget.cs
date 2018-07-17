using System;

namespace spike
{
    public abstract class FocusWidget : Widget
    {
        private bool _hasFocus;
        private ConsoleColor _focusBackgroundColor;
        private ConsoleColor _focusForegroundColor;

        public FocusWidget(int left, int top, int width, int height) : base(left, top, width, height) {
            this.TabIndex = -1;

            _focusBackgroundColor = Console.BackgroundColor;
            _focusForegroundColor = Console.ForegroundColor;
        }
        
        public int TabIndex { get; set; }
        public bool CanHaveFocus => this.TabIndex >= 0;
        
        public ConsoleColor FocusBackgroundColor {
            get => _focusBackgroundColor;
            set { _focusBackgroundColor = value; OnChanged(this,new EventArgs()); }
        }

        public ConsoleColor FocusForegroundColor { 
            get => _focusForegroundColor;
            set { _focusForegroundColor = value; OnChanged(this,new EventArgs()); }
        }

        public bool HasFocus {
            get => _hasFocus;
            set {
                if (value && !this.CanHaveFocus) throw new InvalidOperationException("Widget cannot accept focus!");
                _hasFocus = value;
            }
        }
    }
}