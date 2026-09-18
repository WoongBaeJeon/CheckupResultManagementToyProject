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
using DevExpress.XtraTab;
using infoCheckupResult.Models;

namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 변경이력 조회 폼 
    /// </summary>
    internal sealed class ReceptionHistoryDialog : XtraForm
    {
        public ReceptionHistoryDialog(
            ReceptionHeaderDto header,
            IList<ResultHistoryDto> resultHistory,
            IList<StatusHistoryDto> statusHistory)
        {
            if (header == null)
                throw new ArgumentNullException("header");

            IList<ResultHistoryDto> results = resultHistory ?? new List<ResultHistoryDto>();
            IList<StatusHistoryDto> statuses = statusHistory ?? new List<StatusHistoryDto>();

            InitializeDialog(header, results, statuses);
        }

        private void InitializeDialog(
            ReceptionHeaderDto header,
            IList<ResultHistoryDto> resultHistory,
            IList<StatusHistoryDto> statusHistory)
        {
            Text = "검진 결과 이력 조회";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(900, 520);
            ClientSize = new Size(1050, 620);
            ShowInTaskbar = false;

            PanelControl headerPanel = new PanelControl();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 55;

            LabelControl receptionLabel = new LabelControl();
            receptionLabel.Appearance.Font = new Font("맑은 고딕", 10F);
            receptionLabel.Location = new Point(18, 18);
            receptionLabel.Text = string.Format(
                "접수번호 {0}   |   차트번호 {1}   |   수검자 {2}",
                header.ReceptionId,
                header.ChartNo,
                header.PatientName);
            headerPanel.Controls.Add(receptionLabel);

            XtraTabControl tabs = new XtraTabControl();
            tabs.Dock = DockStyle.Fill;

            XtraTabPage resultPage = new XtraTabPage();
            resultPage.Text = string.Format("결과 변경 이력 ({0:N0})", resultHistory.Count);
            resultPage.Controls.Add(CreateResultHistoryGrid(resultHistory));

            XtraTabPage statusPage = new XtraTabPage();
            statusPage.Text = string.Format("상태 이력 ({0:N0})", statusHistory.Count);
            statusPage.Controls.Add(CreateStatusHistoryGrid(statusHistory));

            tabs.TabPages.AddRange(new[] { resultPage, statusPage });

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 58;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Padding = new Padding(0, 11, 14, 0);

            SimpleButton closeButton = new SimpleButton();
            closeButton.Text = "닫기";
            closeButton.DialogResult = DialogResult.Cancel;
            closeButton.Size = new Size(100, 34);
            buttonPanel.Controls.Add(closeButton);

            Controls.Add(tabs);
            Controls.Add(buttonPanel);
            Controls.Add(headerPanel);
            CancelButton = closeButton;
        }

        private static GridControl CreateResultHistoryGrid(
            IList<ResultHistoryDto> history)
        {
            GridControl grid = CreateGrid(out GridView view);

            AddColumn(view, "ProcessedAt", "변경일시", 0, 145, "yyyy-MM-dd HH:mm:ss");
            AddColumn(view, "ProcessedBy", "처리자", 1, 110, null);
            AddColumn(view, "ClassName", "검진종류", 2, 100, null);
            AddColumn(view, "GroupName", "항목그룹", 3, 120, null);
            AddColumn(view, "ItemName", "검사항목", 4, 150, null);
            AddColumn(view, "BeforeValue", "변경 전", 5, 230, null);
            AddColumn(view, "AfterValue", "변경 후", 6, 230, null);

            grid.DataSource = history.ToList();
            return grid;
        }

        private static GridControl CreateStatusHistoryGrid(
            IList<StatusHistoryDto> history)
        {
            GridControl grid = CreateGrid(out GridView view);

            AddColumn(view, "ProcessedAt", "처리일시", 0, 160, "yyyy-MM-dd HH:mm:ss");
            AddColumn(view, "ProcessedBy", "처리자", 1, 110, null);
            AddColumn(view, "BeforeStatusName", "변경 전 상태", 2, 140, null);
            AddColumn(view, "AfterStatusName", "변경 후 상태", 3, 140, null);
            AddColumn(view, "ProcessTypeName", "처리구분", 4, 140, null);

            grid.DataSource = history.ToList();
            return grid;
        }

        private static GridControl CreateGrid(out GridView view)
        {
            GridControl grid = new GridControl();
            view = new GridView(grid);

            grid.Dock = DockStyle.Fill;
            grid.MainView = view;
            grid.ViewCollection.AddRange(new BaseView[] { view });

            view.OptionsBehavior.Editable = false;
            view.OptionsBehavior.ReadOnly = true;
            view.OptionsCustomization.AllowGroup = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowIndicator = false;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowAutoFilterRow = true;

            return grid;
        }

        private static void AddColumn(
            GridView view,
            string fieldName,
            string caption,
            int visibleIndex,
            int width,
            string displayFormat)
        {
            GridColumn column = view.Columns.AddVisible(fieldName, caption);
            column.VisibleIndex = visibleIndex;
            column.Width = width;
            column.OptionsColumn.AllowEdit = false;
            column.OptionsColumn.ReadOnly = true;

            if (!string.IsNullOrEmpty(displayFormat))
            {
                column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                column.DisplayFormat.FormatString = displayFormat;
            }
        }
    }
}
