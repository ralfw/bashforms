using System;
using bashforms;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace spike
{
    public class Program
    {
        public static void Main(string[] args) {
            var bf = new BashForms();
            
            var dlg = new Form(10,10,20,8){Title = "Message Box"};
            dlg.AddChild(new Label(2,2,10){Text = "Hello!"});
            dlg.AddChild(new Button(2,4,10, "OK"){ OnPressed = (w, a) => { bf.Pop(); }});
            
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Main Form"};
            frm.AddChild(new Label(2,2,10){Text = "Name"});
            frm.AddChild(new TextLine(14,2,10));
            frm.AddChild(new Label(2,4,10){Text = "Alter"});
            frm.AddChild(new TextLine(14,4,4));
            frm.AddChild(new Button(2, 6, 10, "Show...") {OnPressed = (w, a) => {
                bf.Push(dlg);
            }});
            
            bf.Run(frm);
        }
    }
}