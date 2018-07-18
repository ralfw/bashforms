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
            frm.AddChild(new Label(2,2,10){Text = "Name"});
            frm.AddChild(new TextLine(14,2,5){MaxTextLength = 15, Name = "Name" });
            frm.AddChild(new TextLine(14,4,5){Label = "Alter", MaxTextLength = 3});
            frm.AddChild(new Button(2, 6, 10, "Show...") {OnPressed = (w, a) => {
                var answer = MessageBox.ShowQuestion($"You name: {((TextLine)frm["Name"]).Text}?");
                MessageBox.ShowInfo($"The answer: {answer}");
            }});
            
            BashForms.Open(frm);
        }
    }
}