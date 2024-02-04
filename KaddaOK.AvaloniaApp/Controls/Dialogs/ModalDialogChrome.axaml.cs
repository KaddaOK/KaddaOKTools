using Avalonia;
using Avalonia.Controls;

namespace KaddaOK.AvaloniaApp.Controls.Dialogs
{
    public partial class ModalDialogChrome : UserControl
    {
        #region Title property
        public static readonly DirectProperty<ModalDialogChrome, string?> TitleProperty =
            AvaloniaProperty.RegisterDirect<ModalDialogChrome, string?>(nameof(Title), o => o.Title, (o, v) => o.Title = v);

        private string? _title;

        public string? Title
        {
            get => _title;
            set => SetAndRaise(TitleProperty, ref _title, value);
        }
        #endregion

        public ModalDialogChrome()
        {
            InitializeComponent();
        }
    }
}
