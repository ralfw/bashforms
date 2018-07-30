using bashforms.widgets.controls.formatting;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextParsing_tests
    {
        [Test]
        public void ToLines()
        {
            var result = "1 \n 2".ToLines();
            Assert.AreEqual(new[]{"1 ", " 2"}, result);
        }

        [Test]
        public void ToLines_creates_one_line_for_empty_text()
        {
            var result = "".ToLines();
            Assert.AreEqual(new[]{""}, result);
        }
    }
}