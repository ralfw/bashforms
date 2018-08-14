using System;
using System.Linq;
using bashforms.widgets.controls;
using bashforms.widgets.controls.formatting;

namespace bashforms.widgets.windows
{
    public static class MessageBox {
        public static void ShowInfo(string message)
        {
            var lblMessage = Create_message_label();
            var dlgBox = Create_dialog(lblMessage);
            BashForms.OpenModal(dlgBox);


            Label Create_message_label() {
                var MAX_LINE_LEN = (int)(Console.WindowWidth * 0.8);
            
                var lines = message.ToLines();
                var lenOfLongestLine = lines.Max(l => l.Length);

                var lblWidth = lenOfLongestLine <= MAX_LINE_LEN ? lenOfLongestLine : MAX_LINE_LEN;
                return new Label(2, 1, lblWidth) {Text = message, CanBeMultiline = true};
            }

            Dialog<bool> Create_dialog(Widget lbl) {
                var boxWidth = lbl.Size.width + 2 * 2;
                var boxHeight = lbl.Size.height + 2 + 2;

                var boxLeft = (Console.WindowWidth - boxWidth) / 2;
                var boxTop = (Console.WindowHeight - boxHeight) / 2;
            
                var dlg = new Dialog<bool>(boxLeft,boxTop,boxWidth,boxHeight) {Title = "Info"};
                dlg.AddChild(lblMessage);

                const string btnText = "OK";
                var btnWidth = btnText.Length + 2;
                var btnLeft = (dlg.Size.width - 4) / 2;
                var btnTop = lbl.Position.top + lbl.Size.height + 1;
                dlg.AddChild(new Button(btnLeft,btnTop,btnWidth,btnText) { OnPressed = (b,a) => { BashForms.Close(); }});
                return dlg;
            }
        }
        
        public static bool ShowQuestion(string question) {
            var box = new Dialog<bool>(3, 3, Console.WindowWidth / 3, 5) {Title = "Question"};
            box.AddChild(new Label(2,1,box.Size.width-4){Text=question});
            box.AddChild(new Button(2,3,5,"Yes") { OnPressed = (b,a) => { box.Result = true; BashForms.Close(); }});
            box.AddChild(new Button(8,3,4,"No") { OnPressed = (b,a) => { box.Result = false; BashForms.Close(); }});
            return BashForms.OpenModal(box);
        }
    }
}