using System.Collections.Generic;
using System.Linq;
using bashforms.data;

namespace bashforms.widgets.controls.editors
{
    class MenuItemStack
    {
        private readonly MenuItemList _rootMenuItems;
        private readonly Stack<int> _pathItems;

        public MenuItemStack(MenuItemList rootMenuItems) {
            _rootMenuItems = rootMenuItems;
            _pathItems = new Stack<int>();
        }


        public MenuItemList RootMenu => _rootMenuItems;
        

        public MenuItem[] PathMenuItems {
            get {
                return CollectPathItems_rec(new Stack<MenuItem>(), _rootMenuItems, _pathItems.ToList()).Reverse().ToArray();

                MenuItem[] CollectPathItems_rec(Stack<MenuItem> pathItems, MenuItemList items, List<int> pathItemIndexes) {
                    if (pathItemIndexes.Count == 0) return pathItems.ToArray();

                    var item = items.MenuItems[pathItemIndexes.Last()];
                    pathItems.Push(item);
                    pathItemIndexes.RemoveAt(pathItemIndexes.Count-1);

                    return CollectPathItems_rec(pathItems, item.Submenu, pathItemIndexes);
                }
            }
        }

        public MenuItem[] CurrentMenuItems {
            get
            {
                if (PathMenuItems.Length == 0) return _rootMenuItems.MenuItems;
                return PathMenuItems.Last().Submenu.MenuItems;
            }
        }

        public void Clear() => _pathItems.Clear();


        public MenuItem PushItem(int indexInCurrentItem) {
            _pathItems.Push(indexInCurrentItem);
            return PathMenuItems.First();
        }

        public int PopItem() => _pathItems.Pop();
        
    }
}