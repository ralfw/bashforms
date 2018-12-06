using System;
using EventArgs = bashforms.data.eventargs.EventArgs;

namespace bashforms.widgets.controls
{
    partial class Listbox
    {
        public override bool HandleKey(ConsoleKeyInfo key) {
            switch (key.Key)
            {
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    SelectCurrentItem();
                    this.OnPressed(this, new EventArgs());
                    return true;
                
                case ConsoleKey.UpArrow:
                    if (_items.Count == 0) return true;
                
                    _currentItemIndex--;
                    if (_currentItemIndex < 0) _currentItemIndex = 0;

                    if (_currentItemIndex < _firstItemToDisplayIndex) _firstItemToDisplayIndex = _currentItemIndex;
                    return true;
            
                case ConsoleKey.DownArrow:
                    if (_items.Count == 0) return true;

                    _currentItemIndex++;
                    if (_currentItemIndex >= _items.Count) _currentItemIndex = _items.Count - 1;

                    if (_currentItemIndex > _firstItemToDisplayIndex + _height - 1) _firstItemToDisplayIndex++;
                    return true;
            }
            return false;
        }

    
        private void SelectCurrentItem() {
            if (_currentItemIndex < 0) return;

            if (_selectedItemIndexes.Contains(_currentItemIndex)) {
                _selectedItemIndexes.Remove(_currentItemIndex);
                return;
            }

            switch (_selectionMode) {
                case SelectionModes.SingleSelection:
                    _selectedItemIndexes.Clear();
                    _selectedItemIndexes.Add(_currentItemIndex);
                    break;
                case SelectionModes.MultipleSelections:
                    _selectedItemIndexes.Remove(_currentItemIndex); // avoid duplicates
                    _selectedItemIndexes.Add(_currentItemIndex);
                    break;
                case SelectionModes.NoSelections:
                    break;
            }
        }
    }
}