using System.Drawing;
using KaddaOK.Library.Ytmm;
using SkiaSharp;

namespace KaddaOK.Library
{
    public interface IRzProjectGenerator
    {
        RzProject GenerateProject(string rzlrcFileName,
            decimal audioLengthSeconds,
            string? vocalsOnlyAudioPath,
            string? unseparatedAudioPath,
            string? noVocalsAudioPath,
            string? videoPath,
            bool? videoAsOverlay = null,
            decimal? rzVideoLengthSeconds = null,
            SKColor? backgroundColor = null,
            RzProject? template = null);

        void InsertProgressBars(
            RzProject project,
            List<LyricLine> lyrics,
            decimal minimumGapLength,
            int width,
            int height,
            int yPosition,
            SKColor fill,
            SKColor outline,
            int screenWidth = 1920);
    }
    public class RzProjectGenerator : IRzProjectGenerator
    {
        public RzProject GenerateProject(string rzlrcFileName,
            decimal audioLengthSeconds,
            string? vocalsOnlyAudioPath,
            string? unseparatedAudioPath,
            string? noVocalsAudioPath,
            string? videoPath,
            bool? videoAsOverlay = null,
            decimal? rzVideoLengthSeconds = null,
            SKColor? backgroundColor = null,
            RzProject? template = null)
        {
            // TODO: background color

            var offset = 0.1M; // TODO: parameter/configurable?

            var project = template ?? new RzProject();
            var endOfContent = Math.Max(audioLengthSeconds, rzVideoLengthSeconds ?? 0) + offset;
            if (template != null)
            {
                // move items at greater than 60 to end of music/video proportionally
                var itemsToMove = project.Lines
                    .SelectMany(l => l.Items)
                    .Where(i => i.dEditorStartTime > 60)
                    .ToList();

                var endOfItems = itemsToMove.Max(d => d.dEditorEndTime);
                var offsetToAdd = endOfContent - endOfItems;
                foreach (var item in itemsToMove)
                {
                    item.dEditorStartTime += offsetToAdd;
                    item.dEditorEndTime += offsetToAdd;
                }
            }

            if (backgroundColor != null)
            {
                project.BackgroundLine.Items.Add(new RzLineItem
                {
                    dxScale = 1,
                    dyScale = 1,
                    dzScale = 1,
                    dEditorEndTime = endOfContent,
                    eMediaType = 2,
                    eMediaTypeLine = (int)RzLinesSpec.Background,
                    colorblk = new LineItemColorblk
                    {
                        clr = RzlrcContentsGenerator.GetBGRUintColorFromSKColor(backgroundColor)!.Value
                    }
                });
            }

            if (!string.IsNullOrWhiteSpace(rzlrcFileName))
            {
                project.KaraokeLine.Items.Add(new RzLineItem
                {
                    dEditStartTime = 0,
                    dEditEndTime = audioLengthSeconds,
                    eMediaType = 6,
                    eMediaTypeLine = (int)RzLinesSpec.Text0,
                    dxScale = 1,
                    dyScale = 1, 
                    dzScale = 1,
                    source = rzlrcFileName
                });
            }

            if (!string.IsNullOrWhiteSpace(videoPath))
            {
                var videoItem = new RzLineItem
                {
                    dxScale = 1,
                    dyScale = 1,
                    dzScale = 1,
                    bDisableAudio = 1,
                    bHadAudio = 1,
                    dASelfDuration = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dASelfEndTime = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dVSelfDuration = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dVSelfEndTime = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dEditorStartTime = offset,
                    dEditorEndTime = (rzVideoLengthSeconds ?? audioLengthSeconds) + offset,
                    dPlaybackRate = 1,
                    dVoiceScale = 1,
                    eMediaType = 0,
                    source = videoPath
                };
                if (videoAsOverlay == true)
                {
                    videoItem.eMediaTypeLine = (int)RzLinesSpec.Overlay0;
                    videoItem.OverlaynWidth = 500;
                    videoItem.OverlaynHeight = 500;
                    project.VideoOverlayLine.Items.Add(videoItem);
                }
                else
                {
                    videoItem.eMediaTypeLine = (int)RzLinesSpec.Video;
                    project.VideoLine.Items.Add(videoItem);
                }


                project.VideoSoundLine.Items.Add(new RzLineItem
                {
                    bHadAudio = 1,
                    dASelfDuration = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dASelfEndTime = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dVSelfDuration = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dVSelfEndTime = rzVideoLengthSeconds ?? audioLengthSeconds,
                    dEditorStartTime = offset,
                    dEditorEndTime = (rzVideoLengthSeconds ?? audioLengthSeconds) + offset,
                    dPlaybackRate = 1,
                    dVoiceScale = 1,
                    eMediaType = 0,
                    eMediaTypeLine = (int)RzLinesSpec.Audio0,
                    source = videoPath
                });
            }

            if (!string.IsNullOrWhiteSpace(unseparatedAudioPath))
            {
                project.OriginalAudioLine.Items.Add(new RzLineItem
                {
                    bHadAudio = 1,
                    dASelfDuration = audioLengthSeconds,
                    dASelfEndTime = audioLengthSeconds,
                    dEditorStartTime = offset,
                    dEditorEndTime = audioLengthSeconds + offset,
                    dPlaybackRate = 1,
                    dVoiceScale = 1,
                    eMediaType = 1,
                    eMediaTypeLine = (int)RzLinesSpec.Audio1,
                    source = unseparatedAudioPath
                });
            }

            if (!string.IsNullOrWhiteSpace(noVocalsAudioPath))
            {
                project.NoVocalsAudioLine.Items.Add(new RzLineItem
                {
                    bHadAudio = 1,
                    dASelfDuration = audioLengthSeconds,
                    dASelfEndTime = audioLengthSeconds,
                    dEditorStartTime = offset,
                    dEditorEndTime = audioLengthSeconds + offset,
                    dPlaybackRate = 1,
                    dVoiceScale = 1,
                    eMediaType = 1,
                    eMediaTypeLine = (int)RzLinesSpec.Audio2,
                    source = noVocalsAudioPath
                });
            }

            if (!string.IsNullOrWhiteSpace(vocalsOnlyAudioPath))
            {
                project.VocalsOnlyAudioLine.Items.Add(new RzLineItem
                {
                    bHadAudio = 1,
                    dASelfDuration = audioLengthSeconds,
                    dASelfEndTime = audioLengthSeconds,
                    dEditorStartTime = offset,
                    dEditorEndTime = audioLengthSeconds + offset,
                    dPlaybackRate = 1,
                    dVoiceScale = 1,
                    eMediaType = 1,
                    eMediaTypeLine = (int)RzLinesSpec.Audio3,
                    source = vocalsOnlyAudioPath
                });
            }

            return project;
        }

        public void InsertProgressBars(
                                            RzProject project, 
                                            List<LyricLine> lyrics, 
                                            decimal minimumGapLength,
                                            int width, 
                                            int height, 
                                            int yPosition,  
                                            SKColor fill,
                                            SKColor outline,
                                            int screenWidth = 1920)
        {
            decimal songStartGapSeconds = 10; // TODO: parameter/configurable?
            decimal eventPaddingSeconds = 2; // this value determined by observation, might need tweaking
            foreach (var lyric in lyrics)
            {
                decimal startSecond;
                if (lyric == lyrics.First())
                {
                    startSecond = songStartGapSeconds;
                }
                else
                {
                    var previousItem = lyrics[lyrics.IndexOf(lyric) - 1];
                    startSecond = (decimal)previousItem.EndSecond + eventPaddingSeconds;
                }

                var endSecond = (decimal)lyric.StartSecond - eventPaddingSeconds;
                if (endSecond - startSecond > minimumGapLength)
                {
                    var progressBar = GenerateProgressBar(startSecond, endSecond, width, height, yPosition, fill,
                        outline, screenWidth);
                    project.GraffitiLine.Items.Add(progressBar);
                }
            }
        }

        public RzLineItem GenerateProgressBar(
                                                decimal startTime, 
                                                decimal endTime, 
                                                int width, 
                                                int height, 
                                                int yPosition, 
                                                SKColor fill, 
                                                SKColor outline, 
                                                int screenWidth = 1920)
        {
            var barMargin = (screenWidth - width) / 2;
            return new RzLineItem
            {
                dxScale = 1,
                dyScale = 1,
                dzScale = 1,
                dEditorEndTime = endTime,
                dEditorStartTime = startTime,
                eMediaType = 8,
                eMediaTypeLine = (int)RzLinesSpec.Graffiti,
                source = "Progress Bar",
                GraffitiLayers = new List<GraffitiLayer>
                {
                    new()
                    {
                        bVisible = 1,
                        dDrawTime = endTime - startTime,
                        lpszName = "fill",
                        obj = new GraffitiObj
                        {
                            clrPen = RzlrcContentsGenerator.GetBGRUintColorFromSKColor(fill)!.Value,
                            dTransparent = 1 - (fill.Alpha / 255M),
                            nPenWidth = height,
                            nShapeType=1,
                            nOutlineWidth=4,
                            nOutlineType=0,
                            bSmooth=1,
                            Points = new List<GraffitiPoint>
                            {
                                new()
                                {
                                    X = barMargin,
                                    Y = yPosition
                                },
                                new()
                                {
                                    X = screenWidth - barMargin,
                                    Y = yPosition
                                },
                            }
                        }
                    },
                    new()
                    {
                        bVisible = 1,
                        dDrawTime = 0,
                        lpszName = "outline",
                        obj = new GraffitiObj
                        {
                            clrPen = RzlrcContentsGenerator.GetBGRUintColorFromSKColor(outline)!.Value,
                            dTransparent = 1 - (outline.Alpha / 255M),
                            nPenWidth = height,
                            nShapeType=1,
                            nOutlineWidth=4,
                            nOutlineType=2,
                            bSmooth=1,
                            Points = new List<GraffitiPoint>
                            {
                                new()
                                {
                                    X = barMargin,
                                    Y = yPosition
                                },

                                new()
                                {
                                    X = screenWidth - barMargin,
                                    Y = yPosition
                                },
                            }
                        }
                    }
                }
            };
        }
    }
}
