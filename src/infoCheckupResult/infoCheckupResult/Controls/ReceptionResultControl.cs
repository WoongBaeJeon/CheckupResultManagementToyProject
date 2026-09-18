using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using infoCheckupResult.Models;
using infoCheckupResult.Repositories;
using infoCheckupResult.Services;

namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 필드, 생성자, 접수 로드, 초기화, 버튼 상태 관리
    /// </summary>
    public partial class ReceptionResultControl : XtraUserControl
    {
        private readonly IReceptionRepository receptionRepository;
        private readonly ResultJudgementService judgementService;
        private readonly ToolTipController judgementToolTipController;
        private IList<CheckupResultDto> allResults;
        private IList<CommonCodeDto> commonCodes;
        private Dictionary<int, ResultHistoryDto> latestResultHistories;
        private ReceptionHeaderDto currentHeader;
        private bool isLoading;
        private bool isDirty;
        private RepositoryItemTextEdit numericEditor;
        private RepositoryItemTextEdit visionEditor;
        private RepositoryItemTextEdit textEditor;
        private RepositoryItemTextEdit codeEditor;
        private RepositoryItemMemoEdit memoEditor;
        private RepositoryItemButtonEdit opinionPhraseEditor;
        private int? missingRequiredItemId;

        public event Action<int, string, string> ReceptionStatusChanged;

        public ReceptionResultControl()
        {
            InitializeComponent();
            receptionRepository = new ReceptionRepository();
            judgementService = new ResultJudgementService();
            judgementToolTipController = new ToolTipController();
            judgementToolTipController.GetActiveObjectInfo += judgementToolTipController_GetActiveObjectInfo;
            gridControlResult.ToolTipController = judgementToolTipController;
            allResults = new List<CheckupResultDto>();
            commonCodes = new List<CommonCodeDto>();
            latestResultHistories = new Dictionary<int, ResultHistoryDto>();
            ConfigureResultEditors();
            ClearReception();
        }

        public void BindReceptionSource(BindingSource receptionSource)
        {
            if (receptionSource == null)
                throw new ArgumentNullException("receptionSource");

            if (receptionSource.DataSource == null)
                receptionSource.DataSource = typeof(ReceptionDto);

            BindHeaderControl(
                txtReceptionId,
                receptionSource,
                "ReceptionId");
            BindHeaderControl(
                txtChartNo,
                receptionSource,
                "ChartNo");
            BindHeaderControl(
                txtPatientName,
                receptionSource,
                "PatientName");
            BindHeaderControl(
                txtResultStatus,
                receptionSource,
                "ResultStatusName");
        }

        private static void BindHeaderControl(
            BaseEdit editor,
            BindingSource receptionSource,
            string propertyName)
        {
            editor.DataBindings.Clear();
            editor.DataBindings.Add(
                "EditValue",
                receptionSource,
                propertyName,
                true,
                DataSourceUpdateMode.Never);
        }

        public void LoadReception(int receptionId)
        {
            isLoading = true;

            try
            {
                ReceptionResultData data = receptionRepository.GetReceptionResult(receptionId);
                IList<CheckupStructureDto> structure = receptionRepository.GetExamStructure(receptionId);

                if (data.Header == null)
                {
                    ClearReception();
                    return;
                }

                LoadLatestResultHistories(
                    receptionRepository.GetResultHistory(receptionId));

                currentHeader = data.Header;
                allResults = data.Results;
                commonCodes = data.CommonCodes;
                PrepareResultDisplayValues();
                judgementService.Evaluate(allResults);
                BuildTree(structure);
                gridControlResult.DataSource = allResults.ToList();
                UpdateCodeGuide();

                bool finalized = data.Header.IsFinalized;
                isDirty = false;
                gridViewResult.OptionsBehavior.Editable = !finalized;
                gridViewResult.OptionsBehavior.ReadOnly = finalized;

                UpdateActionButtons();
            }
            finally
            {
                isLoading = false;
            }
        }

        public void ClearReception()
        {
            isLoading = true;

            try
            {
                currentHeader = null;
                allResults = new List<CheckupResultDto>();
                commonCodes = new List<CommonCodeDto>();
                latestResultHistories.Clear();
                isDirty = false;
                gridControlResult.DataSource = null;
                treeCheckup.ClearNodes();
                labelCodeGuide.Text = string.Empty;
                panelCodeGuide.Visible = false;
                btnSave.Enabled = false;
                btnFinalize.Enabled = false;
                btnCancelFinalize.Enabled = false;
                btnPdf.Enabled = false;
                btnHistory.Enabled = false;
            }
            finally
            {
                isLoading = false;
            }
        }

        public bool CanChangeReception()
        {
            if (!isDirty)
                return true;

            DialogResult result = SaveChangesDialog.ShowSavePrompt(this);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.No)
                return true;

            return SaveCurrentResults(true);
        }

        private void UpdateActionButtons()
        {
            bool hasReception = currentHeader != null;
            bool finalized = hasReception && currentHeader.IsFinalized;
            bool saved = hasReception && currentHeader.ResultStatus == "S";

            btnSave.Enabled = hasReception && !finalized && isDirty;
            btnFinalize.Enabled = saved && !isDirty;
            btnCancelFinalize.Enabled = finalized;
            btnPdf.Enabled = finalized && !isDirty;
            btnHistory.Enabled = hasReception;

            btnSave.Cursor = btnSave.Enabled? Cursors.Hand : Cursors.Default;
            btnFinalize.Cursor = btnFinalize.Enabled ? Cursors.Hand : Cursors.Default;
            btnCancelFinalize.Cursor = btnCancelFinalize.Enabled ? Cursors.Hand : Cursors.Default;
            btnHistory.Cursor = btnHistory.Enabled ? Cursors.Hand : Cursors.Default;
            btnPdf.Cursor = btnPdf.Enabled ? Cursors.Hand : Cursors.Default;
        }
    }
}
