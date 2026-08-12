using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA03Form
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
    private TextBox edtCvcod;  // 거래처코드(품번 앞 4자리)
    private TextBox edtCvnam;  // 거래처명(참조표시, 읽기전용)
    private TextBox edtCode;   // 품번 전체
    private CheckBox chkAuto;
    private TextBox edtItdsc;  // 품명
    private TextBox edtSpec;   // 규격
    private ComboBox cboDanwi; // 단위
    private NumericUpDown eItwgt;  // 중량
    private NumericUpDown edtIcost; // 입고단가
    private NumericUpDown edtBcost; // 기준단가
    private NumericUpDown edtOcost; // 출고단가
    private ComboBox cboSavLoc; // 저장위치
    private TextBox edtBigo;
    private TextBox edtWord;

    private Button btnNew;
    private Button btnAdd;
    private Button btnOne;
    private Button btnDel;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Label lblDbCnt;
    private Panel panelTop;
    private Panel searchPanel;
    private Label lblWord;
    private Panel editPanel;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtCvcod = new TextBox();
        this.edtCvnam = new TextBox();
        this.edtCode = new TextBox();
        this.chkAuto = new CheckBox() { Text = "자동채번" };
        this.edtItdsc = new TextBox();
        this.edtSpec = new TextBox();
        this.cboDanwi = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
        this.eItwgt = new NumericUpDown() { DecimalPlaces = 2, Maximum = 999999 };
        this.edtIcost = new NumericUpDown() { DecimalPlaces = 0, Maximum = 999999999 };
        this.edtBcost = new NumericUpDown() { DecimalPlaces = 0, Maximum = 999999999 };
        this.edtOcost = new NumericUpDown() { DecimalPlaces = 0, Maximum = 999999999 };
        this.cboSavLoc = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
        this.edtBigo = new TextBox();
        this.edtWord = new TextBox();
        this.btnNew = new Button() { Text = "신규(F1)" };
        this.btnAdd = new Button() { Text = "연속저장(F2)" };
        this.btnOne = new Button() { Text = "저장(F3)" };
        this.btnDel = new Button() { Text = "삭제(F4)" };
        this.btnExcel = new Button() { Text = "엑셀저장" };
        this.btnPrint = new Button() { Text = "인쇄" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.lblDbCnt = new Label() { AutoSize = true };
        this.panelTop = new Panel();
        this.searchPanel = new Panel();
        this.lblWord = new Label();
        this.editPanel = new Panel();
        this.SuspendLayout();
        //
        // BA03Form (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "품목,단가 마스터";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;
        //
        // 원본 BuildLayout()
        //
        foreach (var n in new[] { this.eItwgt, this.edtIcost, this.edtBcost, this.edtOcost })
            PublicLib.MakeTypingFriendly(n);

        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.Add(this.btnNew);
        this.panelTop.Controls.Add(this.btnAdd);
        this.panelTop.Controls.Add(this.btnOne);
        this.panelTop.Controls.Add(this.btnDel);
        this.panelTop.Controls.Add(this.btnExcel);
        this.panelTop.Controls.Add(this.btnPrint);
        this.panelTop.Controls.Add(this.btnClose);
        this.panelTop.Controls.Add(this.lblDbCnt);
        int bx = 5;
        foreach (Control c in new Control[] { this.btnNew, this.btnAdd, this.btnOne, this.btnDel, this.btnExcel, this.btnPrint, this.btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        this.lblDbCnt.Left = bx + 20; this.lblDbCnt.Top = 14;
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "품목단가마스터");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "품목단가마스터");

        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 35;
        this.lblWord.Text = "검색어"; this.lblWord.Left = 5; this.lblWord.Top = 10; this.lblWord.AutoSize = true;
        this.edtWord.Left = 60; this.edtWord.Top = 6; this.edtWord.Width = 200;
        this.edtWord.KeyUp += (_, _) => LocateInGrid(this.edtWord.Text);
        this.searchPanel.Controls.AddRange(new Control[] { this.lblWord, this.edtWord });

        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 250;
        this.editPanel.AutoScroll = true;
        var layout = new GridLayout(this.editPanel, 5, 5, slotWidth: 250, labelWidth: 85, rowHeight: 30, slotsPerRow: 3);

        layout.Add("거래처코드", this.edtCvcod);
        this.edtCvnam.ReadOnly = true;
        layout.Add("거래처명", this.edtCvnam, span: 2);

        layout.Add("품번", this.edtCode);
        layout.AddRaw(this.chkAuto, width: 100);

        layout.NewRow();
        layout.Add("품명", this.edtItdsc, span: 2);

        layout.Add("규격", this.edtSpec);
        layout.Add("단위", this.cboDanwi);

        layout.Add("중량", this.eItwgt);
        layout.Add("저장위치", this.cboSavLoc);

        layout.NewRow();
        layout.Add("입고단가", this.edtIcost);
        layout.Add("기준단가", this.edtBcost);
        layout.Add("출고단가", this.edtOcost);

        layout.NewRow();
        layout.Add("비고", this.edtBigo, span: 3);

        this.editPanel.Height = layout.Bottom(15);

        this.edtCvcod.KeyDown += new KeyEventHandler(this.EdtCvcod_KeyDown);
        this.chkAuto.Click += (_, _) => CodeToggle(!this.chkAuto.Checked);

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += (_, _) => SyncEditFromGrid();
        this.grid.KeyDown += (_, e) => { if (e.KeyCode == Keys.Delete) this.btnDel.PerformClick(); };

        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.searchPanel);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => { ClearEdit(); this.edtCvcod.Focus(); };
        this.btnAdd.Click += (_, _) => { if (Save()) { ClearEdit(); this.edtCvcod.Focus(); } };
        this.btnOne.Click += (_, _) => Save();
        this.btnDel.Click += (_, _) => Delete();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => { ResetDanwiList(); ResetSavLocList(); this._vSort = "Code"; ReloadList(); };
        this.KeyDown += new KeyEventHandler(this.BA03Form_KeyDown);
        this.ResumeLayout(false);
    }
}
