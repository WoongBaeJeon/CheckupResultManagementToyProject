namespace infoCheckupResult
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.receptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.masterDetailSplit = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControlReception = new DevExpress.XtraGrid.GridControl();
            this.gridViewReception = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.columnReceptionDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnReceptionId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnChartNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnPatientName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnSocialNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnGender = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnPatientAge = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnResultStatusName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.columnReceptionMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelSearch = new DevExpress.XtraEditors.PanelControl();
            this.layoutControlSearch = new DevExpress.XtraLayout.LayoutControl();
            this.dateReceptionFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelDateSeparator = new DevExpress.XtraEditors.LabelControl();
            this.dateReceptionTo = new DevExpress.XtraEditors.DateEdit();
            this.txtSearchKeyword = new DevExpress.XtraEditors.TextEdit();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroupSearch = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutReceptionFrom = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutDateSeparator = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutReceptionTo = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceSearchTop = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutSearchKeyword = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutSearchButton = new DevExpress.XtraLayout.LayoutControlItem();
            this.receptionResultControl = new infoCheckupResult.Controls.ReceptionResultControl();
            ((System.ComponentModel.ISupportInitialize)(this.receptionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterDetailSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterDetailSplit.Panel1)).BeginInit();
            this.masterDetailSplit.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.masterDetailSplit.Panel2)).BeginInit();
            this.masterDetailSplit.Panel2.SuspendLayout();
            this.masterDetailSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlReception)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReception)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelSearch)).BeginInit();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlSearch)).BeginInit();
            this.layoutControlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReceptionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutDateSeparator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReceptionTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceSearchTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSearchKeyword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSearchButton)).BeginInit();
            this.SuspendLayout();
            // 
            // masterDetailSplit
            // 
            this.masterDetailSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.masterDetailSplit.Location = new System.Drawing.Point(12, 11);
            this.masterDetailSplit.Name = "masterDetailSplit";
            // 
            // masterDetailSplit.Panel1
            // 
            this.masterDetailSplit.Panel1.Controls.Add(this.gridControlReception);
            this.masterDetailSplit.Panel1.Controls.Add(this.panelSearch);
            this.masterDetailSplit.Panel1.MinSize = 460;
            // 
            // masterDetailSplit.Panel2
            // 
            this.masterDetailSplit.Panel2.Controls.Add(this.receptionResultControl);
            this.masterDetailSplit.Panel2.MinSize = 670;
            this.masterDetailSplit.Size = new System.Drawing.Size(1374, 766);
            this.masterDetailSplit.SplitterPosition = 600;
            this.masterDetailSplit.TabIndex = 0;
            // 
            // gridControlReception
            // 
            this.gridControlReception.DataSource = this.receptionBindingSource;
            this.gridControlReception.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlReception.Location = new System.Drawing.Point(0, 88);
            this.gridControlReception.MainView = this.gridViewReception;
            this.gridControlReception.Name = "gridControlReception";
            this.gridControlReception.Size = new System.Drawing.Size(600, 678);
            this.gridControlReception.TabIndex = 1;
            this.gridControlReception.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewReception});
            // 
            // gridViewReception
            // 
            this.gridViewReception.Appearance.FooterPanel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.gridViewReception.Appearance.FooterPanel.Options.UseFont = true;
            this.gridViewReception.Appearance.HeaderPanel.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.gridViewReception.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewReception.Appearance.Row.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.gridViewReception.Appearance.Row.Options.UseFont = true;
            this.gridViewReception.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.columnReceptionDate,
            this.columnReceptionId,
            this.columnChartNo,
            this.columnPatientName,
            this.columnSocialNumber,
            this.columnGender,
            this.columnPatientAge,
            this.columnResultStatusName,
            this.columnReceptionMemo});
            this.gridViewReception.DetailHeight = 327;
            this.gridViewReception.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewReception.GridControl = this.gridControlReception;
            this.gridViewReception.Name = "gridViewReception";
            this.gridViewReception.OptionsBehavior.Editable = false;
            this.gridViewReception.OptionsBehavior.ReadOnly = true;
            this.gridViewReception.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewReception.OptionsView.ColumnAutoWidth = false;
            this.gridViewReception.OptionsView.ShowFooter = true;
            this.gridViewReception.OptionsView.ShowGroupPanel = false;
            this.gridViewReception.OptionsView.ShowIndicator = false;
            this.gridViewReception.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewReception_FocusedRowChanged);
            this.gridViewReception.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gridViewReception_BeforeLeaveRow);
            // 
            // columnReceptionDate
            // 
            this.columnReceptionDate.Caption = "접수일자";
            this.columnReceptionDate.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.columnReceptionDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.columnReceptionDate.FieldName = "ReceptionDate";
            this.columnReceptionDate.Name = "columnReceptionDate";
            this.columnReceptionDate.Visible = true;
            this.columnReceptionDate.VisibleIndex = 0;
            this.columnReceptionDate.Width = 105;
            // 
            // columnReceptionId
            // 
            this.columnReceptionId.Caption = "접수번호";
            this.columnReceptionId.FieldName = "ReceptionId";
            this.columnReceptionId.Name = "columnReceptionId";
            this.columnReceptionId.Visible = true;
            this.columnReceptionId.VisibleIndex = 1;
            this.columnReceptionId.Width = 80;
            // 
            // columnChartNo
            // 
            this.columnChartNo.Caption = "차트번호";
            this.columnChartNo.FieldName = "ChartNo";
            this.columnChartNo.Name = "columnChartNo";
            this.columnChartNo.Visible = true;
            this.columnChartNo.VisibleIndex = 2;
            this.columnChartNo.Width = 100;
            // 
            // columnPatientName
            // 
            this.columnPatientName.Caption = "수검자명";
            this.columnPatientName.FieldName = "PatientName";
            this.columnPatientName.Name = "columnPatientName";
            this.columnPatientName.Visible = true;
            this.columnPatientName.VisibleIndex = 3;
            this.columnPatientName.Width = 100;
            // 
            // columnSocialNumber
            // 
            this.columnSocialNumber.Caption = "주민번호";
            this.columnSocialNumber.FieldName = "MaskedSocialNumber";
            this.columnSocialNumber.Name = "columnSocialNumber";
            this.columnSocialNumber.OptionsColumn.AllowEdit = false;
            this.columnSocialNumber.OptionsColumn.ReadOnly = true;
            this.columnSocialNumber.Visible = true;
            this.columnSocialNumber.VisibleIndex = 4;
            this.columnSocialNumber.Width = 130;
            // 
            // columnGender
            // 
            this.columnGender.Caption = "성별";
            this.columnGender.FieldName = "Gender";
            this.columnGender.Name = "columnGender";
            this.columnGender.Visible = true;
            this.columnGender.VisibleIndex = 5;
            this.columnGender.Width = 55;
            // 
            // columnPatientAge
            // 
            this.columnPatientAge.Caption = "나이";
            this.columnPatientAge.FieldName = "PatientAge";
            this.columnPatientAge.Name = "columnPatientAge";
            this.columnPatientAge.Visible = true;
            this.columnPatientAge.VisibleIndex = 6;
            this.columnPatientAge.Width = 55;
            // 
            // columnResultStatusName
            // 
            this.columnResultStatusName.Caption = "결과상태";
            this.columnResultStatusName.FieldName = "ResultStatusName";
            this.columnResultStatusName.Name = "columnResultStatusName";
            this.columnResultStatusName.Visible = true;
            this.columnResultStatusName.VisibleIndex = 7;
            this.columnResultStatusName.Width = 80;
            // 
            // columnReceptionMemo
            // 
            this.columnReceptionMemo.Caption = "접수메모";
            this.columnReceptionMemo.FieldName = "ReceptionMemo";
            this.columnReceptionMemo.Name = "columnReceptionMemo";
            this.columnReceptionMemo.Visible = true;
            this.columnReceptionMemo.VisibleIndex = 8;
            this.columnReceptionMemo.Width = 300;
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.layoutControlSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(600, 88);
            this.panelSearch.TabIndex = 0;
            // 
            // layoutControlSearch
            // 
            this.layoutControlSearch.Controls.Add(this.dateReceptionFrom);
            this.layoutControlSearch.Controls.Add(this.labelDateSeparator);
            this.layoutControlSearch.Controls.Add(this.dateReceptionTo);
            this.layoutControlSearch.Controls.Add(this.txtSearchKeyword);
            this.layoutControlSearch.Controls.Add(this.btnSearch);
            this.layoutControlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlSearch.Location = new System.Drawing.Point(2, 2);
            this.layoutControlSearch.Name = "layoutControlSearch";
            this.layoutControlSearch.Root = this.layoutControlGroupSearch;
            this.layoutControlSearch.Size = new System.Drawing.Size(596, 84);
            this.layoutControlSearch.TabIndex = 0;
            this.layoutControlSearch.Text = "layoutControlSearch";
            // 
            // dateReceptionFrom
            // 
            this.dateReceptionFrom.EditValue = null;
            this.dateReceptionFrom.Location = new System.Drawing.Point(81, 12);
            this.dateReceptionFrom.Name = "dateReceptionFrom";
            this.dateReceptionFrom.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dateReceptionFrom.Properties.Appearance.Options.UseFont = true;
            this.dateReceptionFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateReceptionFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateReceptionFrom.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateReceptionFrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateReceptionFrom.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateReceptionFrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateReceptionFrom.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dateReceptionFrom.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateReceptionFrom.Size = new System.Drawing.Size(177, 24);
            this.dateReceptionFrom.StyleController = this.layoutControlSearch;
            this.dateReceptionFrom.TabIndex = 1;
            // 
            // labelDateSeparator
            // 
            this.labelDateSeparator.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.labelDateSeparator.Appearance.Options.UseFont = true;
            this.labelDateSeparator.Location = new System.Drawing.Point(262, 12);
            this.labelDateSeparator.Name = "labelDateSeparator";
            this.labelDateSeparator.Size = new System.Drawing.Size(24, 24);
            this.labelDateSeparator.StyleController = this.layoutControlSearch;
            this.labelDateSeparator.TabIndex = 2;
            this.labelDateSeparator.Text = "~";
            // 
            // dateReceptionTo
            // 
            this.dateReceptionTo.EditValue = null;
            this.dateReceptionTo.Location = new System.Drawing.Point(290, 12);
            this.dateReceptionTo.Name = "dateReceptionTo";
            this.dateReceptionTo.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.dateReceptionTo.Properties.Appearance.Options.UseFont = true;
            this.dateReceptionTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateReceptionTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateReceptionTo.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.dateReceptionTo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateReceptionTo.Properties.EditFormat.FormatString = "yyyy-MM-dd";
            this.dateReceptionTo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateReceptionTo.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dateReceptionTo.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateReceptionTo.Size = new System.Drawing.Size(160, 24);
            this.dateReceptionTo.StyleController = this.layoutControlSearch;
            this.dateReceptionTo.TabIndex = 3;
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Location = new System.Drawing.Point(81, 40);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Properties.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.txtSearchKeyword.Properties.Appearance.Options.UseFont = true;
            this.txtSearchKeyword.Properties.MaxLength = 50;
            this.txtSearchKeyword.Properties.NullValuePrompt = "차트번호 또는 수검자명";
            this.txtSearchKeyword.Size = new System.Drawing.Size(401, 24);
            this.txtSearchKeyword.StyleController = this.layoutControlSearch;
            this.txtSearchKeyword.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Appearance.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSearch.Appearance.Options.UseFont = true;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Location = new System.Drawing.Point(486, 40);
            this.btnSearch.MaximumSize = new System.Drawing.Size(0, 24);
            this.btnSearch.MinimumSize = new System.Drawing.Size(0, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(98, 24);
            this.btnSearch.StyleController = this.layoutControlSearch;
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // layoutControlGroupSearch
            // 
            this.layoutControlGroupSearch.AppearanceItemCaption.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.layoutControlGroupSearch.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroupSearch.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlGroupSearch.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlGroupSearch.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupSearch.GroupBordersVisible = false;
            this.layoutControlGroupSearch.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutReceptionFrom,
            this.layoutDateSeparator,
            this.layoutReceptionTo,
            this.emptySpaceSearchTop,
            this.layoutSearchKeyword,
            this.layoutSearchButton});
            this.layoutControlGroupSearch.Name = "layoutControlGroupSearch";
            this.layoutControlGroupSearch.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignModeGroup.CustomSize;
            this.layoutControlGroupSearch.OptionsItemText.TextToControlDistance = 8;
            this.layoutControlGroupSearch.Size = new System.Drawing.Size(596, 84);
            this.layoutControlGroupSearch.TextVisible = false;
            // 
            // layoutReceptionFrom
            // 
            this.layoutReceptionFrom.Control = this.dateReceptionFrom;
            this.layoutReceptionFrom.Location = new System.Drawing.Point(0, 0);
            this.layoutReceptionFrom.MaxSize = new System.Drawing.Size(250, 28);
            this.layoutReceptionFrom.MinSize = new System.Drawing.Size(250, 28);
            this.layoutReceptionFrom.Name = "layoutReceptionFrom";
            this.layoutReceptionFrom.Size = new System.Drawing.Size(250, 28);
            this.layoutReceptionFrom.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutReceptionFrom.Text = "접수 일자";
            this.layoutReceptionFrom.TextSize = new System.Drawing.Size(61, 19);
            // 
            // layoutDateSeparator
            // 
            this.layoutDateSeparator.Control = this.labelDateSeparator;
            this.layoutDateSeparator.Location = new System.Drawing.Point(250, 0);
            this.layoutDateSeparator.MaxSize = new System.Drawing.Size(28, 28);
            this.layoutDateSeparator.MinSize = new System.Drawing.Size(28, 28);
            this.layoutDateSeparator.Name = "layoutDateSeparator";
            this.layoutDateSeparator.Size = new System.Drawing.Size(28, 28);
            this.layoutDateSeparator.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutDateSeparator.TextSize = new System.Drawing.Size(0, 0);
            this.layoutDateSeparator.TextVisible = false;
            // 
            // layoutReceptionTo
            // 
            this.layoutReceptionTo.Control = this.dateReceptionTo;
            this.layoutReceptionTo.Location = new System.Drawing.Point(278, 0);
            this.layoutReceptionTo.MaxSize = new System.Drawing.Size(164, 28);
            this.layoutReceptionTo.MinSize = new System.Drawing.Size(120, 28);
            this.layoutReceptionTo.Name = "layoutReceptionTo";
            this.layoutReceptionTo.Size = new System.Drawing.Size(164, 28);
            this.layoutReceptionTo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutReceptionTo.TextSize = new System.Drawing.Size(0, 0);
            this.layoutReceptionTo.TextVisible = false;
            // 
            // emptySpaceSearchTop
            // 
            this.emptySpaceSearchTop.AllowHotTrack = false;
            this.emptySpaceSearchTop.Location = new System.Drawing.Point(442, 0);
            this.emptySpaceSearchTop.Name = "emptySpaceSearchTop";
            this.emptySpaceSearchTop.Size = new System.Drawing.Size(134, 28);
            this.emptySpaceSearchTop.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutSearchKeyword
            // 
            this.layoutSearchKeyword.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutSearchKeyword.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.layoutSearchKeyword.Control = this.txtSearchKeyword;
            this.layoutSearchKeyword.Location = new System.Drawing.Point(0, 28);
            this.layoutSearchKeyword.MaxSize = new System.Drawing.Size(474, 36);
            this.layoutSearchKeyword.MinSize = new System.Drawing.Size(474, 36);
            this.layoutSearchKeyword.Name = "layoutSearchKeyword";
            this.layoutSearchKeyword.Size = new System.Drawing.Size(474, 36);
            this.layoutSearchKeyword.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutSearchKeyword.Text = "검색어";
            this.layoutSearchKeyword.TextSize = new System.Drawing.Size(61, 19);
            // 
            // layoutSearchButton
            // 
            this.layoutSearchButton.Control = this.btnSearch;
            this.layoutSearchButton.ControlAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.layoutSearchButton.ControlMaxSize = new System.Drawing.Size(0, 24);
            this.layoutSearchButton.ControlMinSize = new System.Drawing.Size(0, 24);
            this.layoutSearchButton.Location = new System.Drawing.Point(474, 28);
            this.layoutSearchButton.MaxSize = new System.Drawing.Size(102, 36);
            this.layoutSearchButton.MinSize = new System.Drawing.Size(90, 36);
            this.layoutSearchButton.Name = "layoutSearchButton";
            this.layoutSearchButton.Size = new System.Drawing.Size(102, 36);
            this.layoutSearchButton.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutSearchButton.TextSize = new System.Drawing.Size(0, 0);
            this.layoutSearchButton.TextVisible = false;
            // 
            // receptionResultControl
            // 
            this.receptionResultControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.receptionResultControl.Location = new System.Drawing.Point(0, 0);
            this.receptionResultControl.Name = "receptionResultControl";
            this.receptionResultControl.Size = new System.Drawing.Size(764, 766);
            this.receptionResultControl.TabIndex = 0;
            // 
            // Form1
            // 
            this.AcceptButton = this.btnSearch;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1398, 788);
            this.Controls.Add(this.masterDetailSplit);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(1250, 700);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "검진 결과 관리 프로그램";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.receptionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.masterDetailSplit.Panel1)).EndInit();
            this.masterDetailSplit.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterDetailSplit.Panel2)).EndInit();
            this.masterDetailSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.masterDetailSplit)).EndInit();
            this.masterDetailSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlReception)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReception)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelSearch)).EndInit();
            this.panelSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlSearch)).EndInit();
            this.layoutControlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateReceptionTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearchKeyword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReceptionFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutDateSeparator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutReceptionTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceSearchTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSearchKeyword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutSearchButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl masterDetailSplit;
        private DevExpress.XtraEditors.PanelControl panelSearch;
        private DevExpress.XtraLayout.LayoutControl layoutControlSearch;
        private DevExpress.XtraEditors.DateEdit dateReceptionFrom;
        private DevExpress.XtraEditors.LabelControl labelDateSeparator;
        private DevExpress.XtraEditors.DateEdit dateReceptionTo;
        private DevExpress.XtraEditors.TextEdit txtSearchKeyword;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupSearch;
        private DevExpress.XtraLayout.LayoutControlItem layoutReceptionFrom;
        private DevExpress.XtraLayout.LayoutControlItem layoutDateSeparator;
        private DevExpress.XtraLayout.LayoutControlItem layoutReceptionTo;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceSearchTop;
        private DevExpress.XtraLayout.LayoutControlItem layoutSearchKeyword;
        private DevExpress.XtraLayout.LayoutControlItem layoutSearchButton;
        private DevExpress.XtraGrid.GridControl gridControlReception;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewReception;
        private DevExpress.XtraGrid.Columns.GridColumn columnReceptionDate;
        private DevExpress.XtraGrid.Columns.GridColumn columnReceptionId;
        private DevExpress.XtraGrid.Columns.GridColumn columnChartNo;
        private DevExpress.XtraGrid.Columns.GridColumn columnPatientName;
        private DevExpress.XtraGrid.Columns.GridColumn columnGender;
        private DevExpress.XtraGrid.Columns.GridColumn columnPatientAge;
        private DevExpress.XtraGrid.Columns.GridColumn columnResultStatusName;
        private DevExpress.XtraGrid.Columns.GridColumn columnReceptionMemo;
        private infoCheckupResult.Controls.ReceptionResultControl receptionResultControl;
        private DevExpress.XtraGrid.Columns.GridColumn columnSocialNumber;
        private System.Windows.Forms.BindingSource receptionBindingSource;
    }
}
