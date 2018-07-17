using System;

namespace bashforms.widgets.controls
{
    public class Button : FocusControl
    {
        private string _text;

        public Action<Widget, EventArgs> OnPressed;
        
        
        public Button(int left, int top, int width, string text) : base(left, top, width, 1) {
            _text = "";
            _focusBackgroundColor = ConsoleColor.DarkMagenta;
            _focusForegroundColor = ConsoleColor.White;
        }
        
        
        public string Text {
            get => _text;
            set {
                _text = value;
                OnChanged(this, new data.eventargs.EventArgs());
            }
        }

        
        public override void HandleKey(ConsoleKeyInfo key) {
            OnPressed(this, new EventArgs());
        }
    }
}