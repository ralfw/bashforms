using System;
using bashforms;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace bashforms_tests
{
    public class Program
    {
        public static void Main(string[] args) {
            var frm = new Form(0, 0, Console.WindowWidth, Console.WindowHeight) {Title = "Main Form"};
            frm.AddChild(new TextArea(1,1,20,20)
            {
                Text = 
                    @"12345678901234567890
She should have died hereafter;
There would have been a time for such a word.

— To-morrow, and to-morrow, and to-morrow,
Creeps in this petty pace from day to day,
To the last syllable of recorded time;
And all our yesterdays have lighted fools
The way to dusty death.

Out, out, brief candle! Life's but a walking shadow, a poor player
That struts and frets his hour upon the stage
And then is heard no more. It is a tale
Told by an idiot, full of sound and fury
Signifying nothing."
            });
            frm.AddChild(new TextArea(25, 1, 10, 5){Label = "Short Text"});
            
            BashForms.Open(frm);
        }
    }
}