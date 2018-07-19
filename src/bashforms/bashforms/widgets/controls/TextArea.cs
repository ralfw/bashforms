using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public partial class TextArea : CursorControl
    {
        protected int _insertionPoint;
        protected int _displayFromIndex;
        protected string _text;
        protected int _maxTextLength;
        protected string _label;
        protected ConsoleColor _labelForegroundColor;


        public Action<Widget, EventArgs> OnEdited = (s, a) => { };
        
        
        public TextArea(int left, int top, int width, int height) : base(left, top, width, height) {
            _text = "";
            _label = "";
            _insertionPoint = 0;
            _displayFromIndex = 0;
            _maxTextLength = width;
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
            _labelForegroundColor = ConsoleColor.DarkGray;
        }

        
        public string Text {
            get => _text.Replace("\n", Environment.NewLine);
            set { 
                _text = value.Replace("\r", "");
                _insertionPoint = _text.Length;
                OnUpdated(this, new EventArgs());
            }
        }
        
        
        public int MaxTextLength {
            get => _maxTextLength;
            set { 
                _maxTextLength = value;
                if (_text.Length > _maxTextLength) {
                    _text = _text.Substring(0, _maxTextLength);
                    _insertionPoint = Math.Min(_insertionPoint, _text.Length);
                }
                OnUpdated(this, new EventArgs());
            }
        }

        
        public string Label {
            get => _label;
            set {
                _label = value;
                OnUpdated(this, new EventArgs());
            }
        }
        
        
        public ConsoleColor LabelForegroundColor {
            get => _labelForegroundColor;
            set { _labelForegroundColor = value; OnUpdated(this,new EventArgs()); }
        }

        
        public override (int x, int y) CursorPosition => (0, 0);
    }
}