using System.ComponentModel;
using System.Diagnostics;
using bashforms.widgets.controls.formatting;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class LineFormatting_tests
    {
        [Test]
        public void Single_line()         {
            var result = "abc".Break(5);
            Assert.AreEqual(new[]{"abc"}, result);
        }
        
        [Test]
        public void Multiple_lines()         {
            var result = "abc cd fghi".Break(5);
            Assert.AreEqual(new[]{"abc ", "cd ", "fghi"}, result);
        }
        
        [Test]
        public void With_too_long_words()         {
            var result = "abc 0123456789AB fghi".Break(5);
            Assert.AreEqual(new[]{"abc ", "01234", "56789", "AB ", "fghi"}, result);
        }
    }
}