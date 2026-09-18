using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using infoCheckupResult.Models;

namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 편집기 구성, 키 입력, 값 변환, 유효성 검증
    /// </summary>
    public partial class ReceptionResultControl
    {
        private void ConfigureResultEditors()
        {
            numericEditor = new RepositoryItemTextEdit();
            numericEditor.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            numericEditor.Mask.EditMask = "#############0.0";
            numericEditor.Mask.UseMaskAsDisplayFormat = true;

            visionEditor = new RepositoryItemTextEdit();
            visionEditor.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            visionEditor.Mask.EditMask = "0.0;-0.0";
            visionEditor.Mask.UseMaskAsDisplayFormat = true;

            textEditor = new RepositoryItemTextEdit();
            codeEditor = new RepositoryItemTextEdit();
            codeEditor.MaxLength = 10;
            memoEditor = new RepositoryItemMemoEdit();
            memoEditor.ScrollBars = ScrollBars.Vertical;
            memoEditor.WordWrap = true;
            memoEditor.AllowMouseWheel = true;

            opinionPhraseEditor = new RepositoryItemButtonEdit();
            opinionPhraseEditor.TextEditStyle = TextEditStyles.HideTextEditor;
            opinionPhraseEditor.Buttons.Clear();

            treeCheckup.Appearance.Row.Font = new Font("맑은 고딕", 10F);
            treeCheckup.Appearance.Row.Options.UseFont = true;

            EditorButton opinionPhraseButton = new EditorButton(ButtonPredefines.Glyph);
            opinionPhraseButton.Image = CreateSearchGlyph(20);
            opinionPhraseButton.ToolTip = "소견 상용구 선택";
            opinionPhraseButton.DrawBackground = true;
            opinionPhraseButton.Width = 45;
            opinionPhraseEditor.Buttons.Add(opinionPhraseButton);
            opinionPhraseEditor.ButtonClick += opinionPhraseEditor_ButtonClick;

            gridControlResult.RepositoryItems.AddRange(
                new RepositoryItem[]
                {
                    numericEditor,
                    visionEditor,
                    textEditor,
                    codeEditor,
                    memoEditor,
                    opinionPhraseEditor
                });

            columnGroupName.OptionsColumn.AllowEdit = false;
            columnItemName.OptionsColumn.AllowEdit = false;
            columnResultValue.OptionsColumn.AllowEdit = true;
            columnOpinionPhrase.OptionsColumn.AllowEdit = true;
            columnJudgement.OptionsColumn.AllowEdit = false;
            columnUnit.OptionsColumn.AllowEdit = false;
            columnRequiredYn.OptionsColumn.AllowEdit = false;
            columnResultValue.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

            gridViewResult.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            gridViewResult.CustomRowCellEdit += gridViewResult_CustomRowCellEdit;
            gridViewResult.ShowingEditor += gridViewResult_ShowingEditor;
            gridViewResult.ShownEditor += gridViewResult_ShownEditor;
            gridViewResult.CellValueChanged += gridViewResult_CellValueChanged;
            gridViewResult.FocusedRowChanged += gridViewResult_FocusedRowChanged;
            gridViewResult.ValidatingEditor += gridViewResult_ValidatingEditor;
            gridViewResult.InvalidValueException += gridViewResult_InvalidValueException;
            gridViewResult.KeyDown += gridViewResult_KeyDown;
            gridViewResult.CustomColumnDisplayText += gridViewResult_CustomColumnDisplayText;
            gridViewResult.CalcRowHeight += gridViewResult_CalcRowHeight;
            gridViewResult.RowCellStyle += gridViewResult_RowCellStyle;
            gridViewResult.CustomDrawCell += gridViewResult_CustomDrawCell;
            btnSave.Click += btnSave_Click;
            btnFinalize.Click += btnFinalize_Click;
            btnCancelFinalize.Click += btnCancelFinalize_Click;
            btnPdf.Click += btnPdf_Click;
            btnHistory.Click += btnHistory_Click;
        }

        private void gridViewResult_ShowingEditor(
            object sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (gridViewResult.FocusedColumn != columnOpinionPhrase)
                return;

            CheckupResultDto item =
                gridViewResult.GetFocusedRow() as CheckupResultDto;

            e.Cancel = item == null ||
                       item.InputType != "MEMO" ||
                       currentHeader == null ||
                       currentHeader.IsFinalized;
        }

        private void gridViewResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None ||
                gridViewResult.FocusedColumn != columnResultValue)
            {
                return;
            }

            CheckupResultDto item = gridViewResult.GetFocusedRow() as CheckupResultDto;
            if (item == null || item.InputType == "MEMO")
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            CommitAndMoveToNextRow(false);
        }

        private void gridViewResult_ShownEditor(object sender, EventArgs e)
        {
            CheckupResultDto item =
                gridViewResult.GetFocusedRow() as CheckupResultDto;

            if (gridViewResult.ActiveEditor == null ||
                gridViewResult.FocusedColumn != columnResultValue ||
                item == null || item.InputType != "MEMO")
            {
                return;
            }

            gridViewResult.ActiveEditor.KeyDown -= memoEditor_KeyDown;
            gridViewResult.ActiveEditor.KeyDown += memoEditor_KeyDown;
        }

        private void memoEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Modifiers != Keys.None)
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;
            BeginInvoke(new Action(() => CommitAndMoveToNextRow(true)));
        }

        private void CommitAndMoveToNextRow(bool memoOnly)
        {
            gridViewResult.CloseEditor();
            if (!gridViewResult.UpdateCurrentRow())
                return;

            int nextRowHandle = gridViewResult.GetNextVisibleRow(
                gridViewResult.FocusedRowHandle);

            while (memoOnly && nextRowHandle >= 0)
            {
                CheckupResultDto nextItem =
                    gridViewResult.GetRow(nextRowHandle) as CheckupResultDto;

                if (nextItem != null && nextItem.InputType == "MEMO")
                    break;

                nextRowHandle = gridViewResult.GetNextVisibleRow(nextRowHandle);
            }

            if (nextRowHandle >= 0)
            {
                gridViewResult.FocusedRowHandle = nextRowHandle;
                gridViewResult.FocusedColumn = columnResultValue;
                gridViewResult.ShowEditor();
            }
        }

        private void PrepareResultDisplayValues()
        {
            foreach (CheckupResultDto item in allResults)
            {
                item.OriginalResultValue = item.ResultValue;
                item.DisplayResultValue = item.ResultValue;

                if (item.InputType != "CODE" ||
                    !item.CodeGroupId.HasValue ||
                    string.IsNullOrWhiteSpace(item.ResultValue))
                {
                    continue;
                }

                CommonCodeDto code = commonCodes.FirstOrDefault(
                    x => x.CodeGroupId == item.CodeGroupId.Value &&
                         string.Equals(
                             x.Code,
                             item.ResultValue,
                             StringComparison.OrdinalIgnoreCase));

                if (code != null)
                    item.DisplayResultValue = code.CodeName;
            }
        }

        private static CommonCodeDto FindCodeByInput(
            IList<CommonCodeDto> codes,
            string input)
        {
            return codes.FirstOrDefault(
                x => string.Equals(
                         x.InputCode,
                         input,
                         StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(
                         x.Code,
                         input,
                         StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(
                         x.CodeName,
                         input,
                         StringComparison.OrdinalIgnoreCase));
        }

        private void gridViewResult_CustomRowCellEdit(
            object sender,
            CustomRowCellEditEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            CheckupResultDto item = gridViewResult.GetRow(e.RowHandle) as CheckupResultDto;
            if (item == null)
                return;

            if (e.Column == columnOpinionPhrase)
            {
                if (item.InputType == "MEMO")
                    e.RepositoryItem = opinionPhraseEditor;

                return;
            }

            if (e.Column != columnResultValue)
                return;

            switch (item.InputType)
            {
                case "NUM":
                    e.RepositoryItem = IsVisionItem(item.ItemCode)
                        ? visionEditor
                        : numericEditor;
                    break;
                case "CODE":
                    if (item.CodeGroupId.HasValue)
                        e.RepositoryItem = codeEditor;
                    break;
                case "MEMO":
                    e.RepositoryItem = memoEditor;
                    break;
                default:
                    e.RepositoryItem = textEditor;
                    break;
            }
        }

        private static bool IsVisionItem(string itemCode)
        {
            return itemCode == "VISION_LEFT" ||
                   itemCode == "VISION_RIGHT" ||
                   itemCode == "STUDENT_VISION_LEFT" ||
                   itemCode == "STUDENT_VISION_RIGHT";
        }

        private void gridViewResult_CellValueChanged(
            object sender,
            DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (isLoading || e.Column != columnResultValue || currentHeader == null)
                return;

            CheckupResultDto item =
                gridViewResult.GetRow(e.RowHandle) as CheckupResultDto;

            if (item == null)
                return;

            string displayValue = Convert.ToString(e.Value).Trim();

            if (item.InputType == "CODE" && item.CodeGroupId.HasValue &&
                displayValue.Length > 0)
            {
                IList<CommonCodeDto> codes = commonCodes
                    .Where(x => x.CodeGroupId == item.CodeGroupId.Value)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
                CommonCodeDto code = FindCodeByInput(codes, displayValue);
                item.ResultValue = code == null ? displayValue : code.Code;
                item.DisplayResultValue = code == null
                    ? displayValue
                    : code.CodeName;
            }
            else
            {
                item.ResultValue = displayValue;
                item.DisplayResultValue = displayValue;
            }

            if (missingRequiredItemId == item.CheckupItemId && !string.IsNullOrWhiteSpace(item.ResultValue))
            {
                missingRequiredItemId = null;
            }

            UpdateDirtyState();
            judgementService.Evaluate(allResults);
            gridViewResult.RefreshData();
            UpdateActionButtons();
        }

        private void UpdateDirtyState()
        {
            isDirty = !allResults.All(
                x => string.Equals(
                    x.ResultValue ?? string.Empty,
                    x.OriginalResultValue ?? string.Empty,
                    StringComparison.Ordinal));
        }

        private void gridViewResult_ValidatingEditor(
            object sender,
            BaseContainerValidateEditorEventArgs e)
        {
            CheckupResultDto item = gridViewResult.GetFocusedRow() as CheckupResultDto;
            if (item == null)
                return;

            string value = e.Value == null ? string.Empty : Convert.ToString(e.Value).Trim();
            if (value.Length == 0)
                return;

            if (item.InputType == "NUM")
            {
                decimal number;
                if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out number))
                {
                    e.Valid = false;
                    e.ErrorText = "숫자 형식으로 입력하세요.";
                    return;
                }

                if ((item.MinValue.HasValue && number < item.MinValue.Value) ||
                    (item.MaxValue.HasValue && number > item.MaxValue.Value))
                {
                    e.Valid = false;
                    e.ErrorText = string.Format(
                        "허용 범위를 확인하세요. ({0} ~ {1})",
                        item.MinValue,
                        item.MaxValue);
                }
            }
            else if (item.InputType == "CODE" && item.CodeGroupId.HasValue)
            {
                IList<CommonCodeDto> codes = commonCodes
                    .Where(x => x.CodeGroupId == item.CodeGroupId.Value)
                    .OrderBy(x => x.SortOrder)
                    .ToList();
                CommonCodeDto matchedCode = FindCodeByInput(codes, value);

                if (matchedCode == null)
                {
                    e.Valid = false;
                    e.ErrorText = "입력 가능한 코드가 아닙니다. " +
                                  GetCodeGuideText(item.CodeGroupId.Value);
                }
                else
                {
                    e.Value = matchedCode.CodeName;
                }
            }
            else if ((item.InputType == "TEXT" || item.InputType == "MEMO") &&
                     item.MaxLength.HasValue && value.Length > item.MaxLength.Value)
            {
                e.Valid = false;
                e.ErrorText = string.Format("최대 {0}자까지 입력할 수 있습니다.", item.MaxLength.Value);
            }
        }

        private void gridViewResult_InvalidValueException(
            object sender,
            InvalidValueExceptionEventArgs e)
        {
            CheckupResultDto item = gridViewResult.GetFocusedRow() as CheckupResultDto;

            if (item == null)
                return;

            e.ExceptionMode = ExceptionMode.NoAction;
            XtraMessageBox.Show(
                this,
                e.ErrorText,
                "입력 확인",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
