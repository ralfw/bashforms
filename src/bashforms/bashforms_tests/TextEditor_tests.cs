using System.Collections.Generic;
using System.Linq;
using bashforms.widgets.controls.utils;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextEditor_tests
    {
        [Test]
        public void Insertions()
        {
            var sut = new TextEditor("", 5);
            var position = sut.Insert(0, 0, "a");
            Assert.AreEqual((0,1), position);
            Assert.AreEqual("a", sut.Text);
        }
    }


    class TextEditor
    {
        private readonly int _maxLineLength;
        private readonly List<LineEditor> _lines;
        
        public TextEditor(int maxLineLength) : this ("", maxLineLength) {}
        public TextEditor(string text, int maxLineLength) {
            _maxLineLength = maxLineLength;
            _lines = text.ToLines().Select(l => new LineEditor(l, maxLineLength)).ToList();
        }

        public string Text => string.Join("\n", _lines.Select(l => l.Line));
        public string[] Lines => _lines.Select(l => l.Line).ToArray();
        public string[] SoftLines => _lines.SelectMany(l => l.SoftLines).ToArray();

        public (int row, int index) Insert(int row, int index, string text) {
            index = _lines[row].Insert(index, text, null);
            return (row, index);
        }
    }
}