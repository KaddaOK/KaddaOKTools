using KaddaOK.Library;

namespace KaddaOK.AvaloniaApp.Models
{
    public class AddingLine
    {
        public LyricLine? PreviousLine { get; set; }
        public LyricLine? NextLine { get; set; }
        public string? EnteredText { get; set; }
    }
}
