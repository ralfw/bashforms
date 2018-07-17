﻿using System;

namespace spike
{
    public class Label : Widget
    {
        private string _text;
        
        public Label(int left, int top, int width) : base(left, top, width, 1) {
            _text = "";
        }

        public string Text {
            get => _text;
            set {
                _text = value;
                OnChanged(this, new EventArgs());
            }
        }

        public override void HandleKey(ConsoleKeyInfo key){ }
    }
}