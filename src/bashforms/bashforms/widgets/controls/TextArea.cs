using System;
using System.Linq;
using bashforms.widgets.controls.editors;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    public partial class TextArea : CursorControl
    {
        protected (int row, int index) _insertionPoint;
        protected int _displayFromSoftRow;
        protected TextEditor _text;
        protected int _maxTextLength;
        protected string _label;
        protected ConsoleColor _labelForegroundColor;


        public Action<Widget, EventArgs> OnEdited = (s, a) => { };
        
        
        public TextArea(int left, int top, int width, int height) : base(left, top, width, height) {
            _text = new TextEditor("", width);
            _label = "";
            _insertionPoint = (0,0);
            _displayFromSoftRow = 0;
            _maxTextLength = 1024 * 1024;
            _focusBackgroundColor = ConsoleColor.Blue;
            _focusForegroundColor = ConsoleColor.White;
            _labelForegroundColor = ConsoleColor.DarkGray;
        }

        
        public string Text {
            get => _text.Text.Replace("\n", Environment.NewLine);
            set {
                _text = new TextEditor(value.Replace("\r", ""), _width);
                _insertionPoint = (0, 0);
                OnUpdated(this, new EventArgs());
            }
        }

        
        public int Length => _text.SoftLines.Sum(sl => sl.Length);
        
        
        public int MaxTextLength {
            get => _maxTextLength;
            set { 
                _maxTextLength = value;
                if (this.Length > _maxTextLength) {
                    _text = new TextEditor(_text.Text.Substring(0, _maxTextLength), _width);
                    _insertionPoint = (0, 0);
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


        public override (int x, int y) CursorPosition {
            get {
                var position = _text.GetSoftPosition(_insertionPoint.row, _insertionPoint.index);
                return (position.softCol, position.softRow);
            }
        }
    }
}