using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaddaOK.Library;
using Microsoft.CognitiveServices.Speech;
using NAudio.Wave;
using Newtonsoft.Json;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DesignTimeEditLinesViewModel : EditLinesViewModel
    {
        public DesignTimeEditLinesViewModel() : base(DesignTimeKaraokeProcess.Get(), new LineSplitter(), new WordMerger(), new MinMaxFloatWaveStreamSampler())
        {
            UndoStack.Add(new ChosenLinesAction("[]", "did this before this view"));
            UndoStack.Add(new ChosenLinesAction("[]", "and then I did this"));
            RedoStack.Add(new ChosenLinesAction("[]", "already undid this first"));
            RedoStack.Add(new ChosenLinesAction("[]", "then undid this"));
        }
    }
}
