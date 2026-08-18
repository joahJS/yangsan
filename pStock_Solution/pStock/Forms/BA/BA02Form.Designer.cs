using pStock.Common;
using Guna.UI2.WinForms;

namespace pStock.Forms.BA;

partial class BA02Form
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private FastDataGridView gridGroup;
    private FastDataGridView gridCode;
    private RadioButton radModeGroup;
    private RadioButton radModeCode;
    private Guna2TextBox edtRcdtp;
    private Guna2TextBox edtRetxf;
    private Label lblRcdtp;
    private Label lblRetxf;
    private Label lblCode;
    private Guna2TextBox edtCode;
    private Label lblRetxs;
    private Guna2TextBox edtRetxs;
    private Guna2Button btnNew;
    private Guna2Button btnSave;
    private Guna2Button btnUpd;
    private Guna2Button btnDel;
    private Guna2Button btnSearch;
    private Guna2Button btnClose;

    private Guna2Panel cardPanel;
    private Panel toolbarPanel;
    private Panel filterAreaPanel;
    private Guna2Panel filterPanel;
    private Label filterTitleLabel;
    private Panel gridAreaPanel;
    private Guna2Panel groupCardPanel;
    private Panel groupHeaderPanel;
    private Label groupTitleLabel;
    private Panel splitterGap;
    private Guna2Panel codeCardPanel;
    private Panel codeHeaderPanel;
    private Label codeTitleLabel;

    private void InitializeComponent()
    {
        this.gridGroup = new FastDataGridView();
        this.gridCode = new FastDataGridView();
        this.radModeGroup = new RadioButton();
        this.radModeCode = new RadioButton();
        this.lblRcdtp = new Label();
        this.edtRcdtp = new Guna2TextBox();
        this.lblRetxf = new Label();
        this.edtRetxf = new Guna2TextBox();
        this.lblCode = new Label();
        this.edtCode = new Guna2TextBox();
        this.lblRetxs = new Label();
        this.edtRetxs = new Guna2TextBox();
        this.btnNew = new Guna2Button();
        this.btnSave = new Guna2Button();
        this.btnUpd = new Guna2Button();
        this.btnDel = new Guna2Button();
        this.btnSearch = new Guna2Button();
        this.btnClose = new Guna2Button();
        this.cardPanel = new Guna2Panel();
        this.toolbarPanel = new Panel();
        this.filterAreaPanel = new Panel();
        this.filterPanel = new Guna2Panel();
        this.filterTitleLabel = new Label();
        this.gridAreaPanel = new Panel();
        this.groupCardPanel = new Guna2Panel();
        this.groupHeaderPanel = new Panel();
        this.groupTitleLabel = new Label();
        this.splitterGap = new Panel();
        this.codeCardPanel = new Guna2Panel();
        this.codeHeaderPanel = new Panel();
        this.codeTitleLabel = new Label();
        this.toolbarPanel.SuspendLayout();
        this.filterAreaPanel.SuspendLayout();
        this.filterPanel.SuspendLayout();
        this.groupHeaderPanel.SuspendLayout();
        this.groupCardPanel.SuspendLayout();
        this.codeHeaderPanel.SuspendLayout();
        this.codeCardPanel.SuspendLayout();
        this.gridAreaPanel.SuspendLayout();
        this.cardPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // radModeGroup / radModeCode
        //
        this.radModeGroup.Text = "구분코드";
        this.radModeGroup.Checked = true;
        this.radModeGroup.AutoSize = true;
        this.radModeCode.Text = "코드";
        this.radModeCode.AutoSize = true;
        //
        // lblCode / lblRetxs
        //
        this.lblCode.Text = "코드:";
        this.lblCode.AutoSize = true;
        this.lblRetxs.Text = "약칭(S):";
        this.lblRetxs.AutoSize = true;
        //
        // 상단 버튼 6개 (신규만 강조 채움, 나머지는 흰 배경 + 파란 테두리)
        //
        this.btnNew.Text = "신규(F1)";
        this.btnSave.Text = "저장(F2)";
        this.btnUpd.Text = "수정(F3)";
        this.btnDel.Text = "삭제(F4)";
        this.btnSearch.Text = "조회";
        this.btnClose.Text = "닫기(Esc)";

        this.btnNew.Left = 20; this.btnNew.Top = 15; this.btnNew.Width = 110; this.btnNew.Height = 36;
        this.btnSave.Left = 138; this.btnSave.Top = 15; this.btnSave.Width = 110; this.btnSave.Height = 36;
        this.btnUpd.Left = 256; this.btnUpd.Top = 15; this.btnUpd.Width = 110; this.btnUpd.Height = 36;
        this.btnDel.Left = 374; this.btnDel.Top = 15; this.btnDel.Width = 110; this.btnDel.Height = 36;
        this.btnSearch.Left = 492; this.btnSearch.Top = 15; this.btnSearch.Width = 110; this.btnSearch.Height = 36;
        this.btnClose.Left = 610; this.btnClose.Top = 15; this.btnClose.Width = 110; this.btnClose.Height = 36;

        this.btnNew.BorderRadius = 8;
        this.btnSave.BorderRadius = 8;
        this.btnUpd.BorderRadius = 8;
        this.btnDel.BorderRadius = 8;
        this.btnSearch.BorderRadius = 8;
        this.btnClose.BorderRadius = 8;

        this.btnNew.ImageSize = new Size(15, 15);
        this.btnSave.ImageSize = new Size(15, 15);
        this.btnUpd.ImageSize = new Size(15, 15);
        this.btnDel.ImageSize = new Size(15, 15);
        this.btnSearch.ImageSize = new Size(15, 15);
        this.btnClose.ImageSize = new Size(15, 15);

        this.btnNew.FillColor = Color.FromArgb(47, 111, 237);
        this.btnNew.ForeColor = Color.White;
        this.btnNew.BorderThickness = 0;
        this.btnNew.HoverState.FillColor = Color.FromArgb(35, 96, 220);

        this.btnSave.FillColor = Color.White;
        this.btnUpd.FillColor = Color.White;
        this.btnDel.FillColor = Color.White;
        this.btnSearch.FillColor = Color.White;
        this.btnClose.FillColor = Color.White;

        this.btnSave.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnUpd.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnDel.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnSearch.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnClose.ForeColor = Color.FromArgb(75, 85, 99);

        this.btnSave.BorderThickness = 1;
        this.btnUpd.BorderThickness = 1;
        this.btnDel.BorderThickness = 1;
        this.btnSearch.BorderThickness = 1;
        this.btnClose.BorderThickness = 1;

        this.btnSave.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnUpd.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnDel.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnSearch.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnClose.BorderColor = Color.FromArgb(209, 213, 219);

        this.btnNew.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        this.btnSave.Font = new Font("맑은 고딕", 9.5F);
        this.btnUpd.Font = new Font("맑은 고딕", 9.5F);
        this.btnDel.Font = new Font("맑은 고딕", 9.5F);
        this.btnSearch.Font = new Font("맑은 고딕", 9.5F);
        this.btnClose.Font = new Font("맑은 고딕", 9.5F);

        this.btnNew.TextAlign = HorizontalAlignment.Center;
        this.btnSave.TextAlign = HorizontalAlignment.Center;
        this.btnUpd.TextAlign = HorizontalAlignment.Center;
        this.btnDel.TextAlign = HorizontalAlignment.Center;
        this.btnSearch.TextAlign = HorizontalAlignment.Center;
        this.btnClose.TextAlign = HorizontalAlignment.Center;
        //
        // toolbarPanel
        //
        this.toolbarPanel.Dock = DockStyle.Top;
        this.toolbarPanel.Height = 56;
        this.toolbarPanel.BackColor = Color.White;
        this.toolbarPanel.Controls.Add(this.btnNew);
        this.toolbarPanel.Controls.Add(this.btnSave);
        this.toolbarPanel.Controls.Add(this.btnUpd);
        this.toolbarPanel.Controls.Add(this.btnDel);
        this.toolbarPanel.Controls.Add(this.btnSearch);
        this.toolbarPanel.Controls.Add(this.btnClose);
        //
        // filterPanel (검색조건 카드 — 구분 모드 라디오버튼 + 검색 조건 입력란)
        //
        this.filterPanel.Dock = DockStyle.Fill;
        this.filterPanel.BackColor = Color.White;
        this.filterPanel.FillColor = Color.White;
        this.filterPanel.BorderColor = Color.FromArgb(209, 213, 219);
        this.filterPanel.BorderThickness = 1;
        this.filterPanel.BorderRadius = 10;
        this.filterPanel.ShadowDecoration.Enabled = false;

        this.filterTitleLabel.Text = "검색조건";
        this.filterTitleLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        this.filterTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
        this.filterTitleLabel.AutoSize = true;
        this.filterTitleLabel.Left = 16; this.filterTitleLabel.Top = 13;

        this.radModeGroup.Left = 20; this.radModeGroup.Top = 62;
        this.radModeCode.Left = this.radModeGroup.Right + 24; this.radModeCode.Top = 62;

        this.lblRcdtp.Text = "구분:";
        this.lblRcdtp.AutoSize = true;
        this.lblRcdtp.Left = this.radModeCode.Right + 30; this.lblRcdtp.Top = 60;
        this.edtRcdtp.Left = this.lblRcdtp.Right + 4; this.edtRcdtp.Top = 54; this.edtRcdtp.Width = 130;

        this.lblRetxf.Text = "전체명:";
        this.lblRetxf.AutoSize = true;
        this.lblRetxf.Left = this.edtRcdtp.Right + 24; this.lblRetxf.Top = 60;
        this.edtRetxf.Left = this.lblRetxf.Right + 4; this.edtRetxf.Top = 54; this.edtRetxf.Width = 160;

        this.lblCode.Left = this.edtRetxf.Right + 24; this.lblCode.Top = 60;
        this.edtCode.Left = this.lblCode.Right + 4; this.edtCode.Top = 54; this.edtCode.Width = 110;

        this.lblRetxs.Left = this.edtCode.Right + 24; this.lblRetxs.Top = 60;
        this.edtRetxs.Left = this.lblRetxs.Right + 4; this.edtRetxs.Top = 54; this.edtRetxs.Width = 110;

        this.edtRcdtp.BorderRadius = 6;
        this.edtRetxf.BorderRadius = 6;
        this.edtCode.BorderRadius = 6;
        this.edtRetxs.BorderRadius = 6;
        this.edtRcdtp.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtRetxf.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtCode.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtRetxs.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtRcdtp.FillColor = Color.White;
        this.edtRetxf.FillColor = Color.White;
        this.edtCode.FillColor = Color.White;
        this.edtRetxs.FillColor = Color.White;
        this.edtRcdtp.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.edtRetxf.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.edtCode.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.edtRetxs.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);

        this.filterPanel.Controls.AddRange(new Control[]
        {
            this.filterTitleLabel,
            this.radModeGroup, this.radModeCode,
            this.lblRcdtp, this.edtRcdtp,
            this.lblRetxf, this.edtRetxf,
            this.lblCode, this.edtCode,
            this.lblRetxs, this.edtRetxs
        });
        //
        // filterAreaPanel (검색조건 카드를 cardPanel 가장자리에서 여백을 두고 감싸는 영역)
        //
        this.filterAreaPanel.Dock = DockStyle.Top;
        this.filterAreaPanel.Height = 112;
        this.filterAreaPanel.BackColor = Color.White;
        this.filterAreaPanel.Padding = new Padding(16, 12, 16, 0);
        this.filterAreaPanel.Controls.Add(this.filterPanel);
        //
        // groupCardPanel (좌측: 구분코드 목록)
        //
        this.groupTitleLabel.Text = "구분코드";
        this.groupTitleLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        this.groupTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
        this.groupTitleLabel.AutoSize = true;
        this.groupTitleLabel.Left = 16; this.groupTitleLabel.Top = 11;

        this.groupHeaderPanel.Dock = DockStyle.Top;
        this.groupHeaderPanel.Height = 44;
        this.groupHeaderPanel.BackColor = Color.White;
        this.groupHeaderPanel.Controls.Add(this.groupTitleLabel);

        this.gridGroup.Dock = DockStyle.Fill;
        this.gridGroup.ReadOnly = true;
        this.gridGroup.AllowUserToAddRows = false;
        this.gridGroup.BackgroundColor = Color.White;
        this.gridGroup.BorderStyle = BorderStyle.None;
        this.gridGroup.EnableHeadersVisualStyles = false;
        this.gridGroup.ColumnHeadersHeight = 36;
        this.gridGroup.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
        this.gridGroup.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(75, 85, 99);
        this.gridGroup.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        this.gridGroup.RowTemplate.Height = 32;
        this.gridGroup.GridColor = Color.FromArgb(209, 213, 219);
        this.gridGroup.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        this.gridGroup.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        this.gridGroup.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        this.gridGroup.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.gridGroup.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
        this.gridGroup.DefaultCellStyle.SelectionForeColor = Color.FromArgb(31, 41, 55);
        this.gridGroup.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 251);
        this.gridGroup.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 249, 251);
        this.gridGroup.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);
        this.gridGroup.RowHeadersDefaultCellStyle.SelectionBackColor = Color.White;
        this.gridGroup.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);

        this.groupCardPanel.Dock = DockStyle.Left;
        this.groupCardPanel.Width = 560;
        this.groupCardPanel.BackColor = Color.White;
        this.groupCardPanel.FillColor = Color.White;
        this.groupCardPanel.BorderRadius = 10;
        this.groupCardPanel.BorderColor = Color.FromArgb(209, 213, 219);
        this.groupCardPanel.BorderThickness = 1;
        this.groupCardPanel.Padding = new Padding(10);
        this.groupCardPanel.ShadowDecoration.Enabled = false;
        this.groupCardPanel.Controls.Add(this.gridGroup);
        this.groupCardPanel.Controls.Add(this.groupHeaderPanel);
        //
        // splitterGap (두 카드 사이 여백)
        //
        this.splitterGap.Dock = DockStyle.Left;
        this.splitterGap.Width = 16;
        this.splitterGap.BackColor = Color.White;
        //
        // codeCardPanel (우측: 선택된 구분에 속한 코드 목록)
        //
        this.codeTitleLabel.Text = "공통코드";
        this.codeTitleLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        this.codeTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
        this.codeTitleLabel.AutoSize = true;
        this.codeTitleLabel.Left = 16; this.codeTitleLabel.Top = 11;

        this.codeHeaderPanel.Dock = DockStyle.Top;
        this.codeHeaderPanel.Height = 44;
        this.codeHeaderPanel.BackColor = Color.White;
        this.codeHeaderPanel.Controls.Add(this.codeTitleLabel);

        this.gridCode.Dock = DockStyle.Fill;
        this.gridCode.ReadOnly = true;
        this.gridCode.AllowUserToAddRows = false;
        this.gridCode.BackgroundColor = Color.White;
        this.gridCode.BorderStyle = BorderStyle.None;
        this.gridCode.EnableHeadersVisualStyles = false;
        this.gridCode.ColumnHeadersHeight = 36;
        this.gridCode.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
        this.gridCode.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(75, 85, 99);
        this.gridCode.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        this.gridCode.RowTemplate.Height = 32;
        this.gridCode.GridColor = Color.FromArgb(209, 213, 219);
        this.gridCode.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        this.gridCode.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        this.gridCode.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        this.gridCode.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.gridCode.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
        this.gridCode.DefaultCellStyle.SelectionForeColor = Color.FromArgb(31, 41, 55);
        this.gridCode.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 251);
        this.gridCode.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 249, 251);
        this.gridCode.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);
        this.gridCode.RowHeadersDefaultCellStyle.SelectionBackColor = Color.White;
        this.gridCode.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);

        this.codeCardPanel.Dock = DockStyle.Fill;
        this.codeCardPanel.BackColor = Color.White;
        this.codeCardPanel.FillColor = Color.White;
        this.codeCardPanel.BorderRadius = 10;
        this.codeCardPanel.BorderColor = Color.FromArgb(209, 213, 219);
        this.codeCardPanel.BorderThickness = 1;
        this.codeCardPanel.Padding = new Padding(10);
        this.codeCardPanel.ShadowDecoration.Enabled = false;
        this.codeCardPanel.Controls.Add(this.gridCode);
        this.codeCardPanel.Controls.Add(this.codeHeaderPanel);
        //
        // gridAreaPanel (Fill을 가장 먼저 추가해야 나중에 추가되는 Left 카드들이 우선권을 가짐)
        //
        this.gridAreaPanel.Dock = DockStyle.Fill;
        this.gridAreaPanel.BackColor = Color.White;
        this.gridAreaPanel.Padding = new Padding(16, 12, 16, 16);
        this.gridAreaPanel.Controls.Add(this.codeCardPanel);
        this.gridAreaPanel.Controls.Add(this.splitterGap);
        this.gridAreaPanel.Controls.Add(this.groupCardPanel);
        //
        // cardPanel (탭 콘텐츠 영역을 가장자리까지 꽉 채운다 — 더 이상 떠 있는 카드가 아니다)
        //
        this.cardPanel.Dock = DockStyle.Fill;
        this.cardPanel.FillColor = Color.White;
        this.cardPanel.BorderRadius = 0;
        this.cardPanel.BorderThickness = 0;
        this.cardPanel.ShadowDecoration.Enabled = false;
        this.cardPanel.Controls.Add(this.gridAreaPanel);
        this.cardPanel.Controls.Add(this.filterAreaPanel);
        this.cardPanel.Controls.Add(this.toolbarPanel);
        //
        // 이벤트 배선
        //
        this.radModeGroup.CheckedChanged += new EventHandler(this.RadModeGroup_CheckedChanged);
        this.radModeCode.CheckedChanged += new EventHandler(this.RadModeCode_CheckedChanged);
        this.gridGroup.SelectionChanged += new System.EventHandler(this.GridGroup_SelectionChanged);
        this.gridGroup.CellDoubleClick += new DataGridViewCellEventHandler(this.GridGroup_CellDoubleClick);
        this.gridCode.CellDoubleClick += new DataGridViewCellEventHandler(this.GridCode_CellDoubleClick);
        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        this.btnUpd.Click += new EventHandler(this.BtnUpd_Click);
        this.btnDel.Click += new EventHandler(this.BtnDel_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // BA02Form
        //
        this.Text = "공통코드 마스터";
        this.Width = 1200;
        this.Height = 800;
        this.KeyPreview = true;
        this.BackColor = Color.FromArgb(244, 247, 250);
        this.Padding = new Padding(12, 0, 0, 0);
        this.Controls.Add(this.cardPanel);
        this.Load += new EventHandler(this.BA02Form_Load);
        this.KeyDown += new KeyEventHandler(this.BA02Form_KeyDown);
        this.groupHeaderPanel.ResumeLayout(false);
        this.groupHeaderPanel.PerformLayout();
        this.groupCardPanel.ResumeLayout(false);
        this.codeHeaderPanel.ResumeLayout(false);
        this.codeHeaderPanel.PerformLayout();
        this.codeCardPanel.ResumeLayout(false);
        this.gridAreaPanel.ResumeLayout(false);
        this.toolbarPanel.ResumeLayout(false);
        this.filterPanel.ResumeLayout(false);
        this.filterPanel.PerformLayout();
        this.filterAreaPanel.ResumeLayout(false);
        this.cardPanel.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
