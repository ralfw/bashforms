using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using bashforms.widgets.controls;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.windows.dialogs
{
    public class FilesystemDialog : Dialog<string[]>
    {
        private class FilesystemListbox : Listbox
        {
            public FilesystemListbox(int left, int top, int width, int height, IEnumerable<string> itemTexts) : base(
                left, top, width, height, itemTexts)
            {
            }

            public FilesystemListbox(int left, int top, int width, int height) : base(left, top, width, height)
            {
            }

            public override bool HandleKey(ConsoleKeyInfo key)
            {
                switch (key.Key)
                {
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


        private class FilesystemAttachment
        {
            public string Path;
            public bool IsDirectory;
            public bool IsExpanded;
        }


        private string _path;
        private readonly FilesystemListbox _lbFilesystem;
        private bool _listDirectoriesOnly;


        public FilesystemDialog(string path = "", string title = "") : base(0, 0, 1, 1)
        {
            _width = Console.WindowWidth / 2;
            _height = (int) (Console.WindowHeight * 0.8);
            _top = (Console.WindowHeight - _height) / 2;
            _left = (Console.WindowWidth - _width) / 2;

            _title = title == "" ? "Select file or folder" : title;
            _listDirectoriesOnly = false;

            this.AddChild(new Label(2, 2, "Path: ") {BackgroundColor = ConsoleColor.DarkGray});
            this.AddChild(
                new Label(8, 2, _width - 10, path) {Name = "lblPath", BackgroundColor = ConsoleColor.DarkGray});

            _lbFilesystem =
                new FilesystemListbox(2, 3, _width - 4, _height - 8)
                {
                    SelectionMode = Listbox.SelectionModes.SingleSelection
                };
            _lbFilesystem.OnExpandRequested += Handle_expand_request;
            _lbFilesystem.OnCollapseRequested += Handle_collapse_request;
            this.AddChild(_lbFilesystem);

            this.AddChild(new TextLine(2, _height - 4, _width - 4) { 
                Label = "file or folder name",
                Name = "txtFileOrFoldername",
                Visible = false
            });

            const int BUTTON_WIDTH = 10;
            var totalButtonWidth = 2 * BUTTON_WIDTH + 2;
            var buttonLeft = (_width - totalButtonWidth) / 2;

            this.AddChild(new Button(buttonLeft, _height - 2, BUTTON_WIDTH, "Select")
            {
                OnPressed = (s, e) => {
                    var selections = _lbFilesystem.SelectedItemIndexes.Select(i => ((FilesystemAttachment) _lbFilesystem.Items[i].Attachment).Path);
                    if (this.AllowNewFileOrFoldername) selections = selections.Concat(new[] {this.Child<TextLine>("txtFileOrFoldername").Text});
                    base.Result = selections.ToArray();
                    BashForms.Close();
                }
            });
            this.AddChild(new Button(buttonLeft + BUTTON_WIDTH + 2, _height - 2, BUTTON_WIDTH, "Cancel")
            {
                OnPressed = (s, e) =>
                {
                    base.Result = new string[0];
                    BashForms.Close();
                }
            });

            if (path != "") Fill(path);
        }


        public string Path
        {
            get => _path;
            set
            {
                Fill(value);
                this.Child<Label>("lblPath").Text = value;
                this.OnChanged(this, new EventArgs());
            }
        }


        public bool ListDirectoriesOnly
        {
            get => _listDirectoriesOnly;
            set
            {
                _listDirectoriesOnly = value;
                Fill(_path);
                this.OnChanged(this, new EventArgs());
            }
        }

        public bool AllowNewFileOrFoldername {
            get => this.Child<TextLine>("txtFileOrFoldername").Visible;
            set {
                this.Child<TextLine>("txtFileOrFoldername").Visible = value;
                OnUpdated(this, new EventArgs());
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
            var attachment = (FilesystemAttachment) _lbFilesystem.Items[listboxItemIndex].Attachment;
            if (!attachment.IsDirectory) return;
            if (attachment.IsExpanded) return;
            
            var currentIndentationLevel = Get_indendation_level(_lbFilesystem.Items[listboxItemIndex].Text);
            var nItemsAdded = Expand(attachment.Path, listboxItemIndex+1, currentIndentationLevel+1);
            Adjust_selections_after_expansion(listboxItemIndex + 1, nItemsAdded);
            attachment.IsExpanded = true;
            
            
            void Adjust_selections_after_expansion(int indexesAffectedFrom, int numberOfChanges) {
                var currentSelections = _lbFilesystem.SelectedItemIndexes;
                _lbFilesystem.ClearSelections();
                foreach (var s in currentSelections) {
                    if (s >= indexesAffectedFrom)
                        _lbFilesystem.AddSelection(s+numberOfChanges);
                    else
                        _lbFilesystem.AddSelection(s);
                }
            }
        }

        
        private void Handle_collapse_request(int listboxItemIndex) {
            var attachment = (FilesystemAttachment) _lbFilesystem.Items[listboxItemIndex].Attachment;
            if (!attachment.IsDirectory) return;
            if (!attachment.IsExpanded) return;
            
            var currentIndentationLevel = Get_indendation_level(_lbFilesystem.Items[listboxItemIndex].Text);
            var nItemsRemoved = Collapse(listboxItemIndex+1, currentIndentationLevel+1);
            Adjust_selections_after_collapse(listboxItemIndex + 1, -nItemsRemoved);
            attachment.IsExpanded = false;
            
            
            void Adjust_selections_after_collapse(int indexesAffectedFrom, int numberOfChanges) {
                var currentSelections = _lbFilesystem.SelectedItemIndexes;
                _lbFilesystem.ClearSelections();
            
                var lastCollapsedIndex = indexesAffectedFrom + numberOfChanges - 1;
                foreach (var s in currentSelections) {
                    if (s > lastCollapsedIndex)
                        _lbFilesystem.AddSelection(s+numberOfChanges);
                    else if (s < indexesAffectedFrom)
                        _lbFilesystem.AddSelection(s);
                }
            }
        }


        private void Fill(string path) {
            _path = path;
            _lbFilesystem.Clear();
            Expand(_path, 0, 0);
        }


        private bool IsDirectory(int index) => ((FilesystemAttachment) _lbFilesystem.Items[index].Attachment).IsDirectory;
        
        private int Get_indendation_level(string text) => (text.Length - text.TrimStart().Length) / 2;

        
        private int Expand(string path, int insertAtIndex, int indentationLevel) {
            var numberOfItemsAdded = 0;

            var indendation = "".PadLeft(indentationLevel * 2, ' ');
            
            // display subfolders
            var folderpaths = Directory.GetDirectories(path);
            foreach (var folderpath in folderpaths) {
                _lbFilesystem.Insert(insertAtIndex++, new Listbox.Item($"{indendation}>{System.IO.Path.GetFileName(folderpath)}") {
                    Attachment = new FilesystemAttachment(){Path = folderpath, IsDirectory = true}
                });
                numberOfItemsAdded++;
            }

            if (_listDirectoriesOnly) return numberOfItemsAdded;
            
            // display files
            var filepaths = Directory.GetFiles(path);
            foreach (var filepath in filepaths) {
                _lbFilesystem.Insert(insertAtIndex++, new Listbox.Item($"{indendation} {System.IO.Path.GetFileName(filepath)}") {
                    Attachment = new FilesystemAttachment(){Path = filepath}
                });
                numberOfItemsAdded++;
            }

            return numberOfItemsAdded;
        }

        private int Collapse(int removeAtIndex, int indentationLevelToBeRemoved) {
            var numberOfItemsRemoved = 0;
            while (removeAtIndex < _lbFilesystem.Items.Length) {
                if (Get_indendation_level(_lbFilesystem.Items[removeAtIndex].Text) < indentationLevelToBeRemoved) break;
                _lbFilesystem.RemoveAt(removeAtIndex);
                numberOfItemsRemoved++;
            }
            return numberOfItemsRemoved;
        }
    }
}