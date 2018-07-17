using System;
using System.Collections.Generic;
using System.Linq;

namespace spike
{
    class BashForms
    {
        private readonly Stack<Window> _windowStack = new Stack<Window>();
        private readonly Rendering _renderer;

        public BashForms() {
            _renderer = new Rendering();   
        }
        
        
        public void Push(Window win) {
            _windowStack.Push(win);
        }
        public Window Pop() {
            return _windowStack.Pop();
        }
        
        
        public void Run(Window win) {
            Push(win);
            Run();
        }
        public void Run() {
            _windowStack.Peek().InitializeFocus();
            while (_windowStack.Count > 0) {
                _renderer.Render(new[]{_windowStack.Peek()});
                var key = Console.ReadKey(true);
                _windowStack.Peek().HandleKey(key);
            }
        }
    }
}