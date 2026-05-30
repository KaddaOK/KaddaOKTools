using System.Collections.ObjectModel;
using System.Text.Json;
using KaddaOK.Library.AutoSubs;

namespace KaddaOK.Library.Tests
{
    public class AutoSubsJsonContentsGeneratorTests
    {
        public class GenerateAutoSubsJsonContents
        {
            [Fact]
            public void ShouldGenerateCorrectly_SimpleCase()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Hello ", StartSecond = 0.0, EndSecond = 0.5 },
                            new() { Text = "world", StartSecond = 0.5, EndSecond = 1.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "This ", StartSecond = 2.0, EndSecond = 2.5 },
                            new() { Text = "is ", StartSecond = 2.5, EndSecond = 3.0 },
                            new() { Text = "a ", StartSecond = 3.0, EndSecond = 3.5 },
                            new() { Text = "test.", StartSecond = 3.5, EndSecond = 4.0 }
                        }
                    }
                };

                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines, 0, 0);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                Assert.Equal("test.json", deserializedTranscript!.filename);
                Assert.Equal("test", deserializedTranscript.transcriptId);
                Assert.Equal(2, deserializedTranscript.segments.Count);

                var firstSegment = deserializedTranscript.segments[0];
                Assert.Equal(0, firstSegment.id);
                Assert.Equal(0.0, firstSegment.start);
                Assert.Equal(1.0, firstSegment.end);
                Assert.Equal("Hello world", firstSegment.text);
                Assert.Equal(2, firstSegment.words.Count);
                Assert.Equal("Hello ", firstSegment.words[0].word);
                Assert.Equal(0.0, firstSegment.words[0].start);
                Assert.Equal(0.5, firstSegment.words[0].end);
                Assert.Equal("world", firstSegment.words[1].word);
                Assert.Equal(0.5, firstSegment.words[1].start);
                Assert.Equal(1.0, firstSegment.words[1].end);

                var secondSegment = deserializedTranscript.segments[1];
                Assert.Equal(1, secondSegment.id);
                Assert.Equal(2.0, secondSegment.start);
                Assert.Equal(4.0, secondSegment.end);
                Assert.Equal("This is a test.", secondSegment.text);
                Assert.Equal(4, secondSegment.words.Count);
                Assert.Equal("This ", secondSegment.words[0].word);
                Assert.Equal(2.0, secondSegment.words[0].start);
                Assert.Equal(2.5, secondSegment.words[0].end);
                Assert.Equal("is ", secondSegment.words[1].word);
                Assert.Equal(2.5, secondSegment.words[1].start);
                Assert.Equal(3.0, secondSegment.words[1].end);
                Assert.Equal("a ", secondSegment.words[2].word);
                Assert.Equal(3.0, secondSegment.words[2].start);
                Assert.Equal(3.5, secondSegment.words[2].end);
                Assert.Equal("test.", secondSegment.words[3].word);
                Assert.Equal(3.5, secondSegment.words[3].start);
                Assert.Equal(4.0, secondSegment.words[3].end);
            }

            [Fact]
            public void ShouldGenerateCorrectly_WithPages()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "I ", StartSecond = 0.0, EndSecond = 0.5 },
                            new() { Text = "am ", StartSecond = 0.5, EndSecond = 1.0 }
                        },
                        PageIndex = 0
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "one ", StartSecond = 2.0, EndSecond = 2.5 },
                            new() { Text = "single ", StartSecond = 2.5, EndSecond = 3.0 },
                            new() { Text = "page ", StartSecond = 3.0, EndSecond = 3.5 }
                        },
                        PageIndex = 0
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "I'm ", StartSecond = 7.0, EndSecond = 7.5 },
                            new() { Text = "another ", StartSecond = 7.5, EndSecond = 8.0 },
                            new() { Text = "page ", StartSecond = 8.0, EndSecond = 8.5 }
                        },
                        PageIndex = 1
                    }
                };

                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines, 0, 0);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                Assert.Equal("test.json", deserializedTranscript!.filename);
                Assert.Equal("test", deserializedTranscript.transcriptId);
                Assert.Equal(2, deserializedTranscript.segments.Count);

                var firstSegment = deserializedTranscript.segments[0];
                var secondSegment = deserializedTranscript.segments[1];

                Assert.Equal(0, firstSegment.id);
                Assert.Equal(1, secondSegment.id);

                Assert.Equal("I am\none single page", firstSegment.text);
                Assert.Equal("I'm another page", secondSegment.text);

                Assert.Equal(0.0, firstSegment.start);
                Assert.Equal(3.5, firstSegment.end);

                Assert.Equal(5, firstSegment.words.Count);
                Assert.Equal(3, secondSegment.words.Count);

                Assert.Equal("I ", firstSegment.words[0].word);
                Assert.Equal(0.0, firstSegment.words[0].start);
                Assert.Equal(0.5, firstSegment.words[0].end);
                Assert.Equal("am ", firstSegment.words[1].word);
                Assert.Equal(0.5, firstSegment.words[1].start);
                Assert.Equal(1.0, firstSegment.words[1].end);
                Assert.Equal("one ", firstSegment.words[2].word);
                Assert.Equal(2.0, firstSegment.words[2].start);
                Assert.Equal(2.5, firstSegment.words[2].end);
                Assert.Equal("single ", firstSegment.words[3].word);
                Assert.Equal(2.5, firstSegment.words[3].start);
                Assert.Equal(3.0, firstSegment.words[3].end);
                Assert.Equal("page ", firstSegment.words[4].word);
                Assert.Equal(3.0, firstSegment.words[4].start);
                Assert.Equal(3.5, firstSegment.words[4].end);

                Assert.Equal("I'm ", secondSegment.words[0].word);
                Assert.Equal(7.0, secondSegment.words[0].start);
                Assert.Equal(7.5, secondSegment.words[0].end);
                Assert.Equal("another ", secondSegment.words[1].word);
                Assert.Equal(7.5, secondSegment.words[1].start);
                Assert.Equal(8.0, secondSegment.words[1].end);
                Assert.Equal("page ", secondSegment.words[2].word);
                Assert.Equal(8.0, secondSegment.words[2].start);
                Assert.Equal(8.5, secondSegment.words[2].end);
            }

            [Fact]
            public void ShouldApplyPaddingCorrectly_WhenEnoughSpaceAvailable()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "First", StartSecond = 10.0, EndSecond = 11.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Second", StartSecond = 20.0, EndSecond = 21.0 }
                        }
                    }
                };

                // Apply 2 second padding on both sides
                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines, 
                    padSegmentStartBackFromFirstWordStartSeconds: 2, 
                    padSegmentEndForwardFromLastWordEndSeconds: 2);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                Assert.Equal(2, deserializedTranscript!.segments.Count);

                // First segment should start 2 seconds before the word and end 2 seconds after
                var firstSegment = deserializedTranscript.segments[0];
                Assert.Equal(8.0, firstSegment.start);  // 10.0 - 2.0
                Assert.Equal(13.0, firstSegment.end);   // 11.0 + 2.0

                // Second segment should have the same padding applied
                var secondSegment = deserializedTranscript.segments[1];
                Assert.Equal(18.0, secondSegment.start); // 20.0 - 2.0
                Assert.Equal(23.0, secondSegment.end);   // 21.0 + 2.0
            }

            [Fact]
            public void ShouldAdjustPaddingForOverlaps_WhenSegmentsWouldOverlap()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "First", StartSecond = 10.0, EndSecond = 11.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Second", StartSecond = 12.0, EndSecond = 13.0 }
                        }
                    }
                };

                // Apply 2 second padding that would cause overlap (gap is only 1 second: 11.0 to 12.0)
                // With Equally strategy, padding is reduced equally from both segments
                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2,
                    paddingStrategy: PaddingStrategy.Equally);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                Assert.Equal(2, deserializedTranscript!.segments.Count);

                var firstSegment = deserializedTranscript.segments[0];
                var secondSegment = deserializedTranscript.segments[1];

                // First segment should use full back padding (no previous segment to collide with)
                Assert.Equal(8.0, firstSegment.start);   // 10.0 - 2.0

                // With Equally strategy: available gap is 1.0 (11.0 to 12.0), split equally
                // first.end = 11.0 + 0.5 = 11.5
                // second.start = 12.0 - 0.5 = 11.5
                Assert.Equal(11.5, firstSegment.end);
                Assert.Equal(11.5, secondSegment.start);
                Assert.Equal(15.0, secondSegment.end);    // 13.0 + 2.0
                
                // Verify no overlap
                Assert.True(secondSegment.start >= firstSegment.end, 
                    "Segments should not overlap");
            }

            [Fact]
            public void ShouldPrioritizeEndPadding_WhenSegmentsWouldOverlap()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "First", StartSecond = 10.0, EndSecond = 11.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Second", StartSecond = 12.0, EndSecond = 13.0 }
                        }
                    }
                };

                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2,
                    paddingStrategy: PaddingStrategy.PrioritizeEndPadding);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                var firstSegment = deserializedTranscript!.segments[0];
                var secondSegment = deserializedTranscript.segments[1];

                // With PrioritizeEndPadding: prev segment gets priority for end padding
                // Gap is 1.0 (12.0 - 11.0). End padding gets min(2.0, 1.0) = 1.0
                // Start padding gets 1.0 - 1.0 = 0.0
                Assert.Equal(8.0, firstSegment.start);    // 10.0 - 2.0 (unchanged)
                Assert.Equal(12.0, firstSegment.end);     // 11.0 + 1.0 (full gap for end padding)
                Assert.Equal(12.0, secondSegment.start);  // 12.0 - 0.0 (no start padding left)
                Assert.Equal(15.0, secondSegment.end);    // 13.0 + 2.0
            }

            [Fact]
            public void ShouldPrioritizeStartPadding_WhenSegmentsWouldOverlap()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "First", StartSecond = 10.0, EndSecond = 11.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Second", StartSecond = 12.0, EndSecond = 13.0 }
                        }
                    }
                };

                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2,
                    paddingStrategy: PaddingStrategy.PrioritizeStartPadding);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                var firstSegment = deserializedTranscript!.segments[0];
                var secondSegment = deserializedTranscript.segments[1];

                // With PrioritizeStartPadding: current segment gets priority for start padding
                // Gap is 1.0 (12.0 - 11.0). Start padding gets min(2.0, 1.0) = 1.0
                // End padding gets 1.0 - 1.0 = 0.0
                Assert.Equal(8.0, firstSegment.start);    // 10.0 - 2.0 (unchanged)
                Assert.Equal(11.0, firstSegment.end);     // 11.0 + 0.0 (no end padding left)
                Assert.Equal(11.0, secondSegment.start);  // 12.0 - 1.0 (full gap for start padding)
                Assert.Equal(15.0, secondSegment.end);    // 13.0 + 2.0
            }

            [Fact]
            public void ShouldDistributePaddingFairly_WhenGapIsLarger()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "First", StartSecond = 10.0, EndSecond = 11.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Second", StartSecond = 14.0, EndSecond = 15.0 }
                        }
                    }
                };

                // Gap is 3.0 seconds (14.0 - 11.0), both padding params are 2.0
                var jsonContentsEqual = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2,
                    paddingStrategy: PaddingStrategy.Equally);

                var transcriptEqual = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContentsEqual);
                Assert.NotNull(transcriptEqual);
                var firstSegEqual = transcriptEqual!.segments[0];
                var secondSegEqual = transcriptEqual.segments[1];
                
                // With Equally: gap is split 50/50 (1.5 each)
                Assert.Equal(8.0, firstSegEqual.start);   // 10.0 - 2.0
                Assert.Equal(12.5, firstSegEqual.end);    // 11.0 + 1.5
                Assert.Equal(12.5, secondSegEqual.start); // 14.0 - 1.5
                Assert.Equal(17.0, secondSegEqual.end);   // 15.0 + 2.0

                // With PrioritizeEndPadding: prev gets full 2.0, current gets remaining 1.0
                var jsonContentsPriorEnd = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2,
                    paddingStrategy: PaddingStrategy.PrioritizeEndPadding);

                var transcriptPriorEnd = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContentsPriorEnd);
                Assert.NotNull(transcriptPriorEnd);
                var firstSegPriorEnd = transcriptPriorEnd!.segments[0];
                var secondSegPriorEnd = transcriptPriorEnd.segments[1];
                
                Assert.Equal(8.0, firstSegPriorEnd.start);  // 10.0 - 2.0
                Assert.Equal(13.0, firstSegPriorEnd.end);   // 11.0 + 2.0 (full requested end padding)
                Assert.Equal(13.0, secondSegPriorEnd.start); // 14.0 - 1.0 (remainder of gap)
                Assert.Equal(17.0, secondSegPriorEnd.end);  // 15.0 + 2.0

                // With PrioritizeStartPadding: current gets full 2.0, prev gets remaining 1.0
                var jsonContentsPriorStart = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2,
                    paddingStrategy: PaddingStrategy.PrioritizeStartPadding);

                var transcriptPriorStart = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContentsPriorStart);
                Assert.NotNull(transcriptPriorStart);
                var firstSegPriorStart = transcriptPriorStart!.segments[0];
                var secondSegPriorStart = transcriptPriorStart.segments[1];
                
                Assert.Equal(8.0, firstSegPriorStart.start);  // 10.0 - 2.0
                Assert.Equal(12.0, firstSegPriorStart.end);   // 11.0 + 1.0 (remainder of gap)
                Assert.Equal(12.0, secondSegPriorStart.start); // 14.0 - 2.0 (full requested start padding)
                Assert.Equal(17.0, secondSegPriorStart.end);  // 15.0 + 2.0
            }

            [Fact]
            public void ShouldHandleOverlappingWords_WithoutMakingItWorse()
            {
                var generator = new AutoSubsJsonContentsGenerator();

                var lines = new List<LyricLine>
                {
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "First", StartSecond = 10.0, EndSecond = 12.0 }
                        }
                    },
                    new()
                    {
                        Words = new ObservableCollection<LyricWord>
                        {
                            new() { Text = "Second", StartSecond = 11.0, EndSecond = 13.0 }
                        }
                    }
                };

                // Words overlap: first ends at 12.0, second starts at 11.0 (gap = -1.0)
                // Padding should not make this worse
                var jsonContents = generator.GenerateAutoSubsJsonContents("test.json", lines,
                    padSegmentStartBackFromFirstWordStartSeconds: 2,
                    padSegmentEndForwardFromLastWordEndSeconds: 2);

                var deserializedTranscript = JsonSerializer.Deserialize<AutoSubsTranscript>(jsonContents);

                Assert.NotNull(deserializedTranscript);
                var firstSegment = deserializedTranscript!.segments[0];
                var secondSegment = deserializedTranscript.segments[1];

                // First segment gets full start padding, but end padding gets trimmed to avoid overlap
                Assert.Equal(8.0, firstSegment.start);    // 10.0 - 2.0
                Assert.Equal(12.0, firstSegment.end);     // 12.0 (raw end, no end padding due to overlap)

                // Second segment: gap is negative, so just use raw start boundary to avoid making overlap worse
                Assert.Equal(11.0, secondSegment.start);  // 11.0 (raw start, no start padding due to negative gap)
                Assert.Equal(15.0, secondSegment.end);    // 13.0 + 2.0

                // Note: there's still a word overlap (11.0-12.0), but that's unavoidable
                // We at least didn't make it worse by padding into it
            }
        }
    }
}