using pStock.Common;

namespace pStock.Forms.SS;

partial class SS020Form
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

    private FastDataGridView gridList;
    private FastDataGridView gridDetail;
    private TextBox eDate1;
    private TextBox eDate2;
    private ComboBox cSrcd;
    private TextBox eSrwd;
    private Label lblCnt;
    private Label lblAmt;

    private Button btnNew;
    private Button btnDel;
    private Button btnCut;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;

    private Panel panelTop;
    private Panel panelEdit;
    private SplitContainer split;

    private void InitializeComponent()
    {
        this.gridList = new FastDataGridView();
        this.gridDetail = new FastDataGridView();
        this.eDate1 = new TextBox();
        this.eDate2 = new TextBox();
        this.cSrcd = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        this.eSrwd = new TextBox();
        this.lblCnt = new Label { AutoSize = true };
        this.lblAmt = new Label { AutoSize = true };
        this.btnNew = new Button { Text = "신규(F1)" };
        this.btnDel = new Button { Text = "삭제(F4)" };
        this.btnCut = new Button { Text = "상세삭제" };
        this.btnSearch = new Button { Text = "조회(F5)" };
        this.btnExcel = new Button { Text = "엑셀저장" };
        this.btnPrint = new Button { Text = "인쇄" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.split = new SplitContainer();
        ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
        this.split.Panel1.SuspendLayout();
        this.split.Panel2.SuspendLayout();
        this.split.SuspendLayout();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // SS020Form (원본 생성자 프롤로그)
        //
        this.Text = "출고 관리";
        this.Width = 1300;
        this.Height = 700;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        this.cSrcd.Items.AddRange(new object[] { "출고번호", "거래처코드", "거래처명", "발주번호", "착지처코드", "착지처명", "주소", "비고" });

        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnDel.Left = 100; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnCut.Left = 195; this.btnCut.Top = 8; this.btnCut.Width = 90;
        this.btnSearch.Left = 290; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnExcel.Left = 385; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 480; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 575; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnDel, this.btnCut, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.gridList, "출고관리");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.gridList, "출고관리");

        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        var lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        this.eDate1.Left = 50; this.eDate1.Top = 8; this.eDate1.Width = 90;
        var lblTilde = new Label { Text = "~", Left = 145, Top = 12, AutoSize = true };
        this.eDate2.Left = 160; this.eDate2.Top = 8; this.eDate2.Width = 90;
        var lblSearch = new Label { Text = "검색조건", Left = 270, Top = 12, AutoSize = true };
        this.cSrcd.Left = 330; this.cSrcd.Top = 8; this.cSrcd.Width = 100;
        this.eSrwd.Left = 440; this.eSrwd.Top = 8; this.eSrwd.Width = 180;
        var lblCntCap = new Label { Text = "선택건수:", Left = 650, Top = 12, AutoSize = true };
        this.lblCnt.Left = 720; this.lblCnt.Top = 12;
        var lblAmtCap = new Label { Text = "선택금액:", Left = 800, Top = 12, AutoSize = true };
        this.lblAmt.Left = 870; this.lblAmt.Top = 12;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            lblDate, this.eDate1, lblTilde, this.eDate2, lblSearch, this.cSrcd, this.eSrwd, lblCntCap, this.lblCnt, lblAmtCap, this.lblAmt
        });

        this.split.Dock = DockStyle.Fill;
        this.split.Orientation = Orientation.Horizontal;
        this.split.SplitterDistance = 300;
        this.gridList.Dock = DockStyle.Fill;
        this.gridList.ReadOnly = true;
        this.gridList.AllowUserToAddRows = false;
        this.gridList.SelectionChanged += (_, _) => LoadDetail();
        this.gridList.CellDoubleClick += (_, _) => OpenEditForSelected();

        this.gridDetail.Dock = DockStyle.Fill;
        this.gridDetail.ReadOnly = true;
        this.gridDetail.AllowUserToAddRows = false;

        this.split.Panel1.Controls.Add(this.gridList);
        this.split.Panel2.Controls.Add(this.gridDetail);

        this.Controls.Add(this.split);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) =>
        {
            using var dlg = new SS020F01Form();
            dlg.ShowDialog(this);
            if (dlg.Saved) Search();
        };
        this.btnDel.Click += (_, _) => DeleteMaster();
        this.btnCut.Click += (_, _) => DeleteDetail();
        this.btnSearch.Click += (_, _) => Search();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) =>
        {
            this.eDate2.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.eDate1.Text = this.eDate2.Text;
            this.cSrcd.SelectedIndex = 2;
            Search();
        };
        this.KeyDown += new KeyEventHandler(this.SS020Form_KeyDown);

        this.split.Panel1.ResumeLayout(false);
        this.split.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
        this.split.ResumeLayout(false);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
