using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace infoCheckupResult.Controls
{
    internal sealed class ActionConfirmationDialog : XtraForm
    {
        private ActionConfirmationDialog(
            string title,
            string message,
            string confirmButtonText)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(430, 145);
    
            LabelControl messageLabel = new LabelControl();
            messageLabel.Text = message;
            messageLabel.Appearance.Font = new Font("맑은 고딕", 10F);
            messageLabel.AutoSizeMode = LabelAutoSizeMode.None;
            messageLabel.Location = new Point(25, 25);
            messageLabel.Size = new Size(380, 35);
            messageLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

            SimpleButton confirmButton = CreateButton(confirmButtonText, DialogResult.Yes, 100);
            SimpleButton cancelButton = CreateButton("취소", DialogResult.Cancel, 220);

            Controls.Add(messageLabel);
            Controls.Add(confirmButton);
            Controls.Add(cancelButton);

            AcceptButton = confirmButton;
            CancelButton = cancelButton;
        }

        public static DialogResult ShowConfirmation(
            IWin32Window owner,
            string title,
            string message,
            string confirmButtonText)
        {
            using (ActionConfirmationDialog dialog =
                new ActionConfirmationDialog(title, message, confirmButtonText))
            {
                return dialog.ShowDialog(owner);
            }
        }

        private static SimpleButton CreateButton(
            string text,
            DialogResult result,
            int left)
        {
            SimpleButton button = new SimpleButton();
            button.Text = text;
            button.DialogResult = result;
            button.Location = new Point(left, 85);
            button.Size = new Size(110, 36);
            return button;
        }
    }
}
