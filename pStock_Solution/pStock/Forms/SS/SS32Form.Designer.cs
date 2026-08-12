using pStock.Common;

namespace pStock.Forms.SS;

partial class SS32Form
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
    private FastDataGridView gridSum;
    private DateTimePicker dtpDate1;
    private DateTimePicker dtpDate2;
    private Label lblAmt1;
    private Button btnNew;
    private Button btnDel;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblDate;
    private Label lblTilde;
    private Label lblAmtCap;
    private SplitContainer splitContainer;

    private void InitializeComponent()
    {
        this.gridList = new FastDataGridView();
        this.gridSum = new FastDataGridView();
        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.lblAmt1 = new Label();
        this.btnNew = new Button();
        this.btnDel = new Button();
        this.btnSearch = new Button();
        this.btnExcel = new Button();
        this.btnPrint = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.lblDate = new Label();
        this.lblTilde = new Label();
        this.lblAmtCap = new Label();
        this.splitContainer = new SplitContainer();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
        this.splitContainer.Panel1.SuspendLayout();
        this.splitContainer.Panel2.SuspendLayout();
        this.splitContainer.SuspendLayout();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // lblAmt1 (원본 필드 초기값)
        //
        this.lblAmt1.AutoSize = true;
        //
        // 상단 버튼 6개
        //
        this.btnNew.Text = "신규(F1)";
        this.btnDel.Text = "삭제(F4)";
        this.btnSearch.Text = "조회";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";
        //
        // panelTop (원본 BuildLayout()의 top 패널: 버튼 6개를 95px 간격으로 가로 배치)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnDel, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnDel.Left = 100; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnSearch.Left = 195; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnExcel.Left = 290; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 385; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 480; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.gridList, "수금관리");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.gridList, "수금관리");
        //
        // panelEdit (원본 BuildLayout()의 editPanel)
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
        this.lblAmtCap.Text = "수금합계:";
        this.lblAmtCap.Left = 320;
        this.lblAmtCap.Top = 12;
        this.lblAmtCap.AutoSize = true;
        this.lblAmt1.Left = 390;
        this.lblAmt1.Top = 12;
        this.panelEdit.Controls.AddRange(new Control[] { this.lblDate, this.dtpDate1, this.lblTilde, this.dtpDate2, this.lblAmtCap, this.lblAmt1 });
        //
        // splitContainer / gridList / gridSum
        //
        this.splitContainer.Dock = DockStyle.Fill;
        this.splitContainer.SplitterDistance = 750;
        this.gridList.Dock = DockStyle.Fill;
        this.gridList.ReadOnly = true;
        this.gridList.AllowUserToAddRows = false;
        this.gridList.CellDoubleClick += (_, _) => OpenEntry(isNew: false);

        this.gridSum.Dock = DockStyle.Fill;
        this.gridSum.ReadOnly = true;
        this.gridSum.AllowUserToAddRows = false;
        this.gridSum.Columns.Add("ARDAT", "일자");
        this.gridSum.Columns.Add("CNT", "건수");
        this.gridSum.Columns.Add("SAMT", "금액");

        this.splitContainer.Panel1.Controls.Add(this.gridList);
        this.splitContainer.Panel2.Controls.Add(this.gridSum);
        //
        // SS32Form
        //
        this.Text = "수금 관리";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;
        this.Controls.Add(this.splitContainer);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => OpenEntry(isNew: true);
        this.btnDel.Click += (_, _) => Delete();
        this.btnSearch.Click += (_, _) => Search();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => { this.dtpDate1.Value = DateTime.Now; this.dtpDate2.Value = DateTime.Now; Search(); };
        this.KeyDown += new KeyEventHandler(this.SS32Form_KeyDown);

        this.splitContainer.Panel1.ResumeLayout(false);
        this.splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
        this.splitContainer.ResumeLayout(false);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
