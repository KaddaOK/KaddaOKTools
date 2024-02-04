namespace KaddaOK.AvaloniaApp.Models
{
    public class SupportedLanguage
    {
        public string? Bcp47 { get; set; }
        public string? DisplayName { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as SupportedLanguage;
            if (other == null) return false;
            return Bcp47 == other.Bcp47;
        }

        public override int GetHashCode()
        {
            return Bcp47?.GetHashCode() ?? 0;
        }
    }
}
