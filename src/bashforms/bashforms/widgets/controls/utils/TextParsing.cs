using System;

namespace bashforms.widgets.controls.utils
{
    static class TextParsing
    {
        public static string[] ToWords(this string text) {
            return text.Split(new char[] {' ', '\t', '\n'}, StringSplitOptions.RemoveEmptyEntries);
        }

        
        public static string[] ToParagraphs(this string text) {
            const char FF = (char)0x0C; // https://www.ascii-code.com
            return text.Replace("\n\n", FF.ToString()).Split(FF);
        }
    }
}