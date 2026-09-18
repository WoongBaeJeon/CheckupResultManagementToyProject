using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using infoCheckupResult.Models;
using infoCheckupResult.Repositories;

namespace infoCheckupResult
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private readonly IReceptionRepository receptionRepository;
        private bool isBindingReceptions;

        public Form1()
        {
            InitializeComponent();

            ConfigureDateEditors();

            gridViewReception.CustomDrawFooter += gridViewReception_CustomDrawFooter;

            receptionRepository = new ReceptionRepository();
            receptionResultControl.BindReceptionSource(receptionBindingSource);
            receptionResultControl.ReceptionStatusChanged += receptionResultControl_ReceptionStatusChanged;
        }

        private void gridViewReception_CustomDrawFooter(
            object sender,
            RowObjectCustomDrawEventArgs e)
        {
            int receptionCount = gridViewReception.DataRowCount;

            e.Cache.FillRectangle(Color.White, e.Bounds);

            Rectangle textBounds = e.Bounds;
            textBounds.X += 12;
            textBounds.Width -= 12;

            TextRenderer.DrawText(
                e.Cache.Graphics,
                string.Format("접수 건수 : {0:N0}건", receptionCount),
                gridViewReception.Appearance.FooterPanel.Font,
                textBounds,
                Color.FromArgb(45, 45, 48),
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            e.Handled = true;
        }

        private void ConfigureDateEditors()
        {
            dateReceptionFrom.Properties.Mask.MaskType =
                DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;

            dateReceptionFrom.Properties.Mask.EditMask =
                "yyyy-MM-dd";

            dateReceptionFrom.Properties.Mask.UseMaskAsDisplayFormat =
                true;

            dateReceptionTo.Properties.Mask.MaskType =
                DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;

            dateReceptionTo.Properties.Mask.EditMask =
                "yyyy-MM-dd";

            dateReceptionTo.Properties.Mask.UseMaskAsDisplayFormat =
                true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateReceptionFrom.DateTime = DateTime.Today.AddDays(-7);
            dateReceptionTo.DateTime = DateTime.Today;
            SearchReceptions();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (receptionResultControl.CanChangeReception())
                SearchReceptions();
        }

        private void gridViewReception_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (isBindingReceptions)
                return;

            if (e.FocusedRowHandle < 0)
            {
                receptionResultControl.ClearReception();
                return;
            }

            ReceptionDto reception = gridViewReception.GetRow(e.FocusedRowHandle) as ReceptionDto;
            if (reception == null)
                return;

            int dataSourceIndex =
                gridViewReception.GetDataSourceRowIndex(e.FocusedRowHandle);

            if (dataSourceIndex >= 0)
                receptionBindingSource.Position = dataSourceIndex;

            LoadReceptionDetail(reception.ReceptionId);
        }

        private void gridViewReception_BeforeLeaveRow(object sender, RowAllowEventArgs e)
        {
            if (isBindingReceptions)
                return;

            e.Allow = receptionResultControl.CanChangeReception();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!receptionResultControl.CanChangeReception())
                e.Cancel = true;
        }

        private void receptionResultControl_ReceptionStatusChanged(
            int receptionId,
            string resultStatus,
            string resultStatusName)
        {
            ReceptionDto reception = gridViewReception.GetFocusedRow() as ReceptionDto;
            if (reception == null || reception.ReceptionId != receptionId)
                return;

            reception.ResultStatus = resultStatus;
            reception.ResultStatusName = resultStatusName;
            receptionBindingSource.ResetCurrentItem();
            gridViewReception.RefreshRow(gridViewReception.FocusedRowHandle);
        }

        private void SearchReceptions()
        {
            if (dateReceptionFrom.DateTime.Date > dateReceptionTo.DateTime.Date)
            {
                dateReceptionTo.DateTime = dateReceptionFrom.DateTime.Date;
            }

            try
            {
                IList<ReceptionDto> receptions = receptionRepository.Search(
                    dateReceptionFrom.DateTime.Date,
                    dateReceptionTo.DateTime.Date,
                    txtSearchKeyword.Text);

                isBindingReceptions = true;
                try
                {
                    receptionBindingSource.DataSource = receptions;

                    if (receptions.Count > 0)
                        gridViewReception.FocusedRowHandle = 0;
                }
                finally
                {
                    isBindingReceptions = false;
                }

                if (receptions.Count > 0)
                    LoadReceptionDetail(receptions[0].ReceptionId);
                else
                    receptionResultControl.ClearReception();
            }
            catch (Exception ex)
            {
                receptionBindingSource.DataSource = null;
                receptionResultControl.ClearReception();

                XtraMessageBox.Show(
                    this,
                    "접수 목록을 조회하지 못했습니다.\r\n\r\n" + ex.Message,
                    "조회 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadReceptionDetail(int receptionId)
        {
            try
            {
                receptionResultControl.LoadReception(receptionId);
            }
            catch (Exception ex)
            {
                receptionResultControl.ClearReception();

                XtraMessageBox.Show(
                    this,
                    "검사결과 정보를 조회하지 못했습니다.\r\n\r\n" + ex.Message,
                    "상세 조회 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
