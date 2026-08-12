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
    private Label lblDate;
    private Label lblTilde;
    private Label lblCvcod;
    private Label lblAmtCap2;
    private Label lblAmtCap3;

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
        this.lblAmt2 = new Label();
        this.lblAmt2.AutoSize = true;
        this.lblAmt3 = new Label();
        this.lblAmt3.AutoSize = true;
        this.btnNew = new Button();
        this.btnNew.Text = "신규(F1)";
        this.btnDel = new Button();
        this.btnDel.Text = "삭제(F4)";
        this.btnSearch = new Button();
        this.btnSearch.Text = "조회";
        this.btnExcel = new Button();
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint = new Button();
        this.btnPrint.Text = "인쇄";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";
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
        this.lblDate = new Label();
        this.lblDate.Text = "기간";
        this.lblDate.Left = 5;
        this.lblDate.Top = 12;
        this.lblDate.AutoSize = true;
        this.dtpDate1.Left = 50; this.dtpDate1.Top = 8; this.dtpDate1.Width = 110; this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.lblTilde = new Label();
        this.lblTilde.Text = "~";
        this.lblTilde.Left = 165;
        this.lblTilde.Top = 12;
        this.lblTilde.AutoSize = true;
        this.dtpDate2.Left = 180; this.dtpDate2.Top = 8; this.dtpDate2.Width = 110; this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.lblCvcod = new Label();
        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 310;
        this.lblCvcod.Top = 12;
        this.lblCvcod.AutoSize = true;
        this.edtCvcd1.Left = 380; this.edtCvcd1.Top = 8; this.edtCvcd1.Width = 80;

        this.lblAmtCap2 = new Label();
        this.lblAmtCap2.Text = "입고금액:";
        this.lblAmtCap2.Left = 480;
        this.lblAmtCap2.Top = 12;
        this.lblAmtCap2.AutoSize = true;
        this.lblAmt2.Left = 550; this.lblAmt2.Top = 12;
        this.lblAmtCap3 = new Label();
        this.lblAmtCap3.Text = "부가세:";
        this.lblAmtCap3.Left = 650;
        this.lblAmtCap3.Top = 12;
        this.lblAmtCap3.AutoSize = true;
        this.lblAmt3.Left = 710; this.lblAmt3.Top = 12;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblDate, this.dtpDate1, this.lblTilde, this.dtpDate2, this.lblCvcod, this.edtCvcd1,
            this.lblAmtCap2, this.lblAmt2, this.lblAmtCap3, this.lblAmt3
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
