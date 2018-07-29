﻿using System.Collections.Generic;
using System.Linq;

namespace bashforms.widgets.controls.utils
{
    static class TextFormatting
    {
        public static string[] Wrap(this string text, int maxLineLength) {
            var fragments = SplitLongWords(text.ToWords(), maxLineLength);
            var rowFragments = ComposeRows(fragments, maxLineLength);
            return TextJustification.AlignLeft(rowFragments).ToArray();
        }

        
        static string[] SplitLongWords(IEnumerable<string> words, int maxWordLength) {
            return words.SelectMany(w => Split(new List<string>(), w))
                        .ToArray();


            IEnumerable<string> Split(List<string> fragments, string word_) {
                if (word_.Length <= maxWordLength) {
                    fragments.Add(word_);
                    return fragments;
                }

                var fragment = word_.Substring(0, maxWordLength);
                fragments.Add(fragment);
                return Split(fragments, word_.Substring(maxWordLength));
            }
        }
        
        
        private static IEnumerable<IEnumerable<string>> ComposeRows(IEnumerable<string> fragments, int zeilenlänge) {
            var currRow = new List<string>();
            foreach (var fragment in fragments) {
                var lengthOfAllWords = currRow.Sum(x => x.Length);
                var spacesBetweenWords = currRow.Count > 0 ? currRow.Count : 0;
                var candidateLineLen = lengthOfAllWords + spacesBetweenWords + fragment.Length;

                if (candidateLineLen <= zeilenlänge)
                    currRow.Add(fragment);
                else {
                    yield return currRow;
                    currRow = new List<string> { fragment };
                }
            }
            if (currRow.Count > 0) yield return currRow;
        }
    }
}