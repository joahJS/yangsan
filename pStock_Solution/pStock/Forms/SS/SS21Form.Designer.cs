using pStock.Common;

namespace pStock.Forms.SS;

partial class SS21Form
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
    private TextBox edtCvcd1;
    private Label lblAmt2;
    private Label lblAmt3;

    private Button btnNew;
    private Button btnDel;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;

    private Panel panelTop;
    private Panel panelEdit;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.edtCvcd1 = new TextBox();
        this.lblAmt2 = new Label { AutoSize = true };
        this.lblAmt3 = new Label { AutoSize = true };
        this.btnNew = new Button { Text = "신규(F1)" };
        this.btnDel = new Button { Text = "삭제(F4)" };
        this.btnSearch = new Button { Text = "조회" };
        this.btnExcel = new Button { Text = "엑셀저장" };
        this.btnPrint = new Button { Text = "인쇄" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // SS21Form (원본 생성자 프롤로그)
        //
        this.Text = "입고 관리";
        this.Width = 1200;
        this.Height = 650;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnDel.Left = 100; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnSearch.Left = 195; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnExcel.Left = 290; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 385; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 480; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnDel, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "입고관리");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "입고관리");

        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        var lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        this.dtpDate1.Left = 50; this.dtpDate1.Top = 8; this.dtpDate1.Width = 110; this.dtpDate1.Format = DateTimePickerFormat.Short;
        var lblTilde = new Label { Text = "~", Left = 165, Top = 12, AutoSize = true };
        this.dtpDate2.Left = 180; this.dtpDate2.Top = 8; this.dtpDate2.Width = 110; this.dtpDate2.Format = DateTimePickerFormat.Short;
        var lblCvcod = new Label { Text = "거래처코드", Left = 310, Top = 12, AutoSize = true };
        this.edtCvcd1.Left = 380; this.edtCvcd1.Top = 8; this.edtCvcd1.Width = 80;

        var lblAmtCap2 = new Label { Text = "입고금액:", Left = 480, Top = 12, AutoSize = true };
        this.lblAmt2.Left = 550; this.lblAmt2.Top = 12;
        var lblAmtCap3 = new Label { Text = "부가세:", Left = 650, Top = 12, AutoSize = true };
        this.lblAmt3.Left = 710; this.lblAmt3.Top = 12;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            lblDate, this.dtpDate1, lblTilde, this.dtpDate2, lblCvcod, this.edtCvcd1,
            lblAmtCap2, this.lblAmt2, lblAmtCap3, this.lblAmt3
        });

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += (_, _) => OpenEntry(isNew: false);

        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => OpenEntry(isNew: true);
        this.btnDel.Click += (_, _) => Delete();
        this.btnSearch.Click += (_, _) => Search();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => { this.dtpDate1.Value = DateTime.Now; this.dtpDate2.Value = DateTime.Now; Search(); };
        this.KeyDown += new KeyEventHandler(this.SS21Form_KeyDown);

        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
