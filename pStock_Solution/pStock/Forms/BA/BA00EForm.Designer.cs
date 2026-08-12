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
    private Label lblCode;
    private Label lblName;
    private Label lblPass;
    private Label lblDate1;
    private Label lblDate2;
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
        this.lblCode = new Label();
        this.lblName = new Label();
        this.lblPass = new Label();
        this.lblDate1 = new Label();
        this.lblDate2 = new Label();
        this.btnNew = new Button();
        this.btnSave = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // panelTop
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Text = "신규(F1)";
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnSave.Text = "저장(F2)";
        this.btnSave.Left = 100; this.btnSave.Top = 8; this.btnSave.Width = 90;
        this.btnClose.Text = "닫기(Esc)";
        this.btnClose.Left = 195; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.panelTop.Controls.Add(this.btnNew);
        this.panelTop.Controls.Add(this.btnSave);
        this.panelTop.Controls.Add(this.btnClose);
        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // panelEdit (원본 GridLayout(panelEdit, 10, 5, slotWidth:250, labelWidth:90, rowHeight:30, slotsPerRow:2) 계산 결과를 리터럴로 반영)
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 105;
        this.lblCode.Text = "사용자 ID";
        this.lblCode.Left = 10; this.lblCode.Top = 8; this.lblCode.AutoSize = true;
        this.edtCode.Left = 100; this.edtCode.Top = 5; this.edtCode.Width = 148;
        this.lblName.Text = "성명";
        this.lblName.Left = 260; this.lblName.Top = 8; this.lblName.AutoSize = true;
        this.edtName.Left = 350; this.edtName.Top = 5; this.edtName.Width = 148;
        this.lblPass.Text = "비밀번호";
        this.lblPass.Left = 10; this.lblPass.Top = 38; this.lblPass.AutoSize = true;
        this.edtPass.Left = 100; this.edtPass.Top = 35; this.edtPass.Width = 148;
        this.lblDate1.Text = "사용시작일";
        this.lblDate1.Left = 260; this.lblDate1.Top = 38; this.lblDate1.AutoSize = true;
        this.edtDate1.Left = 350; this.edtDate1.Top = 35; this.edtDate1.Width = 148;
        this.lblDate2.Text = "사용종료일";
        this.lblDate2.Left = 10; this.lblDate2.Top = 68; this.lblDate2.AutoSize = true;
        this.edtDate2.Left = 100; this.edtDate2.Top = 65; this.edtDate2.Width = 148;
        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblCode, this.edtCode, this.lblName, this.edtName,
            this.lblPass, this.edtPass, this.lblDate1, this.edtDate1,
            this.lblDate2, this.edtDate2
        });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellClick += new DataGridViewCellEventHandler(this.Grid_CellClick);
        this.grid.KeyDown += new KeyEventHandler(this.Grid_KeyDown);
        //
        // BA00EForm
        //
        this.Text = "패스워드 변경";
        this.Width = 700;
        this.Height = 500;
        this.KeyPreview = true;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.BA00EForm_Load);
        this.KeyDown += new KeyEventHandler(this.BA00EForm_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
