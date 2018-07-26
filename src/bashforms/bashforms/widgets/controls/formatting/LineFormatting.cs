using System;
using System.Collections.Generic;

namespace bashforms_tests
{
    static class LineFormatting
    {
        public static string[] Break(this string line, int maxLineLength)
        {
            if (line.Length <= maxLineLength) return new[] {line};

            var segments = new List<string>();
            while (line.Length > 0) {
                var (segment, restOfLine) = Break_off_segment(line, maxLineLength);
                (segment, restOfLine) = Adjust_cut(segment, restOfLine);
                
                segments.Add(segment);
                line = restOfLine;
            }
            return segments.ToArray();
        }
        
        static (string segment, string restOfLine) Break_off_segment(string line, int maxLineLength) {
            var segment = line.Substring(0, Math.Min(maxLineLength, line.Length));
            line = line.Length > maxLineLength ? line.Substring(maxLineLength) : "";
            return (segment, line);
        }

        static (string segment, string restOfLine) Adjust_cut(string segment, string restOfLine) {
            if (EndOfLineReached() || AClearCutOnSpace()) return (segment, restOfLine);
            return APrematureCut() ? (segment + " ", restOfLine.Substring(1)) 
                : Handle_possibly_too_long_word();


            bool EndOfLineReached() => restOfLine.Length == 0;
            bool AClearCutOnSpace() => segment.EndsWith(" ");
            bool APrematureCut() => restOfLine.StartsWith(" ");

            (string segment, string restOfLine) Handle_possibly_too_long_word() {
                var iSpace = segment.LastIndexOf(' ');
                if (iSpace < 0) return (segment, restOfLine);
                
                restOfLine = segment.Substring(iSpace + 1) + restOfLine;
                segment = segment.Substring(0, iSpace + 1);
                return (segment, restOfLine);
            }
        }
    }
}