using pStock.Common;

namespace pStock.Forms.JA;

partial class JA01Form
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
    private TabPage tab3;
    private FastDataGridView grid;
    private TextBox edtMonth;
    private TextBox edtNo;
    private TextBox edtCvnam;
    private ComboBox cboHouse;
    private Button btnNew;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Button btnMonthDown;
    private Button btnMonthUp;
    private Panel panelTop;
    private Label lblMonth;
    private Label lblNo;
    private Label lblCvnam;
    private Label lblHouse;

    private void InitializeComponent()
    {
        this.tabs = new TabControl();
        this.tab1 = new TabPage("품목별");
        this.tab2 = new TabPage("거래처별");
        this.tab3 = new TabPage("저장위치별");
        this.grid = new FastDataGridView();
        this.edtMonth = new TextBox();
        this.edtNo = new TextBox();
        this.edtCvnam = new TextBox();
        this.cboHouse = new ComboBox();
        this.btnNew = new Button();
        this.btnSearch = new Button();
        this.btnExcel = new Button();
        this.btnPrint = new Button();
        this.btnClose = new Button();
        this.btnMonthDown = new Button();
        this.btnMonthUp = new Button();
        this.panelTop = new Panel();
        this.lblMonth = new Label();
        this.lblNo = new Label();
        this.lblCvnam = new Label();
        this.lblHouse = new Label();
        this.panelTop.SuspendLayout();
        this.tab1.SuspendLayout();
        this.tab2.SuspendLayout();
        this.tab3.SuspendLayout();
        this.SuspendLayout();
        //
        // cboHouse
        //
        this.cboHouse.DropDownStyle = ComboBoxStyle.DropDownList;
        //
        // 버튼 텍스트 (원본 필드 초기화값 그대로)
        //
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";
        this.btnMonthDown.Text = "◀";
        this.btnMonthUp.Text = "▶";
        //
        // panelTop (원본 BuildLayout()의 top 패널)
        //
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

        this.lblMonth.Text = "조회월(YYYY-MM)";
        this.lblMonth.Left = 550;
        this.lblMonth.Top = 14;
        this.lblMonth.AutoSize = true;
        this.edtMonth.Left = 680; this.edtMonth.Top = 10; this.edtMonth.Width = 80;
        this.edtMonth.KeyDown += new KeyEventHandler(this.EdtMonth_KeyDown);
        this.btnMonthDown.Left = 765; this.btnMonthDown.Top = 8; this.btnMonthDown.Width = 30;
        this.btnMonthUp.Left = 798; this.btnMonthUp.Top = 8; this.btnMonthUp.Width = 30;
        this.btnMonthDown.Click += new EventHandler(this.BtnMonthDown_Click);
        this.btnMonthUp.Click += new EventHandler(this.BtnMonthUp_Click);
        this.panelTop.Controls.AddRange(new Control[] { this.lblMonth, this.edtMonth, this.btnMonthDown, this.btnMonthUp });
        //
        // tab1 (품목별)
        //
        this.lblNo.Text = "품명검색";
        this.lblNo.Left = 10; this.lblNo.Top = 12; this.lblNo.AutoSize = true;
        this.edtNo.Left = 80; this.edtNo.Top = 8; this.edtNo.Width = 200;
        this.edtNo.KeyUp += new KeyEventHandler(this.EdtNo_KeyUp);
        this.tab1.Controls.AddRange(new Control[] { this.lblNo, this.edtNo });
        //
        // tab2 (거래처별)
        //
        this.lblCvnam.Text = "거래처검색";
        this.lblCvnam.Left = 10; this.lblCvnam.Top = 12; this.lblCvnam.AutoSize = true;
        this.edtCvnam.Left = 90; this.edtCvnam.Top = 8; this.edtCvnam.Width = 200;
        this.edtCvnam.KeyUp += new KeyEventHandler(this.EdtCvnam_KeyUp);
        this.tab2.Controls.AddRange(new Control[] { this.lblCvnam, this.edtCvnam });
        //
        // tab3 (저장위치별)
        //
        this.lblHouse.Text = "저장위치";
        this.lblHouse.Left = 10; this.lblHouse.Top = 12; this.lblHouse.AutoSize = true;
        this.cboHouse.Left = 90; this.cboHouse.Top = 8; this.cboHouse.Width = 150;
        this.cboHouse.SelectedIndexChanged += new EventHandler(this.CboHouse_SelectedIndexChanged);
        this.tab3.Controls.AddRange(new Control[] { this.lblHouse, this.cboHouse });
        //
        // tabs
        //
        this.tabs.Dock = DockStyle.Top;
        this.tabs.Height = 70;
        this.tabs.TabPages.AddRange(new TabPage[] { this.tab1, this.tab2, this.tab3 });
        this.tabs.SelectedIndexChanged += new EventHandler(this.Tabs_SelectedIndexChanged);
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        //
        // 이벤트 배선 (원본 BuildLayout() 끝부분)
        //
        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnSearch.Click += new EventHandler(this.BtnSearch_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // JA01Form
        //
        this.Text = "재고관리";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.tabs);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.JA01Form_Load);
        this.KeyDown += new KeyEventHandler(this.JA01Form_KeyDown);
        this.tab1.ResumeLayout(false);
        this.tab1.PerformLayout();
        this.tab2.ResumeLayout(false);
        this.tab2.PerformLayout();
        this.tab3.ResumeLayout(false);
        this.tab3.PerformLayout();
        this.panelTop.ResumeLayout(false);
        this.panelTop.PerformLayout();
        this.ResumeLayout(false);
    }
}
