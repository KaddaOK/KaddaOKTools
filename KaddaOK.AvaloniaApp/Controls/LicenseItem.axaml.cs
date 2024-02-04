using Avalonia;
using Avalonia.Controls;
using Avalonia.Metadata;
using KaddaOK.AvaloniaApp.Controls.Dialogs;

namespace KaddaOK.AvaloniaApp.Controls
{
    public partial class LicenseItem : UserControl
    {
        #region PackageName property
        public static readonly DirectProperty<LicenseItem, string?> PackageNameProperty =
            AvaloniaProperty.RegisterDirect<LicenseItem, string?>(nameof(PackageName), o => o.PackageName, (o, v) => o.PackageName = v);

        private string? _packageName;

        public string? PackageName
        {
            get => _packageName;
            set => SetAndRaise(PackageNameProperty, ref _packageName, value);
        }
        #endregion

        public LicenseItem()
        {
            InitializeComponent();
        }
    }
}
