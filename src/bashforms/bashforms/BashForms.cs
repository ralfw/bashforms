using System.Runtime.CompilerServices;
using bashforms.engine;
using bashforms.widgets.windows;
using bashforms.widgets.windows.baseclasses;

namespace bashforms
{
    public static class BashForms
    {
        private static BashFormsEngine __engine;
        
        public static void Open(Window window) {
            if (__engine == null) {
                __engine = new BashFormsEngine();
                __engine.Run(window);
            }
            else
                __engine.Push(window);
        }

        
        public static void OpenModal(Window dialog) {
            if (__engine == null) __engine = new BashFormsEngine();
            __engine.Push(dialog);
            __engine.RunModal();
        }
        
        public static TResult OpenModal<TResult>(Dialog<TResult> dialog) {
            if (__engine == null) __engine = new BashFormsEngine();
            __engine.Push(dialog);
            __engine.RunModal();
            return dialog.Result;
        }

        
        public static void Close() {
            __engine.Pop();
        }


        public static BashFormsEngine Current => __engine;
    }
}