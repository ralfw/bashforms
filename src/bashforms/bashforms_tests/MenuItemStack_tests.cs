using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using bashforms.widgets.controls;
using bashforms.widgets.controls.editors;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class MenuItemStack_tests
    {
        [Test]
        public void Acceptance()
        {
            var items = new MenuBar.MenuItems();
            var a = items.AddItem(new MenuBar.Item("a", "ia"));
            var a1 = a.Submenu.AddItem("a1");
            a1.Submenu.AddItem("a11");
            a1.Submenu.AddItem("a12");
            var a2 = a.Submenu.AddItem("a2");
            a2.Submenu.AddItem("a21");
            a2.Submenu.AddItem("a22");
            a.Submenu.AddItem("a3");
            var b = items.AddItem(new MenuBar.Item("b", "ib"));
            var b1 = b.Submenu.AddItem("b1");
            b1.Submenu.AddItem("b11");
            b1.Submenu.AddItem("b12");
            b.Submenu.AddItem("b2");
            items.AddItem(new MenuBar.Item("c", "ic"));
            
            var sut = new MenuItemStack(items);
            
            var pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(0, pathItemTexts.Length);

            var currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a", "b", "c"}, currentItemTexts);

            var item = sut.PushItem(0); // +a
            Assert.AreSame(a, item);
            
            pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a"}, pathItemTexts);
            
            currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a1", "a2", "a3"}, currentItemTexts);


            sut.PushItem(1); // +a2
            
            pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a", "a2"}, pathItemTexts);
            
            currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a21", "a22"}, currentItemTexts);


            sut.PushItem(0); // +a21
            
            pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a", "a2", "a21"}, pathItemTexts);
            
            currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(0, currentItemTexts.Length);

            var itemIndex = sut.PopItem(); // -a21
            Assert.AreEqual(0, itemIndex);
            
            pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a", "a2"}, pathItemTexts);
            
            currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a21", "a22"}, currentItemTexts);

            itemIndex = sut.PopItem(); // -a2
            Assert.AreEqual(1, itemIndex);
            
            sut.PopItem(); // -a
            sut.PushItem(1); // +b
            sut.PushItem(0); // +b1

            pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"b", "b1"}, pathItemTexts);
            
            currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"b11", "b12"}, currentItemTexts);

            sut.Clear();
            
            pathItemTexts = sut.PathItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(0, pathItemTexts.Length);

            currentItemTexts = sut.CurrentItems.Select(i => i.Text).ToArray();
            Assert.AreEqual(new[]{"a", "b", "c"}, currentItemTexts);
        }
    }
}