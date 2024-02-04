using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Avalonia;
using Avalonia.Svg.Skia;

namespace KaddaOK.AvaloniaApp.Windows;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp()
               .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            new NativeErrorMessage(e).ShowDialog();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        GC.KeepAlive(typeof(SvgImageExtension).Assembly);
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}
