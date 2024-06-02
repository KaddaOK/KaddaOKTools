using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.AvaloniaApp.Services;
using KaddaOK.AvaloniaApp.Views;
using KaddaOK.Library;
using KaddaOK.Library.Ytmm;

namespace KaddaOK.AvaloniaApp.ViewModels
{
    public partial class StartViewModel : DrawsFullLengthVocalsBase
    {
        private bool _gettingFile;
        public bool GettingFile
        {
            get => _gettingFile;
            set => SetProperty(ref _gettingFile, value);
        }

        private IRzlrcImporter RzlrcImporter { get; }
        private IKbpImporter KbpImporter { get; }

        public WindowNotificationManager? NotificationManager { get; set; }

        public StartViewModel(KaraokeProcess karaokeProcess, IRzlrcImporter rzlrcImporter, IKbpImporter kbpImporter) : base(karaokeProcess)
        {
            RzlrcImporter = rzlrcImporter;
            KbpImporter = kbpImporter;
        }

        private bool manualInstructionsVisible;
        public bool ManualInstructionsVisible
        {
            get => manualInstructionsVisible;
            set => SetProperty(ref manualInstructionsVisible, value);
        }

        [RelayCommand]
        public void HideManualInstructions(object? parameter)
        {
            ManualInstructionsVisible = false;
        }
        [RelayCommand]
        public void ShowManualInstructions(object? parameter)
        {
            ManualInstructionsVisible = true;
        }

        private bool alignerInstructionsVisible;
        public bool AlignerInstructionsVisible
        {
            get => alignerInstructionsVisible;
            set => SetProperty(ref alignerInstructionsVisible, value);
        }

        [RelayCommand]
        public void HideAlignerInstructions(object? parameter)
        {
            AlignerInstructionsVisible = false;
        }
        [RelayCommand]
        public void ShowAlignerInstructions(object? parameter)
        {
            AlignerInstructionsVisible = true;
        }

        private bool azureInstructionsVisible;
        public bool AzureInstructionsVisible
        {
            get => azureInstructionsVisible;
            set => SetProperty(ref azureInstructionsVisible, value);
        }

        [RelayCommand]
        public void HideAzureInstructions(object? parameter)
        {
            AzureInstructionsVisible = false;
        }
        [RelayCommand]
        public void ShowAzureInstructions(object? parameter)
        {
            AzureInstructionsVisible = true;
        }



        private bool kbsImportInstructionsVisible;
        public bool KbsImportInstructionsVisible
        {
            get => kbsImportInstructionsVisible;
            set => SetProperty(ref kbsImportInstructionsVisible, value);
        }

        [RelayCommand]
        public void HideKbsImportInstructions(object? parameter)
        {
            KbsImportInstructionsVisible = false;
        }
        [RelayCommand]
        public void ShowKbsImportInstructions(object? parameter)
        {
            KbsImportInstructionsVisible = true;
        }


        private bool ytmmImportInstructionsVisible;
        public bool YtmmImportInstructionsVisible
        {
            get => ytmmImportInstructionsVisible;
            set => SetProperty(ref ytmmImportInstructionsVisible, value);
        }

        [RelayCommand]
        public void HideYtmmImportInstructions(object? parameter)
        {
            YtmmImportInstructionsVisible = false;
        }
        [RelayCommand]
        public void ShowYtmmImportInstructions(object? parameter)
        {
            YtmmImportInstructionsVisible = true;
        }

        [RelayCommand]
        public void SelectManualProcess()
        {
            CurrentProcess.KaraokeSource = InitialKaraokeSource.ManualSync;
            CurrentProcess.SelectedTabIndex = (int)TabIndexes.Audio;
        }

        [RelayCommand]
        private void LinkToForcedAligner()
        {
            UrlOpener.OpenUrl("https://github.com/KaddaOK/Forced-Aligner-for-Karaoke");
        }

        [RelayCommand]
        protected void ClearAll()
        {
            // TODO: show confirm
            CurrentProcess?.ClearAudioAndDownstream();
        }

        [RelayCommand]
        protected async Task Import()
        {
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select an existing file to import",
                FileTypeFilter = new FilePickerFileType[] { new ("Karaoke lyrics file")
                {
                    Patterns = new[] { "*.rzlrc", "*.kbp" }
                } }
            };
            try
            {
                Dispatcher.UIThread.Invoke(() => { GettingFile = true; });
                var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
                var result = results?.FirstOrDefault();
                if (result != null)
                {
                    var previousFilePath = CurrentProcess!.ImportedKaraokeSourceFilePath;
                    var existingKaraokeImportFilePath = result.TryGetLocalPath();

                    if (!string.IsNullOrWhiteSpace(existingKaraokeImportFilePath))
                    {
                        var extension = System.IO.Path.GetExtension(existingKaraokeImportFilePath);
                        switch (extension)
                        {
                            case ".rzlrc":
                                var imported = RzlrcImporter.ImportRzlrc(existingKaraokeImportFilePath);

                                // TODO: if (imported == null || imported.Count == 0) // inform and cancel
                                RzlrcLyric? selectedLayer = null;
                                if (imported.Count > 1) // select one
                                {
                                    var dialogResult = await DialogHost.Show(imported, "AudioViewDialogHost");
                                    selectedLayer = dialogResult as RzlrcLyric;
                                }
                                CurrentProcess.ClearAudioAndDownstream();
                                CurrentProcess.OriginalImportedRzlrcFile = imported;
                                CurrentProcess.OriginalImportedRzlrcPage = selectedLayer ?? imported.First();
                                await RzlrcImporter.LoadRzlrcPageIntoKaraokeProcessAsync(CurrentProcess, CurrentProcess.OriginalImportedRzlrcPage, existingKaraokeImportFilePath);
                                break;
                            case ".kbp":
                                CurrentProcess.ClearAudioAndDownstream();
                                await KbpImporter.ImportKbpAsync(CurrentProcess, existingKaraokeImportFilePath);
                                break;
                            default:
                                // TODO: inform better!
                                throw new InvalidOperationException($"Import of {extension} files not yet supported.");
                        }
                    }
                    CurrentProcess!.SelectedTabIndex = (int)TabIndexes.Edit;
                }
                GettingFile = false;
            }
            catch (Exception e)
            {
                // The user canceled or something went wrong
                if (NotificationManager != null)
                {
                    NotificationManager.Position = NotificationPosition.BottomRight;
                    NotificationManager.Show(new Notification("Error", $"An error occurred: {e.Message}", NotificationType.Error, TimeSpan.Zero));
                }
                GettingFile = false;
            }
        }

        [RelayCommand]
        public async Task SelectCtm()
        {
            if (App.MainWindow == null)
            {
                throw new InvalidOperationException(
                    "Couldn't find the reference to MainWindow in order to show a dialog");
            }

            var options = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                Title = "Select a .CTM file to import",
                FileTypeFilter = new FilePickerFileType[] { new ("NeMo Forced Aligner tokens .ctm file")
                {
                    Patterns = new[] { "*.ctm" }
                } }
            };

            try
            {
                Dispatcher.UIThread.Invoke(() => { GettingFile = true; });
                var results = await App.MainWindow.StorageProvider.OpenFilePickerAsync(options);
                var result = results?.FirstOrDefault();
                if (result != null)
                {

                    var ctmFilePath = result.TryGetLocalPath();

                    if (!string.IsNullOrWhiteSpace(ctmFilePath))
                    {
                        // CTM import process wants to have the lyrics already, so let's wait to do the import and go through audio and lyrics pages
                        CurrentProcess.ImportedKaraokeSourceFilePath = ctmFilePath;
                        CurrentProcess.KaraokeSource = InitialKaraokeSource.CtmImport;
                        CurrentProcess!.SelectedTabIndex = (int)TabIndexes.Audio;
                    }
                }
                GettingFile = false;
            }
            catch (Exception e)
            {
                // The user canceled or something went wrong
                if (NotificationManager != null)
                {
                    NotificationManager.Position = NotificationPosition.BottomRight;
                    NotificationManager.Show(new Notification("Error", $"An error occurred: {e.Message}", NotificationType.Error, TimeSpan.Zero));
                }
                GettingFile = false;
            }
        }
    }
}
