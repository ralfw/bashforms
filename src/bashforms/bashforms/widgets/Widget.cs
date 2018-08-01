using System;
using bashforms.data;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets
{
    public abstract class Widget
    {
        protected int _left;
        protected int _top;
        protected int _width;
        protected int _height;
        protected ConsoleColor _backgroundColor;
        protected ConsoleColor _foregroundColor;

        public Action<Widget, EventArgs> OnUpdated = (w, a) => { };


        public Widget(int left, int top, int width, int height) {
            _left = left;
            _top = top;
            _width = width;
            _height = height;

            _backgroundColor = Console.BackgroundColor;
            _foregroundColor = Console.ForegroundColor;
        } 
       
        public (int left, int top) Position => (_left, _top);
        public (int width, int height) Size => (_width, _height);
        
        public string Name { get; set; }
        
        public ConsoleColor BackgroundColor {
            get => _backgroundColor;
            set { _backgroundColor = value; OnUpdated(this,new EventArgs()); }
        }

        public ConsoleColor ForegroundColor { 
            get => _foregroundColor;
            set { _foregroundColor = value; OnUpdated(this,new EventArgs()); }
        }
        
        
        public object Attachment { get; set; }
        
        public abstract bool HandleKey(ConsoleKeyInfo key);

        public abstract Canvas Draw();
    }
}