using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA01Form
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
    private ComboBox cboGu;
    private CheckBox chkAuto;
    private TextBox edtCode;
    private TextBox edtName;
    private TextBox edtOwnam;
    private TextBox edtSano;   // 사업자번호
    private TextBox edtCvno;   // 법인번호
    private TextBox edtUptae;  // 업태
    private TextBox edtJongk;  // 종목
    private TextBox edtPost;   // 우편번호
    private TextBox edtDDD;    // 지역번호
    private TextBox edtTel;
    private TextBox edtFax;
    private TextBox edtAddr;
    private TextBox edtSdate;  // 시작일
    private TextBox edtEdate;  // 종료일
    private TextBox edtBigo;   // 비고
    private TextBox edtWord;   // 검색어

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
        this.cboGu = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
        this.chkAuto = new CheckBox() { Text = "자동채번" };
        this.edtCode = new TextBox();
        this.edtName = new TextBox();
        this.edtOwnam = new TextBox();
        this.edtSano = new TextBox();
        this.edtCvno = new TextBox();
        this.edtUptae = new TextBox();
        this.edtJongk = new TextBox();
        this.edtPost = new TextBox();
        this.edtDDD = new TextBox();
        this.edtTel = new TextBox();
        this.edtFax = new TextBox();
        this.edtAddr = new TextBox();
        this.edtSdate = new TextBox();
        this.edtEdate = new TextBox();
        this.edtBigo = new TextBox();
        this.edtWord = new TextBox();
        this.btnNew = new Button() { Text = "신규(F1)" };
        this.btnAdd = new Button() { Text = "저장(F2)" };
        this.btnOne = new Button() { Text = "수정(F3)" };
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
        // BA01Form (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "거래처 마스터";
        this.Width = 1100;
        this.Height = 700;
        this.KeyPreview = true;
        //
        // 원본 BuildLayout()
        //
        this.cboGu.Items.AddRange(new object[] { "1. 매입처", "2. 매출처", "3. 공통" });

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
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }
        this.lblDbCnt.Left = bx + 20; this.lblDbCnt.Top = 14;
        this.btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(this.grid, "거래처마스터");
        this.btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(this.grid, "거래처마스터");

        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 35;
        this.lblWord.Text = "검색어"; this.lblWord.Left = 5; this.lblWord.Top = 10; this.lblWord.AutoSize = true;
        this.edtWord.Left = 60; this.edtWord.Top = 6; this.edtWord.Width = 200;
        this.edtWord.KeyUp += (_, _) => LocateInGrid(this.edtWord.Text);
        this.searchPanel.Controls.AddRange(new Control[] { this.lblWord, this.edtWord });

        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 230;
        this.editPanel.AutoScroll = true;
        var layout = new GridLayout(this.editPanel, 5, 5, slotWidth: 250, labelWidth: 85, rowHeight: 30, slotsPerRow: 3);

        layout.Add("구분", this.cboGu);
        layout.Add("코드", this.edtCode);
        layout.AddRaw(this.chkAuto, width: 100);

        layout.Add("거래처명", this.edtName);
        layout.Add("대표자", this.edtOwnam);

        layout.Add("사업자번호", this.edtSano);
        layout.Add("법인번호", this.edtCvno);

        layout.Add("업태", this.edtUptae);
        layout.Add("종목", this.edtJongk);

        layout.Add("우편번호", this.edtPost);
        layout.Add("주소", this.edtAddr, span: 2);

        layout.Add("지역번호", this.edtDDD);
        layout.Add("전화번호", this.edtTel);
        layout.Add("팩스번호", this.edtFax);

        layout.Add("시작일", this.edtSdate);
        layout.Add("종료일", this.edtEdate);

        layout.NewRow();
        layout.Add("비고", this.edtBigo, span: 3);

        this.editPanel.Height = layout.Bottom(15);

        this.edtPost.DoubleClick += (_, _) => LookupPostalCode();
        this.edtAddr.DoubleClick += (_, _) => LookupPostalCode();
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

        this.btnNew.Click += (_, _) => { ClearEdit(); CodeToggle(!this.chkAuto.Checked); this.cboGu.Focus(); };
        this.btnAdd.Click += (_, _) => Save(isInsert: true);
        this.btnOne.Click += (_, _) => Save(isInsert: false);
        this.btnDel.Click += (_, _) => Delete();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => { this._vSort = "Code"; ReloadList(); };
        this.KeyDown += new KeyEventHandler(this.BA01Form_KeyDown);
        this.ResumeLayout(false);
    }
}
