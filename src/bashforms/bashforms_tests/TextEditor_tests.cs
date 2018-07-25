using System;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextEditor_tests
    {
        [Test]
        public void Scenario()
        {
            var sut = new TextEditor(5);
            var index = sut.Insert(0, "a");
            Assert.AreEqual(1, index);
            Assert.AreEqual("a", sut.Text);
            Assert.AreEqual(new[]{"a"}, sut.Lines);
            sut.Insert(1, "b");
            sut.Insert(2, " ");
            index = sut.Insert(3, "cd");
            Assert.AreEqual(5, index);
            Assert.AreEqual("ab cd", sut.Text);
            Assert.AreEqual(new[]{"ab cd"}, sut.Lines);
            sut.Insert(5, " ");
            Assert.AreEqual("ab cd ", sut.Text);
            Assert.AreEqual(new[]{"ab cd "}, sut.Lines);
            index = sut.Insert(6, "e");
            Assert.AreEqual(7, index);
            Assert.AreEqual("ab cd e", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "e"}, sut.Lines);
        }
    }


    class TextEditor
    {
        private readonly int _maxLineLength;
        private string _text;

        public TextEditor(int maxLineLength) : this("", maxLineLength) {}
        public TextEditor(string text, int maxLineLength) {
            if (text.IndexOf("\n") >= 0) throw new InvalidOperationException("Text may not include line breaks!");
            
            _maxLineLength = maxLineLength;
            _text = text;
        }

        public string Text => _text;
        public string[] Lines => new[] {_text};

        //TODO: handle text with line breaks
        public int Insert(int index, string textToInsert) {
            _text = _text.Insert(index, textToInsert);
            return index + textToInsert.Length;
        }
    }
    
    
}