using System;
using System.Windows.Documents;

namespace Logs_Chat_Record_Extractor_WPF.Utils
{
    public static class FindWord
    {
        public static TextPointer FindWordPosition(TextPointer position, string word)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    var textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "word".
                    var indexInRun = textRun.IndexOf(word, StringComparison.Ordinal);
                    if (indexInRun >= 0)
                    {
                        position = position.GetPositionAtOffset(indexInRun);
                        break;
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return position;
        }
    }
}