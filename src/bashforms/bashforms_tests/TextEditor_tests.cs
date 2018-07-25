using System;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextEditor_tests
    {
        [Test]
        public void Insertions() {
            var sut = new TextEditor(5);
            var index = sut.Insert(0, "a", null);
            Assert.AreEqual(1, index);
            Assert.AreEqual("a", sut.Text);
            Assert.AreEqual(new[]{"a"}, sut.Lines);
            sut.Insert(1, "b", null);
            sut.Insert(2, " ", null);
            index = sut.Insert(3, "cd", null);
            Assert.AreEqual(5, index);
            Assert.AreEqual("ab cd", sut.Text);
            Assert.AreEqual(new[]{"ab cd"}, sut.Lines);
            sut.Insert(5, " ", null);
            Assert.AreEqual("ab cd ", sut.Text);
            Assert.AreEqual(new[]{"ab cd "}, sut.Lines);
            index = sut.Insert(6, "e", null);
            Assert.AreEqual(7, index);
            Assert.AreEqual("ab cd e", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "e"}, sut.Lines);
        }
        
        [Test]
        public void Linebreak_inserted() {
            var sut = new TextEditor("ab cd efg", 5);
            
            var result = "";
            var index = sut.Insert(5, "x\ny\nz",
                excessText => result = excessText);
            Assert.AreEqual(6, index);
            Assert.AreEqual("ab cdx", sut.Text);
            Assert.AreEqual("y\nz efg", result);

            index = sut.Insert(1, "\n", 
                excessText => result = excessText);
            Assert.AreEqual(1, index);
            Assert.AreEqual("a", sut.Text);
            Assert.AreEqual("b cdx", result);
        }

        
        [Test]
        public void Deletions()
        {
                                    //0123456789012345678901234
            var sut = new TextEditor("ab cd efg hijklmn op qrst", 5);
            
            var index = sut.Delete(12);
            Assert.AreEqual(12, index);
                           //012345678901234567890123
            Assert.AreEqual("ab cd efg hiklmn op qrst", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hiklm", "n op ", "qrst"}, sut.Lines);

            index = sut.Delete(23);
            Assert.AreEqual(22, index);
                           //012345678901234567890123
            Assert.AreEqual("ab cd efg hiklmn op qrs", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hiklm", "n op ", "qrs"}, sut.Lines);
            
            index = sut.Delete(5);
            Assert.AreEqual(5, index);
            Assert.AreEqual("ab cdefg hiklmn op qrs", sut.Text);
            Assert.AreEqual(new[]{"ab ", "cdefg ", "hiklm", "n op ", "qrs"}, sut.Lines);
        }
        
        
        [Test]
        public void Backspace()
        {
                                    //0123456789012345678901234
            var sut = new TextEditor("ab cd efg hijklmn op qrst", 5);
            
            var index = sut.Backspace(12);
            Assert.AreEqual(11, index);
                           //012345678901234567890123
            Assert.AreEqual("ab cd efg hjklmn op qrst", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hjklm", "n op ", "qrst"}, sut.Lines);

            index = sut.Backspace(24);
            Assert.AreEqual(23, index);
            Assert.AreEqual("ab cd efg hjklmn op qrs", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hjklm", "n op ", "qrs"}, sut.Lines);
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
        public string[] Lines => _text.Break(_maxLineLength);

        //TODO: handle multi line insertions
        public int Insert(int index, string textToInsert, Action<string> onLinebreakInserted) {
            _text = _text.Insert(index, textToInsert);
            
            var iEOL = _text.IndexOf('\n');
            if (iEOL < 0) return index + textToInsert.Length;

            var excessText = _text.Substring(iEOL + 1);
            _text = _text.Substring(0, iEOL);
            onLinebreakInserted(excessText);
            
            return _text.Length;
        }

        public int Delete(int index) {
            if (index < 0 || index >= _text.Length) throw new IndexOutOfRangeException("Index for deletion outside of text!");

            _text = _text.Remove(index, 1);
            return (index < _text.Length) ? index : index - 1;
        }
        
        public int Backspace(int index) {
            if (index < 1 || index > _text.Length) throw new IndexOutOfRangeException("Index for backspace outside of text!");

            index -= 1;
            _text = _text.Remove(index, 1);
            return index;
        }
    }
}