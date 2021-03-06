using System;
using bashforms.widgets.controls.editors;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextEditor_tests
    {
        [Test]
        public void Insertions() {
            var sut = new TextEditor("", 5);
            
            var position = sut.Insert(0, 0, "a");
            Assert.AreEqual((0,1), position);
            Assert.AreEqual("a", sut.Text);

            position = sut.Insert(0, position.index, "b cd efg");
            Assert.AreEqual((0,9), position);
            Assert.AreEqual("ab cd efg", sut.Text);
            Assert.AreEqual(new[]{"ab cd ", "efg"}, sut.SoftLines);
        }
        
        [Test]
        public void Insertion_with_multiple_lines() {
            var sut = new TextEditor("abc", 5);
            
            var position = sut.Insert(0, 1, "\nx\ny\nz");
            Assert.AreEqual((3,1), position);
            Assert.AreEqual("a\nx\ny\nzbc", sut.Text);
            Assert.AreEqual(new[]{"a", "x", "y", "zbc"}, sut.Lines);
        }


        [Test]
        public void Delete_in_line() {
            var sut = new TextEditor("abc\ndef", 5);

            var position = sut.Delete(1, 1);
            Assert.AreEqual((1,1), position);
            Assert.AreEqual("abc\ndf", sut.Text);
        }
        
        [Test]
        public void Merge_two_lines_by_deleting_after_the_end_of_the_first() {
            var sut = new TextEditor("abc\ndef", 5);

            var position = sut.Delete(0, 3);
            Assert.AreEqual((0,3), position);
            Assert.AreEqual("abcdef", sut.Text);
            Assert.AreEqual(1, sut.Lines.Length);
        }
        
        [Test]
        public void Delete_after_end_of_last_line_does_no_harm() {
            var sut = new TextEditor("abc\ndef", 5);

            var position = sut.Delete(1, 3);
            Assert.AreEqual((1,3), position);
            Assert.AreEqual("abc\ndef", sut.Text);
            Assert.AreEqual(2, sut.Lines.Length);
        }
        
        [Test]
        public void Backspace_in_line() {
            var sut = new TextEditor("abc\ndef", 5);

            var position = sut.Backspace(1, 1);
            Assert.AreEqual((1,0), position);
            Assert.AreEqual("abc\nef", sut.Text);
        }
        
        [Test]
        public void Merge_current_line_with_previous_if_backspacing_at_start() {
            var sut = new TextEditor("abc\ndef", 5);

            var position = sut.Backspace(1, 0);
            Assert.AreEqual((0,3), position);
            Assert.AreEqual("abcdef", sut.Text);
        }

        [Test]
        public void Calculate_cursor_position() {
            /*
             *   01234
             * 0 ab_cd
             * 1 efg
             * ---
             * 2 hijkl
             * 3 mn op
             * ---
             * 4 qrst
            */
            var sut = new TextEditor("ab cd efg\nhijklmn op \nqrst", 5);
            var position = sut.GetSoftPosition(0, 1);
            Assert.AreEqual((0,1), position);
            
            position = sut.GetSoftPosition(0, 7);
            Assert.AreEqual((1,1), position);
            
            position = sut.GetSoftPosition(2, 3);
            Assert.AreEqual((4,3), position);
        }

        [Test]
        public void Calculate_index()
        {
            /*
             *   01234
             * 0 ab_cd
             * 1 efg
             * ---
             * 2 hijkl
             * 3 mn op
             * ---
             * 4 qrst
            */
            var sut = new TextEditor("ab cd efg\nhijklmn op \nqrst", 5);
            var index = sut.GetIndex(0, 1);
            Assert.AreEqual((0,1), index);
            
            index = sut.GetIndex(1, 1);
            Assert.AreEqual((0,7), index);
            
            index = sut.GetIndex(4, 3);
            Assert.AreEqual((2,3), index);
        }
    }
}