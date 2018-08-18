using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using bashforms.data.eventargs;
using bashforms.widgets.controls;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.windows.dialogs
{
    /*
     * Dialog does selection management itself.
     * It renders the entries with "+" and "√" itself.
     * Directories carry info about opened items.
     * Closing dirs is difficult.
     */
    public class FilesystemDialog : Dialog<string[]>
    {
        private class FilesystemListbox : Listbox {
            public FilesystemListbox(int left, int top, int width, int height, IEnumerable<string> itemTexts) : base(left, top, width, height, itemTexts) {}
            public FilesystemListbox(int left, int top, int width, int height) : base(left, top, width, height) {}

            public override bool HandleKey(ConsoleKeyInfo key) {
                switch (key.Key) {
                    case ConsoleKey.RightArrow:
                        this.OnExpandRequested(this.CurrentItemIndex);
                        return true;
                    case ConsoleKey.LeftArrow:
                        this.OnCollapseRequested(this.CurrentItemIndex);
                        return true;
                    default:
                        return base.HandleKey(key);
                }
            }

            public event Action<int> OnExpandRequested;
            public event Action<int> OnCollapseRequested;
        }
        
        
        private string _path;
        private readonly FilesystemListbox _lbFilesystem;
        
        
        public FilesystemDialog(string path = "", string title = "") : base(0, 0, 1, 1) {
            _width = Console.WindowWidth / 2;
            _height = (int)(Console.WindowHeight * 0.8);
            _top = (Console.WindowHeight - _height) / 2;
            _left = (Console.WindowWidth - _width) / 2;

            _title = title == "" ? "Select file or folder" : title;

            _lbFilesystem = new FilesystemListbox(2, 2, _width - 4, _height - 4){SelectionMode = Listbox.SelectionModes.SingleSelection};
            _lbFilesystem.OnExpandRequested += Handle_expand_request;
            _lbFilesystem.OnCollapseRequested += Handle_collapse_request;
            this.AddChild(_lbFilesystem);

            const int BUTTON_WIDTH = 10;
            var totalButtonWidth = 2 * BUTTON_WIDTH + 2;
            var buttonLeft = (_width - totalButtonWidth) / 2;

            this.AddChild(new Button(buttonLeft, _height - 2, BUTTON_WIDTH, "Select") {
                OnPressed = (s, e) => {
                    base.Result = _lbFilesystem.SelectedItemIndexes.Select(i => (string)_lbFilesystem.Items[i].Attachment)
                                                                   .ToArray();
                    BashForms.Close();
                }
            });
            this.AddChild(new Button(buttonLeft + BUTTON_WIDTH + 2, _height - 2, BUTTON_WIDTH, "Cancel") {
                OnPressed = (s, e) => {
                    base.Result = new string[0];
                    BashForms.Close();
                }
            });

            if (path != "") Fill(path);
        }


        public string Path
        {
            get => _path;
            set {
                Fill(value);
                this.OnChanged(this,new EventArgs());
            }
        }

        
        public Listbox.SelectionModes SelectionMode {
            get => _lbFilesystem.SelectionMode;
            set {
                _lbFilesystem.SelectionMode = value; 
                this.OnUpdated(this, new EventArgs());
            }
        }


        private void Handle_expand_request(int listboxItemIndex)
        {
            var currentIndentationLevel = Get_indendation_level(_lbFilesystem.Items[listboxItemIndex].Text);
            Expand((string)_lbFilesystem.Items[listboxItemIndex].Attachment, listboxItemIndex+1, currentIndentationLevel+1);
        }

        private void Handle_collapse_request(int listboxItemIndex) {
            var currentIndentationLevel = Get_indendation_level(_lbFilesystem.Items[listboxItemIndex].Text);
            Collapse(listboxItemIndex+1, currentIndentationLevel+1);
        }

        
        private void Fill(string path) {
            _path = path;
            _lbFilesystem.Clear();
            Expand(_path, 0, 0);
        }

        private void Expand(string path, int insertAtIndex, int indentationLevel) {
            if (!Directory.Exists(path)) return;

            var indendation = "".PadLeft(indentationLevel * 2, ' ');
            
            // display subfolders
            var folderpaths = Directory.GetDirectories(path);
            foreach (var folderpath in folderpaths) {
                _lbFilesystem.Insert(insertAtIndex++, new Listbox.Item($"{indendation}>{System.IO.Path.GetFileName(folderpath)}"){Attachment = folderpath});
            }

            // display files
            var filepaths = Directory.GetFiles(path);
            foreach (var filepath in filepaths) {
                _lbFilesystem.Insert(insertAtIndex++, new Listbox.Item($"{indendation} {System.IO.Path.GetFileName(filepath)}"){Attachment = filepath});
            }
        }

        private void Collapse(int removeAtIndex, int indentationLevelToBeRemoved) {
            while (removeAtIndex < _lbFilesystem.Items.Length)
            {
                if (Get_indendation_level(_lbFilesystem.Items[removeAtIndex].Text) < indentationLevelToBeRemoved) break;
                _lbFilesystem.RemoveAt(removeAtIndex);
            }
        }

        private int Get_indendation_level(string text) => (text.Length - text.TrimStart().Length) / 2;
    }
}