using System.Drawing;

namespace KaddaOK.AvaloniaApp.Windows
{
    partial class NativeErrorMessage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NativeErrorMessage));
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            errorMessageTextBox = new System.Windows.Forms.RichTextBox();
            label3 = new System.Windows.Forms.Label();
            stackTraceTextBox = new System.Windows.Forms.RichTextBox();
            ErrorMessageCopyButton = new System.Windows.Forms.Button();
            StackTraceCopyButton = new System.Windows.Forms.Button();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(60, 25);
            label1.Name = "label1";
            label1.Size = new Size(472, 21);
            label1.TabIndex = 0;
            label1.Text = "Sorry, an unhandled error occurred! The application must now exit.";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 1;
            label2.Text = "Error Message:";
            // 
            // button1
            // 
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button1.Location = new Point(497, 326);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Close";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(15, 19);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(32, 32);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // errorMessageTextBox
            // 
            errorMessageTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            errorMessageTextBox.BackColor = SystemColors.Control;
            errorMessageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            errorMessageTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            errorMessageTextBox.Location = new Point(15, 85);
            errorMessageTextBox.Name = "errorMessageTextBox";
            errorMessageTextBox.ReadOnly = true;
            errorMessageTextBox.Size = new Size(557, 82);
            errorMessageTextBox.TabIndex = 5;
            errorMessageTextBox.Text = "This is example error text for the design view.  It will be replaced with the exception message at runtime.";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 180);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 6;
            label3.Text = "Stack Trace:";
            // 
            // stackTraceTextBox
            // 
            stackTraceTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            stackTraceTextBox.BackColor = SystemColors.Control;
            stackTraceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            stackTraceTextBox.Location = new Point(15, 200);
            stackTraceTextBox.Name = "stackTraceTextBox";
            stackTraceTextBox.Size = new Size(557, 112);
            stackTraceTextBox.TabIndex = 7;
            stackTraceTextBox.Text = resources.GetString("stackTraceTextBox.Text");
            // 
            // ErrorMessageCopyButton
            // 
            ErrorMessageCopyButton.BackColor = SystemColors.Control;
            ErrorMessageCopyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            ErrorMessageCopyButton.FlatAppearance.BorderSize = 0;
            ErrorMessageCopyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            ErrorMessageCopyButton.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point);
            ErrorMessageCopyButton.Location = new Point(537, 67);
            ErrorMessageCopyButton.Margin = new System.Windows.Forms.Padding(0);
            ErrorMessageCopyButton.Name = "ErrorMessageCopyButton";
            ErrorMessageCopyButton.Size = new Size(35, 21);
            ErrorMessageCopyButton.TabIndex = 8;
            ErrorMessageCopyButton.Text = "Copy";
            ErrorMessageCopyButton.TextAlign = ContentAlignment.TopCenter;
            toolTip1.SetToolTip(ErrorMessageCopyButton, "Copy message to clipboard");
            ErrorMessageCopyButton.UseVisualStyleBackColor = false;
            ErrorMessageCopyButton.Click += ErrorMessageCopyButton_Click;
            // 
            // StackTraceCopyButton
            // 
            StackTraceCopyButton.BackColor = SystemColors.Control;
            StackTraceCopyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            StackTraceCopyButton.FlatAppearance.BorderSize = 0;
            StackTraceCopyButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            StackTraceCopyButton.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point);
            StackTraceCopyButton.Location = new Point(537, 179);
            StackTraceCopyButton.Margin = new System.Windows.Forms.Padding(0);
            StackTraceCopyButton.Name = "StackTraceCopyButton";
            StackTraceCopyButton.Size = new Size(35, 21);
            StackTraceCopyButton.TabIndex = 9;
            StackTraceCopyButton.Text = "Copy";
            toolTip1.SetToolTip(StackTraceCopyButton, "Copy stack trace to clipboard");
            StackTraceCopyButton.UseVisualStyleBackColor = false;
            StackTraceCopyButton.Click += StackTraceCopyButton_Click;
            // 
            // NativeErrorMessage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(584, 361);
            Controls.Add(StackTraceCopyButton);
            Controls.Add(ErrorMessageCopyButton);
            Controls.Add(stackTraceTextBox);
            Controls.Add(label3);
            Controls.Add(errorMessageTextBox);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "NativeErrorMessage";
            Text = "Crashed!";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox errorMessageTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox stackTraceTextBox;
        private System.Windows.Forms.Button ErrorMessageCopyButton;
        private System.Windows.Forms.Button StackTraceCopyButton;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}