using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets
{
    public abstract class Widget
    {
        protected readonly int _left;
        protected readonly int _top;
        protected readonly int _width;
        protected readonly int _height;
        protected ConsoleColor _backgroundColor;
        protected ConsoleColor _foregroundColor;

        public Action<Widget, EventArgs> OnChanged = (w, a) => { };


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
            set { _backgroundColor = value; OnChanged(this,new EventArgs()); }
        }

        public ConsoleColor ForegroundColor { 
            get => _foregroundColor;
            set { _foregroundColor = value; OnChanged(this,new EventArgs()); }
        }
        
        
        public abstract void HandleKey(ConsoleKeyInfo key);
    }
}