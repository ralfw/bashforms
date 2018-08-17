using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using bashforms.data.eventargs;
using bashforms.widgets.controls;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.windows.dialogs
{
    public class FilesystemDialog : Dialog<string[]>
    {
        private string _path;
        private readonly Listbox _lbFilesystem;
        
        
        public FilesystemDialog(string path = "", string title = "") : base(0, 0, 1, 1) {
            _width = Console.WindowWidth / 2;
            _height = (int)(Console.WindowHeight * 0.8);
            _top = (Console.WindowHeight - _height) / 2;
            _left = (Console.WindowWidth - _width) / 2;

            _title = title == "" ? "Select file or folder" : title;

            _lbFilesystem = new Listbox(2, 2, _width - 4, _height - 4){SelectionMode = Listbox.SelectionModes.SingleSelection};
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
        

        private void Fill(string path) {
            _path = path;
            _lbFilesystem.Clear();

            var folderpaths = Directory.GetDirectories(_path);
            foreach (var folderpath in folderpaths) {
                _lbFilesystem.Add(new Listbox.Item($"+{System.IO.Path.GetFileName(folderpath)}"){Attachment = folderpath});
            }

            var filepaths = Directory.GetFiles(_path);
            foreach (var filepath in filepaths) {
                _lbFilesystem.Add(new Listbox.Item($" {System.IO.Path.GetFileName(filepath)}"){Attachment = filepath});
            }
        }
    }
}