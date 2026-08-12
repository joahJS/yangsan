using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA00EForm
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
    private TextBox edtName;
    private TextBox edtPass;
    private TextBox edtDate1;
    private TextBox edtDate2;
    private Button btnNew;
    private Button btnSave;
    private Button btnClose;
    private Panel panelTop;
    private Panel panelEdit;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtCode = new TextBox();
        this.edtName = new TextBox();
        this.edtPass = new TextBox();
        this.edtDate1 = new TextBox();
        this.edtDate2 = new TextBox();
        this.btnNew = new Button() { Text = "신규(F1)" };
        this.btnSave = new Button() { Text = "저장(F2)" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.SuspendLayout();
        //
        // panelTop (원본 BuildLayout()의 top 패널)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.Add(this.btnNew);
        this.panelTop.Controls.Add(this.btnSave);
        this.panelTop.Controls.Add(this.btnClose);
        {
            int bx = 5;
            foreach (Control c in this.panelTop.Controls)
            {
                c.Left = bx;
                c.Top = 8;
                c.Width = 90;
                bx += 95;
            }
        }
        //
        // panelEdit (원본 BuildLayout()의 editPanel: GridLayout 헬퍼로 라벨/입력란 배치)
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 100;
        var layout = new GridLayout(this.panelEdit, 10, 5, slotWidth: 250, labelWidth: 90, rowHeight: 30, slotsPerRow: 2);
        layout.Add("사용자 ID", this.edtCode);
        layout.Add("성명", this.edtName);
        layout.Add("비밀번호", this.edtPass);
        layout.Add("사용시작일", this.edtDate1);
        layout.Add("사용종료일", this.edtDate2);
        this.panelEdit.Height = layout.Bottom();
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellClick += (_, _) => SyncEditFromGrid();
        this.grid.KeyDown += new KeyEventHandler(this.Grid_KeyDown);
        //
        // Controls.Add 순서 (원본 BuildLayout())
        //
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        //
        // 이벤트 배선
        //
        this.btnNew.Click += (_, _) => { ClearEdit(); edtCode.Focus(); };
        this.btnSave.Click += (_, _) => Save();
        this.btnClose.Click += (_, _) => Close();
        //
        // BA00EForm
        //
        this.Text = "패스워드 변경";
        this.Width = 700;
        this.Height = 500;
        this.KeyPreview = true;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Load += (_, _) => ReloadList();
        this.KeyDown += new KeyEventHandler(this.BA00EForm_KeyDown);
        this.ResumeLayout(false);
    }
}
