using bashforms.widgets.controls.editors;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class LineEditor_tests
    {
        [Test]
        public void Insertions() {
            var sut = new LineEditor(5);
            var index = sut.Insert(0, "a", null);
            Assert.AreEqual(1, index);
            Assert.AreEqual("a", sut.Line);
            Assert.AreEqual(new[]{"a"}, sut.SoftLines);
            sut.Insert(1, "b", null);
            sut.Insert(2, " ", null);
            index = sut.Insert(3, "cd", null);
            Assert.AreEqual(5, index);
            Assert.AreEqual("ab cd", sut.Line);
            Assert.AreEqual(new[]{"ab cd"}, sut.SoftLines);
            sut.Insert(5, " ", null);
            Assert.AreEqual("ab cd ", sut.Line);
            Assert.AreEqual(new[]{"ab cd "}, sut.SoftLines);
            index = sut.Insert(6, "e", null);
            Assert.AreEqual(7, index);
            Assert.AreEqual("ab cd e", sut.Line);
            Assert.AreEqual(new[]{"ab cd ", "e"}, sut.SoftLines);
        }

        [Test]
        public void Insert_past_end()
        {
            var sut = new LineEditor("ab", 5);
            var index = sut.Insert(10, "c", null);
            Assert.AreEqual(3, index);
            Assert.AreEqual("abc", sut.Line);
        }
        
        [Test]
        public void Insertion_with_linebreaks() {
            var sut = new LineEditor("ab cd efg", 5);
            
            var result = "";
            var index = sut.Insert(5, "x\ny\nz",
                excessText => result = excessText);
            Assert.AreEqual(6, index);
            Assert.AreEqual("ab cdx", sut.Line);
            Assert.AreEqual("y\nz efg", result);

            index = sut.Insert(1, "\n", 
                excessText => result = excessText);
            Assert.AreEqual(1, index);
            Assert.AreEqual("a", sut.Line);
            Assert.AreEqual("b cdx", result);
        }

        
        [Test]
        public void Deletions()
        {
                                    //0123456789012345678901234
            var sut = new LineEditor("ab cd efg hijklmn op qrst", 5);
            
            var index = sut.Delete(12);
            Assert.AreEqual(12, index);
                           //012345678901234567890123
            Assert.AreEqual("ab cd efg hiklmn op qrst", sut.Line);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hiklm", "n op ", "qrst"}, sut.SoftLines);

            index = sut.Delete(23);
            Assert.AreEqual(22, index);
                           //012345678901234567890123
            Assert.AreEqual("ab cd efg hiklmn op qrs", sut.Line);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hiklm", "n op ", "qrs"}, sut.SoftLines);
            
            index = sut.Delete(5);
            Assert.AreEqual(5, index);
            Assert.AreEqual("ab cdefg hiklmn op qrs", sut.Line);
            Assert.AreEqual(new[]{"ab ", "cdefg ", "hiklm", "n op ", "qrs"}, sut.SoftLines);
        }
        
        
        [Test]
        public void Backspace()
        {
                                    //0123456789012345678901234
            var sut = new LineEditor("ab cd efg hijklmn op qrst", 5);
            
            var index = sut.Backspace(12);
            Assert.AreEqual(11, index);
                           //012345678901234567890123
            Assert.AreEqual("ab cd efg hjklmn op qrst", sut.Line);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hjklm", "n op ", "qrst"}, sut.SoftLines);

            index = sut.Backspace(24);
            Assert.AreEqual(23, index);
            Assert.AreEqual("ab cd efg hjklmn op qrs", sut.Line);
            Assert.AreEqual(new[]{"ab cd ", "efg ", "hjklm", "n op ", "qrs"}, sut.SoftLines);
        }
        
        
        [Test]
        public void Calculate_cursor_position_from_index()
        {
            /*
             *   01234
             * 0 ab_cd
             * 1 efg
             * 2 hijkl
             * 3 mn op
             * 4 qrst
             */
            var sut = new LineEditor("ab cd efg hijklmn op qrst", 5);
            var position = sut.GetSoftPosition(0); // before "a"
            Assert.AreEqual((0,0), position);
            
            position = sut.GetSoftPosition(4); // before "d"
            Assert.AreEqual((0,4), position);
            
            position = sut.GetSoftPosition(5); // after "d"
            Assert.AreEqual((0,5), position);
            
            position = sut.GetSoftPosition(6); // before "e"
            Assert.AreEqual((1,0), position);
            
            position = sut.GetSoftPosition(18); // before "o"
            Assert.AreEqual((3,3), position);
            
            position = sut.GetSoftPosition(99); // far after "t"
            Assert.AreEqual((4,4), position);
        }


        [Test]
        public void Calculate_index_from_position() {
            /*
             *   01234
             * 0 ab_cd
             * 1 efg
             * 2 hijkl
             * 3 mn op
             * 4 qrst
             */
            var sut = new LineEditor("ab cd efg hijklmn op qrst", 5);
            var index = sut.GetIndex(0, 0); // before "a"
            Assert.AreEqual(0, index);
            
            index = sut.GetIndex(0, 4); // before "d"
            Assert.AreEqual(4, index);
                        
            index = sut.GetIndex(1,0); // before "e"
            Assert.AreEqual(6, index);
                                    
            index = sut.GetIndex(3,3); // before "o"
            Assert.AreEqual(18, index);
            
            index = sut.GetIndex(5,10); // far after "t"
            Assert.AreEqual(sut.Line.Length, index);
        }
    }
}