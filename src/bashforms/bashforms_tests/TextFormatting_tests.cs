using System;
using bashforms.widgets.controls.utils;
using NUnit.Framework;

namespace bashforms_tests
{
    [TestFixture]
    public class TextFormatting_tests
    {
        [Test]
        public void Reproduce_bug()
        {
            var text = @"time for such a word.";

            var result = text.Wrap(20).Split('\n');
            
            foreach(var l in result)
                Console.WriteLine($"<{l}>");
            
            Assert.AreEqual("time for such a", result[0]);
            Assert.AreEqual("word.", result[1]);
            Assert.AreEqual(2, result.Length);
        }

        [Test]
        public void Split_long_words()
        {
            var words = new[] {"abc", "defghij"};
            var result = TextFormatting.SplitLongWords(words, 3);
            Assert.AreEqual(new[]{"abc", "def", "ghi", "j"}, result);
        }
    }
}