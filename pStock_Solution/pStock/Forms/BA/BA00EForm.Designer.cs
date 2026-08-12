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
        // 원본 BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다. 그래서
        // BuildDynamicLayout()(BA00EForm.cs)으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후에 실행한다 — 동작은 100% 동일하다(panelTop/panelEdit/grid 구성,
        // Controls.Add 순서, 이벤트 배선 전부 포함).
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
