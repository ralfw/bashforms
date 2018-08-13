using System.Collections.Generic;
using System.Linq;

namespace bashforms.widgets.controls.editors
{
    class MenuItemStack
    {
        private readonly MenuBar.MenuItems _rootItems;
        private readonly Stack<int> _pathItems;

        public MenuItemStack(MenuBar.MenuItems rootItems) {
            _rootItems = rootItems;
            _pathItems = new Stack<int>();
        }


        public MenuBar.Item[] PathItems {
            get {
                return CollectPathItems_rec(new Stack<MenuBar.Item>(), _rootItems, _pathItems.ToList()).Reverse().ToArray();

                MenuBar.Item[] CollectPathItems_rec(Stack<MenuBar.Item> pathItems, MenuBar.MenuItems items, List<int> pathItemIndexes) {
                    if (pathItemIndexes.Count == 0) return pathItems.ToArray();

                    var item = items.Items[pathItemIndexes.Last()];
                    pathItems.Push(item);
                    pathItemIndexes.RemoveAt(pathItemIndexes.Count-1);

                    return CollectPathItems_rec(pathItems, item.Submenu, pathItemIndexes);
                }
            }
        }

        public MenuBar.Item[] CurrentItems {
            get
            {
                if (PathItems.Length == 0) return _rootItems.Items;
                return PathItems.Last().Submenu.Items;
            }
        }

        public void Clear() => _pathItems.Clear();


        public MenuBar.Item PushItem(int indexInCurrentItem) {
            _pathItems.Push(indexInCurrentItem);
            return PathItems.First();
        }

        public int PopItem() => _pathItems.Pop();
        
    }
}