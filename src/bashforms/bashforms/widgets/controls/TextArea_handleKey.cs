using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    partial class TextArea {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            return true;
        }
    }
}