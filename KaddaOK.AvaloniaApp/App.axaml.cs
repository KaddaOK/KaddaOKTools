using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.AvaloniaApp.ViewModels;
using KaddaOK.AvaloniaApp.Views;
using KaddaOK.Library;
using Microsoft.Extensions.DependencyInjection;

namespace KaddaOK.AvaloniaApp;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = null!;
    public static KaraokeProcess KaraokeProcess { get; private set; } = null!;
    public static MainWindow MainWindow { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        var services = new ServiceCollection();

        KaraokeProcess = new KaraokeProcess
        {
            KaraokeSource = InitialKaraokeSource.AzureSpeechService
        };

        services.AddSingleton(KaraokeProcess);
        services.AddTransient<IAudioFileLengthChecker, AudioFileLengthChecker>();
        services.AddTransient<IAudioFromFile, AudioFromFile>();
        services.AddTransient<IAzureRecognizer, AzureRecognizer>();
        services.AddTransient<IKaddaOKSettingsPersistor, KaddaOKSettingsPersistor>();
        services.AddTransient<IKbpContentsGenerator, KbpContentsGenerator>();
        services.AddTransient<IKbpImporter, KbpImporter>();
        services.AddTransient<IKbpSerializer, KbpSerializer>();
        services.AddTransient<ILineSplitter, LineSplitter>();
        services.AddTransient<IMinMaxFloatWaveStreamSampler, MinMaxFloatWaveStreamSampler>();
        services.AddTransient<IResultRanker, ResultRanker>();
        services.AddTransient<IRzlrcContentsGenerator, RzlrcContentsGenerator>();
        services.AddTransient<IRzlrcImporter, RzlrcImporter>();
        services.AddTransient<IRzProjectGenerator, RzProjectGenerator>();
        services.AddTransient<IRzProjectSerializer, RzProjectSerializer>();

        services.AddTransient<AboutViewModel>();
        services.AddTransient<AudioViewModel>();
        services.AddTransient<EditLinesViewModel>();
        services.AddTransient<ExportViewModel>();
        services.AddTransient<LyricsViewModel>();
        services.AddTransient<NarrowingViewModel>();
        services.AddTransient<RecognizeViewModel>();

        // Build the service provider
        ServiceProvider = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(KaraokeProcess)
            };
            desktop.MainWindow = MainWindow;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(KaraokeProcess)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
