﻿using System;
using bashforms;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace spike
{
    public class Program
    {
        public static void Main(string[] args) {
            var bf = new BashForms();
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Main Form"};
            
            frm.AddChild(new Label(2,2,10){Text = "Name"});
            frm.AddChild(new TextLine(14,2,10));
            frm.AddChild(new Label(2,4,10){Text = "Alter"});
            frm.AddChild(new TextLine(14,4,4));
            frm.AddChild(new Button(2, 6, 10, "OK") {OnPressed = (w,a) => { Console.Beep(); }});
            
            bf.Run(frm);
        }
    }
}