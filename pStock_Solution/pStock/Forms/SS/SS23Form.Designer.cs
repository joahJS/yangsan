using pStock.Common;

namespace pStock.Forms.SS;

partial class SS23Form
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
    private DateTimePicker dtpDate1;
    private DateTimePicker dtpDate2;
    private Label lblAmt0;
    private Label lblAmt1;
    private Button btnNew;
    private Button btnDel;
    private Button btnCompute;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblDate;
    private Label lblTilde;
    private Label lblAmtCap0;
    private Label lblAmtCap1;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.lblAmt0 = new Label();
        this.lblAmt1 = new Label();
        this.btnNew = new Button();
        this.btnDel = new Button();
        this.btnCompute = new Button();
        this.btnSearch = new Button();
        this.btnExcel = new Button();
        this.btnPrint = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.lblDate = new Label();
        this.lblTilde = new Label();
        this.lblAmtCap0 = new Label();
        this.lblAmtCap1 = new Label();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // lblAmt0 / lblAmt1 (원본 필드 초기값)
        //
        this.lblAmt0.AutoSize = true;
        this.lblAmt1.AutoSize = true;
        //
        // 상단 버튼 7개
        //
        this.btnNew.Text = "신규(F1)";
        this.btnDel.Text = "삭제(F4)";
        this.btnCompute.Text = "자동계산";
        this.btnSearch.Text = "조회";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";
        //
        // panelTop (원본 BuildLayout()의 top 패널: 버튼 7개를 95px 간격으로 가로 배치)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnDel, this.btnCompute, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnDel.Left = 100; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnCompute.Left = 195; this.btnCompute.Top = 8; this.btnCompute.Width = 90;
        this.btnSearch.Left = 290; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnExcel.Left = 385; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 480; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 575; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.btnExcel.Click += new EventHandler(this.BtnExcel_Click);
        this.btnPrint.Click += new EventHandler(this.BtnPrint_Click);
        //
        // panelEdit (원본 BuildLayout()의 editPanel: 기간 두 DateTimePicker + 보관금액/부가세 표시)
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.lblDate.Text = "기간";
        this.lblDate.Left = 5;
        this.lblDate.Top = 12;
        this.lblDate.AutoSize = true;
        this.dtpDate1.Left = 50;
        this.dtpDate1.Top = 8;
        this.dtpDate1.Width = 110;
        this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.lblTilde.Text = "~";
        this.lblTilde.Left = 165;
        this.lblTilde.Top = 12;
        this.lblTilde.AutoSize = true;
        this.dtpDate2.Left = 180;
        this.dtpDate2.Top = 8;
        this.dtpDate2.Width = 110;
        this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.lblAmtCap0.Text = "보관금액:";
        this.lblAmtCap0.Left = 320;
        this.lblAmtCap0.Top = 12;
        this.lblAmtCap0.AutoSize = true;
        this.lblAmt0.Left = 390;
        this.lblAmt0.Top = 12;
        this.lblAmtCap1.Text = "부가세:";
        this.lblAmtCap1.Left = 500;
        this.lblAmtCap1.Top = 12;
        this.lblAmtCap1.AutoSize = true;
        this.lblAmt1.Left = 560;
        this.lblAmt1.Top = 12;

        this.panelEdit.Controls.AddRange(new Control[] { this.lblDate, this.dtpDate1, this.lblTilde, this.dtpDate2, this.lblAmtCap0, this.lblAmt0, this.lblAmtCap1, this.lblAmt1 });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += new DataGridViewCellEventHandler(this.Grid_CellDoubleClick);
        //
        // 이벤트 배선 (원본 BuildLayout() 나머지)
        //
        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnDel.Click += new EventHandler(this.BtnDel_Click);
        this.btnCompute.Click += new EventHandler(this.BtnCompute_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // SS23Form
        //
        this.Text = "보관료 관리";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.SS23Form_Load);
        this.KeyDown += new KeyEventHandler(this.SS23Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
