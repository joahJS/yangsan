using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA00Form
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

    private TextBox edtSang;   // 상호
    private TextBox edtName;   // 대표자명
    private TextBox edtSa1;    // 사업자번호 앞3
    private TextBox edtSa2;    // 사업자번호 중2
    private TextBox edtSa3;    // 사업자번호 뒤5
    private TextBox edtNo1;    // 법인번호 앞6
    private TextBox edtNo2;    // 법인번호 뒤7
    private TextBox edtUptae;  // 업태
    private TextBox edtJong;   // 종목
    private TextBox edtPost1;  // 우편번호 앞3
    private TextBox edtPost2;  // 우편번호 뒤3
    private TextBox edtAddr;   // 주소
    private TextBox edtDDD;    // 지역번호
    private TextBox edtTel;
    private TextBox edtFax;
    private TextBox edtBigo;
    private Button btnSave;
    private Button btnClose;
    private Panel editPanel;
    private Button btnPost;

    private void InitializeComponent()
    {
        this.edtSang = new TextBox();
        this.edtName = new TextBox();
        this.edtSa1 = new TextBox();
        this.edtSa2 = new TextBox();
        this.edtSa3 = new TextBox();
        this.edtNo1 = new TextBox();
        this.edtNo2 = new TextBox();
        this.edtUptae = new TextBox();
        this.edtJong = new TextBox();
        this.edtPost1 = new TextBox();
        this.edtPost2 = new TextBox();
        this.edtAddr = new TextBox();
        this.edtDDD = new TextBox();
        this.edtTel = new TextBox();
        this.edtFax = new TextBox();
        this.edtBigo = new TextBox();
        this.btnSave = new Button() { Text = "저장(F2)" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.editPanel = new Panel();
        this.btnPost = new Button();
        this.SuspendLayout();
        //
        // BA00Form (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "사업장 마스터";
        this.Width = 660;
        this.KeyPreview = true;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        // 원본 BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다. 그래서
        // BuildDynamicLayout()(BA00Form.cs)으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후에 실행한다 — 동작은 100% 동일하다.

        this.Load += (_, _) => LoadCompanyInfo();
        this.KeyDown += new KeyEventHandler(this.BA00Form_KeyDown);
        this.ResumeLayout(false);
    }
}
