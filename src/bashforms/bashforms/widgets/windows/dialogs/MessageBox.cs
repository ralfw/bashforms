using System;
using System.Linq;
using System.Reflection;
using bashforms.widgets.controls;
using bashforms.widgets.controls.formatting;

namespace bashforms.widgets.windows
{


    public static class MessageBox {
        public enum Results {
            None,
            Ok,
            Cancel,
            Yes,
            No,
            Continue,
            Ignore
        }
        
        
        public static void ShowInfo(string message, string title = "")
            => Show(message, title == "" ? "Info" : title);
        public static bool AskForYes(string message, string title = "")
            => Show(message, (Results.No, "No"), (Results.Yes, "Yes"), title) == Results.Yes;
        public static bool AskForOk(string message, string title = "")
            => Show(message, (Results.Cancel, "Cancel"), (Results.Ok, "Ok"), title) == Results.Ok;
        
        public static void Show(string message, string title = "") {
            Show(message, (Results.Ok, "OK"), title);
        }
        public static Results Show(
            string message,
            (Results result, string text) option,
            string title="")
        {
            return Show(message, option, (Results.None, ""), title);
        }
        public static Results Show(
            string message,
            (Results result, string text) option0,
            (Results result, string text) option1,
            string title="")
        {
            return Show(message, option0, option1, (Results.None, ""), title);
        }

        public static Results Show(
                string message,
                (Results result, string text) option0,
                (Results result, string text) option1,
                (Results result, string text) option2,
                string title="")
        {
            var lblMessage = Create_message_label(message);
            var dlgBox = Create_dialog(lblMessage, title);
            Add_option_buttons(dlgBox, Aggregated_options());
            return BashForms.OpenModal(dlgBox);


            (Results result, string text)[] Aggregated_options() {
                var options = new[] {option0, option1, option2}.Where(o => o.result != Results.None).ToArray();
                if (options.Length == 0)
                    options = new[] {(Results.Ok, "OK")};
                return options;
            }
        }
        

        
        
        private static Label Create_message_label(string message) {
            var MAX_LINE_LEN = (int)(Console.WindowWidth * 0.8);
            
            var lines = message.ToLines();
            var lenOfLongestLine = lines.Max(l => l.Length);

            var lblWidth = lenOfLongestLine <= MAX_LINE_LEN ? lenOfLongestLine : MAX_LINE_LEN;
            return new Label(2, 1, lblWidth) {Text = message, CanBeMultiline = true, Name = "lblMessage"};
        }


        private static Dialog<Results> Create_dialog(Label lbl, string title) {
            var boxWidth = lbl.Size.width + 2 * 2;
            var boxHeight = lbl.Size.height + 2 + 2;

            var boxLeft = (Console.WindowWidth - boxWidth) / 2;
            var boxTop = (Console.WindowHeight - boxHeight) / 2;
            
            var dlg = new Dialog<Results>(boxLeft,boxTop,boxWidth,boxHeight) {Title = title};
            dlg.AddChild(lbl);
            return dlg;
        }


        private static void Add_option_buttons(Dialog<Results> dlg, (Results result, string text)[] options) {
            var totalOptionsWidth = options.Sum(o => o.text.Length + 2) + (options.Length - 1);

            if (totalOptionsWidth > dlg.Size.width - 2) {
                var delta = totalOptionsWidth - dlg.Size.width + 4;
                dlg.Resize(dlg.Size.width + delta, dlg.Size.height);
                dlg.MoveTo(dlg.Position.left - delta / 2, dlg.Position.top);

                var lblMessage = dlg["lblMessage"];
                lblMessage.MoveTo((dlg.Size.width - lblMessage.Size.width)/2, lblMessage.Position.top);
            } 
            
            var btnLeft = (dlg.Size.width - totalOptionsWidth) / 2;
            var btnTop = dlg.Size.height - 2;

            foreach (var opt in options) {
                var btn = new Button(btnLeft, btnTop, opt.text.Length + 2, opt.text) {
                    OnPressed = (b, a) => {
                        dlg.Result = opt.result;
                        BashForms.Close();
                    }
                };
                dlg.AddChild(btn);
                btnLeft += btn.Size.width + 1;
            }
        }
    }
}