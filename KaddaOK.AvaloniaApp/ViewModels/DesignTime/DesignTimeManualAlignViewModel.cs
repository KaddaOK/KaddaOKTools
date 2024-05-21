using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DesignTimeManualAlignViewModel : ManualAlignViewModel
    {
        public DesignTimeManualAlignViewModel() : base(DesignTimeKaraokeProcess.Get(), new LineSplitter(), new WordMerger())
        {
        }
    }
}
