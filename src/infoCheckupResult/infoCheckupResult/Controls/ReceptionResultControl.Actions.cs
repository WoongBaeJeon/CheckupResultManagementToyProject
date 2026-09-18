using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using infoCheckupResult.Models;
using infoCheckupResult.Services;

namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 저장, 확정, 확정취소, 이력, 상용구, PDF처리
    /// </summary>
    public partial class ReceptionResultControl
    {
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentResults(false);
        }

        private void btnFinalize_Click(object sender, EventArgs e)
        {
            if (currentHeader == null)
                return;

            if (isDirty)
            {
                XtraMessageBox.Show(
                    this,
                    "저장하지 않은 검사결과가 있습니다.\r\n먼저 저장해 주세요.",
                    "확정 불가",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (TryFocusMissingRequiredResult())
                return;

            DialogResult confirmation =
                ActionConfirmationDialog.ShowConfirmation(
                    this,
                    "결과 확정",
                    "저장된 검사결과를 확정하시겠습니까?",
                    "확정");

            if (confirmation != DialogResult.Yes)
                return;

            ChangeReceptionStatus(true);
        }

        private bool TryFocusMissingRequiredResult()
        {
            CheckupResultDto missing = allResults.FirstOrDefault(
                x => x.RequiredYn == "Y" &&
                     string.IsNullOrWhiteSpace(x.ResultValue));

            if (missing == null)
            {
                missingRequiredItemId = null;
                return false;
            }

            // 트리 필터로 항목이 가려져 있을 수 있으므로 전체 조회
            treeCheckup.FocusedNode = treeCheckup.Nodes[0];

            int rowHandle = gridViewResult.LocateByValue("CheckupItemId", missing.CheckupItemId);

            missingRequiredItemId = missing.CheckupItemId;
            gridViewResult.FocusedRowHandle = rowHandle;
            gridViewResult.FocusedColumn = columnResultValue;
            gridViewResult.MakeRowVisible(rowHandle);
            gridViewResult.RefreshRow(rowHandle);

            XtraMessageBox.Show(
                this,
                "필수 검사결과를 입력해 주세요.\r\n\r\n미입력 항목: " +
                missing.ItemName,
                "확정 불가",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return true;
        }

        private void btnCancelFinalize_Click(object sender, EventArgs e)
        {
            if (currentHeader == null)
                return;

            DialogResult confirmation =
                ActionConfirmationDialog.ShowConfirmation(
                    this,
                    "확정 취소",
                    "확정된 검사결과를 취소하시겠습니까?",
                    "확정 취소");

            if (confirmation != DialogResult.Yes)
                return;

            ChangeReceptionStatus(false);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (currentHeader == null)
                return;

            try
            {
                IList<ResultHistoryDto> resultHistory =
                    receptionRepository.GetResultHistory(currentHeader.ReceptionId);
                IList<StatusHistoryDto> statusHistory =
                    receptionRepository.GetStatusHistory(currentHeader.ReceptionId);

                using (ReceptionHistoryDialog dialog = new ReceptionHistoryDialog(
                    currentHeader,
                    resultHistory,
                    statusHistory))
                {
                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    this,
                    "이력을 조회하지 못했습니다.\r\n\r\n" + ex.Message,
                    "이력 조회 실패",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void opinionPhraseEditor_ButtonClick(
            object sender,
            ButtonPressedEventArgs e)
        {
            OpenOpinionPhrase();
        }

        private void OpenOpinionPhrase()
        {
            if (currentHeader == null || currentHeader.IsFinalized)
                return;

            gridViewResult.CloseEditor();
            if (!gridViewResult.UpdateCurrentRow())
                return;

            CheckupResultDto item = gridViewResult.GetFocusedRow() as CheckupResultDto;
            if (item == null || item.InputType != "MEMO")
                return;

            try
            {
                IList<OpinionPhraseDto> phrases =
                    receptionRepository.GetOpinionPhrases(item.CheckupItemId);

                if (phrases.Count == 0)
                {
                    XtraMessageBox.Show(
                        this,
                        "사용 가능한 소견 상용구가 없습니다.",
                        "소견 상용구",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                using (OpinionPhraseDialog dialog =
                    new OpinionPhraseDialog(item.ItemName, phrases))
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK ||
                        dialog.SelectedPhrase == null)
                    {
                        return;
                    }

                    string currentValue = (item.ResultValue ?? string.Empty).TrimEnd();
                    string phraseText = dialog.SelectedPhrase.PhraseText ?? string.Empty;
                    string changedValue = currentValue.Length == 0
                        ? phraseText
                        : currentValue + Environment.NewLine + phraseText;

                    if (item.MaxLength.HasValue &&
                        changedValue.Length > item.MaxLength.Value)
                    {
                        XtraMessageBox.Show(
                            this,
                            string.Format(
                                "상용구를 추가하면 최대 입력 길이 {0}자를 초과합니다.",
                                item.MaxLength.Value),
                            "소견 상용구 입력 불가",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    gridViewResult.SetFocusedRowCellValue(
                        columnResultValue,
                        changedValue);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    this,
                    "소견 상용구를 조회하지 못했습니다.\r\n\r\n" + ex.Message,
                    "소견 상용구 조회 실패",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            if (currentHeader == null ||
                currentHeader.ResultStatus != "F" ||
                !currentHeader.IsFinalized)
            {
                XtraMessageBox.Show(
                    this,
                    "확정된 검진 결과만 PDF로 저장할 수 있습니다.",
                    "PDF 저장 불가",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (allResults == null || allResults.Count == 0)
            {
                XtraMessageBox.Show(
                    this,
                    "PDF로 저장할 검사결과가 없습니다.",
                    "PDF 저장 불가",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "검진 결과 PDF 저장";
                saveDialog.Filter = "PDF 파일 (*.pdf)|*.pdf";
                saveDialog.DefaultExt = "pdf";
                saveDialog.AddExtension = true;
                saveDialog.OverwritePrompt = true;
                saveDialog.RestoreDirectory = true;
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveDialog.FileName = ReceptionResultPdfExporter.CreateDefaultFileName(currentHeader);

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    ReceptionResultPdfExporter.Export(
                        saveDialog.FileName,
                        currentHeader,
                        allResults,
                        commonCodes);

                    XtraMessageBox.Show(
                        this,
                        "검진 결과 PDF를 저장했습니다.\r\n\r\n" + saveDialog.FileName,
                        "PDF 저장 완료",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(
                        this,
                        "검진 결과 PDF를 저장하지 못했습니다.\r\n\r\n" + ex.Message,
                        "PDF 저장 실패",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void ChangeReceptionStatus(bool finalize)
        {
            try
            {
                StatusChangeResultDto result = finalize
                    ? receptionRepository.FinalizeReception(
                        currentHeader.ReceptionId,
                        "TEST_USER")
                    : receptionRepository.CancelReceptionFinalize(
                        currentHeader.ReceptionId,
                        "TEST_USER");

                int receptionId = currentHeader.ReceptionId;
                LoadReception(receptionId);

                string statusName = result.ResultStatus == "F"
                    ? "확정"
                    : "저장완료";

                Action<int, string, string> handler = ReceptionStatusChanged;
                if (handler != null)
                    handler(receptionId, result.ResultStatus, statusName);

                XtraMessageBox.Show(
                    this,
                    finalize
                        ? "검사결과를 확정했습니다."
                        : "검사결과 확정을 취소했습니다.",
                    finalize ? "확정 완료" : "확정 취소 완료",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    this,
                    (finalize
                        ? "검사결과를 확정하지 못했습니다."
                        : "검사결과 확정을 취소하지 못했습니다.") +
                    "\r\n\r\n" + ex.Message,
                    finalize ? "확정 오류" : "확정 취소 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool SaveCurrentResults(bool navigating)
        {
            if (currentHeader == null)
                return false;

            gridViewResult.CloseEditor();
            if (!gridViewResult.UpdateCurrentRow())
                return false;

            try
            {
                SaveResultDto result = receptionRepository.SaveResults(
                    currentHeader.ReceptionId,
                    allResults,
                    "TEST_USER");

                int receptionId = currentHeader.ReceptionId;
                LoadReception(receptionId);

                Action<int, string, string> handler = ReceptionStatusChanged;
                if (handler != null)
                    handler(receptionId, result.ResultStatus, "저장완료");

                if (!navigating)
                {
                    XtraMessageBox.Show(
                        this,
                        string.Format(
                            "검사결과를 저장했습니다.\r\n추가 {0}건 / 변경 {1}건 / 삭제 {2}건",
                            result.InsertedCount,
                            result.UpdatedCount,
                            result.DeletedCount),
                        "저장 완료",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                return true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(
                    this,
                    "검사결과를 저장하지 못했습니다.\r\n\r\n" + ex.Message,
                    "저장 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }
    }
}

