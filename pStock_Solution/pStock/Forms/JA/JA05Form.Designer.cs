using pStock.Common;
using Guna.UI2.WinForms;

namespace pStock.Forms.JA;

partial class JA05Form
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
    private Guna2ComboBox cSrcd;
    private Guna2TextBox eSrwd;
    private CheckBox chkOnlyNonZero;
    private Guna2Button btnNew;
    private Guna2Button btnSearch;
    private Guna2Button btnClose;
    private Guna2Button btnMonthDown;
    private Guna2Button btnMonthUp;
    private Label lblSum;
    private Label lblMonth;
    private Label lblSearch;

    private Guna2Panel cardPanel;
    private Panel toolbarPanel;
    private Panel filterAreaPanel;
    private Guna2Panel filterPanel;
    private Label filterTitleLabel;
    private Panel gridAreaPanel;
    private Guna2Panel gridCardPanel;
    private Panel gridHeaderPanel;
    private Label gridTitleLabel;
    private Panel sumPanel;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtMonth = new Guna2TextBox();
        this.cSrcd = new Guna2ComboBox();
        this.eSrwd = new Guna2TextBox();
        this.chkOnlyNonZero = new CheckBox();
        this.btnNew = new Guna2Button();
        this.btnSearch = new Guna2Button();
        this.btnClose = new Guna2Button();
        this.btnMonthDown = new Guna2Button();
        this.btnMonthUp = new Guna2Button();
        this.lblSum = new Label();
        this.lblMonth = new Label();
        this.lblSearch = new Label();
        this.cardPanel = new Guna2Panel();
        this.toolbarPanel = new Panel();
        this.filterAreaPanel = new Panel();
        this.filterPanel = new Guna2Panel();
        this.filterTitleLabel = new Label();
        this.gridAreaPanel = new Panel();
        this.gridCardPanel = new Guna2Panel();
        this.gridHeaderPanel = new Panel();
        this.gridTitleLabel = new Label();
        this.sumPanel = new Panel();
        this.toolbarPanel.SuspendLayout();
        this.filterAreaPanel.SuspendLayout();
        this.filterPanel.SuspendLayout();
        this.gridHeaderPanel.SuspendLayout();
        this.sumPanel.SuspendLayout();
        this.gridCardPanel.SuspendLayout();
        this.gridAreaPanel.SuspendLayout();
        this.cardPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // cSrcd / chkOnlyNonZero
        //
        this.cSrcd.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cSrcd.Items.AddRange(new object[] { "거래처코드", "거래처명" });
        this.chkOnlyNonZero.Text = "금액 0 제외";
        this.chkOnlyNonZero.AutoSize = true;
        //
        // 상단 버튼 3개
        //
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnClose.Text = "닫기(Esc)";

        this.btnNew.Left = 12; this.btnNew.Top = 15; this.btnNew.Width = 110; this.btnNew.Height = 36;
        this.btnSearch.Left = 130; this.btnSearch.Top = 15; this.btnSearch.Width = 110; this.btnSearch.Height = 36;
        this.btnClose.Left = 248; this.btnClose.Top = 15; this.btnClose.Width = 110; this.btnClose.Height = 36;

        this.btnNew.BorderRadius = 8;
        this.btnSearch.BorderRadius = 8;
        this.btnClose.BorderRadius = 8;

        this.btnNew.ImageSize = new Size(15, 15);
        this.btnSearch.ImageSize = new Size(15, 15);
        this.btnClose.ImageSize = new Size(15, 15);

        this.btnNew.FillColor = Color.FromArgb(47, 111, 237);
        this.btnNew.ForeColor = Color.White;
        this.btnNew.BorderThickness = 0;
        this.btnNew.HoverState.FillColor = Color.FromArgb(35, 96, 220);

        this.btnSearch.FillColor = Color.White;
        this.btnClose.FillColor = Color.White;
        this.btnSearch.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnClose.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnSearch.BorderThickness = 1;
        this.btnClose.BorderThickness = 1;
        this.btnSearch.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnClose.BorderColor = Color.FromArgb(209, 213, 219);

        this.btnNew.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        this.btnSearch.Font = new Font("맑은 고딕", 9.5F);
        this.btnClose.Font = new Font("맑은 고딕", 9.5F);

        this.btnNew.TextAlign = HorizontalAlignment.Center;
        this.btnSearch.TextAlign = HorizontalAlignment.Center;
        this.btnClose.TextAlign = HorizontalAlignment.Center;
        this.btnNew.ImageAlign = HorizontalAlignment.Left;
        this.btnSearch.ImageAlign = HorizontalAlignment.Left;
        this.btnClose.ImageAlign = HorizontalAlignment.Left;
        this.btnNew.ImageOffset = new Point(14, 0);
        this.btnSearch.ImageOffset = new Point(14, 0);
        this.btnClose.ImageOffset = new Point(14, 0);
        this.btnNew.TextOffset = new Point(18, 0);
        this.btnSearch.TextOffset = new Point(18, 0);
        this.btnClose.TextOffset = new Point(18, 0);
        //
        // toolbarPanel
        //
        this.toolbarPanel.Dock = DockStyle.Top;
        this.toolbarPanel.Height = 56;
        this.toolbarPanel.BackColor = Color.White;
        this.toolbarPanel.Controls.Add(this.btnNew);
        this.toolbarPanel.Controls.Add(this.btnSearch);
        this.toolbarPanel.Controls.Add(this.btnClose);
        //
        // filterPanel (검색조건 카드)
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

        this.lblMonth.Text = "조회월(YYYY-MM):";
        this.lblMonth.AutoSize = false;
        this.lblMonth.TextAlign = ContentAlignment.MiddleLeft;
        this.lblMonth.Width = 120; this.lblMonth.Height = 20;
        this.lblMonth.Left = 20; this.lblMonth.Top = 60;
        this.edtMonth.Left = this.lblMonth.Right + 2; this.edtMonth.Top = 54; this.edtMonth.Width = 80;
        this.edtMonth.KeyDown += new KeyEventHandler(this.EdtMonth_KeyDown);
        this.btnMonthDown.Text = "◀";
        this.btnMonthDown.Left = this.edtMonth.Right + 8; this.btnMonthDown.Top = 53; this.btnMonthDown.Width = 32; this.btnMonthDown.Height = 32;
        this.btnMonthUp.Text = "▶";
        this.btnMonthUp.Left = this.btnMonthDown.Right + 4; this.btnMonthUp.Top = 53; this.btnMonthUp.Width = 32; this.btnMonthUp.Height = 32;
        this.btnMonthDown.Click += new EventHandler(this.BtnMonthDown_Click);
        this.btnMonthUp.Click += new EventHandler(this.BtnMonthUp_Click);

        this.lblSearch.Text = "검색조건:";
        this.lblSearch.AutoSize = false;
        this.lblSearch.TextAlign = ContentAlignment.MiddleLeft;
        this.lblSearch.Width = 70; this.lblSearch.Height = 20;
        this.lblSearch.Left = this.btnMonthUp.Right + 24; this.lblSearch.Top = 60;
        this.cSrcd.Left = this.lblSearch.Right + 2; this.cSrcd.Top = 54; this.cSrcd.Width = 110;
        this.eSrwd.Left = this.cSrcd.Right + 8; this.eSrwd.Top = 54; this.eSrwd.Width = 180;
        this.chkOnlyNonZero.Left = this.eSrwd.Right + 16; this.chkOnlyNonZero.Top = 60;

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
        this.cSrcd.BorderRadius = 6;
        this.eSrwd.BorderRadius = 6;
        this.edtMonth.BorderColor = Color.FromArgb(209, 213, 219);
        this.cSrcd.BorderColor = Color.FromArgb(209, 213, 219);
        this.eSrwd.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtMonth.FillColor = Color.White;
        this.cSrcd.FillColor = Color.White;
        this.eSrwd.FillColor = Color.White;
        this.edtMonth.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.eSrwd.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);

        this.filterPanel.Controls.AddRange(new Control[]
        {
            this.filterTitleLabel,
            this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp,
            this.lblSearch, this.cSrcd, this.eSrwd, this.chkOnlyNonZero
        });
        //
        // filterAreaPanel
        //
        this.filterAreaPanel.Dock = DockStyle.Top;
        this.filterAreaPanel.Height = 112;
        this.filterAreaPanel.BackColor = Color.White;
        this.filterAreaPanel.Padding = new Padding(16, 12, 16, 0);
        this.filterAreaPanel.Controls.Add(this.filterPanel);
        //
        // gridHeaderPanel / gridTitleLabel
        //
        this.gridTitleLabel.Text = "운송현황 목록";
        this.gridTitleLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        this.gridTitleLabel.ForeColor = Color.FromArgb(31, 41, 55);
        this.gridTitleLabel.AutoSize = true;
        this.gridTitleLabel.Left = 16; this.gridTitleLabel.Top = 11;

        this.gridHeaderPanel.Dock = DockStyle.Top;
        this.gridHeaderPanel.Height = 44;
        this.gridHeaderPanel.BackColor = Color.White;
        this.gridHeaderPanel.Controls.Add(this.gridTitleLabel);
        //
        // sumPanel (합계금액 표시줄)
        //
        this.lblSum.AutoSize = true;
        this.lblSum.Font = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        this.lblSum.ForeColor = Color.FromArgb(31, 41, 55);
        this.lblSum.Left = 16; this.lblSum.Top = 10;
        this.sumPanel.Dock = DockStyle.Bottom;
        this.sumPanel.Height = 36;
        this.sumPanel.BackColor = Color.FromArgb(248, 249, 251);
        this.sumPanel.Controls.Add(this.lblSum);
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
        this.grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        this.grid.Columns.Add("SALNO", "전표번호");
        this.grid.Columns.Add("TDATE", "일자");
        this.grid.Columns.Add("CVNAM", "거래처명");
        this.grid.Columns.Add("LNNAM", "착지처명");
        this.grid.Columns.Add("ADDR1", "주소");
        this.grid.Columns.Add("SSAMT", "금액");
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
        this.gridCardPanel.Controls.Add(this.sumPanel);
        this.gridCardPanel.Controls.Add(this.gridHeaderPanel);
        //
        // gridAreaPanel
        //
        this.gridAreaPanel.Dock = DockStyle.Fill;
        this.gridAreaPanel.BackColor = Color.White;
        this.gridAreaPanel.Padding = new Padding(16, 6, 16, 16);
        this.gridAreaPanel.Controls.Add(this.gridCardPanel);
        //
        // cardPanel
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
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // JA05Form
        //
        this.Text = "운송현황";
        this.Width = 1100;
        this.Height = 800;
        this.KeyPreview = true;
        this.BackColor = Color.FromArgb(244, 247, 250);
        this.Padding = new Padding(12, 0, 0, 0);
        this.Controls.Add(this.cardPanel);
        this.Load += new EventHandler(this.JA05Form_Load);
        this.KeyDown += new KeyEventHandler(this.JA05Form_KeyDown);
        this.gridHeaderPanel.ResumeLayout(false);
        this.gridHeaderPanel.PerformLayout();
        this.sumPanel.ResumeLayout(false);
        this.sumPanel.PerformLayout();
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
