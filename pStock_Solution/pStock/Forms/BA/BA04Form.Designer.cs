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
        // 원본 BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다. 그래서
        // BuildDynamicLayout()(BA04Form.cs)으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후에 실행한다 — 동작은 100% 동일하다.

        this.Load += (_, _) => { this._vSort = "Code"; ReloadList(); };
        this.KeyDown += new KeyEventHandler(this.BA04Form_KeyDown);
        this.ResumeLayout(false);
    }
}
