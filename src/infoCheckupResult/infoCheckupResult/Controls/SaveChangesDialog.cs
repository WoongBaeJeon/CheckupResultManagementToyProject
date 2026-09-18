using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace infoCheckupResult.Controls
{
    internal sealed class SaveChangesDialog : XtraForm
    {
        private SaveChangesDialog()
        {
            Text = "변경사항 확인";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(430, 145);

            LabelControl messageLabel = new LabelControl();
            messageLabel.Text = "변경된 검사결과가 있습니다. 저장하시겠습니까?";
            messageLabel.Appearance.Font = new Font("맑은 고딕", 10F);
            messageLabel.AutoSizeMode = LabelAutoSizeMode.None;
            messageLabel.Location = new Point(25, 25);
            messageLabel.Size = new Size(380, 35);
            messageLabel.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

            SimpleButton saveButton = CreateButton("저장", DialogResult.Yes, 40);
            SimpleButton discardButton = CreateButton("저장 안 함", DialogResult.No, 160);
            SimpleButton cancelButton = CreateButton("취소", DialogResult.Cancel, 280);

            Controls.Add(messageLabel);
            Controls.Add(saveButton);
            Controls.Add(discardButton);
            Controls.Add(cancelButton);

            AcceptButton = saveButton;
            CancelButton = cancelButton;
        }

        public static DialogResult ShowSavePrompt(IWin32Window owner)
        {
            using (SaveChangesDialog dialog = new SaveChangesDialog())
                return dialog.ShowDialog(owner);
        }

        private static SimpleButton CreateButton(string text, DialogResult result, int left)
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
