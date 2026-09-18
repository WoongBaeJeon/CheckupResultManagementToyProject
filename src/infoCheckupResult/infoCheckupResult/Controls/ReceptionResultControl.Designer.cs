namespace infoCheckupResult.Controls
{
    /// <summary>
    /// 결과 입력 폼 디자인 선언
    /// </summary>
    partial class ReceptionResultControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new DevExpress.XtraEditors.PanelControl();
            this.layoutControlHeader = new DevExpress.XtraLayout.LayoutControl();
            this.txtReceptionId = new DevExpress.XtraEditors.TextEdit();
            this.txtChartNo = new DevExpress.XtraEditors.TextEdit();
            this.txtPatientName = new DevExpress.XtraEditors.TextEdit();
            this.txtResultStatus = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroupHeader = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutReceptionId = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutChartNo = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutPatientName = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutResultStatus = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceHeader = new DevExpress.XtraLayout.EmptySpaceItem();
            this.panelActions = new DevExpress.XtraEditors.PanelControl();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnFinalize = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancelFinalize = new DevExpress.XtraEditors.SimpleButton();
            this.btnPdf = new DevExpress.XtraEditors.SimpleButton();
            this.btnHistory = new DevExpress.XtraEditors.SimpleButton();
            this.detailSplit = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeCheckup = new DevExpress.XtraTreeList.TreeList();
            this.treeColumnName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.gridControlResult = new DevExpress.XtraGrid.GridControl();
            this.gridViewResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.columnGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnOpinionPhrase = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnResultValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnJudgement = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnRequiredYn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelCodeGuide = new DevExpress.XtraEditors.PanelControl();
            this.labelCodeGuide = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelHeader)).BeginInit();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlHeader)).BeginInit();
            this.layoutControlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReceptionId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChartNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReceptionId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutChartNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutPatientName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutResultStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelActions)).BeginInit();
            this.panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.detailSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailSplit.Panel1)).BeginInit();
            this.detailSplit.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.detailSplit.Panel2)).BeginInit();
            this.detailSplit.Panel2.SuspendLayout();
            this.detailSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeCheckup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelCodeGuide)).BeginInit();
            this.panelCodeGuide.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.layoutControlHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(950, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // layoutControlHeader
            // 
            this.layoutControlHeader.Controls.Add(this.txtReceptionId);
            this.layoutControlHeader.Controls.Add(this.txtChartNo);
            this.layoutControlHeader.Controls.Add(this.txtPatientName);
            this.layoutControlHeader.Controls.Add(this.txtResultStatus);
            this.layoutControlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlHeader.Location = new System.Drawing.Point(2, 2);
            this.layoutControlHeader.Name = "layoutControlHeader";
            this.layoutControlHeader.Root = this.layoutControlGroupHeader;
            this.layoutControlHeader.Size = new System.Drawing.Size(946, 52);
            this.layoutControlHeader.TabIndex = 0;
            this.layoutControlHeader.Text = "layoutControlHeader";
            // 
            // txtReceptionId
            // 
            this.txtReceptionId.Location = new System.Drawing.Point(81, 12);
            this.txtReceptionId.Name = "txtReceptionId";
            this.txtReceptionId.Properties.AllowFocused = false;
            this.txtReceptionId.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtReceptionId.Properties.Appearance.Options.UseFont = true;
            this.txtReceptionId.Properties.ReadOnly = true;
            this.txtReceptionId.Size = new System.Drawing.Size(117, 24);
            this.txtReceptionId.StyleController = this.layoutControlHeader;
            this.txtReceptionId.TabIndex = 0;
            this.txtReceptionId.TabStop = false;
            // 
            // txtChartNo
            // 
            this.txtChartNo.Location = new System.Drawing.Point(271, 12);
            this.txtChartNo.Name = "txtChartNo";
            this.txtChartNo.Properties.AllowFocused = false;
            this.txtChartNo.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtChartNo.Properties.Appearance.Options.UseFont = true;
            this.txtChartNo.Properties.ReadOnly = true;
            this.txtChartNo.Size = new System.Drawing.Size(137, 24);
            this.txtChartNo.StyleController = this.layoutControlHeader;
            this.txtChartNo.TabIndex = 1;
            this.txtChartNo.TabStop = false;
            // 
            // txtPatientName
            // 
            this.txtPatientName.Location = new System.Drawing.Point(481, 12);
            this.txtPatientName.Name = "txtPatientName";
            this.txtPatientName.Properties.AllowFocused = false;
            this.txtPatientName.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtPatientName.Properties.Appearance.Options.UseFont = true;
            this.txtPatientName.Properties.ReadOnly = true;
            this.txtPatientName.Size = new System.Drawing.Size(177, 24);
            this.txtPatientName.StyleController = this.layoutControlHeader;
            this.txtPatientName.TabIndex = 2;
            this.txtPatientName.TabStop = false;
            // 
            // txtResultStatus
            // 
            this.txtResultStatus.Location = new System.Drawing.Point(731, 12);
            this.txtResultStatus.Name = "txtResultStatus";
            this.txtResultStatus.Properties.AllowFocused = false;
            this.txtResultStatus.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtResultStatus.Properties.Appearance.Options.UseFont = true;
            this.txtResultStatus.Properties.ReadOnly = true;
            this.txtResultStatus.Size = new System.Drawing.Size(117, 24);
            this.txtResultStatus.StyleController = this.layoutControlHeader;
            this.txtResultStatus.TabIndex = 3;
            this.txtResultStatus.TabStop = false;
            // 
            // layoutControlGroupHeader
            // 
            this.layoutControlGroupHeader.AppearanceItemCaption.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.layoutControlGroupHeader.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroupHeader.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlGroupHeader.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlGroupHeader.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupHeader.GroupBordersVisible = false;
            this.layoutControlGroupHeader.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutReceptionId,
            this.layoutChartNo,
            this.layoutPatientName,
            this.layoutResultStatus,
            this.emptySpaceHeader});
            this.layoutControlGroupHeader.Name = "layoutControlGroupHeader";
            this.layoutControlGroupHeader.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignModeGroup.CustomSize;
            this.layoutControlGroupHeader.OptionsItemText.TextToControlDistance = 8;
            this.layoutControlGroupHeader.Size = new System.Drawing.Size(946, 52);
            this.layoutControlGroupHeader.TextVisible = false;
            // 
            // layoutReceptionId
            // 
            this.layoutReceptionId.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 13F);
            this.layoutReceptionId.AppearanceItemCaption.Options.UseFont = true;
            this.layoutReceptionId.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutReceptionId.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.layoutReceptionId.Control = this.txtReceptionId;
            this.layoutReceptionId.Location = new System.Drawing.Point(0, 0);
            this.layoutReceptionId.MaxSize = new System.Drawing.Size(190, 32);
            this.layoutReceptionId.MinSize = new System.Drawing.Size(150, 30);
            this.layoutReceptionId.Name = "layoutReceptionId";
            this.layoutReceptionId.Size = new System.Drawing.Size(190, 32);
            this.layoutReceptionId.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutReceptionId.Text = "접수 번호";
            this.layoutReceptionId.TextSize = new System.Drawing.Size(61, 24);
            // 
            // layoutChartNo
            // 
            this.layoutChartNo.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 13F);
            this.layoutChartNo.AppearanceItemCaption.Options.UseFont = true;
            this.layoutChartNo.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutChartNo.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.layoutChartNo.Control = this.txtChartNo;
            this.layoutChartNo.Location = new System.Drawing.Point(190, 0);
            this.layoutChartNo.MaxSize = new System.Drawing.Size(210, 32);
            this.layoutChartNo.MinSize = new System.Drawing.Size(150, 30);
            this.layoutChartNo.Name = "layoutChartNo";
            this.layoutChartNo.Size = new System.Drawing.Size(210, 32);
            this.layoutChartNo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutChartNo.Text = "차트 번호";
            this.layoutChartNo.TextSize = new System.Drawing.Size(61, 24);
            // 
            // layoutPatientName
            // 
            this.layoutPatientName.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 13F);
            this.layoutPatientName.AppearanceItemCaption.Options.UseFont = true;
            this.layoutPatientName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutPatientName.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.layoutPatientName.Control = this.txtPatientName;
            this.layoutPatientName.Location = new System.Drawing.Point(400, 0);
            this.layoutPatientName.MaxSize = new System.Drawing.Size(250, 32);
            this.layoutPatientName.MinSize = new System.Drawing.Size(150, 30);
            this.layoutPatientName.Name = "layoutPatientName";
            this.layoutPatientName.Size = new System.Drawing.Size(250, 32);
            this.layoutPatientName.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutPatientName.Text = "수검자 명";
            this.layoutPatientName.TextSize = new System.Drawing.Size(61, 24);
            // 
            // layoutResultStatus
            // 
            this.layoutResultStatus.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 13F);
            this.layoutResultStatus.AppearanceItemCaption.Options.UseFont = true;
            this.layoutResultStatus.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutResultStatus.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.layoutResultStatus.Control = this.txtResultStatus;
            this.layoutResultStatus.Location = new System.Drawing.Point(650, 0);
            this.layoutResultStatus.MaxSize = new System.Drawing.Size(190, 32);
            this.layoutResultStatus.MinSize = new System.Drawing.Size(150, 30);
            this.layoutResultStatus.Name = "layoutResultStatus";
            this.layoutResultStatus.Size = new System.Drawing.Size(190, 32);
            this.layoutResultStatus.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutResultStatus.Text = "결과 상태";
            this.layoutResultStatus.TextSize = new System.Drawing.Size(61, 24);
            // 
            // emptySpaceHeader
            // 
            this.emptySpaceHeader.AllowHotTrack = false;
            this.emptySpaceHeader.Location = new System.Drawing.Point(840, 0);
            this.emptySpaceHeader.Name = "emptySpaceHeader";
            this.emptySpaceHeader.Size = new System.Drawing.Size(86, 32);
            this.emptySpaceHeader.TextSize = new System.Drawing.Size(0, 0);
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.btnSave);
            this.panelActions.Controls.Add(this.btnFinalize);
            this.panelActions.Controls.Add(this.btnCancelFinalize);
            this.panelActions.Controls.Add(this.btnPdf);
            this.panelActions.Controls.Add(this.btnHistory);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelActions.Location = new System.Drawing.Point(0, 56);
            this.panelActions.Name = "panelActions";
            this.panelActions.Size = new System.Drawing.Size(950, 52);
            this.panelActions.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(18, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 32);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "저장";
            // 
            // btnFinalize
            // 
            this.btnFinalize.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnFinalize.Appearance.Options.UseFont = true;
            this.btnFinalize.Enabled = false;
            this.btnFinalize.Location = new System.Drawing.Point(118, 10);
            this.btnFinalize.Name = "btnFinalize";
            this.btnFinalize.Size = new System.Drawing.Size(90, 32);
            this.btnFinalize.TabIndex = 1;
            this.btnFinalize.Text = "확정";
            // 
            // btnCancelFinalize
            // 
            this.btnCancelFinalize.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnCancelFinalize.Appearance.Options.UseFont = true;
            this.btnCancelFinalize.Enabled = false;
            this.btnCancelFinalize.Location = new System.Drawing.Point(218, 10);
            this.btnCancelFinalize.Name = "btnCancelFinalize";
            this.btnCancelFinalize.Size = new System.Drawing.Size(100, 32);
            this.btnCancelFinalize.TabIndex = 2;
            this.btnCancelFinalize.Text = "확정 취소";
            // 
            // btnPdf
            // 
            this.btnPdf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPdf.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnPdf.Appearance.Options.UseFont = true;
            this.btnPdf.Enabled = false;
            this.btnPdf.Location = new System.Drawing.Point(714, 10);
            this.btnPdf.Name = "btnPdf";
            this.btnPdf.Size = new System.Drawing.Size(104, 32);
            this.btnPdf.TabIndex = 4;
            this.btnPdf.Text = "PDF 저장";
            // 
            // btnHistory
            // 
            this.btnHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistory.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnHistory.Appearance.Options.UseFont = true;
            this.btnHistory.Enabled = false;
            this.btnHistory.Location = new System.Drawing.Point(828, 10);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(104, 32);
            this.btnHistory.TabIndex = 5;
            this.btnHistory.Text = "이력 조회";
            // 
            // detailSplit
            // 
            this.detailSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailSplit.Location = new System.Drawing.Point(0, 108);
            this.detailSplit.Name = "detailSplit";
            // 
            // detailSplit.Panel1
            // 
            this.detailSplit.Panel1.Controls.Add(this.treeCheckup);
            this.detailSplit.Panel1.MinSize = 180;
            // 
            // detailSplit.Panel2
            // 
            this.detailSplit.Panel2.Controls.Add(this.gridControlResult);
            this.detailSplit.Panel2.Controls.Add(this.panelCodeGuide);
            this.detailSplit.Panel2.MinSize = 450;
            this.detailSplit.Size = new System.Drawing.Size(950, 663);
            this.detailSplit.SplitterPosition = 180;
            this.detailSplit.TabIndex = 2;
            // 
            // treeCheckup
            // 
            this.treeCheckup.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeColumnName});
            this.treeCheckup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeCheckup.Location = new System.Drawing.Point(0, 0);
            this.treeCheckup.Name = "treeCheckup";
            this.treeCheckup.OptionsBehavior.Editable = false;
            this.treeCheckup.OptionsBehavior.ReadOnly = true;
            this.treeCheckup.OptionsView.ShowColumns = false;
            this.treeCheckup.OptionsView.ShowHorzLines = false;
            this.treeCheckup.OptionsView.ShowIndicator = false;
            this.treeCheckup.OptionsView.ShowVertLines = false;
            this.treeCheckup.Size = new System.Drawing.Size(180, 663);
            this.treeCheckup.TabIndex = 0;
            this.treeCheckup.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeCheckup_FocusedNodeChanged);
            // 
            // treeColumnName
            // 
            this.treeColumnName.Caption = "검진 항목";
            this.treeColumnName.FieldName = "Name";
            this.treeColumnName.Name = "treeColumnName";
            this.treeColumnName.Visible = true;
            this.treeColumnName.VisibleIndex = 0;
            // 
            // gridControlResult
            // 
            this.gridControlResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlResult.Location = new System.Drawing.Point(0, 0);
            this.gridControlResult.MainView = this.gridViewResult;
            this.gridControlResult.Name = "gridControlResult";
            this.gridControlResult.Size = new System.Drawing.Size(760, 633);
            this.gridControlResult.TabIndex = 0;
            this.gridControlResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewResult});
            // 
            // gridViewResult
            // 
            this.gridViewResult.Appearance.HeaderPanel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.gridViewResult.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewResult.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.gridViewResult.Appearance.Row.Options.UseFont = true;
            this.gridViewResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.columnGroupName,
            this.columnItemName,
            this.columnOpinionPhrase,
            this.columnResultValue,
            this.columnJudgement,
            this.columnUnit,
            this.columnRequiredYn});
            this.gridViewResult.DetailHeight = 327;
            this.gridViewResult.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewResult.GridControl = this.gridControlResult;
            this.gridViewResult.Name = "gridViewResult";
            this.gridViewResult.OptionsBehavior.Editable = false;
            this.gridViewResult.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewResult.OptionsView.ShowGroupPanel = false;
            this.gridViewResult.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewResult.OptionsView.ShowIndicator = false;
            this.gridViewResult.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            // 
            // columnGroupName
            // 
            this.columnGroupName.Caption = "항목그룹";
            this.columnGroupName.FieldName = "GroupName";
            this.columnGroupName.Name = "columnGroupName";
            this.columnGroupName.Visible = true;
            this.columnGroupName.VisibleIndex = 0;
            this.columnGroupName.Width = 150;
            // 
            // columnItemName
            // 
            this.columnItemName.Caption = "검사항목";
            this.columnItemName.FieldName = "ItemName";
            this.columnItemName.Name = "columnItemName";
            this.columnItemName.Visible = true;
            this.columnItemName.VisibleIndex = 1;
            this.columnItemName.Width = 180;
            // 
            // columnOpinionPhrase
            // 
            this.columnOpinionPhrase.Caption = "상용구";
            this.columnOpinionPhrase.FieldName = "OpinionPhraseAction";
            this.columnOpinionPhrase.Name = "columnOpinionPhrase";
            this.columnOpinionPhrase.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.columnOpinionPhrase.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.columnOpinionPhrase.OptionsColumn.FixedWidth = true;
            this.columnOpinionPhrase.OptionsFilter.AllowAutoFilter = false;
            this.columnOpinionPhrase.OptionsFilter.AllowFilter = false;
            this.columnOpinionPhrase.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.columnOpinionPhrase.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.columnOpinionPhrase.Visible = true;
            this.columnOpinionPhrase.VisibleIndex = 2;
            this.columnOpinionPhrase.Width = 55;
            // 
            // columnResultValue
            // 
            this.columnResultValue.Caption = "결과 입력";
            this.columnResultValue.FieldName = "DisplayResultValue";
            this.columnResultValue.Name = "columnResultValue";
            this.columnResultValue.Visible = true;
            this.columnResultValue.VisibleIndex = 3;
            this.columnResultValue.Width = 220;
            // 
            // columnJudgement
            // 
            this.columnJudgement.AppearanceCell.Options.UseTextOptions = true;
            this.columnJudgement.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.columnJudgement.AppearanceHeader.Options.UseTextOptions = true;
            this.columnJudgement.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.columnJudgement.Caption = "판정";
            this.columnJudgement.FieldName = "JudgementText";
            this.columnJudgement.Name = "columnJudgement";
            this.columnJudgement.OptionsColumn.AllowEdit = false;
            this.columnJudgement.OptionsColumn.FixedWidth = true;
            this.columnJudgement.Visible = true;
            this.columnJudgement.VisibleIndex = 4;
            this.columnJudgement.Width = 90;
            // 
            // columnUnit
            // 
            this.columnUnit.Caption = "단위";
            this.columnUnit.FieldName = "Unit";
            this.columnUnit.Name = "columnUnit";
            this.columnUnit.Visible = true;
            this.columnUnit.VisibleIndex = 5;
            this.columnUnit.Width = 80;
            // 
            // columnRequiredYn
            // 
            this.columnRequiredYn.Caption = "필수";
            this.columnRequiredYn.FieldName = "RequiredYn";
            this.columnRequiredYn.Name = "columnRequiredYn";
            this.columnRequiredYn.Visible = true;
            this.columnRequiredYn.VisibleIndex = 6;
            this.columnRequiredYn.Width = 55;
            // 
            // panelCodeGuide
            // 
            this.panelCodeGuide.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(230)))));
            this.panelCodeGuide.Appearance.Options.UseBackColor = true;
            this.panelCodeGuide.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelCodeGuide.Controls.Add(this.labelCodeGuide);
            this.panelCodeGuide.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelCodeGuide.Location = new System.Drawing.Point(0, 633);
            this.panelCodeGuide.Name = "panelCodeGuide";
            this.panelCodeGuide.Size = new System.Drawing.Size(760, 30);
            this.panelCodeGuide.TabIndex = 1;
            this.panelCodeGuide.Visible = false;
            // 
            // labelCodeGuide
            // 
            this.labelCodeGuide.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(252)))), ((int)(((byte)(230)))));
            this.labelCodeGuide.Appearance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.labelCodeGuide.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.labelCodeGuide.Appearance.Options.UseBackColor = true;
            this.labelCodeGuide.Appearance.Options.UseFont = true;
            this.labelCodeGuide.Appearance.Options.UseForeColor = true;
            this.labelCodeGuide.Appearance.Options.UseTextOptions = true;
            this.labelCodeGuide.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelCodeGuide.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelCodeGuide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCodeGuide.Location = new System.Drawing.Point(0, 0);
            this.labelCodeGuide.Name = "labelCodeGuide";
            this.labelCodeGuide.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.labelCodeGuide.Size = new System.Drawing.Size(760, 30);
            this.labelCodeGuide.TabIndex = 0;
            // 
            // ReceptionResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.detailSplit);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelHeader);
            this.Name = "ReceptionResultControl";
            this.Size = new System.Drawing.Size(950, 771);
            ((System.ComponentModel.ISupportInitialize)(this.panelHeader)).EndInit();
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlHeader)).EndInit();
            this.layoutControlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtReceptionId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChartNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReceptionId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutChartNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutPatientName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutResultStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelActions)).EndInit();
            this.panelActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.detailSplit.Panel1)).EndInit();
            this.detailSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.detailSplit.Panel2)).EndInit();
            this.detailSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.detailSplit)).EndInit();
            this.detailSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeCheckup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelCodeGuide)).EndInit();
            this.panelCodeGuide.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelHeader;
        private DevExpress.XtraLayout.LayoutControl layoutControlHeader;
        private DevExpress.XtraEditors.TextEdit txtReceptionId;
        private DevExpress.XtraEditors.TextEdit txtChartNo;
        private DevExpress.XtraEditors.TextEdit txtPatientName;
        private DevExpress.XtraEditors.TextEdit txtResultStatus;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupHeader;
        private DevExpress.XtraLayout.LayoutControlItem layoutReceptionId;
        private DevExpress.XtraLayout.LayoutControlItem layoutChartNo;
        private DevExpress.XtraLayout.LayoutControlItem layoutPatientName;
        private DevExpress.XtraLayout.LayoutControlItem layoutResultStatus;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceHeader;
        private DevExpress.XtraEditors.PanelControl panelActions;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnFinalize;
        private DevExpress.XtraEditors.SimpleButton btnCancelFinalize;
        private DevExpress.XtraEditors.SimpleButton btnPdf;
        private DevExpress.XtraEditors.SimpleButton btnHistory;
        private DevExpress.XtraEditors.SplitContainerControl detailSplit;
        private DevExpress.XtraTreeList.TreeList treeCheckup;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeColumnName;
        private DevExpress.XtraEditors.PanelControl panelCodeGuide;
        private DevExpress.XtraEditors.LabelControl labelCodeGuide;
        private DevExpress.XtraGrid.GridControl gridControlResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewResult;
        private DevExpress.XtraGrid.Columns.GridColumn columnGroupName;
        private DevExpress.XtraGrid.Columns.GridColumn columnItemName;
        private DevExpress.XtraGrid.Columns.GridColumn columnResultValue;
        private DevExpress.XtraGrid.Columns.GridColumn columnOpinionPhrase;
        private DevExpress.XtraGrid.Columns.GridColumn columnJudgement;
        private DevExpress.XtraGrid.Columns.GridColumn columnUnit;
        private DevExpress.XtraGrid.Columns.GridColumn columnRequiredYn;
    }
}
