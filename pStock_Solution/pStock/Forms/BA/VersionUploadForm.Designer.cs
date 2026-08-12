using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class VersionUploadForm
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

    private TextBox edtVersionId;
    private TextBox edtRmk;
    private FastDataGridView grid;

    private Button btnUpload;
    private Button btnSave;
    private Button btnClose;
    private Panel gridButtons;
    private Label lblHint;
    private Panel bottom;

    private void InitializeComponent()
    {
        this.edtVersionId = new TextBox();
        this.edtRmk = new TextBox();
        this.grid = new FastDataGridView();
        this.btnUpload = new Button();
        this.btnSave = new Button();
        this.btnClose = new Button();
        this.gridButtons = new Panel();
        this.lblHint = new Label();
        this.bottom = new Panel();
        this.SuspendLayout();
        //
        // btnUpload / btnSave / btnClose
        //
        this.btnUpload.Text = "파일선택";
        this.btnSave.Text = "저장(F3)";
        this.btnClose.Text = "닫기(Esc)";
        //
        // VersionUploadForm (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "버전 등록";
        this.Width = 620;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        // 원본 BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다. 그래서
        // BuildDynamicLayout()(VersionUploadForm.cs)으로 분리해 생성자에서
        // InitializeComponent() 호출 직후에 실행한다 — 동작은 100% 동일하다.

        this.KeyDown += new KeyEventHandler(this.VersionUploadForm_KeyDown);
        this.Shown += (_, _) => this.edtVersionId.Focus();
        this.ResumeLayout(false);
    }
}
