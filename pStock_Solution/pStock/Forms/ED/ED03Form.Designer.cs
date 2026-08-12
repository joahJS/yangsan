using pStock.Common;

namespace pStock.Forms.ED;

partial class ED03Form
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
    private TextBox edtYear;
    private TextBox edtCode;
    private TextBox edtCvnam;
    private TextBox edtOwnam;
    private NumericUpDown edtBamt;

    private Button btnNew;
    private Button btnAdd;
    private Button btnDel;
    private Button btnSearch;
    private Button btnClose;
    private Button btnYearDown;
    private Button btnYearUp;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblYear;
    private Label lblCode;
    private Label lblBamt;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtYear = new TextBox();
        this.edtCode = new TextBox();
        this.edtCvnam = new TextBox();
        this.edtOwnam = new TextBox();
        this.edtBamt = new NumericUpDown();

        this.btnNew = new Button();
        this.btnAdd = new Button();
        this.btnDel = new Button();
        this.btnSearch = new Button();
        this.btnClose = new Button();
        this.btnYearDown = new Button();
        this.btnYearUp = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.lblYear = new Label();
        this.lblCode = new Label();
        this.lblBamt = new Label();
        //
        // 컨트롤 기본 속성
        //
        this.edtCvnam.ReadOnly = true;
        this.edtOwnam.ReadOnly = true;
        this.edtBamt.Maximum = 9999999999;
        this.edtBamt.DecimalPlaces = 0;
        this.btnNew.Text = "신규(F1)";
        this.btnAdd.Text = "저장(F2)";
        this.btnDel.Text = "삭제(F4)";
        this.btnSearch.Text = "조회";
        this.btnClose.Text = "닫기(Esc)";
        this.btnYearDown.Text = "◀";
        this.btnYearUp.Text = "▶";
        ((System.ComponentModel.ISupportInitialize)(this.edtBamt)).BeginInit();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // ED03Form
        //
        this.Text = "기초잔액 보수";
        this.Width = 1000;
        this.Height = 650;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        PublicLib.MakeTypingFriendly(this.edtBamt);

        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnAdd, this.btnDel, this.btnSearch, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnAdd.Left = 100; this.btnAdd.Top = 8; this.btnAdd.Width = 90;
        this.btnDel.Left = 195; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnSearch.Left = 290; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnClose.Left = 385; this.btnClose.Top = 8; this.btnClose.Width = 90;

        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.lblYear.Text = "년도";
        this.lblYear.Left = 5;
        this.lblYear.Top = 12;
        this.lblYear.AutoSize = true;
        this.edtYear.Left = 45; this.edtYear.Top = 8; this.edtYear.Width = 60;
        this.edtYear.KeyDown += new KeyEventHandler(this.EdtYear_KeyDown);
        this.btnYearDown.Left = 110; this.btnYearDown.Top = 8; this.btnYearDown.Width = 30;
        this.btnYearUp.Left = 143; this.btnYearUp.Top = 8; this.btnYearUp.Width = 30;
        this.btnYearDown.Click += new EventHandler(this.BtnYearDown_Click);
        this.btnYearUp.Click += new EventHandler(this.BtnYearUp_Click);

        this.lblCode.Text = "거래처코드";
        this.lblCode.Left = 190;
        this.lblCode.Top = 12;
        this.lblCode.AutoSize = true;
        this.edtCode.Left = 260; this.edtCode.Top = 8; this.edtCode.Width = 80;
        this.edtCode.KeyDown += new KeyEventHandler(this.EdtCode_KeyDown);
        this.edtCvnam.Left = 350; this.edtCvnam.Top = 8; this.edtCvnam.Width = 150;
        this.edtOwnam.Left = 510; this.edtOwnam.Top = 8; this.edtOwnam.Width = 120;

        this.lblBamt.Text = "기초잔액";
        this.lblBamt.Left = 640;
        this.lblBamt.Top = 12;
        this.lblBamt.AutoSize = true;
        this.edtBamt.Left = 710; this.edtBamt.Top = 8; this.edtBamt.Width = 130;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblYear, this.edtYear, this.btnYearDown, this.btnYearUp, this.lblCode, this.edtCode, this.edtCvnam, this.edtOwnam, this.lblBamt, this.edtBamt
        });

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += new DataGridViewCellEventHandler(this.Grid_CellDoubleClick);

        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
        this.btnDel.Click += new EventHandler(this.BtnDel_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // Load / KeyDown
        //
        this.Load += new EventHandler(this.ED03Form_Load);
        this.KeyDown += new KeyEventHandler(this.ED03Form_KeyDown);
        ((System.ComponentModel.ISupportInitialize)(this.edtBamt)).EndInit();
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
