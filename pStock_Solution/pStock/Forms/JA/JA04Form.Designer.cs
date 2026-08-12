using pStock.Common;

namespace pStock.Forms.JA;

partial class JA04Form
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
    private Button btnNew;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Button btnMonthDown;
    private Button btnMonthUp;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblMonth;
    private Label lblSearch;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtMonth = new TextBox();
        this.cSrcd = new ComboBox();
        this.eSrwd = new TextBox();
        this.btnNew = new Button();
        this.btnSearch = new Button();
        this.btnExcel = new Button();
        this.btnPrint = new Button();
        this.btnClose = new Button();
        this.btnMonthDown = new Button();
        this.btnMonthUp = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.lblMonth = new Label();
        this.lblSearch = new Label();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // cSrcd
        //
        this.cSrcd.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cSrcd.Items.AddRange(new object[] { "품번", "품명", "거래처코드", "거래처명" });
        //
        // 버튼 텍스트
        //
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";
        this.btnMonthDown.Text = "◀";
        this.btnMonthUp.Text = "▶";
        //
        // panelTop
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "재고관리-품목");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "재고관리-품목");
        //
        // panelEdit
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.lblMonth.Text = "조회월(YYYY-MM)";
        this.lblMonth.Left = 10; this.lblMonth.Top = 12; this.lblMonth.AutoSize = true;
        this.edtMonth.Left = 140; this.edtMonth.Top = 8; this.edtMonth.Width = 80;
        this.edtMonth.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) ReloadList(); };
        this.btnMonthDown.Left = 225; this.btnMonthDown.Top = 8; this.btnMonthDown.Width = 30;
        this.btnMonthUp.Left = 258; this.btnMonthUp.Top = 8; this.btnMonthUp.Width = 30;
        this.btnMonthDown.Click += (_, _) => { ShiftMonth(-1); ReloadList(); };
        this.btnMonthUp.Click += (_, _) => { ShiftMonth(1); ReloadList(); };

        this.lblSearch.Text = "검색조건";
        this.lblSearch.Left = 320; this.lblSearch.Top = 12; this.lblSearch.AutoSize = true;
        this.cSrcd.Left = 390; this.cSrcd.Top = 8; this.cSrcd.Width = 100;
        this.eSrwd.Left = 500; this.eSrwd.Top = 8; this.eSrwd.Width = 200;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp, this.lblSearch, this.cSrcd, this.eSrwd
        });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("ITDSC", "품명");
        this.grid.Columns.Add("ITNBR", "품번");
        this.grid.Columns.Add("BQTY", "기초");
        this.grid.Columns.Add("IQT1", "입고(상순)");
        this.grid.Columns.Add("IQT2", "입고(하순)");
        this.grid.Columns.Add("OQT1", "출고(상순)");
        this.grid.Columns.Add("OQT2", "출고(하순)");
        this.grid.Columns.Add("JQTY1", "재고(상순)");
        this.grid.Columns.Add("JQTY2", "재고(하순)");
        this.grid.Columns.Add("IAMT", "입고금액");
        this.grid.Columns.Add("OAMT", "보관금액");
        this.grid.Columns.Add("JAM1", "출고금액1");
        this.grid.Columns.Add("JAM2", "출고금액2");
        this.grid.Columns.Add("TAMT", "합계금액");
        //
        // 이벤트 배선
        //
        this.btnNew.Click += (_, _) => { this.cSrcd.SelectedIndex = 1; this.eSrwd.Clear(); this.grid.Rows.Clear(); this.eSrwd.Focus(); };
        this.btnSearch.Click += (_, _) => ReloadList();
        this.btnClose.Click += (_, _) => Close();
        //
        // JA04Form
        //
        this.Text = "재고관리-품목";
        this.Width = 1200;
        this.Height = 650;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += (_, _) =>
        {
            this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM");
            this.cSrcd.SelectedIndex = 1;
        };
        this.KeyDown += new KeyEventHandler(this.JA04Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
