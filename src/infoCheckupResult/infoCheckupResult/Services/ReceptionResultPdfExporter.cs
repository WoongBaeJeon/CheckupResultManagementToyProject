using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using infoCheckupResult.Models;

namespace infoCheckupResult.Services
{
    /// <summary>
    /// PDF 저장 및 결과지 생성
    /// </summary>
    internal static class ReceptionResultPdfExporter
    {
        public static string CreateDefaultFileName(ReceptionHeaderDto header)
        {
            if (header == null)
                throw new ArgumentNullException("header");

            string fileName = string.Format(
                "검진결과_{0}_{1}_{2:yyyyMMdd}.pdf",
                header.ChartNo,
                header.PatientName,
                header.ReceptionDate);

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(invalidCharacter, '_');

            return fileName;
        }

        public static void Export(
            string filePath,
            ReceptionHeaderDto header,
            IEnumerable<CheckupResultDto> results,
            IEnumerable<CommonCodeDto> commonCodes)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("PDF 저장 경로가 올바르지 않습니다.", "filePath");

            if (header == null)
                throw new ArgumentNullException("header");

            if (header.ResultStatus != "F" || !header.IsFinalized)
                throw new InvalidOperationException("확정된 검진 결과만 PDF로 저장할 수 있습니다.");

            IList<CommonCodeDto> codes = commonCodes == null
                ? new List<CommonCodeDto>()
                : commonCodes.ToList();

            IList<CheckupResultDto> resultItems =
                (results ?? Enumerable.Empty<CheckupResultDto>()).ToList();

            new ResultJudgementService().Evaluate(resultItems);

            IList<PdfResultRow> rows = resultItems
                .OrderBy(x => x.ClassSortOrder)
                .ThenBy(x => x.GroupSortOrder)
                .ThenBy(x => x.ItemSortOrder)
                .Select(x => new PdfResultRow
                {
                    ClassName = x.ClassName,
                    GroupName = x.GroupName,
                    ItemName = x.ItemName,
                    ResultValue = GetDisplayValue(x, codes),
                    Judgement = x.JudgementText,
                    Unit = x.Unit
                })
                .ToList();

            if (rows.Count == 0)
                throw new InvalidOperationException("PDF로 저장할 검사결과가 없습니다.");

            DateTime exportedAt = DateTime.Now;

            using (GridControl grid = new GridControl())
            using (GridView view = new GridView(grid))
            using (RepositoryItemMemoEdit resultEditor = new RepositoryItemMemoEdit())
            using (PrintingSystem printingSystem = new PrintingSystem())
            using (PrintableComponentLink link = new PrintableComponentLink(printingSystem))
            {
                ConfigureGrid(grid, view, resultEditor, rows);

                link.Component = grid;
                link.PaperKind = PaperKind.A4;
                link.Landscape = true;
                link.Margins = new Margins(40, 40, 110, 55);
                link.CreateMarginalHeaderArea += delegate(object sender, CreateAreaEventArgs e)
                {
                    DrawHeader(e, header);
                };
                link.CreateMarginalFooterArea += delegate(object sender, CreateAreaEventArgs e)
                {
                    DrawFooter(e, exportedAt);
                };

                link.CreateDocument();
                link.ExportToPdf(filePath);
            }
        }

        private static void ConfigureGrid(
            GridControl grid,
            GridView view,
            RepositoryItemMemoEdit resultEditor,
            IList<PdfResultRow> rows)
        {
            grid.BindingContext = new BindingContext();
            grid.Size = new Size(1000, 600);
            grid.MainView = view;
            grid.ViewCollection.AddRange(new BaseView[] { view });
            grid.RepositoryItems.Add(resultEditor);

            view.OptionsBehavior.Editable = false;
            view.OptionsBehavior.ReadOnly = true;
            view.OptionsBehavior.AutoPopulateColumns = false;
            view.OptionsCustomization.AllowGroup = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowIndicator = false;
            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.RowAutoHeight = true;
            view.OptionsPrint.AutoWidth = true;
            view.OptionsPrint.PrintHorzLines = true;
            view.OptionsPrint.PrintVertLines = true;
            view.OptionsPrint.UsePrintStyles = true;
            view.AppearancePrint.HeaderPanel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            view.AppearancePrint.HeaderPanel.Options.UseFont = true;
            view.AppearancePrint.HeaderPanel.ForeColor = Color.Black;
            view.AppearancePrint.HeaderPanel.Options.UseForeColor = true;
            view.AppearancePrint.Row.Font = new Font("맑은 고딕", 9F, FontStyle.Regular);
            view.AppearancePrint.Row.Options.UseFont = true;
            view.AppearancePrint.Row.ForeColor = Color.Black;
            view.AppearancePrint.Row.Options.UseForeColor = true;

            AddColumn(view, "ClassName", "검진종류", 0, 105);
            AddColumn(view, "GroupName", "항목그룹", 1, 125);
            AddColumn(view, "ItemName", "검사항목", 2, 155);
            GridColumn resultColumn = AddColumn(view, "ResultValue", "결과값", 3, 260);
            AddColumn(view, "Judgement", "판정", 4, 70);
            AddColumn(view, "Unit", "단위", 5, 75);

            resultEditor.WordWrap = true;
            resultEditor.Appearance.Font = new Font("맑은 고딕", 9F, FontStyle.Regular);
            resultEditor.Appearance.Options.UseFont = true;
            resultColumn.ColumnEdit = resultEditor;
            resultColumn.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;

            grid.DataSource = rows;
            grid.CreateControl();
            grid.ForceInitialize();
            view.RefreshData();

            if (view.DataRowCount != rows.Count)
            {
                throw new InvalidOperationException(
                    "PDF 출력용 검사결과를 Grid에 연결하지 못했습니다.");
            }
        }

        private static GridColumn AddColumn(
            GridView view,
            string fieldName,
            string caption,
            int visibleIndex,
            int width)
        {
            GridColumn column = view.Columns.AddVisible(fieldName, caption);
            column.VisibleIndex = visibleIndex;
            column.Width = width;
            column.OptionsColumn.AllowEdit = false;
            column.OptionsColumn.ReadOnly = true;
            return column;
        }

        private static string GetDisplayValue(
            CheckupResultDto result,
            IList<CommonCodeDto> commonCodes)
        {
            decimal number;
            if (result.InputType == "NUM" &&
                decimal.TryParse(
                    result.ResultValue,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out number))
            {
                return number.ToString("0.0", CultureInfo.InvariantCulture);
            }

            if (result.InputType != "CODE" || !result.CodeGroupId.HasValue)
                return result.ResultValue;

            CommonCodeDto code = commonCodes.FirstOrDefault(
                x => x.CodeGroupId == result.CodeGroupId.Value &&
                     x.Code == result.ResultValue);

            return code == null ? result.ResultValue : code.CodeName;
        }

        private static void DrawHeader(CreateAreaEventArgs e, ReceptionHeaderDto header)
        {
            BrickGraphics graph = e.Graph;
            graph.PageUnit = GraphicsUnit.Pixel;
            float width = graph.ClientPageSize.Width;

            using (Font titleFont = new Font("맑은 고딕", 16F, FontStyle.Bold))
            using (Font informationFont = new Font("맑은 고딕", 9F))
            {
                graph.Font = titleFont;
                graph.StringFormat = new BrickStringFormat(StringAlignment.Center);
                graph.DrawString(
                    "검진 결과지",
                    Color.Black,
                    new RectangleF(0, 0, width, 30),
                    BorderSide.None);

                graph.Font = informationFont;
                graph.StringFormat = new BrickStringFormat(StringAlignment.Near);
                graph.DrawString(
                    string.Format(
                        "접수번호: {0}    접수일자: {1:yyyy-MM-dd}    차트번호: {2}",
                        header.ReceptionId,
                        header.ReceptionDate,
                        header.ChartNo),
                    Color.Black,
                    new RectangleF(0, 36, width, 22),
                    BorderSide.None);

                graph.DrawString(
                    string.Format(
                        "수검자명: {0}    성별: {1}    나이: {2}    결과상태: 확정",
                        header.PatientName,
                        GetGenderName(header.Gender),
                        header.PatientAge.HasValue ? header.PatientAge.Value.ToString() : string.Empty),
                    Color.Black,
                    new RectangleF(0, 61, width, 22),
                    BorderSide.None);
            }
        }

        private static void DrawFooter(
             CreateAreaEventArgs e,
             DateTime exportedAt)
        {
            BrickGraphics graph = e.Graph;
            graph.PageUnit = GraphicsUnit.Pixel;

            using (Font footerFont =
                new Font("맑은 고딕", 8F))
            {
                graph.Font = footerFont;

                string dateText =
                    "출력일시: " +
                    exportedAt.ToString("yyyy-MM-dd HH:mm:ss");

                SizeF dateSize =
                    graph.MeasureString(dateText, footerFont);

                TextBrick dateBrick = graph.DrawString(
                    dateText,
                    Color.DimGray,
                    new RectangleF(
                        0,
                        0,
                        dateSize.Width + 5,
                        24),
                    BorderSide.None);

                dateBrick.HorzAlignment =
                    HorzAlignment.Near;

                PageInfoBrick pageBrick = graph.DrawPageInfo(
                    PageInfo.NumberOfTotal,
                    "{0} / {1}",
                    Color.DimGray,
                    new RectangleF(0, 0, 0, 24),
                    BorderSide.None);

                pageBrick.AutoWidth = true;
                pageBrick.Alignment = BrickAlignment.Center;
            }
        }

        private static string GetGenderName(string gender)
        {
            if (gender == "M")
                return "남";

            if (gender == "F")
                return "여";

            return gender;
        }

        private sealed class PdfResultRow
        {
            public string ClassName { get; set; }
            public string GroupName { get; set; }
            public string ItemName { get; set; }
            public string ResultValue { get; set; }
            public string Judgement { get; set; }
            public string Unit { get; set; }
        }
    }
}
