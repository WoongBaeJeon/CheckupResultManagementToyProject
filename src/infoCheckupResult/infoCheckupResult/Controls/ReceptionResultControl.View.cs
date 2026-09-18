using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using infoCheckupResult.Models;

namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 셀 스타일, 미입력 점선, 툴팁, 코드 안내, 트리 필터
    /// </summary>
    public partial class ReceptionResultControl
    {
        private void gridViewResult_CustomDrawCell(
            object sender,
            RowCellCustomDrawEventArgs e)
        {
            CheckupResultDto item =
                gridViewResult.GetRow(e.RowHandle) as CheckupResultDto;

            if (e.Column != columnResultValue ||
                item == null ||
                item.CheckupItemId != missingRequiredItemId)
            {
                return;
            }

            e.DefaultDraw();

            Rectangle bounds = e.Bounds;
            bounds.Inflate(-1, -1);

            using (Pen pen = new Pen(Color.Red, 2F))
            {
                pen.DashStyle = DashStyle.Dash;
                e.Cache.DrawRectangle(pen, bounds);
            }

            e.Handled = true;
        }

        private static Image CreateSearchGlyph(int size)
        {
            Bitmap image = new Bitmap(size, size);

            using (Graphics graphics = Graphics.FromImage(image))
            using (Pen pen = new Pen(Color.DimGray, 2F))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.Transparent);

                float lensSize = size * 0.55F;
                graphics.DrawEllipse(pen, 1.5F, 1.5F, lensSize, lensSize);
                graphics.DrawLine(
                    pen,
                    lensSize,
                    lensSize,
                    size - 2F,
                    size - 2F);
            }

            return image;
        }

        private void gridViewResult_RowCellStyle(
            object sender,
            RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            CheckupResultDto item =
                gridViewResult.GetRow(e.RowHandle) as CheckupResultDto;

            if (item == null)
                return;

            Color backColor = Color.White;

            if (currentHeader != null && currentHeader.IsFinalized)
            {
                backColor = Color.FromArgb(242, 242, 242);
            }
            else if (e.Column == columnResultValue)
            {
                bool isChanged = !string.Equals(
                    item.ResultValue ?? string.Empty,
                    item.OriginalResultValue ?? string.Empty,
                    StringComparison.Ordinal);

                if (isChanged)
                {
                    backColor = Color.FromArgb(255, 235, 190);
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.Appearance.Options.UseFont = true;
                }
                else
                {
                    backColor = Color.FromArgb(255, 252, 230);
                }
            }
            else if (e.Column == columnJudgement)
            {
                if (item.JudgementStatus == ResultJudgementStatus.LowBloodPressure)
                {
                    backColor = Color.FromArgb(232, 242, 255);
                    e.Appearance.ForeColor = Color.FromArgb(35, 90, 160);
                }
                else if (item.JudgementStatus == ResultJudgementStatus.Normal)
                {
                    backColor = Color.FromArgb(232, 245, 233);
                    e.Appearance.ForeColor = Color.FromArgb(32, 105, 55);
                }
                else if (item.JudgementStatus == ResultJudgementStatus.Caution)
                {
                    backColor = Color.FromArgb(255, 249, 220);
                    e.Appearance.ForeColor = Color.FromArgb(150, 110, 20);
                }
                else if (item.JudgementStatus == ResultJudgementStatus.Prehypertension)
                {
                    backColor = Color.FromArgb(255, 238, 210);
                    e.Appearance.ForeColor = Color.FromArgb(190, 95, 20);
                }
                else if (item.JudgementStatus == ResultJudgementStatus.Hypertension ||
                         item.JudgementStatus == ResultJudgementStatus.Abnormal)
                {
                    backColor = Color.FromArgb(255, 232, 232);
                    e.Appearance.ForeColor = Color.FromArgb(180, 35, 35);
                }

                if (item.JudgementStatus == ResultJudgementStatus.Pending)
                {
                    e.Appearance.ForeColor = Color.DimGray;
                    e.Appearance.Options.UseForeColor = true;
                }
                else if (item.JudgementStatus != ResultJudgementStatus.None)
                {
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                    e.Appearance.Options.UseForeColor = true;
                    e.Appearance.Options.UseFont = true;
                }
            }
            e.Appearance.BackColor = backColor;
            e.Appearance.BackColor2 = backColor;
            e.Appearance.Options.UseBackColor = true;
        }

        private void judgementToolTipController_GetActiveObjectInfo(
            object sender,
            ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            GridHitInfo hitInfo = gridViewResult.CalcHitInfo(e.ControlMousePosition);

            if (!hitInfo.InRowCell)
                return;

            CheckupResultDto item = gridViewResult.GetRow(hitInfo.RowHandle) as CheckupResultDto;

            if (item == null)
                return;

            if (hitInfo.Column == columnJudgement &&
                !string.IsNullOrWhiteSpace(item.JudgementReason))
            {
                e.Info = new ToolTipControlInfo("Judgement_" + item.CheckupItemId, item.JudgementReason);
                return;
            }

            ResultHistoryDto history;

            if (hitInfo.Column == columnResultValue &&
                latestResultHistories.TryGetValue(item.CheckupItemId, out history))
            {
                string text = string.Format(
                    "최근 변경 이력\r\n\r\n변경 전: {0}\r\n변경 후: {1}" +
                    "\r\n변경일시: {2:yyyy-MM-dd HH:mm:ss}\r\n처리자: {3}",
                    GetHistoryDisplayValue(item, history.BeforeValue, false),
                    GetHistoryDisplayValue(item, history.AfterValue, true),
                    history.ProcessedAt,
                    history.ProcessedBy);

                e.Info = new ToolTipControlInfo(
                    "History_" + item.CheckupItemId,
                    text);
            }
        }

        private void LoadLatestResultHistories(
            IList<ResultHistoryDto> histories)
        {
            latestResultHistories.Clear();

            foreach (ResultHistoryDto history in histories)
            {
                if (!latestResultHistories.ContainsKey(history.CheckupItemId))
                    latestResultHistories.Add(history.CheckupItemId, history);
            }
        }

        private string GetHistoryDisplayValue(
            CheckupResultDto item,
            string value,
            bool afterValue)
        {
            if (string.IsNullOrEmpty(value))
                return afterValue ? "(삭제)" : "(없음)";

            if (item.InputType == "CODE" && item.CodeGroupId.HasValue)
            {
                CommonCodeDto code = commonCodes.FirstOrDefault(
                    x => x.CodeGroupId == item.CodeGroupId.Value &&
                         string.Equals(
                             x.Code,
                             value,
                             StringComparison.OrdinalIgnoreCase));

                if (code != null)
                    value = code.CodeName;
            }

            return value.Length <= 200
                ? value
                : value.Substring(0, 200) + "...";
        }

        private void gridViewResult_CalcRowHeight(
            object sender,
            DevExpress.XtraGrid.Views.Grid.RowHeightEventArgs e)
        {
            CheckupResultDto item = gridViewResult.GetRow(e.RowHandle) as CheckupResultDto;

            if (item != null && item.InputType == "MEMO")
                e.RowHeight = 60;
        }

        private void gridViewResult_CustomColumnDisplayText(
            object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column != columnGroupName || e.ListSourceRowIndex < 0)
            {
                return;
            }

            int currentRowHandle = gridViewResult.GetRowHandle(e.ListSourceRowIndex);
            int previousRowHandle = gridViewResult.GetPrevVisibleRow(currentRowHandle);

            if (previousRowHandle < 0)
                return;

            object currentGroupId = gridViewResult.GetRowCellValue(currentRowHandle, "CheckupItemGroupId");
            object previousGroupId = gridViewResult.GetRowCellValue(previousRowHandle, "CheckupItemGroupId");

            if (Equals(currentGroupId, previousGroupId))
                e.DisplayText = string.Empty;
        }

        private void gridViewResult_FocusedRowChanged(
            object sender,
            FocusedRowChangedEventArgs e)
        {
            UpdateActionButtons();
            UpdateCodeGuide();
        }

        private void UpdateCodeGuide()
        {
            CheckupResultDto item = gridViewResult.GetFocusedRow() as CheckupResultDto;
            bool showGuide = item != null &&
                             item.InputType == "CODE" &&
                             item.CodeGroupId.HasValue;

            if (!showGuide)
            {
                labelCodeGuide.Text = string.Empty;
                panelCodeGuide.Visible = false;
                return;
            }

            string guideText = GetCodeGuideText(item.CodeGroupId.Value);
            labelCodeGuide.Text = guideText;
            panelCodeGuide.Visible = guideText.Length > 0;
        }

        private string GetCodeGuideText(int codeGroupId)
        {
            IList<CommonCodeDto> codes = commonCodes
                .Where(x => x.CodeGroupId == codeGroupId)
                .OrderBy(x => x.SortOrder)
                .ToList();

            string[] values = codes
                .Select(x => x.InputCode + " - " + x.CodeName)
                .ToArray();

            return values.Length == 0
                ? string.Empty
                : "입력 가능 값 : " + string.Join("  |  ", values);
        }

        private void BuildTree(IList<CheckupStructureDto> structure)
        {
            treeCheckup.BeginUnboundLoad();

            try
            {
                treeCheckup.ClearNodes();
                TreeListNode rootNode = treeCheckup.AppendNode(new object[] { "전체" }, null);
                rootNode.Tag = new ResultTreeFilter();

                foreach (IGrouping<int, CheckupStructureDto> classGroup in
                    structure.GroupBy(x => x.CheckupClassId))
                {
                    CheckupStructureDto classInfo = classGroup.First();
                    TreeListNode classNode = treeCheckup.AppendNode(new object[] { classInfo.ClassName }, rootNode);

                    classNode.Tag = new ResultTreeFilter
                    {
                        CheckupClassId = classInfo.CheckupClassId
                    };

                    foreach (CheckupStructureDto group in classGroup)
                    {
                        TreeListNode groupNode = treeCheckup.AppendNode(
                            new object[] { group.GroupName },
                            classNode);
                        groupNode.Tag = new ResultTreeFilter
                        {
                            CheckupClassId = group.CheckupClassId,
                            CheckupItemGroupId = group.CheckupItemGroupId
                        };
                    }
                }

                rootNode.ExpandAll();
                treeCheckup.FocusedNode = rootNode;
            }
            finally
            {
                treeCheckup.EndUnboundLoad();
            }
        }

        private void treeCheckup_FocusedNodeChanged(
            object sender,
            FocusedNodeChangedEventArgs e)
        {
            if (isLoading || e.Node == null)
                return;

            ResultTreeFilter filter = e.Node.Tag as ResultTreeFilter;
            IEnumerable<CheckupResultDto> filtered = allResults;

            if (filter != null && filter.CheckupClassId.HasValue)
            {
                filtered = filtered.Where(
                    x => x.CheckupClassId == filter.CheckupClassId.Value);
            }

            if (filter != null && filter.CheckupItemGroupId.HasValue)
            {
                filtered = filtered.Where(
                    x => x.CheckupItemGroupId == filter.CheckupItemGroupId.Value);
            }

            gridControlResult.DataSource = filtered.ToList();
            UpdateActionButtons();
            UpdateCodeGuide();
        }

        private sealed class ResultTreeFilter
        {
            public int? CheckupClassId { get; set; }
            public int? CheckupItemGroupId { get; set; }
        }
    }
}
