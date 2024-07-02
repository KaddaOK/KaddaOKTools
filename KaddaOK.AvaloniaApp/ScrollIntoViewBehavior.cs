using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Mvvm.Input;
using KaddaOK.AvaloniaApp.Models;

namespace KaddaOK.AvaloniaApp
{
    public class ScrollIntoViewMessage
    {
        public TimingWord Item { get; }

        public ScrollIntoViewMessage(TimingWord item)
        {
            Item = item;
        }
    }
}
