using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    partial class TextArea {
        public override bool HandleKey(ConsoleKeyInfo key)
        {
            if (Handle_insertion(key)) return true;
            return false;
        }
        
        bool Handle_insertion(ConsoleKeyInfo key) {
            if (key.KeyChar < ' ') return false;
            if (this.Length == _maxTextLength) return true;

            var index = _text.GetIndex(_insertionPoint.softRow, _insertionPoint.softCol);
            index = _text.Insert(index.row, index.index, key.KeyChar.ToString());
            _insertionPoint = _text.GetSoftPosition(index.row, index.index);

            OnEdited(this,new EventArgs());
            return true;
        }
    }
}