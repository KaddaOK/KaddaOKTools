using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using DialogHostAvalonia;

namespace KaddaOK.AvaloniaApp.Controls.Dialogs
{
    public partial class EditSyllableTextDialog : UserControl
    {
        private DialogHost? dialogHost;

        public EditSyllableTextDialog()
        {
            InitializeComponent();
        }

        private void TextBox_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
        {
            (sender as TextBox)?.Focus();
            dialogHost = this.FindAncestorOfType<DialogHost>();
        }

        private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dialogHost?.CloseDialogCommand.Execute(((TextBox)sender).Text);
            }

            if (e.Key == Key.Escape)
            {
                dialogHost?.CloseDialogCommand.Execute(null);
            }
        }
    }
}
