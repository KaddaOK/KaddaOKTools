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
using NAudio.Wave;
using Newtonsoft.Json;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DesignTimeNarrowingViewModel : NarrowingViewModel
    {
        public DesignTimeNarrowingViewModel() : base(DesignTimeKaraokeProcess.Get())
        {
        }
    }
}
