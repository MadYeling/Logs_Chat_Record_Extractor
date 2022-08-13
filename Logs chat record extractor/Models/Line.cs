namespace Logs_chat_record_extractor.Models
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