using System.Collections.Generic;
using System.Linq;

namespace bashforms.widgets.controls.formatting
{
    static class TextJustification {
        public static IEnumerable<string> AlignLeft(IEnumerable<IEnumerable<string>> wordsInLines)
            => wordsInLines.Select(f => string.Join(" ", f));    
    }
}