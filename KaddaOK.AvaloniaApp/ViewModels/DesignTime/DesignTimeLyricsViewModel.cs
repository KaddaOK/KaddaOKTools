using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DesignTimeLyricsViewModel : LyricsViewModel
    {
        public DesignTimeLyricsViewModel() : base(DesignTimeKaraokeProcess.Get(), null!)
        {
            var lyricsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Assets", "DesignTime", "Example Song - Lyrics.txt");
            var lyrics = File.ReadAllText(lyricsPath);
            LyricEditorText = lyrics;
        }
    }
}
