using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.widgets.windows;

namespace bashforms.engine
{
    public class BashFormsEngine
    {
        private readonly Stack<Window> _windowStack = new Stack<Window>();
        private readonly Rendering _renderer;

        public BashFormsEngine() {
            _renderer = new Rendering();   
        }
        
        
        public void Push(Window win) {
            _windowStack.Push(win);
            win.InitializeFocus();
        }
        
        public void Pop() {
            _windowStack.Pop();
        }


        public int Depth => _windowStack.Count;
        
        
        public void Run(Window win) { Push(win); Run(); }
        public void Run() => Run(1);
        public void RunModal() => Run(_windowStack.Count);

        private void Run(int minimalWindowsStackDepth) {
            _renderer.Render(_windowStack.Reverse().ToArray());
            
            while (_windowStack.Count >= minimalWindowsStackDepth) {
                var key = Console.ReadKey(true);
                if (_windowStack.Peek().HandleKey(key) || HandleKey(key))
                    _renderer.Render(_windowStack.Reverse().ToArray());
            }

            
            bool HandleKey(ConsoleKeyInfo key) {
                return (key.Modifiers & ConsoleModifiers.Control) > 0 && key.Key == ConsoleKey.R;
            }
        }
    }
}