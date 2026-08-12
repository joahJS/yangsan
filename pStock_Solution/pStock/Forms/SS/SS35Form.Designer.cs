using pStock.Common;

namespace pStock.Forms.SS;

partial class SS35Form
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

    private TabControl tabs;
    private TabPage tab1;
    private TabPage tab2;

    private DateTimePicker dtpDate1;
    private DateTimePicker dtpDate2;
    private TextBox edtCvcod;
    private TextBox dspName;
    private Label lblAmt;

    private TextBox edtCd2;
    private TextBox dspNm2;
    private Label lblQty1;
    private Label lblQty2;

    private FastDataGridView grid1;
    private FastDataGridView grid2;

    private Button btnNew;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;

    private Panel panelTop;
    private Label dateLbl1;
    private Label dateLblT;
    private Label lbl1;
    private Label lblAmtCap;
    private Panel panel1;
    private Label lbl2;
    private Label lblQty1Cap;
    private Label lblQty2Cap;
    private Panel panel2;

    private void InitializeComponent()
    {
        this.tabs = new TabControl();
        this.tab1 = new TabPage("거래처별 원장");
        this.tab2 = new TabPage("품목별 입출고");

        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.edtCvcod = new TextBox();
        this.dspName = new TextBox { ReadOnly = true };
        this.lblAmt = new Label { AutoSize = true };

        this.edtCd2 = new TextBox();
        this.dspNm2 = new TextBox { ReadOnly = true };
        this.lblQty1 = new Label { AutoSize = true };
        this.lblQty2 = new Label { AutoSize = true };

        this.grid1 = new FastDataGridView();
        this.grid2 = new FastDataGridView();

        this.btnNew = new Button { Text = "초기화(F1)" };
        this.btnSearch = new Button { Text = "조회(F5)" };
        this.btnExcel = new Button { Text = "엑셀저장" };
        this.btnPrint = new Button { Text = "인쇄" };
        this.btnClose = new Button { Text = "닫기(Esc)" };

        this.SuspendLayout();
        //
        // SS35Form
        //
        this.Text = "거래처원장 조회";
        this.Width = 1300;
        this.Height = 700;
        this.KeyPreview = true;

        //
        // 원본 BuildLayout() 그대로 이동
        //
        this.panelTop = new Panel { Dock = DockStyle.Top, Height = 40 };
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(
            this.tabs.SelectedTab == this.tab1 ? this.grid1 : this.grid2,
            this.tabs.SelectedTab == this.tab1 ? "거래처별원장" : "품목별입출고");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(
            this.tabs.SelectedTab == this.tab1 ? this.grid1 : this.grid2,
            this.tabs.SelectedTab == this.tab1 ? "거래처별원장" : "품목별입출고");

        this.dateLbl1 = new Label { Text = "기간", Left = 340, Top = 12, AutoSize = true };
        this.dtpDate1.Left = 380; this.dtpDate1.Top = 8; this.dtpDate1.Width = 110; this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.dateLblT = new Label { Text = "~", Left = 495, Top = 12, AutoSize = true };
        this.dtpDate2.Left = 510; this.dtpDate2.Top = 8; this.dtpDate2.Width = 110; this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.panelTop.Controls.AddRange(new Control[] { this.dateLbl1, this.dtpDate1, this.dateLblT, this.dtpDate2 });

        // Tab1
        this.lbl1 = new Label { Text = "거래처코드", Left = 10, Top = 12, AutoSize = true };
        this.edtCvcod.Left = 90; this.edtCvcod.Top = 8; this.edtCvcod.Width = 80;
        this.edtCvcod.KeyDown += this.EdtCvcod_KeyDown;
        this.dspName.Left = 180; this.dspName.Top = 8; this.dspName.Width = 200;
        this.lblAmtCap = new Label { Text = "미수잔액:", Left = 400, Top = 12, AutoSize = true };
        this.lblAmt.Left = 470; this.lblAmt.Top = 12;
        this.panel1 = new Panel { Dock = DockStyle.Top, Height = 35 };
        this.panel1.Controls.AddRange(new Control[] { this.lbl1, this.edtCvcod, this.dspName, this.lblAmtCap, this.lblAmt });

        this.grid1.Dock = DockStyle.Fill;
        this.grid1.ReadOnly = true;
        this.grid1.AllowUserToAddRows = false;

        this.tab1.Controls.Add(this.grid1);
        this.tab1.Controls.Add(this.panel1);

        // Tab2
        this.lbl2 = new Label { Text = "거래처코드", Left = 10, Top = 12, AutoSize = true };
        this.edtCd2.Left = 90; this.edtCd2.Top = 8; this.edtCd2.Width = 80;
        this.edtCd2.KeyDown += this.EdtCd2_KeyDown;
        this.dspNm2.Left = 180; this.dspNm2.Top = 8; this.dspNm2.Width = 200;
        this.lblQty1Cap = new Label { Text = "입고수량:", Left = 400, Top = 12, AutoSize = true };
        this.lblQty1.Left = 470; this.lblQty1.Top = 12;
        this.lblQty2Cap = new Label { Text = "출고수량:", Left = 560, Top = 12, AutoSize = true };
        this.lblQty2.Left = 630; this.lblQty2.Top = 12;
        this.panel2 = new Panel { Dock = DockStyle.Top, Height = 35 };
        this.panel2.Controls.AddRange(new Control[] { this.lbl2, this.edtCd2, this.dspNm2, this.lblQty1Cap, this.lblQty1, this.lblQty2Cap, this.lblQty2 });

        this.grid2.Dock = DockStyle.Fill;
        this.grid2.ReadOnly = true;
        this.grid2.AllowUserToAddRows = false;

        this.tab2.Controls.Add(this.grid2);
        this.tab2.Controls.Add(this.panel2);

        this.tabs.Dock = DockStyle.Fill;
        this.tabs.TabPages.AddRange(new[] { this.tab1, this.tab2 });

        this.Controls.Add(this.tabs);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) =>
        {
            this.edtCvcod.Clear(); this.dspName.Clear(); this.edtCd2.Clear(); this.dspNm2.Clear();
            this.grid1.DataSource = null; this.grid2.DataSource = null;
            this.lblAmt.Text = "0"; this.lblQty1.Text = "0"; this.lblQty2.Text = "0";
        };
        this.btnSearch.Click += (_, _) => { if (this.tabs.SelectedTab == this.tab1) this.LoadCustLedger(); else this.LoadItemIO(); };
        this.btnClose.Click += (_, _) => this.Close();

        this.Load += (_, _) =>
        {
            var now = DateTime.Now;
            this.dtpDate1.Value = new DateTime(now.Year, now.Month, 1);
            this.dtpDate2.Value = now;
        };
        this.KeyDown += new KeyEventHandler(this.SS35Form_KeyDown);

        this.ResumeLayout(false);
    }
}
