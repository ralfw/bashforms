using System.Collections.Generic;
using System.Linq;

namespace bashforms.widgets.controls.utils
{
    static class TextFormatting
    {
        public static string Wrap(this string text, int maxLineLen) {
            var fragments = SplitLongWords(text.ToWords(), maxLineLen);
            var rowFragments = ComposeRows(fragments, maxLineLen);
            var rows = TextJustification.AlignLeft(rowFragments);
            return JoinRows(rows);

            
            string JoinRows(IEnumerable<string> rows_) => string.Join("\n", rows_);
        }

        
        static string[] SplitLongWords(IEnumerable<string> words, int rowLength)
        {
            var splittedWords = new List<string>();
            foreach (var word in words)
            {
                if (word.Length <= rowLength) {
                    splittedWords.Add(word);
                }
                else {
                    var timesToSplit = word.Length / rowLength;
                    if (word.Length % rowLength > 0) {
                        timesToSplit++;
                    }

                    for (int i = 0; i < timesToSplit; i++) {
                        var length = i == timesToSplit - 1 ? word.Length % rowLength : rowLength;
                        splittedWords.Add(word.Substring(i * rowLength, length));
                    }
                }
            }
            return splittedWords.ToArray();
        }
        
        
        static IEnumerable<IEnumerable<string>> ComposeRows(IEnumerable<string> fragments, int zeilenlänge) {
            var currRow = new List<string>();
            foreach (var fragment in fragments) {
                if (currRow.Sum(x => x.Length) + fragment.Length + (currRow.Count - 1) <= zeilenlänge) {
                    currRow.Add(fragment);
                }
                else {
                    yield return currRow;
                    currRow = new List<string> { fragment };
                }
            }
            if (currRow.Count > 0) {
                yield return currRow;
            }
        }
    }
}