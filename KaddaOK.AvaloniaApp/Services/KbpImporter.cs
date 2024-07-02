using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;
using KaddaOK.Library.Kbs;

namespace KaddaOK.AvaloniaApp.Services
{
    public interface IKbpImporter
    {
        Task ImportKbpAsync(KaraokeProcess karaokeProcess, string kbpFilePath);
    }

    public class KbpImporter : Importer, IKbpImporter
    {
        private IKbpSerializer Serializer { get; }
        public KbpImporter(IKbpSerializer serializer, IAudioFromFile audioFileReader, IMinMaxFloatWaveStreamSampler sampler) : base(audioFileReader, sampler)
        {
            Serializer = serializer;
        }

        public Color FromKbpPaletteColor(KbpPaletteColor from)
        {
            return new Color(255, from.Red, from.Green, from.Blue);
        }

        public async Task ImportKbpAsync(KaraokeProcess karaokeProcess, string kbpFilePath)
        {
            var originalContents = File.ReadAllText(kbpFilePath);
            
            var kbpFile = Serializer.Deserialize(originalContents);

            karaokeProcess.ImportedKaraokeSourceFilePath = kbpFilePath;
            karaokeProcess.OriginalImportedKbpFile = kbpFile;
            karaokeProcess.ExportToFilePath = kbpFilePath;
            karaokeProcess.KaraokeSource = InitialKaraokeSource.KbpImport;

            var palette = kbpFile.Header?.PaletteColors.Select(FromKbpPaletteColor).ToList();

            if (palette != null)
            {
                karaokeProcess.BackgroundColor = palette[0];

                // TODO: warn that we're only loading colors from the first style
                var firstStyle = kbpFile.Header!.Styles[0];

                karaokeProcess.SungTextColor = palette[firstStyle.TextWipeColorPaletteIndex];
                karaokeProcess.SungOutlineColor = palette[firstStyle.OutlineWipeColorPaletteIndex];
                karaokeProcess.UnsungTextColor = palette[firstStyle.TextColorPaletteIndex];
                karaokeProcess.UnsungOutlineColor = palette[firstStyle.OutlineColorPaletteIndex];
            }

            var lines = new List<LyricLine>();
            for (var pageIndex = 0; pageIndex < (kbpFile.Pages!.Count); pageIndex++)
            {
                lines.AddRange(GetFromKbpPage(kbpFile.Pages, pageIndex));
            }
            karaokeProcess.ChosenLines =
                new ObservableCollection<LyricLine>(lines);

            // I *think* the path is either fully absolute or omitted because adjacent, no relative path
            var audioPath = Path.IsPathRooted(kbpFile.Header?.Audio)
                ? kbpFile.Header.Audio
                : Path.Combine(Path.GetDirectoryName(kbpFilePath)!, kbpFile.Header?.Audio ?? "");

            await LoadAudioIntoProcess(karaokeProcess, audioPath);

            karaokeProcess.RaiseChosenLinesChanged();
            karaokeProcess.CanExportFactorsChanged();
        }

        private List<LyricLine> GetFromKbpPage(List<PageV2> pages, int pageIndex)
        {
            return pages[pageIndex].Lines.Select(l => new LyricLine
            {
                IsSelected = true,
                PageIndex = pageIndex,
                KbpStyleIndex = l.StyleIndex,
                KbpIsFixedText = l.IsFixedText,
                KbpAlignmentType = l.Alignment,
                Words = new ObservableCollection<LyricWord>(
                    l.Words.Select(w => new LyricWord
                    {
                        Text = w.Text,
                        EndSecond = w.EndTicks / 100D,
                        StartSecond = w.StartTicks / 100D
                    }))
            }).ToList();
        }
    }
}
