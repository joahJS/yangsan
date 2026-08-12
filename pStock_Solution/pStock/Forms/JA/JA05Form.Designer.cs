using pStock.Common;

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
    private TextBox edtMonth;
    private ComboBox cSrcd;
    private TextBox eSrwd;
    private CheckBox chkOnlyNonZero;
    private Button btnNew;
    private Button btnSearch;
    private Button btnClose;
    private Button btnMonthDown;
    private Button btnMonthUp;
    private Label lblSum;
    private Panel panelTop;
    private Panel panelEdit;
    private Panel panelBottom;
    private Label lblMonth;
    private Label lblSearch;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtMonth = new TextBox();
        this.cSrcd = new ComboBox();
        this.eSrwd = new TextBox();
        this.chkOnlyNonZero = new CheckBox();
        this.btnNew = new Button();
        this.btnSearch = new Button();
        this.btnClose = new Button();
        this.btnMonthDown = new Button();
        this.btnMonthUp = new Button();
        this.lblSum = new Label();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.panelBottom = new Panel();
        this.lblMonth = new Label();
        this.lblSearch = new Label();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.panelBottom.SuspendLayout();
        this.SuspendLayout();
        //
        // cSrcd / chkOnlyNonZero / lblSum
        //
        this.cSrcd.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cSrcd.Items.AddRange(new object[] { "거래처코드", "거래처명" });
        this.chkOnlyNonZero.Text = "금액 0 제외";
        this.lblSum.AutoSize = true;
        //
        // 버튼 텍스트
        //
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnClose.Text = "닫기(Esc)";
        this.btnMonthDown.Text = "◀";
        this.btnMonthUp.Text = "▶";
        //
        // panelTop
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 100;
        this.btnSearch.Left = 110; this.btnSearch.Top = 8; this.btnSearch.Width = 100;
        this.btnClose.Left = 215; this.btnClose.Top = 8; this.btnClose.Width = 100;
        //
        // panelEdit
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.lblMonth.Text = "조회월(YYYY-MM)";
        this.lblMonth.Left = 10; this.lblMonth.Top = 12; this.lblMonth.AutoSize = true;
        this.edtMonth.Left = 140; this.edtMonth.Top = 8; this.edtMonth.Width = 80;
        this.edtMonth.KeyDown += new KeyEventHandler(this.EdtMonth_KeyDown);
        this.btnMonthDown.Left = 225; this.btnMonthDown.Top = 8; this.btnMonthDown.Width = 30;
        this.btnMonthUp.Left = 258; this.btnMonthUp.Top = 8; this.btnMonthUp.Width = 30;
        this.btnMonthDown.Click += new EventHandler(this.BtnMonthDown_Click);
        this.btnMonthUp.Click += new EventHandler(this.BtnMonthUp_Click);

        this.lblSearch.Text = "검색조건";
        this.lblSearch.Left = 320; this.lblSearch.Top = 12; this.lblSearch.AutoSize = true;
        this.cSrcd.Left = 390; this.cSrcd.Top = 8; this.cSrcd.Width = 100;
        this.eSrwd.Left = 500; this.eSrwd.Top = 8; this.eSrwd.Width = 180;
        this.chkOnlyNonZero.Left = 700; this.chkOnlyNonZero.Top = 10; this.chkOnlyNonZero.AutoSize = true;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp, this.lblSearch, this.cSrcd, this.eSrwd, this.chkOnlyNonZero
        });
        //
        // panelBottom
        //
        this.panelBottom.Dock = DockStyle.Bottom;
        this.panelBottom.Height = 30;
        this.lblSum.Left = 10; this.lblSum.Top = 6;
        this.panelBottom.Controls.Add(this.lblSum);
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("SALNO", "전표번호");
        this.grid.Columns.Add("TDATE", "일자");
        this.grid.Columns.Add("CVNAM", "거래처명");
        this.grid.Columns.Add("LNNAM", "착지처명");
        this.grid.Columns.Add("ADDR1", "주소");
        this.grid.Columns.Add("SSAMT", "금액");
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
        this.Width = 1000;
        this.Height = 600;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelBottom);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.JA05Form_Load);
        this.KeyDown += new KeyEventHandler(this.JA05Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.panelBottom.ResumeLayout(false);
        this.panelBottom.PerformLayout();
        this.ResumeLayout(false);
    }
}
