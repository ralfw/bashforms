using System;
using System.Linq;
using bashforms.widgets.controls.formatting;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextFormatting_tests
    {
        [Test]
        public void Wrap_whole_text()
        {
            var result = "she should have\ndied hereafter, there would have been\na time for such a word.".Wrap(10);

            Assert.AreEqual(new[] {
              // 1234567890
                "she should",
                "have died",
                "hereafter,",
                "there",
                "would have",
                "been a",
                "time for",
                "such a",
                "word."
            }, result);
        }
        
        
        [Test]
        public void Wrap_lines_individually()
        {
            var result = "she should have\ndied hereafter, there would have been\na time for such a word.".Wrap(10, true);

            Assert.AreEqual(new[] {
                // 1234567890
                "she should",
                "have",
                "died",
                "hereafter,",
                "there",
                "would have",
                "been",
                "a time for",
                "such a",
                "word."
            }, result);
        }
        
        
        [Test]
        public void Wrap_lines_with_paragraphs()
        {
            var result =
                "she should have died hereafter,\n\nthere would have been a time for such a word.".Wrap(10, true);

            Assert.AreEqual(new[] {
              // 1234567890
                "she should",
                "have died",
                "hereafter,",
                "",
                "there",
                "would have",
                "been a",
                "time for",
                "such a",
                "word."
            }, result);
        }
    }
}