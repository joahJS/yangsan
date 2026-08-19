using pStock.Common;
using Guna.UI2.WinForms;

namespace pStock.Forms.JA;

partial class JA01Form
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

    private FastDataGridView grid;
    private Guna2TextBox edtMonth;
    private Guna2TextBox edtNo;
    private Guna2TextBox edtCvnam;
    private Guna2ComboBox cboHouse;
    private Guna2Button btnNew;
    private Guna2Button btnSearch;
    private Guna2Button btnExcel;
    private Guna2Button btnPrint;
    private Guna2Button btnClose;
    private Guna2Button btnMonthDown;
    private Guna2Button btnMonthUp;
    private Label lblMonth;
    private Label lblNo;
    private Label lblCvnam;
    private Label lblHouse;
    private RadioButton radModeItem;
    private RadioButton radModeVendor;
    private RadioButton radModeHouse;

    private Guna2Panel cardPanel;
    private Panel toolbarPanel;
    private Panel filterAreaPanel;
    private Guna2Panel filterPanel;
    private Label filterTitleLabel;
    private Panel gridAreaPanel;
    private Guna2Panel gridCardPanel;
    private Panel gridHeaderPanel;
    private Label gridTitleLabel;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtMonth = new Guna2TextBox();
        this.edtNo = new Guna2TextBox();
        this.edtCvnam = new Guna2TextBox();
        this.cboHouse = new Guna2ComboBox();
        this.btnNew = new Guna2Button();
        this.btnSearch = new Guna2Button();
        this.btnExcel = new Guna2Button();
        this.btnPrint = new Guna2Button();
        this.btnClose = new Guna2Button();
        this.btnMonthDown = new Guna2Button();
        this.btnMonthUp = new Guna2Button();
        this.lblMonth = new Label();
        this.lblNo = new Label();
        this.lblCvnam = new Label();
        this.lblHouse = new Label();
        this.radModeItem = new RadioButton();
        this.radModeVendor = new RadioButton();
        this.radModeHouse = new RadioButton();
        this.cardPanel = new Guna2Panel();
        this.toolbarPanel = new Panel();
        this.filterAreaPanel = new Panel();
        this.filterPanel = new Guna2Panel();
        this.filterTitleLabel = new Label();
        this.gridAreaPanel = new Panel();
        this.gridCardPanel = new Guna2Panel();
        this.gridHeaderPanel = new Panel();
        this.gridTitleLabel = new Label();
        this.toolbarPanel.SuspendLayout();
        this.filterAreaPanel.SuspendLayout();
        this.filterPanel.SuspendLayout();
        this.gridHeaderPanel.SuspendLayout();
        this.gridCardPanel.SuspendLayout();
        this.gridAreaPanel.SuspendLayout();
        this.cardPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // 상단 버튼 5개 (초기화만 강조 채움, 나머지는 흰 배경 + 회색 테두리)
        //
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";

        this.btnNew.Left = 12; this.btnNew.Top = 15; this.btnNew.Width = 110; this.btnNew.Height = 36;
        this.btnSearch.Left = 130; this.btnSearch.Top = 15; this.btnSearch.Width = 110; this.btnSearch.Height = 36;
        this.btnExcel.Left = 248; this.btnExcel.Top = 15; this.btnExcel.Width = 110; this.btnExcel.Height = 36;
        this.btnPrint.Left = 366; this.btnPrint.Top = 15; this.btnPrint.Width = 110; this.btnPrint.Height = 36;
        this.btnClose.Left = 484; this.btnClose.Top = 15; this.btnClose.Width = 110; this.btnClose.Height = 36;

        this.btnNew.BorderRadius = 8;
        this.btnSearch.BorderRadius = 8;
        this.btnExcel.BorderRadius = 8;
        this.btnPrint.BorderRadius = 8;
        this.btnClose.BorderRadius = 8;

        this.btnNew.ImageSize = new Size(15, 15);
        this.btnSearch.ImageSize = new Size(15, 15);
        this.btnExcel.ImageSize = new Size(15, 15);
        this.btnPrint.ImageSize = new Size(15, 15);
        this.btnClose.ImageSize = new Size(15, 15);

        this.btnNew.FillColor = Color.FromArgb(47, 111, 237);
        this.btnNew.ForeColor = Color.White;
        this.btnNew.BorderThickness = 0;
        this.btnNew.HoverState.FillColor = Color.FromArgb(35, 96, 220);

        this.btnSearch.FillColor = Color.White;
        this.btnExcel.FillColor = Color.White;
        this.btnPrint.FillColor = Color.White;
        this.btnClose.FillColor = Color.White;

        this.btnSearch.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnExcel.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnPrint.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnClose.ForeColor = Color.FromArgb(75, 85, 99);

        this.btnSearch.BorderThickness = 1;
        this.btnExcel.BorderThickness = 1;
        this.btnPrint.BorderThickness = 1;
        this.btnClose.BorderThickness = 1;

        this.btnSearch.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnExcel.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnPrint.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnClose.BorderColor = Color.FromArgb(209, 213, 219);

        this.btnNew.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        this.btnSearch.Font = new Font("맑은 고딕", 9.5F);
        this.btnExcel.Font = new Font("맑은 고딕", 9.5F);
        this.btnPrint.Font = new Font("맑은 고딕", 9.5F);
        this.btnClose.Font = new Font("맑은 고딕", 9.5F);

        this.btnNew.TextAlign = HorizontalAlignment.Center;
        this.btnSearch.TextAlign = HorizontalAlignment.Center;
        this.btnExcel.TextAlign = HorizontalAlignment.Center;
        this.btnPrint.TextAlign = HorizontalAlignment.Center;
        this.btnClose.TextAlign = HorizontalAlignment.Center;

        this.btnNew.ImageAlign = HorizontalAlignment.Left;
        this.btnSearch.ImageAlign = HorizontalAlignment.Left;
        this.btnExcel.ImageAlign = HorizontalAlignment.Left;
        this.btnPrint.ImageAlign = HorizontalAlignment.Left;
        this.btnClose.ImageAlign = HorizontalAlignment.Left;
        this.btnNew.ImageOffset = new Point(14, 0);
        this.btnSearch.ImageOffset = new Point(14, 0);
        this.btnExcel.ImageOffset = new Point(14, 0);
        this.btnPrint.ImageOffset = new Point(14, 0);
        this.btnClose.ImageOffset = new Point(14, 0);
        this.btnNew.TextOffset = new Point(18, 0);
        this.btnSearch.TextOffset = new Point(18, 0);
        this.btnExcel.TextOffset = new Point(18, 0);
        this.btnPrint.TextOffset = new Point(18, 0);
        this.btnClose.TextOffset = new Point(18, 0);
        //
        // toolbarPanel
        //
        this.toolbarPanel.Dock = DockStyle.Top;
        this.toolbarPanel.Height = 56;
        this.toolbarPanel.BackColor = Color.White;
        this.toolbarPanel.Controls.Add(this.btnNew);
        this.toolbarPanel.Controls.Add(this.btnSearch);
        this.toolbarPanel.Controls.Add(this.btnExcel);
        this.toolbarPanel.Controls.Add(this.btnPrint);
        this.toolbarPanel.Controls.Add(this.btnClose);
        //
        // filterPanel (검색조건 카드 — 조회 모드 라디오버튼 + 모드별 검색란 + 조회월)
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

        this.radModeItem.Text = "품목별";
        this.radModeItem.Checked = true;
        this.radModeItem.AutoSize = true;
        this.radModeItem.Left = 20; this.radModeItem.Top = 62;
        this.radModeVendor.Text = "거래처별";
        this.radModeVendor.AutoSize = true;
        this.radModeVendor.Left = this.radModeItem.Right + 24; this.radModeVendor.Top = 62;
        this.radModeHouse.Text = "저장위치별";
        this.radModeHouse.AutoSize = true;
        this.radModeHouse.Left = this.radModeVendor.Right + 24; this.radModeHouse.Top = 62;

        this.lblNo.Text = "품명검색:";
        this.lblNo.AutoSize = false;
        this.lblNo.TextAlign = ContentAlignment.MiddleLeft;
        this.lblNo.Width = 70; this.lblNo.Height = 20;
        this.lblNo.Left = this.radModeHouse.Right + 30; this.lblNo.Top = 60;
        this.edtNo.Left = this.lblNo.Right + 2; this.edtNo.Top = 54; this.edtNo.Width = 180;

        this.lblCvnam.Text = "거래처검색:";
        this.lblCvnam.AutoSize = false;
        this.lblCvnam.TextAlign = ContentAlignment.MiddleLeft;
        this.lblCvnam.Width = 80; this.lblCvnam.Height = 20;
        this.lblCvnam.Left = this.radModeHouse.Right + 30; this.lblCvnam.Top = 60;
        this.edtCvnam.Left = this.lblCvnam.Right + 2; this.edtCvnam.Top = 54; this.edtCvnam.Width = 180;

        this.lblHouse.Text = "저장위치:";
        this.lblHouse.AutoSize = false;
        this.lblHouse.TextAlign = ContentAlignment.MiddleLeft;
        this.lblHouse.Width = 70; this.lblHouse.Height = 20;
        this.lblHouse.Left = this.radModeHouse.Right + 30; this.lblHouse.Top = 60;
        this.cboHouse.Left = this.lblHouse.Right + 2; this.cboHouse.Top = 54; this.cboHouse.Width = 160;
        this.cboHouse.DropDownStyle = ComboBoxStyle.DropDownList;

        this.lblMonth.Text = "조회월(YYYY-MM):";
        this.lblMonth.AutoSize = false;
        this.lblMonth.TextAlign = ContentAlignment.MiddleLeft;
        this.lblMonth.Width = 120; this.lblMonth.Height = 20;
        this.lblMonth.Left = 780; this.lblMonth.Top = 60;
        this.edtMonth.Left = this.lblMonth.Right + 2; this.edtMonth.Top = 54; this.edtMonth.Width = 80;
        this.btnMonthDown.Text = "◀";
        this.btnMonthDown.Left = this.edtMonth.Right + 8; this.btnMonthDown.Top = 53; this.btnMonthDown.Width = 32; this.btnMonthDown.Height = 32;
        this.btnMonthUp.Text = "▶";
        this.btnMonthUp.Left = this.btnMonthDown.Right + 4; this.btnMonthUp.Top = 53; this.btnMonthUp.Width = 32; this.btnMonthUp.Height = 32;

        this.btnMonthDown.FillColor = Color.White;
        this.btnMonthUp.FillColor = Color.White;
        this.btnMonthDown.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnMonthUp.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnMonthDown.BorderThickness = 1;
        this.btnMonthUp.BorderThickness = 1;
        this.btnMonthDown.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnMonthUp.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnMonthDown.BorderRadius = 6;
        this.btnMonthUp.BorderRadius = 6;

        this.edtMonth.BorderRadius = 6;
        this.edtNo.BorderRadius = 6;
        this.edtCvnam.BorderRadius = 6;
        this.cboHouse.BorderRadius = 6;
        this.edtMonth.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtNo.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtCvnam.BorderColor = Color.FromArgb(209, 213, 219);
        this.cboHouse.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtMonth.FillColor = Color.White;
        this.edtNo.FillColor = Color.White;
        this.edtCvnam.FillColor = Color.White;
        this.cboHouse.FillColor = Color.White;
        this.edtMonth.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.edtNo.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.edtCvnam.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);

        this.filterPanel.Controls.AddRange(new Control[]
        {
            this.filterTitleLabel,
            this.radModeItem, this.radModeVendor, this.radModeHouse,
            this.lblNo, this.edtNo,
            this.lblCvnam, this.edtCvnam,
            this.lblHouse, this.cboHouse,
            this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp
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
        // gridHeaderPanel / gridTitleLabel
        //
        this.gridTitleLabel.Text = "재고 목록";
        this.gridTitleLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        this.gridTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
        this.gridTitleLabel.AutoSize = true;
        this.gridTitleLabel.Left = 16; this.gridTitleLabel.Top = 11;

        this.gridHeaderPanel.Dock = DockStyle.Top;
        this.gridHeaderPanel.Height = 44;
        this.gridHeaderPanel.BackColor = Color.White;
        this.gridHeaderPanel.Controls.Add(this.gridTitleLabel);
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.BackgroundColor = Color.White;
        this.grid.BorderStyle = BorderStyle.None;
        this.grid.EnableHeadersVisualStyles = false;
        this.grid.ColumnHeadersHeight = 36;
        this.grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
        this.grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(75, 85, 99);
        this.grid.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        this.grid.RowTemplate.Height = 32;
        this.grid.GridColor = Color.FromArgb(209, 213, 219);
        this.grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
        this.grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        this.grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        this.grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
        this.grid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(31, 41, 55);
        this.grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 251);
        this.grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 249, 251);
        this.grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);
        this.grid.RowHeadersDefaultCellStyle.SelectionBackColor = Color.White;
        this.grid.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(75, 85, 99);
        //
        // gridCardPanel
        //
        this.gridCardPanel.Dock = DockStyle.Fill;
        this.gridCardPanel.BackColor = Color.White;
        this.gridCardPanel.FillColor = Color.White;
        this.gridCardPanel.BorderRadius = 10;
        this.gridCardPanel.BorderColor = Color.FromArgb(209, 213, 219);
        this.gridCardPanel.BorderThickness = 1;
        this.gridCardPanel.Padding = new Padding(10);
        this.gridCardPanel.ShadowDecoration.Enabled = false;
        this.gridCardPanel.Controls.Add(this.grid);
        this.gridCardPanel.Controls.Add(this.gridHeaderPanel);
        //
        // gridAreaPanel
        //
        this.gridAreaPanel.Dock = DockStyle.Fill;
        this.gridAreaPanel.BackColor = Color.White;
        this.gridAreaPanel.Padding = new Padding(16, 6, 16, 16);
        this.gridAreaPanel.Controls.Add(this.gridCardPanel);
        //
        // cardPanel (탭 콘텐츠 영역을 가장자리까지 꽉 채운다)
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
        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnExcel.Click += new EventHandler(this.BtnExcel_Click);
        this.btnPrint.Click += new EventHandler(this.BtnPrint_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        this.btnMonthDown.Click += new EventHandler(this.BtnMonthDown_Click);
        this.btnMonthUp.Click += new EventHandler(this.BtnMonthUp_Click);
        this.edtMonth.KeyDown += new KeyEventHandler(this.EdtMonth_KeyDown);
        this.edtNo.KeyUp += new KeyEventHandler(this.EdtNo_KeyUp);
        this.edtCvnam.KeyUp += new KeyEventHandler(this.EdtCvnam_KeyUp);
        this.cboHouse.SelectedIndexChanged += new EventHandler(this.CboHouse_SelectedIndexChanged);
        this.radModeItem.CheckedChanged += new EventHandler(this.RadMode_CheckedChanged);
        this.radModeVendor.CheckedChanged += new EventHandler(this.RadMode_CheckedChanged);
        this.radModeHouse.CheckedChanged += new EventHandler(this.RadMode_CheckedChanged);
        //
        // JA01Form
        //
        this.Text = "재고관리";
        this.Width = 1200;
        this.Height = 800;
        this.KeyPreview = true;
        this.BackColor = Color.FromArgb(244, 247, 250);
        this.Padding = new Padding(12, 0, 0, 0);
        this.Controls.Add(this.cardPanel);
        this.Load += new EventHandler(this.JA01Form_Load);
        this.KeyDown += new KeyEventHandler(this.JA01Form_KeyDown);
        this.gridHeaderPanel.ResumeLayout(false);
        this.gridHeaderPanel.PerformLayout();
        this.gridCardPanel.ResumeLayout(false);
        this.gridAreaPanel.ResumeLayout(false);
        this.toolbarPanel.ResumeLayout(false);
        this.filterPanel.ResumeLayout(false);
        this.filterPanel.PerformLayout();
        this.filterAreaPanel.ResumeLayout(false);
        this.cardPanel.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
