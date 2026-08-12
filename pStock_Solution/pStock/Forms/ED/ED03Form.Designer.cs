using pStock.Common;

namespace pStock.Forms.ED;

partial class ED03Form
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
    private TextBox edtYear;
    private TextBox edtCode;
    private TextBox edtCvnam;
    private TextBox edtOwnam;
    private NumericUpDown edtBamt;

    private Button btnNew;
    private Button btnAdd;
    private Button btnDel;
    private Button btnSearch;
    private Button btnClose;
    private Button btnYearDown;
    private Button btnYearUp;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblYear;
    private Label lblCode;
    private Label lblBamt;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtYear = new TextBox();
        this.edtCode = new TextBox();
        this.edtCvnam = new TextBox { ReadOnly = true };
        this.edtOwnam = new TextBox { ReadOnly = true };
        this.edtBamt = new NumericUpDown { Maximum = 9999999999, DecimalPlaces = 0 };

        this.btnNew = new Button { Text = "신규(F1)" };
        this.btnAdd = new Button { Text = "저장(F2)" };
        this.btnDel = new Button { Text = "삭제(F4)" };
        this.btnSearch = new Button { Text = "조회" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this.btnYearDown = new Button { Text = "◀" };
        this.btnYearUp = new Button { Text = "▶" };
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        ((System.ComponentModel.ISupportInitialize)(this.edtBamt)).BeginInit();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // ED03Form
        //
        this.Text = "기초잔액 보수";
        this.Width = 1000;
        this.Height = 650;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        PublicLib.MakeTypingFriendly(this.edtBamt);

        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnAdd, this.btnDel, this.btnSearch, this.btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { this.btnNew, this.btnAdd, this.btnDel, this.btnSearch, this.btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }

        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.lblYear = new Label { Text = "년도", Left = 5, Top = 12, AutoSize = true };
        this.edtYear.Left = 45; this.edtYear.Top = 8; this.edtYear.Width = 60;
        this.edtYear.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) Search(); };
        this.btnYearDown.Left = 110; this.btnYearDown.Top = 8; this.btnYearDown.Width = 30;
        this.btnYearUp.Left = 143; this.btnYearUp.Top = 8; this.btnYearUp.Width = 30;
        this.btnYearDown.Click += (_, _) => { edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) - 1).ToString(); Search(); };
        this.btnYearUp.Click += (_, _) => { edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) + 1).ToString(); Search(); };

        this.lblCode = new Label { Text = "거래처코드", Left = 190, Top = 12, AutoSize = true };
        this.edtCode.Left = 260; this.edtCode.Top = 8; this.edtCode.Width = 80;
        this.edtCode.KeyDown += new KeyEventHandler(this.EdtCode_KeyDown);
        this.edtCvnam.Left = 350; this.edtCvnam.Top = 8; this.edtCvnam.Width = 150;
        this.edtOwnam.Left = 510; this.edtOwnam.Top = 8; this.edtOwnam.Width = 120;

        this.lblBamt = new Label { Text = "기초잔액", Left = 640, Top = 12, AutoSize = true };
        this.edtBamt.Left = 710; this.edtBamt.Top = 8; this.edtBamt.Width = 130;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblYear, this.edtYear, this.btnYearDown, this.btnYearUp, this.lblCode, this.edtCode, this.edtCvnam, this.edtOwnam, this.lblBamt, this.edtBamt
        });

        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += (_, _) => SyncEditFromGrid();

        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);

        this.btnNew.Click += (_, _) => { ClearEdit(); edtCode.Focus(); };
        this.btnAdd.Click += (_, _) => Save();
        this.btnDel.Click += (_, _) => Delete();
        this.btnSearch.Click += (_, _) => Search();
        this.btnClose.Click += (_, _) => Close();
        //
        // Load / KeyDown
        //
        this.Load += (_, _) => { this.edtYear.Text = DateTime.Now.Year.ToString(); Search(); };
        this.KeyDown += new KeyEventHandler(this.ED03Form_KeyDown);
        ((System.ComponentModel.ISupportInitialize)(this.edtBamt)).EndInit();
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
