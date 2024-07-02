using System.Collections.Generic;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;

namespace KaddaOK.AvaloniaApp.Controls;

public partial class InitialSyncOption : UserControl
{
    public InitialSyncOption()
    {
        InitializeComponent();
        MoreInfoContent = new List<Control>();
    }
    
    public static readonly StyledProperty<ICommand> ChooseCommandProperty = AvaloniaProperty.Register<InitialSyncOption, ICommand>(nameof(ChooseCommand));
    public static readonly StyledProperty<string> OptionTitleProperty = AvaloniaProperty.Register<InitialSyncOption, string>(nameof(OptionTitle));
    public static readonly StyledProperty<string> OptionSummaryProperty = AvaloniaProperty.Register<InitialSyncOption, string>(nameof(OptionSummary));
    public static readonly StyledProperty<IEnumerable<Control>> MoreInfoContentProperty = AvaloniaProperty.Register<InitialSyncOption, IEnumerable<Control>>(nameof(MoreInfoContent));
    public static readonly StyledProperty<DrawingImage> IconDrawingProperty = AvaloniaProperty.Register<InitialSyncOption, DrawingImage>(nameof(IconDrawing));
    public static readonly StyledProperty<bool> MoreInfoVisibleProperty = AvaloniaProperty.Register<InitialSyncOption, bool>(nameof(MoreInfoVisible));

    public ICommand ChooseCommand
    {
        get => GetValue(ChooseCommandProperty);
        set => SetValue(ChooseCommandProperty, value);
    }
    
    public string OptionTitle
    {
        get => GetValue(OptionTitleProperty);
        set => SetValue(OptionTitleProperty, value);
    }

    public string OptionSummary
    {
        get => GetValue(OptionSummaryProperty);
        set => SetValue(OptionSummaryProperty, value);
    }

    public IEnumerable<Control> MoreInfoContent
    {
        get => GetValue(MoreInfoContentProperty);
        set => SetValue(MoreInfoContentProperty, value);
    }

    public DrawingImage IconDrawing
    {
        get => GetValue(IconDrawingProperty);
        set => SetValue(IconDrawingProperty, value);
    }

    // Encapsulated properties and commands
    public bool MoreInfoVisible
    {
        get => GetValue(MoreInfoVisibleProperty);
        set => SetValue(MoreInfoVisibleProperty, value);
    }

    [RelayCommand]
    public void HideMoreInfo(object? parameter)
    {
        MoreInfoVisible = false;
    }
    [RelayCommand]
    public void ShowMoreInfo(object? parameter)
    {
        MoreInfoVisible = true;
    }
}