using pStock.Common;

namespace pStock.Forms.SS;

partial class SS36Form
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

    private DateTimePicker dtpDate1;
    private DateTimePicker dtpDate2;
    private TextBox edtCode;
    private TextBox dspName;
    private TextBox dspDanwi;
    private Label lblCvnam;
    private FastDataGridView grid;

    private Button btnNew;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;

    private Panel panelTop;
    private Panel editPanel;
    private Label lblDate;
    private Label lblTilde;
    private Label lblCode;

    private void InitializeComponent()
    {
        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.edtCode = new TextBox();
        this.dspName = new TextBox { ReadOnly = true };
        this.dspDanwi = new TextBox { ReadOnly = true };
        this.lblCvnam = new Label { AutoSize = true };
        this.grid = new FastDataGridView();

        this.btnNew = new Button { Text = "초기화(F1)" };
        this.btnSearch = new Button { Text = "조회(F5)" };
        this.btnExcel = new Button { Text = "엑셀저장" };
        this.btnPrint = new Button { Text = "인쇄" };
        this.btnClose = new Button { Text = "닫기(Esc)" };

        this.SuspendLayout();
        //
        // SS36Form
        //
        this.Text = "품목원장 조회";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;

        //
        // 원본 BuildLayout() 그대로 이동
        //
        this.panelTop = new Panel { Dock = DockStyle.Top, Height = 40 };
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 100;
        this.btnSearch.Left = 110; this.btnSearch.Top = 8; this.btnSearch.Width = 100;
        this.btnExcel.Left = 215; this.btnExcel.Top = 8; this.btnExcel.Width = 100;
        this.btnPrint.Left = 320; this.btnPrint.Top = 8; this.btnPrint.Width = 100;
        this.btnClose.Left = 425; this.btnClose.Top = 8; this.btnClose.Width = 100;
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "품목원장조회");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "품목원장조회");

        this.editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        this.lblDate = new Label { Text = "기간", Left = 5, Top = 12, AutoSize = true };
        this.dtpDate1.Left = 50; this.dtpDate1.Top = 8; this.dtpDate1.Width = 110; this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.lblTilde = new Label { Text = "~", Left = 165, Top = 12, AutoSize = true };
        this.dtpDate2.Left = 180; this.dtpDate2.Top = 8; this.dtpDate2.Width = 110; this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.lblCode = new Label { Text = "품번", Left = 310, Top = 12, AutoSize = true };
        this.edtCode.Left = 350; this.edtCode.Top = 8; this.edtCode.Width = 100;
        this.edtCode.KeyDown += this.EdtCode_KeyDown;
        this.dspName.Left = 460; this.dspName.Top = 8; this.dspName.Width = 200;
        this.dspDanwi.Left = 670; this.dspDanwi.Top = 8; this.dspDanwi.Width = 60;
        this.lblCvnam.Left = 740; this.lblCvnam.Top = 12;

        this.editPanel.Controls.AddRange(new Control[]
        {
            this.lblDate, this.dtpDate1, this.lblTilde, this.dtpDate2, this.lblCode, this.edtCode, this.dspName, this.dspDanwi, this.lblCvnam
        });

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("DATE", "일자");
        this.grid.Columns.Add("GUBN", "구분");
        this.grid.Columns.Add("IQTY", "입고(수량/금액)");
        this.grid.Columns.Add("OQTY", "출고(수량/금액)");
        this.grid.Columns.Add("BALANCE", "재고잔량");
        this.grid.Columns.Add("BQTY", "보관(수량/금액)");
        this.grid.Columns.Add("BIGO", "비고");

        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => { this.edtCode.Clear(); this.dspName.Clear(); this.dspDanwi.Clear(); this.lblCvnam.Text = ""; this.grid.Rows.Clear(); this.edtCode.Focus(); };
        this.btnSearch.Click += (_, _) => this.Search();
        this.btnClose.Click += (_, _) => this.Close();

        this.Load += (_, _) =>
        {
            var now = DateTime.Now;
            this.dtpDate1.Value = new DateTime(now.Year, now.Month, 1);
            this.dtpDate2.Value = now;
        };
        this.KeyDown += new KeyEventHandler(this.SS36Form_KeyDown);

        this.ResumeLayout(false);
    }
}
