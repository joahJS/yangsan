using pStock.Common;

namespace pStock.Forms.SS;

partial class SS33Form
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

    private FastDataGridView gridList;
    private FastDataGridView gridSum;
    private DateTimePicker dtpDate1;
    private DateTimePicker dtpDate2;
    private DateTimePicker dtpDate3;
    private DateTimePicker dtpDate4;
    private TextBox edtCvcd1;
    private TextBox edtCvcd2;

    private Button btnNew;
    private Button btnDel;
    private Button btnSum;
    private Button btnAuto;
    private Button btnSearch;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;

    private Panel panelTop;
    private Panel panel1;
    private Label lbl1a;
    private Label lbl1b;
    private SplitContainer split1;
    private Panel panel2;
    private Label lbl2a;
    private Label lbl2b;
    private Label lbl2c;
    private Label lbl2d;

    private void InitializeComponent()
    {
        this.tabs = new TabControl();
        this.tab1 = new TabPage("일자별");
        this.tab2 = new TabPage("거래처별");

        this.gridList = new FastDataGridView();
        this.gridSum = new FastDataGridView();
        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.dtpDate3 = new DateTimePicker();
        this.dtpDate4 = new DateTimePicker();
        this.edtCvcd1 = new TextBox();
        this.edtCvcd2 = new TextBox();

        this.btnNew = new Button();
        this.btnNew.Text = "신규(F1)";
        this.btnDel = new Button();
        this.btnDel.Text = "삭제";
        this.btnSum = new Button();
        this.btnSum.Text = "합계표";
        this.btnAuto = new Button();
        this.btnAuto.Text = "자동발행";
        this.btnSearch = new Button();
        this.btnSearch.Text = "조회";
        this.btnExcel = new Button();
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint = new Button();
        this.btnPrint.Text = "인쇄";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";

        this.split1 = new SplitContainer();

        ((System.ComponentModel.ISupportInitialize)(this.split1)).BeginInit();
        this.split1.Panel1.SuspendLayout();
        this.split1.Panel2.SuspendLayout();
        this.split1.SuspendLayout();
        this.SuspendLayout();
        //
        // SS33Form
        //
        this.Text = "계산서 관리";
        this.Width = 1300;
        this.Height = 700;
        this.KeyPreview = true;

        //
        // 원본 BuildLayout() 그대로 이동
        //
        this.panelTop = new Panel();
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnDel, this.btnSum, this.btnAuto, this.btnSearch, this.btnExcel, this.btnPrint, this.btnClose });
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnDel.Left = 100; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnSum.Left = 195; this.btnSum.Top = 8; this.btnSum.Width = 90;
        this.btnAuto.Left = 290; this.btnAuto.Top = 8; this.btnAuto.Width = 90;
        this.btnSearch.Left = 385; this.btnSearch.Top = 8; this.btnSearch.Width = 90;
        this.btnExcel.Left = 480; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 575; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 670; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.btnAuto.Click += (_, _) =>
        {
            using var dlg = new SS33AForm();
            dlg.ShowDialog(this);
            if (dlg.Executed) this.Search();
        };
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.gridList, "계산서관리");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.gridList, "계산서관리");

        this.panel1 = new Panel();
        this.panel1.Dock = DockStyle.Top;
        this.panel1.Height = 35;
        this.lbl1a = new Label();
        this.lbl1a.Text = "발행기간";
        this.lbl1a.Left = 10;
        this.lbl1a.Top = 10;
        this.lbl1a.AutoSize = true;
        this.dtpDate1.Left = 90; this.dtpDate1.Top = 6; this.dtpDate1.Width = 110; this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.lbl1b = new Label();
        this.lbl1b.Text = "~";
        this.lbl1b.Left = 205;
        this.lbl1b.Top = 10;
        this.lbl1b.AutoSize = true;
        this.dtpDate2.Left = 220; this.dtpDate2.Top = 6; this.dtpDate2.Width = 110; this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.panel1.Controls.AddRange(new Control[] { this.lbl1a, this.dtpDate1, this.lbl1b, this.dtpDate2 });

        this.split1.Dock = DockStyle.Fill;
        this.split1.SplitterDistance = 900;
        this.gridList.Dock = DockStyle.Fill; this.gridList.ReadOnly = true; this.gridList.AllowUserToAddRows = false;
        this.gridList.CellDoubleClick += (_, _) => this.OpenEntry(isNew: false);
        this.gridSum.Dock = DockStyle.Fill; this.gridSum.ReadOnly = true; this.gridSum.AllowUserToAddRows = false;
        this.gridSum.Columns.Add("KEY", "구분");
        this.gridSum.Columns.Add("CNT", "건수");
        this.gridSum.Columns.Add("SAMT", "금액");
        this.split1.Panel1.Controls.Add(this.gridList);
        this.split1.Panel2.Controls.Add(this.gridSum);

        this.tab1.Controls.Add(this.split1);
        this.tab1.Controls.Add(this.panel1);

        this.panel2 = new Panel();
        this.panel2.Dock = DockStyle.Top;
        this.panel2.Height = 35;
        this.lbl2a = new Label();
        this.lbl2a.Text = "발행기간";
        this.lbl2a.Left = 10;
        this.lbl2a.Top = 10;
        this.lbl2a.AutoSize = true;
        this.dtpDate3.Left = 90; this.dtpDate3.Top = 6; this.dtpDate3.Width = 110; this.dtpDate3.Format = DateTimePickerFormat.Short;
        this.lbl2b = new Label();
        this.lbl2b.Text = "~";
        this.lbl2b.Left = 205;
        this.lbl2b.Top = 10;
        this.lbl2b.AutoSize = true;
        this.dtpDate4.Left = 220; this.dtpDate4.Top = 6; this.dtpDate4.Width = 110; this.dtpDate4.Format = DateTimePickerFormat.Short;
        this.lbl2c = new Label();
        this.lbl2c.Text = "거래처";
        this.lbl2c.Left = 350;
        this.lbl2c.Top = 10;
        this.lbl2c.AutoSize = true;
        this.edtCvcd1.Left = 400; this.edtCvcd1.Top = 6; this.edtCvcd1.Width = 70;
        this.lbl2d = new Label();
        this.lbl2d.Text = "~";
        this.lbl2d.Left = 475;
        this.lbl2d.Top = 10;
        this.lbl2d.AutoSize = true;
        this.edtCvcd2.Left = 490; this.edtCvcd2.Top = 6; this.edtCvcd2.Width = 70;
        this.panel2.Controls.AddRange(new Control[] { this.lbl2a, this.dtpDate3, this.lbl2b, this.dtpDate4, this.lbl2c, this.edtCvcd1, this.lbl2d, this.edtCvcd2 });

        this.tab2.Controls.Add(this.panel2);

        this.tabs.Dock = DockStyle.Fill;
        this.tabs.TabPages.AddRange(new[] { this.tab1, this.tab2 });
        this.tabs.SelectedIndexChanged += (_, _) => this.Search();

        this.Controls.Add(this.tabs);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => this.OpenEntry(isNew: true);
        this.btnDel.Click += (_, _) => this.Delete();
        this.btnSum.Click += (_, _) => pStock.Common.GridPrinter.Print(this.gridList, "매출계산서 합계표");
        this.btnSearch.Click += (_, _) => this.Search();
        this.btnClose.Click += (_, _) => this.Close();

        this.Load += (_, _) =>
        {
            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            this.dtpDate1.Value = monthStart; this.dtpDate2.Value = DateTime.Now;
            this.dtpDate3.Value = monthStart; this.dtpDate4.Value = DateTime.Now;
            this.Search();
        };
        this.KeyDown += new KeyEventHandler(this.SS33Form_KeyDown);

        this.split1.Panel1.ResumeLayout(false);
        this.split1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.split1)).EndInit();
        this.split1.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
