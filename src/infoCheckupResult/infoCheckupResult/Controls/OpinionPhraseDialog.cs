using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using infoCheckupResult.Models;

namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 소견 상용구 조회 폼
    /// </summary>
    internal sealed class OpinionPhraseDialog : XtraForm
    {
        private readonly GridView phraseView;

        public OpinionPhraseDto SelectedPhrase { get; private set; }

        public OpinionPhraseDialog(
            string itemName,
            IList<OpinionPhraseDto> phrases)
        {
            if (phrases == null)
                throw new ArgumentNullException("phrases");

            Text = "소견 상용구 선택";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(720, 430);
            ClientSize = new Size(820, 500);
            ShowInTaskbar = false;

            PanelControl headerPanel = new PanelControl();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 55;

            LabelControl itemLabel = new LabelControl();
            itemLabel.Appearance.Font = new Font("맑은 고딕", 10F);
            itemLabel.Location = new Point(18, 18);
            itemLabel.Text = "검사항목: " + itemName;
            headerPanel.Controls.Add(itemLabel);

            GridControl phraseGrid = new GridControl();
            phraseView = new GridView(phraseGrid);
            phraseGrid.Dock = DockStyle.Fill;
            phraseGrid.MainView = phraseView;
            phraseGrid.ViewCollection.AddRange(new BaseView[] { phraseView });

            phraseView.Appearance.HeaderPanel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            phraseView.Appearance.HeaderPanel.Options.UseFont = true;
            phraseView.Appearance.Row.Font = new Font("맑은 고딕", 9F);
            phraseView.Appearance.Row.Options.UseFont = true;
            phraseView.OptionsBehavior.Editable = false;
            phraseView.OptionsBehavior.ReadOnly = true;
            phraseView.OptionsSelection.EnableAppearanceFocusedCell = false;
            phraseView.OptionsView.ShowGroupPanel = false;
            phraseView.OptionsView.ShowIndicator = false;

            AddColumn("ScopeName", "구분", 0, 85);
            AddColumn("PhraseName", "상용구명", 1, 170);
            AddColumn("PhraseText", "상용구 내용", 2, 500);
            phraseGrid.DataSource = phrases.ToList();

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 58;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Padding = new Padding(0, 11, 14, 0);

            SimpleButton cancelButton = new SimpleButton();
            cancelButton.Text = "취소";
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Size = new Size(100, 34);

            SimpleButton selectButton = new SimpleButton();
            selectButton.Text = "선택";
            selectButton.Size = new Size(100, 34);
            selectButton.Click += delegate { SelectFocusedPhrase(); };

            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Controls.Add(selectButton);

            phraseView.DoubleClick += phraseView_DoubleClick;
            Controls.Add(phraseGrid);
            Controls.Add(buttonPanel);
            Controls.Add(headerPanel);
            AcceptButton = selectButton;
            CancelButton = cancelButton;
        }

        private void AddColumn(
            string fieldName,
            string caption,
            int visibleIndex,
            int width)
        {
            GridColumn column = phraseView.Columns.AddVisible(fieldName, caption);
            column.VisibleIndex = visibleIndex;
            column.Width = width;
            column.OptionsColumn.AllowEdit = false;
            column.OptionsColumn.ReadOnly = true;
        }

        private void phraseView_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo hitInfo = phraseView.CalcHitInfo(
                phraseView.GridControl.PointToClient(Control.MousePosition));

            if (hitInfo.InRow || hitInfo.InRowCell)
                SelectFocusedPhrase();
        }

        private void SelectFocusedPhrase()
        {
            OpinionPhraseDto phrase = phraseView.GetFocusedRow() as OpinionPhraseDto;
            if (phrase == null)
                return;

            SelectedPhrase = phrase;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
