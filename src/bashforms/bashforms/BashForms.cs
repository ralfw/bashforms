using System;
using System.Collections.Generic;
using System.Linq;
using bashforms.engine;
using bashforms.widgets.windows;

namespace bashforms
{
    public class BashForms
    {
        private readonly Stack<widgets.windows.Window> _windowStack = new Stack<widgets.windows.Window>();
        private readonly Rendering _renderer;

        public BashForms() {
            _renderer = new Rendering();   
        }
        
        
        public void Push(widgets.windows.Window win) {
            _windowStack.Push(win);
            win.InitializeFocus();
        }
        public widgets.windows.Window Pop() {
            return _windowStack.Pop();
        }
        
        
        public void Run(widgets.windows.Window win) {
            Push(win);
            Run();
        }
        public void Run() {
            while (_windowStack.Count > 0) {
                _renderer.Render(_windowStack.Reverse().ToArray());
                var key = Console.ReadKey(true);
                _windowStack.Peek().HandleKey(key);
            }
        }
    }
}