using System;
using bashforms.widgets.controls;
using bashforms.widgets.windows;

namespace bashforms
{
    public static class MessageBox {
        public static void ShowInfo(string message) {
            var box = new Dialog<bool>(3, 3, Console.WindowWidth / 3, 5) {Title = "Info"};
            box.AddChild(new Label(2,1,box.Size.width-4){Text=message});
            box.AddChild(new Button(2,3,4,"OK") { OnPressed = (b,a) => { BashForms.Close(); }});
            BashForms.OpenModal(box);
        }
    }
}