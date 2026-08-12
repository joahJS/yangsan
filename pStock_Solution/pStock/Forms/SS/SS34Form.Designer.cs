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
        this.dspName = new TextBox { ReadOnly = true };
        this.lblAmt = new Label { AutoSize = true };

        this.btnNew = new Button { Text = "초기화(F1)" };
        this.btnSearch = new Button { Text = "조회(F5)" };
        this.btnExcel = new Button { Text = "엑셀저장" };
        this.btnPrint = new Button { Text = "인쇄" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this.btnMonthDown = new Button { Text = "◀" };
        this.btnMonthUp = new Button { Text = "▶" };

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
        this.panelTop = new Panel { Dock = DockStyle.Top, Height = 40 };
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "미수금조회");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "미수금조회");

        this.editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        this.lblMonth = new Label { Text = "조회월(YYYY-MM)", Left = 10, Top = 12, AutoSize = true };
        this.edtMonth.Left = 140; this.edtMonth.Top = 8; this.edtMonth.Width = 80;
        this.edtMonth.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) this.Search(); };
        this.btnMonthDown.Left = 225; this.btnMonthDown.Top = 8; this.btnMonthDown.Width = 30;
        this.btnMonthUp.Left = 258; this.btnMonthUp.Top = 8; this.btnMonthUp.Width = 30;
        this.btnMonthDown.Click += (_, _) => { this.ShiftMonth(-1); this.Search(); };
        this.btnMonthUp.Click += (_, _) => { this.ShiftMonth(1); this.Search(); };

        this.lblCvcod = new Label { Text = "거래처코드", Left = 320, Top = 12, AutoSize = true };
        this.edtCvcod.Left = 400; this.edtCvcod.Top = 8; this.edtCvcod.Width = 80;
        this.edtCvcod.KeyDown += this.EdtCvcod_KeyDown;
        this.dspName.Left = 490; this.dspName.Top = 8; this.dspName.Width = 200;

        this.lblAmtCap = new Label { Text = "미수금합계:", Left = 720, Top = 12, AutoSize = true };
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

        this.btnNew.Click += (_, _) => { this.edtCvcod.Clear(); this.dspName.Clear(); this.edtCvcod.Focus(); };
        this.btnSearch.Click += (_, _) => this.Search();
        this.btnClose.Click += (_, _) => this.Close();

        this.Load += (_, _) => { this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); this.Search(); };
        this.KeyDown += new KeyEventHandler(this.SS34Form_KeyDown);

        this.ResumeLayout(false);
    }
}
