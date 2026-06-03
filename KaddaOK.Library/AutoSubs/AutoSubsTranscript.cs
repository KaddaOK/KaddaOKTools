namespace KaddaOK.Library.AutoSubs
{
    public class AutoSubsTranscript
    {
        /// <summary>
        /// The JSON filename this will be written to, which is generally the same as the transcript ID with a .json on the end. 
        /// AutoSubs seems to name its files `{timeline name}_tr_{yyyyMMddHHmmss}_{8-char guid}.json`
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// A unique ID for the transcript, which is generally the same as the filename without the extension. 
        /// AutoSubs seems to ID its transcripts `{timeline name}_tr_{yyyyMMddHHmmss}_{8-char guid}`
        /// </summary>
        public string transcriptId { get; set; }
        public DateTime createdAt { get; set; }
        public string sourceType { get; } = "resolve";
        /// <summary>
        /// The "pages" of the transcript, which correspond to TextPlus events in Resolve. Each page has the full text 
        /// of the event as well as timings for each individual word.
        /// </summary>
        public List<AutoSubsPage> segments { get; set; }
    }

    /// <summary>
    /// <para>Describes a single TextPlus event in Resolve, which can be thought of as one "page" of lyrics. </para>
    /// <para>The "text" field is the full text of the entire page, with \n for new lines. 
    /// The "words" field breaks it down into individual words with their own timings, which are also aware of which 
    /// line of the screen they're in.</para>
    /// </summary>
    public class AutoSubsPage
    {
        public int id { get; set; }
        /// <summary>
        /// Start time of the page, in seconds to 2 decimal places. Relative to start of entire timeline.
        /// </summary>
        public double start { get; set; }
        /// <summary>
        /// End time of the page, in seconds to 2 decimal places. Relative to start of entire timeline.
        /// </summary>
        public double end { get; set; }

        /// <summary>
        /// The full text of the page, with \n for new lines.
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// Timing for individual words in the page.
        /// </summary>
        public List<AutoSubsWord> words { get; set; }
    }

    public class AutoSubsWord
    {
        /// <summary>
        /// Text of the individual word. Seems to start with a space rather than ending with one, but I'm not sure if that matters.
        /// </summary>
        public string word { get; set; }
        /// <summary>
        /// Start time of the word, in seconds to 2 decimal places. Relative to start of entire timeline, not the page.
        /// </summary>
        public double start { get; set; }
        /// <summary>
        /// End time of the word, in seconds to 2 decimal places. Relative to start of entire timeline, not the page.
        /// </summary>
        public double end { get; set; }
        /// <summary>
        /// The line the word is on. Starts at 0, and counts up for each new line of the screen.
        /// </summary>
        public int line_number { get; set; }
    }
}