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
        this.dspName = new TextBox();
        this.dspName.ReadOnly = true;
        this.lblAmt = new Label();
        this.lblAmt.AutoSize = true;

        this.edtCd2 = new TextBox();
        this.dspNm2 = new TextBox();
        this.dspNm2.ReadOnly = true;
        this.lblQty1 = new Label();
        this.lblQty1.AutoSize = true;
        this.lblQty2 = new Label();
        this.lblQty2.AutoSize = true;

        this.grid1 = new FastDataGridView();
        this.grid2 = new FastDataGridView();

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

        this.dateLbl1 = new Label();
        this.dateLbl1.Text = "기간";
        this.dateLbl1.Left = 340;
        this.dateLbl1.Top = 12;
        this.dateLbl1.AutoSize = true;
        this.dtpDate1.Left = 380; this.dtpDate1.Top = 8; this.dtpDate1.Width = 110; this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.dateLblT = new Label();
        this.dateLblT.Text = "~";
        this.dateLblT.Left = 495;
        this.dateLblT.Top = 12;
        this.dateLblT.AutoSize = true;
        this.dtpDate2.Left = 510; this.dtpDate2.Top = 8; this.dtpDate2.Width = 110; this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.panelTop.Controls.AddRange(new Control[] { this.dateLbl1, this.dtpDate1, this.dateLblT, this.dtpDate2 });

        // Tab1
        this.lbl1 = new Label();
        this.lbl1.Text = "거래처코드";
        this.lbl1.Left = 10;
        this.lbl1.Top = 12;
        this.lbl1.AutoSize = true;
        this.edtCvcod.Left = 90; this.edtCvcod.Top = 8; this.edtCvcod.Width = 80;
        this.edtCvcod.KeyDown += this.EdtCvcod_KeyDown;
        this.dspName.Left = 180; this.dspName.Top = 8; this.dspName.Width = 200;
        this.lblAmtCap = new Label();
        this.lblAmtCap.Text = "미수잔액:";
        this.lblAmtCap.Left = 400;
        this.lblAmtCap.Top = 12;
        this.lblAmtCap.AutoSize = true;
        this.lblAmt.Left = 470; this.lblAmt.Top = 12;
        this.panel1 = new Panel();
        this.panel1.Dock = DockStyle.Top;
        this.panel1.Height = 35;
        this.panel1.Controls.AddRange(new Control[] { this.lbl1, this.edtCvcod, this.dspName, this.lblAmtCap, this.lblAmt });

        this.grid1.Dock = DockStyle.Fill;
        this.grid1.ReadOnly = true;
        this.grid1.AllowUserToAddRows = false;

        this.tab1.Controls.Add(this.grid1);
        this.tab1.Controls.Add(this.panel1);

        // Tab2
        this.lbl2 = new Label();
        this.lbl2.Text = "거래처코드";
        this.lbl2.Left = 10;
        this.lbl2.Top = 12;
        this.lbl2.AutoSize = true;
        this.edtCd2.Left = 90; this.edtCd2.Top = 8; this.edtCd2.Width = 80;
        this.edtCd2.KeyDown += this.EdtCd2_KeyDown;
        this.dspNm2.Left = 180; this.dspNm2.Top = 8; this.dspNm2.Width = 200;
        this.lblQty1Cap = new Label();
        this.lblQty1Cap.Text = "입고수량:";
        this.lblQty1Cap.Left = 400;
        this.lblQty1Cap.Top = 12;
        this.lblQty1Cap.AutoSize = true;
        this.lblQty1.Left = 470; this.lblQty1.Top = 12;
        this.lblQty2Cap = new Label();
        this.lblQty2Cap.Text = "출고수량:";
        this.lblQty2Cap.Left = 560;
        this.lblQty2Cap.Top = 12;
        this.lblQty2Cap.AutoSize = true;
        this.lblQty2.Left = 630; this.lblQty2.Top = 12;
        this.panel2 = new Panel();
        this.panel2.Dock = DockStyle.Top;
        this.panel2.Height = 35;
        this.panel2.Controls.AddRange(new Control[] { this.lbl2, this.edtCd2, this.dspNm2, this.lblQty1Cap, this.lblQty1, this.lblQty2Cap, this.lblQty2 });

        this.grid2.Dock = DockStyle.Fill;
        this.grid2.ReadOnly = true;
        this.grid2.AllowUserToAddRows = false;

        this.tab2.Controls.Add(this.grid2);
        this.tab2.Controls.Add(this.panel2);

        this.tabs.Dock = DockStyle.Fill;
        this.tabs.TabPages.AddRange(new TabPage[] { this.tab1, this.tab2 });

        this.Controls.Add(this.tabs);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Load += new EventHandler(this.SS35Form_Load);
        this.KeyDown += new KeyEventHandler(this.SS35Form_KeyDown);

        this.ResumeLayout(false);
    }
}
