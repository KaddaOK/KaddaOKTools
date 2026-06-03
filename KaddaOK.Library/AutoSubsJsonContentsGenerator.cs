using System.IO;
using System.Text.Json;
using KaddaOK.Library.AutoSubs;

namespace KaddaOK.Library
{
    public enum PaddingStrategy
    {
        Equally,
        PrioritizeStartPadding,
        PrioritizeEndPadding
    }

    public interface IAutoSubsJsonContentsGenerator
    {
        string GenerateAutoSubsJsonContents(string filename, IEnumerable<LyricLine> processedResults,
            int padSegmentStartBackFromFirstWordStartSeconds = 3, int padSegmentEndForwardFromLastWordEndSeconds = 3,
            PaddingStrategy paddingStrategy = PaddingStrategy.Equally);
    }
    public class AutoSubsJsonContentsGenerator : IAutoSubsJsonContentsGenerator
    {
        public string GenerateAutoSubsJsonContents(string filename, IEnumerable<LyricLine> processedResults,
            int padSegmentStartBackFromFirstWordStartSeconds = 3, int padSegmentEndForwardFromLastWordEndSeconds = 3,
            PaddingStrategy paddingStrategy = PaddingStrategy.Equally)
        {
            var allLines = processedResults.ToList();

            // Group lines by PageIndex only if they all have one, otherwise each line is its own group
            var allHavePageIndex = allLines.All(l => l.PageIndex.HasValue);
            var groupedLines = allHavePageIndex
                ? allLines.GroupBy(l => l.PageIndex!.Value)
                : allLines.Select((l, i) => new { Line = l, Index = i }).GroupBy(x => x.Index, x => x.Line);

            // First pass: calculate raw timings for all segments
            var segmentInfos = new List<(IGrouping<int, LyricLine> group, double rawStart, double rawEnd)>();
            foreach (var group in groupedLines.OrderBy(g => g.Key))
            {
                var allWordsInSegment = group.SelectMany(line => line.Words);
                if (!allWordsInSegment.Any())
                {
                    continue;
                }

                var rawStart = allWordsInSegment.Min(w => w.StartSecond);
                var rawEnd = allWordsInSegment.Max(w => w.EndSecond);
                segmentInfos.Add((group, rawStart, rawEnd));
            }

            // Second pass: apply padding and resolve conflicts
            var segments = new List<AutoSubsPage>();
            for (int i = 0; i < segmentInfos.Count; i++)
            {
                var (group, rawStart, rawEnd) = segmentInfos[i];

                double segmentStart = rawStart - padSegmentStartBackFromFirstWordStartSeconds;
                double segmentEnd = rawEnd + padSegmentEndForwardFromLastWordEndSeconds;

                // Check previous segment for overlap
                if (i > 0)
                {
                    var prevSegment = segments[i - 1];
                    var prevRawEnd = segmentInfos[i - 1].rawEnd;
                    
                    var gap = rawStart - prevRawEnd;
                    
                    if (gap < 0)
                    {
                        // Words already overlap, don't make it worse
                        // Just use raw boundaries
                        segmentStart = rawStart;
                        prevSegment.end = prevRawEnd;
                    }
                    else if (segmentStart < prevSegment.end)
                    {
                        // Overlap in padded segments, resolve based on strategy
                        if (paddingStrategy == PaddingStrategy.Equally)
                        {
                            // Split gap equally between the two segments
                            var halfGap = gap / 2;
                            prevSegment.end = prevRawEnd + halfGap;
                            segmentStart = rawStart - halfGap;
                        }
                        else if (paddingStrategy == PaddingStrategy.PrioritizeEndPadding)
                        {
                            // Previous segment gets priority for end padding, current gets remainder
                            var endPadding = Math.Min(padSegmentEndForwardFromLastWordEndSeconds, gap);
                            var startPadding = gap - endPadding;
                            prevSegment.end = prevRawEnd + endPadding;
                            segmentStart = rawStart - startPadding;
                        }
                        else if (paddingStrategy == PaddingStrategy.PrioritizeStartPadding)
                        {
                            // Current segment gets priority for start padding, previous gets remainder
                            var startPadding = Math.Min(padSegmentStartBackFromFirstWordStartSeconds, gap);
                            var endPadding = gap - startPadding;
                            prevSegment.end = prevRawEnd + endPadding;
                            segmentStart = rawStart - startPadding;
                        }
                    }
                }

                // Ensure padding doesn't move segment boundaries beyond the words themselves
                segmentStart = Math.Min(segmentStart, rawStart);
                segmentEnd = Math.Max(segmentEnd, rawEnd);

                // Build text with newlines for multiple lines
                var lineTexts = group.Select(l => l.Text?.Trim()).ToList();
                var text = string.Join("\n", lineTexts);

                // Assign line numbers and build words list
                var pageWords = new List<AutoSubsWord>();
                int lineNumber = 0;

                foreach (var line in group)
                {
                    foreach (var word in line.Words)
                    {
                        pageWords.Add(new AutoSubsWord
                        {
                            word = word.Text ?? string.Empty,
                            start = Math.Round(word.StartSecond, 2),
                            end = Math.Round(word.EndSecond, 2),
                            line_number = lineNumber
                        });
                    }

                    lineNumber++;
                }

                segments.Add(new AutoSubsPage
                {
                    id = segments.Count,
                    start = Math.Round(segmentStart, 2),
                    end = Math.Round(segmentEnd, 2),
                    text = text,
                    words = pageWords
                });
            }

            var transcript = new AutoSubsTranscript
            {
                filename = filename,
                transcriptId = Path.GetFileNameWithoutExtension(filename),
                createdAt = DateTime.UtcNow,
                segments = segments
            };

            return JsonSerializer.Serialize(transcript, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}