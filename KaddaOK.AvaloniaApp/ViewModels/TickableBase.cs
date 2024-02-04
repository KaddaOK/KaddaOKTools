using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    /// <summary>
    /// per https://github.com/kekekeks/Avalonia.BattleCity/blob/master/Model/GameBase.cs
    /// </summary>
    public abstract class TickableBase : DrawsFullLengthVocalsBase
    {
        public const int TicksPerSecond = 100;
        public long CurrentTick { get; private set; }

        private readonly DispatcherTimer _timer = new DispatcherTimer()
            { Interval = new TimeSpan(0, 0, 0, 0, 1000 / TicksPerSecond) };


        void DoTick()
        {
            Tick();
            CurrentTick++;
        }

        protected abstract void Tick();

        protected TickableBase(KaraokeProcess karaokeProcess) : base(karaokeProcess)
        {
            _timer.Tick += delegate { DoTick(); };
        }

        public void StartTicking() => _timer.IsEnabled = true;
        public void StopTicking() => _timer.IsEnabled = false;
    }
}