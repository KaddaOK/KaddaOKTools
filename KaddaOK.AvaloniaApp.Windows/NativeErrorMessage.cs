using System;
using System.Windows.Forms;

namespace KaddaOK.AvaloniaApp.Windows
{
    public partial class NativeErrorMessage : Form
    {
        private Exception exception { get; }
        public NativeErrorMessage(Exception exception)
        {
            InitializeComponent();
            this.exception = exception;
            this.errorMessageTextBox.Text = exception.Message;
            this.stackTraceTextBox.Text = exception.StackTrace;
        }

        private void ErrorMessageCopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(exception.Message);
        }

        private void StackTraceCopyButton_Click(object sender, EventArgs e)
        {
            if (exception.StackTrace != null)
            {
                Clipboard.SetText(exception.StackTrace);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
