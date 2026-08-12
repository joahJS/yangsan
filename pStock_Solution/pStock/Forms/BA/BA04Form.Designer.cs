using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA04Form
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
    private TextBox edtCode;
    private CheckBox chkAuto;
    private TextBox edtName;
    private TextBox edtPost;
    private TextBox edtAddr1;
    private TextBox edtAddr2;
    private TextBox edtTel;
    private TextBox edtBigo;
    private TextBox edtWord;

    private Button btnNew;
    private Button btnAdd;
    private Button btnOne;
    private Button btnDel;
    private Button btnClose;
    private Label lblDbCnt;
    private Panel panelTop;
    private Panel searchPanel;
    private Label lblWord;
    private Panel editPanel;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtCode = new TextBox();
        this.chkAuto = new CheckBox() { Text = "자동채번" };
        this.edtName = new TextBox();
        this.edtPost = new TextBox();
        this.edtAddr1 = new TextBox();
        this.edtAddr2 = new TextBox();
        this.edtTel = new TextBox();
        this.edtBigo = new TextBox();
        this.edtWord = new TextBox();
        this.btnNew = new Button() { Text = "신규(F1)" };
        this.btnAdd = new Button() { Text = "저장(F2)" };
        this.btnOne = new Button() { Text = "수정(F3)" };
        this.btnDel = new Button() { Text = "삭제(F4)" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.lblDbCnt = new Label() { AutoSize = true };
        this.panelTop = new Panel();
        this.searchPanel = new Panel();
        this.lblWord = new Label();
        this.editPanel = new Panel();
        this.SuspendLayout();
        //
        // BA04Form (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "착지처 마스터";
        this.Width = 950;
        this.Height = 600;
        this.KeyPreview = true;
        //
        // 원본 BuildLayout()
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.Add(this.btnNew);
        this.panelTop.Controls.Add(this.btnAdd);
        this.panelTop.Controls.Add(this.btnOne);
        this.panelTop.Controls.Add(this.btnDel);
        this.panelTop.Controls.Add(this.btnClose);
        this.panelTop.Controls.Add(this.lblDbCnt);
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnAdd.Left = 100; this.btnAdd.Top = 8; this.btnAdd.Width = 90;
        this.btnOne.Left = 195; this.btnOne.Top = 8; this.btnOne.Width = 90;
        this.btnDel.Left = 290; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnClose.Left = 385; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.lblDbCnt.Left = 500; this.lblDbCnt.Top = 14;

        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 35;
        this.lblWord.Text = "검색어"; this.lblWord.Left = 5; this.lblWord.Top = 10; this.lblWord.AutoSize = true;
        this.edtWord.Left = 60; this.edtWord.Top = 6; this.edtWord.Width = 200;
        this.edtWord.KeyUp += (_, _) => LocateInGrid(this.edtWord.Text);
        this.searchPanel.Controls.AddRange(new Control[] { this.lblWord, this.edtWord });

        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 190;
        this.editPanel.AutoScroll = true;
        var layout = new GridLayout(this.editPanel, 5, 5, slotWidth: 250, labelWidth: 80, rowHeight: 30, slotsPerRow: 3);

        layout.Add("코드", this.edtCode);
        layout.AddRaw(this.chkAuto, width: 100);
        layout.NewRow();

        layout.Add("착지처명", this.edtName);
        layout.Add("우편번호", this.edtPost);
        layout.Add("전화번호", this.edtTel);

        layout.Add("주소1", this.edtAddr1, span: 2);
        layout.NewRow();
        layout.Add("주소2", this.edtAddr2, span: 2);

        layout.NewRow();
        layout.Add("비고", this.edtBigo, span: 2);

        this.editPanel.Height = layout.Bottom(15);

        this.edtAddr1.DoubleClick += (_, _) => LookupPostalCode();
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

        this.btnNew.Click += (_, _) => { ClearEdit(); this.edtName.Focus(); };
        this.btnAdd.Click += (_, _) => Save(isInsert: true);
        this.btnOne.Click += (_, _) => Save(isInsert: false);
        this.btnDel.Click += (_, _) => Delete();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => { this._vSort = "Code"; ReloadList(); };
        this.KeyDown += new KeyEventHandler(this.BA04Form_KeyDown);
        this.ResumeLayout(false);
    }
}
