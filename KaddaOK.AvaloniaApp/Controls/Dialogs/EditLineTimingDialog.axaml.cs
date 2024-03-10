using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using KaddaOK.AvaloniaApp.ViewModels;

namespace KaddaOK.AvaloniaApp.Controls.Dialogs
{
    public partial class EditLineTimingDialog : UserControl
    {
        public EditLineTimingDialog()
        {
            InitializeComponent();
        }

        // TODO: this seems like a pretty unhinged thing to do. Probably wanna re-think this.
        public static readonly DirectProperty<EditLineTimingDialog, EditLinesViewModel?> EditLinesViewModelProperty =
            AvaloniaProperty.RegisterDirect<EditLineTimingDialog, EditLinesViewModel?>(nameof(EditLinesViewModel), o => o.EditLinesViewModel, (o, v) => o.EditLinesViewModel = v);
        private EditLinesViewModel? _editLinesViewModel;
        public EditLinesViewModel? EditLinesViewModel
        {
            get => _editLinesViewModel;
            set => SetAndRaise(EditLinesViewModelProperty, ref _editLinesViewModel, value);
        }

        private void NewRectanglesItemsControl_PointerPressed(object? sender, PointerPressedEventArgs args)
        {
            EditLinesViewModel?.EditLineTimingDialogPointerPressed(sender, args);
        }

        private void NewRectanglesItemsControl_PointerMoved(object? sender, PointerEventArgs args)
        {
            EditLinesViewModel?.EditLineTimingDialogPointerMoved(sender, args);
        }

        private void NewRectanglesItemsControl_PointerReleased(object? sender, PointerReleasedEventArgs args)
        {
            EditLinesViewModel?.EditLineTimingDialogPointerReleased(sender, args);
        }

        private void UserControl_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            EditLinesViewModel?.EditLineTimingDialogKeyDown(sender, e);
            e.Handled = true;
        }

        private void UserControl_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
        {
            this.Focus();
        }
    }
}
