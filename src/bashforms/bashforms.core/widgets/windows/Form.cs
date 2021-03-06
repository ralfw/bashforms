﻿using bashforms.widgets.windows.baseclasses;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.windows
{
    public partial class Form : Window
    {
        protected string _title;
        private bool _hasBorder;

        public Form(int left, int top, int width, int height) : base(left,top,width,height) {
            _title = "";
            _hasBorder = true;
        }
        
        public string Title {
            get => _title;
            set { _title = value; OnChanged(this,new EventArgs()); }
        }
        
        public bool HasBorder {
            get => _hasBorder;
            set { _hasBorder = value; OnChanged(this,new EventArgs()); }
        }
    }
}