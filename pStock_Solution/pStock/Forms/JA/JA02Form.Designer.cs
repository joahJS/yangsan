using pStock.Common;
using Guna.UI2.WinForms;

namespace pStock.Forms.JA;

partial class JA02Form
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

    private Guna2TextBox edtCode;
    private Guna2TextBox dspName;
    private Guna2TextBox edtYear;
    private Guna2ComboBox cboHouse;
    private FastDataGridView grid;
    private Guna2Button btnNew;
    private Guna2Button btnSearch;
    private Guna2Button btnClose;
    private Guna2Button btnYearDown;
    private Guna2Button btnYearUp;
    private Label lblCode;
    private Label lblYear;
    private Label lblHouse;

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
        this.edtCode = new Guna2TextBox();
        this.dspName = new Guna2TextBox();
        this.edtYear = new Guna2TextBox();
        this.cboHouse = new Guna2ComboBox();
        this.grid = new FastDataGridView();
        this.btnNew = new Guna2Button();
        this.btnSearch = new Guna2Button();
        this.btnClose = new Guna2Button();
        this.btnYearDown = new Guna2Button();
        this.btnYearUp = new Guna2Button();
        this.lblCode = new Label();
        this.lblYear = new Label();
        this.lblHouse = new Label();
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
        // dspName / cboHouse
        //
        this.dspName.ReadOnly = true;
        this.cboHouse.DropDownStyle = ComboBoxStyle.DropDownList;
        //
        // 상단 버튼 3개 (초기화만 강조 채움, 나머지는 흰 배경 + 회색 테두리)
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

        this.lblCode.Text = "품번:";
        this.lblCode.AutoSize = false;
        this.lblCode.TextAlign = ContentAlignment.MiddleLeft;
        this.lblCode.Width = 40; this.lblCode.Height = 20;
        this.lblCode.Left = 20; this.lblCode.Top = 60;
        this.edtCode.Left = this.lblCode.Right + 2; this.edtCode.Top = 54; this.edtCode.Width = 110;
        this.edtCode.KeyDown += new KeyEventHandler(this.EdtCode_KeyDown);
        this.dspName.Left = this.edtCode.Right + 8; this.dspName.Top = 54; this.dspName.Width = 220;

        this.lblYear.Text = "년도:";
        this.lblYear.AutoSize = false;
        this.lblYear.TextAlign = ContentAlignment.MiddleLeft;
        this.lblYear.Width = 40; this.lblYear.Height = 20;
        this.lblYear.Left = this.dspName.Right + 24; this.lblYear.Top = 60;
        this.edtYear.Left = this.lblYear.Right + 2; this.edtYear.Top = 54; this.edtYear.Width = 60;
        this.edtYear.KeyDown += new KeyEventHandler(this.EdtYear_KeyDown);
        this.btnYearDown.Text = "◀";
        this.btnYearDown.Left = this.edtYear.Right + 8; this.btnYearDown.Top = 53; this.btnYearDown.Width = 32; this.btnYearDown.Height = 32;
        this.btnYearUp.Text = "▶";
        this.btnYearUp.Left = this.btnYearDown.Right + 4; this.btnYearUp.Top = 53; this.btnYearUp.Width = 32; this.btnYearUp.Height = 32;
        this.btnYearDown.Click += new EventHandler(this.BtnYearDown_Click);
        this.btnYearUp.Click += new EventHandler(this.BtnYearUp_Click);

        this.lblHouse.Text = "저장위치:";
        this.lblHouse.AutoSize = false;
        this.lblHouse.TextAlign = ContentAlignment.MiddleLeft;
        this.lblHouse.Width = 70; this.lblHouse.Height = 20;
        this.lblHouse.Left = this.btnYearUp.Right + 24; this.lblHouse.Top = 60;
        this.cboHouse.Left = this.lblHouse.Right + 2; this.cboHouse.Top = 54; this.cboHouse.Width = 130;

        this.btnYearDown.FillColor = Color.White;
        this.btnYearUp.FillColor = Color.White;
        this.btnYearDown.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnYearUp.ForeColor = Color.FromArgb(75, 85, 99);
        this.btnYearDown.BorderThickness = 1;
        this.btnYearUp.BorderThickness = 1;
        this.btnYearDown.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnYearUp.BorderColor = Color.FromArgb(209, 213, 219);
        this.btnYearDown.BorderRadius = 6;
        this.btnYearUp.BorderRadius = 6;

        this.edtCode.BorderRadius = 6;
        this.dspName.BorderRadius = 6;
        this.edtYear.BorderRadius = 6;
        this.cboHouse.BorderRadius = 6;
        this.edtCode.BorderColor = Color.FromArgb(209, 213, 219);
        this.dspName.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtYear.BorderColor = Color.FromArgb(209, 213, 219);
        this.cboHouse.BorderColor = Color.FromArgb(209, 213, 219);
        this.edtCode.FillColor = Color.White;
        this.dspName.FillColor = Color.FromArgb(248, 249, 251);
        this.edtYear.FillColor = Color.White;
        this.cboHouse.FillColor = Color.White;
        this.edtCode.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);
        this.edtYear.FocusedState.BorderColor = Color.FromArgb(47, 111, 237);

        this.filterPanel.Controls.AddRange(new Control[]
        {
            this.filterTitleLabel,
            this.lblCode, this.edtCode, this.dspName,
            this.lblYear, this.edtYear, this.btnYearDown, this.btnYearUp,
            this.lblHouse, this.cboHouse
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
        this.grid.Columns.Add("MONTH", "월");
        this.grid.Columns.Add("BQTY", "기초재고");
        this.grid.Columns.Add("IQTY", "입고수량");
        this.grid.Columns.Add("OQTY", "출고수량");
        this.grid.Columns.Add("XQTY", "재고조정");
        this.grid.Columns.Add("JQTY", "재고");
        this.grid.Columns.Add("IAMT", "입고금액");
        this.grid.Columns.Add("SAMT", "출고금액");
        this.grid.Columns.Add("OAMT", "보관금액");
        this.grid.Columns.Add("TAMT", "누계금액");
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
        // JA02Form
        //
        this.Text = "재고관리-년간";
        this.Width = 1200;
        this.Height = 800;
        this.KeyPreview = true;
        this.BackColor = Color.FromArgb(244, 247, 250);
        this.Padding = new Padding(12, 0, 0, 0);
        this.Controls.Add(this.cardPanel);
        this.Load += new EventHandler(this.JA02Form_Load);
        this.KeyDown += new KeyEventHandler(this.JA02Form_KeyDown);
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
