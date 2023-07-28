namespace Logs_Chat_Record_Extractor_WPF.Models
{
    public class Line
    {
        public string lineText { get; }
        public string lineCheck { get; }

        public Line(string lineText, string lineCheck)
        {
            this.lineText = lineText;
            this.lineCheck = lineCheck;
        }
    }
}