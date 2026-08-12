using pStock.Common;

namespace pStock.Forms.JA;

partial class JA03Form
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
    private Button btnNew;
    private Button btnSearch;
    private Button btnClose;
    private Button btnMonthDown;
    private Button btnMonthUp;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblMonth;
    private Label lblCvcod;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtMonth = new TextBox();
        this.edtCvcod = new TextBox();
        this.dspName = new TextBox();
        this.btnNew = new Button();
        this.btnSearch = new Button();
        this.btnClose = new Button();
        this.btnMonthDown = new Button();
        this.btnMonthUp = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.lblMonth = new Label();
        this.lblCvcod = new Label();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // dspName
        //
        this.dspName.ReadOnly = true;
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
        this.edtMonth.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) ReloadList(); };
        this.btnMonthDown.Left = 225; this.btnMonthDown.Top = 8; this.btnMonthDown.Width = 30;
        this.btnMonthUp.Left = 258; this.btnMonthUp.Top = 8; this.btnMonthUp.Width = 30;
        this.btnMonthDown.Click += (_, _) => { ShiftMonth(-1); ReloadList(); };
        this.btnMonthUp.Click += (_, _) => { ShiftMonth(1); ReloadList(); };

        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 320; this.lblCvcod.Top = 12; this.lblCvcod.AutoSize = true;
        this.edtCvcod.Left = 400; this.edtCvcod.Top = 8; this.edtCvcod.Width = 80;
        this.edtCvcod.KeyDown += EdtCvcod_KeyDown;
        this.dspName.Left = 490; this.dspName.Top = 8; this.dspName.Width = 200;

        this.panelEdit.Controls.AddRange(new Control[] { this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp, this.lblCvcod, this.edtCvcod, this.dspName });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("DAY", "일자");
        this.grid.Columns.Add("IAMT", "입고금액");
        this.grid.Columns.Add("J1", "입고VAT1");
        this.grid.Columns.Add("J2", "입고VAT2");
        this.grid.Columns.Add("J3", "입고VAT3");
        this.grid.Columns.Add("BAMT", "보관금액");
        this.grid.Columns.Add("BVAT", "보관VAT합");
        this.grid.Columns.Add("OAMT", "출고금액");
        this.grid.Columns.Add("OJ1", "출고VAT1");
        this.grid.Columns.Add("OJ2", "출고VAT2");
        this.grid.Columns.Add("OJ3", "출고VAT3");
        this.grid.Columns.Add("SUM", "합계");
        this.grid.Columns.Add("SUM10", "합계/10");
        this.grid.Columns.Add("SUGM", "수금액");
        this.grid.Columns.Add("MISU", "미수금잔액");
        //
        // 이벤트 배선
        //
        this.btnNew.Click += (_, _) => { this.edtCvcod.Clear(); this.dspName.Clear(); this.grid.Rows.Clear(); this.edtCvcod.Focus(); };
        this.btnSearch.Click += (_, _) => ReloadList();
        this.btnClose.Click += (_, _) => Close();
        //
        // JA03Form
        //
        this.Text = "일자별 집계작업";
        this.Width = 1300;
        this.Height = 650;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += (_, _) => { this.edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); ReloadList(); };
        this.KeyDown += new KeyEventHandler(this.JA03Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
