using System;
using System.Collections.Generic;
using System.Linq;

namespace bashforms.engine
{
    public class BashFormsEngine
    {
        private readonly Stack<widgets.windows.Window> _windowStack = new Stack<widgets.windows.Window>();
        private readonly Rendering _renderer;

        public BashFormsEngine() {
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
                return key.Key == ConsoleKey.R && (key.Modifiers & ConsoleModifiers.Control) > 0;
            }
        }
    }
}