using pStock.Common;

namespace pStock.Forms.SS;

partial class SS34Form
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
    private TextBox edtCvcod;
    private TextBox dspName;
    private Label lblAmt;

    private Button btnNew;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Button btnMonthDown;
    private Button btnMonthUp;

    private Panel panelTop;
    private Panel editPanel;
    private Label lblMonth;
    private Label lblCvcod;
    private Label lblAmtCap;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtMonth = new TextBox();
        this.edtCvcod = new TextBox();
        this.dspName = new TextBox();
        this.dspName.ReadOnly = true;
        this.lblAmt = new Label();
        this.lblAmt.AutoSize = true;

        this.btnNew = new Button();
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch = new Button();
        this.btnSearch.Text = "조회(F5)";
        this.btnExcel = new Button();
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint = new Button();
        this.btnPrint.Text = "인쇄";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";
        this.btnMonthDown = new Button();
        this.btnMonthDown.Text = "◀";
        this.btnMonthUp = new Button();
        this.btnMonthUp.Text = "▶";

        this.SuspendLayout();
        //
        // SS34Form
        //
        this.Text = "미수금 조회";
        this.Width = 1100;
        this.Height = 600;
        this.KeyPreview = true;

        //
        // 원본 BuildLayout() 그대로 이동
        //
        this.panelTop = new Panel();
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 100;
        this.btnSearch.Left = 110; this.btnSearch.Top = 8; this.btnSearch.Width = 100;
        this.btnExcel.Left = 215; this.btnExcel.Top = 8; this.btnExcel.Width = 100;
        this.btnPrint.Left = 320; this.btnPrint.Top = 8; this.btnPrint.Width = 100;
        this.btnClose.Left = 425; this.btnClose.Top = 8; this.btnClose.Width = 100;
        this.btnExcel.Click += new EventHandler(this.BtnExcel_Click);
        this.btnPrint.Click += new EventHandler(this.BtnPrint_Click);

        this.editPanel = new Panel();
        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 40;
        this.lblMonth = new Label();
        this.lblMonth.Text = "조회월(YYYY-MM)";
        this.lblMonth.Left = 10;
        this.lblMonth.Top = 12;
        this.lblMonth.AutoSize = true;
        this.edtMonth.Left = 140; this.edtMonth.Top = 8; this.edtMonth.Width = 80;
        this.edtMonth.KeyDown += new KeyEventHandler(this.EdtMonth_KeyDown);
        this.btnMonthDown.Left = 225; this.btnMonthDown.Top = 8; this.btnMonthDown.Width = 30;
        this.btnMonthUp.Left = 258; this.btnMonthUp.Top = 8; this.btnMonthUp.Width = 30;
        this.btnMonthDown.Click += new EventHandler(this.BtnMonthDown_Click);
        this.btnMonthUp.Click += new EventHandler(this.BtnMonthUp_Click);

        this.lblCvcod = new Label();
        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 320;
        this.lblCvcod.Top = 12;
        this.lblCvcod.AutoSize = true;
        this.edtCvcod.Left = 400; this.edtCvcod.Top = 8; this.edtCvcod.Width = 80;
        this.edtCvcod.KeyDown += this.EdtCvcod_KeyDown;
        this.dspName.Left = 490; this.dspName.Top = 8; this.dspName.Width = 200;

        this.lblAmtCap = new Label();
        this.lblAmtCap.Text = "미수금합계:";
        this.lblAmtCap.Left = 720;
        this.lblAmtCap.Top = 12;
        this.lblAmtCap.AutoSize = true;
        this.lblAmt.Left = 800; this.lblAmt.Top = 12;

        this.editPanel.Controls.AddRange(new Control[]
        {
            this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp, this.lblCvcod, this.edtCvcod, this.dspName, this.lblAmtCap, this.lblAmt
        });

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;

        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Load += new EventHandler(this.SS34Form_Load);
        this.KeyDown += new KeyEventHandler(this.SS34Form_KeyDown);

        this.ResumeLayout(false);
    }
}
