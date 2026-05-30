# KaddaOK Tools – Agent Instructions

A .NET desktop app that automates karaoke word-timing via Azure speech recognition, with editing, and export to YTMM (`.rzlrc`/`.rzmmpj`), KBS (`.kbp`), and AutoSubs (`.json`) formats.

See [README.md](README.md) for the feature overview and some known issues.

## Build & Test

```powershell
dotnet build KaddaOKTools.sln
dotnet test KaddaOKTools.sln
```

Target frameworks: `net6.0` (Library + its tests), `net7.0` (AvaloniaApp + its tests), `net7.0-windows` (Windows entry point).
`Nullable=enable` is set globally in [Directory.Build.props](Directory.Build.props) — all null handling must be explicit.

## Project Structure

| Project | Role |
|---------|------|
| `KaddaOK.Library` | Domain models, business logic, audio processing, export generators — **no UI dependencies** |
| `KaddaOK.AvaloniaApp` | Avalonia 11 UI: Views (`.axaml`), ViewModels, Converters, Controls |
| `KaddaOK.AvaloniaApp.Windows` | Thin WinExe entry point; native Windows APIs and COM interop |
| `KaddaOK.Library.Tests` | xUnit tests for the library |
| `KaddaOK.AvaloniaApp.Tests` | xUnit tests for ViewModels |

## Key Conventions

### Domain Models (`KaddaOK.Library`)
- All observable types extend `ObservableBase` and use the `SetProperty<T>(ref field, value)` helper (auto property-name via `[CallerMemberName]`).
- `LyricLine` and `LyricWord` both implement `IAudioSpan` (`StartSecond`, `EndSecond`).
- `KaraokeProcess` is the **single shared state container**; all ViewModels hold a reference to it via `ViewModelBase.CurrentProcess`.

### ViewModels (`KaddaOK.AvaloniaApp/ViewModels`)
- Inherit `ViewModelBase` → `ObservableBase`.
- Commands use **CommunityToolkit.MVVM** `[RelayCommand]` attribute (source-generated).
- Disabled-command tooltip reasons use `RelayCommandWithReason` / `IReportReasonCantExecute`.
- Undo/redo history is managed with `ObservableStack<ChosenLinesAction>` in `EditLinesViewModel`.

### Converters (`KaddaOK.AvaloniaApp/` root)
- Naming: `[Type]Converter` (e.g. `RoundingConverter`, `EnumEqualityBooleanConverter`).
- Implement Avalonia's `IValueConverter` or `IMultiValueConverter`.
- Kept in the project root (not a sub-folder).

### Controls (`KaddaOK.AvaloniaApp/Controls/`)
- Each control has a paired `.axaml` + `.axaml.cs` code-behind.
- Dialogs live in `Controls/Dialogs/`.
- Suffix convention: `Control` for complex controls (e.g. `DrawnWaveformControl`); omitted for simpler items (e.g. `LicenseItem`).

### Export Generators (`KaddaOK.Library`)
- `RzlrcContentsGenerator` → YTMM `.rzlrc` XML; colors as BGR `uint`.
- `KbpContentsGenerator` → KBS `.kbp` binary; uses `IKbpSerializer`.
- `RzProjectGenerator` → full YTMM `.rzmmpj` project with templates and progress bars.
- `AutoSubsJsonContentsGenerator` → auto-subs `.json`.

### Importers (`KaddaOK.AvaloniaApp/Services/`)
- Named `[Format]Importer` (e.g. `KbpImporter`, `RzlrcImporter`), inherit `ImporterBase`.

## Testing Conventions
- Framework: **xUnit** (`[Fact]` / `[Theory]` + `[InlineData]`).
- Test class naming: `[ClassName]Tests`.
- Group related tests with **nested classes** (e.g. `LineSplitterTests.SplitLineAt`).
- Shared setup lives in an inner **`TestHarness`** class with factory helpers.
