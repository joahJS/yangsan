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
        this.btnUpload = new Button() { Text = "파일선택" };
        this.btnSave = new Button() { Text = "저장(F3)" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.gridButtons = new Panel();
        this.lblHint = new Label();
        this.bottom = new Panel();
        this.SuspendLayout();
        //
        // VersionUploadForm (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "버전 등록";
        this.Width = 620;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // 원본 BuildLayout()
        //
        var layout = new GridLayout(this, 20, 15, 280, 90, 30, 2);
        layout.Add("VersionID", this.edtVersionId, editWidth: 150);
        layout.NewRow();
        layout.Add("비고", this.edtRmk, span: 2, editWidth: 470);
        layout.NewRow();

        this.gridButtons.Left = 20; this.gridButtons.Top = layout.Bottom(5); this.gridButtons.Width = 560; this.gridButtons.Height = 30;
        this.btnUpload.Left = 0; this.btnUpload.Top = 0; this.btnUpload.Width = 100;
        this.gridButtons.Controls.Add(this.btnUpload);

        this.lblHint.Text = "※ debug 폴더내의 pStock.zip 파일 업로드";
        this.lblHint.ForeColor = Color.Red;
        this.lblHint.AutoSize = true;
        this.lblHint.Left = this.btnUpload.Right + 25;
        this.lblHint.Top = this.btnUpload.Top + 4;
        this.gridButtons.Controls.Add(this.lblHint);

        this.grid.Left = 20; this.grid.Top = this.gridButtons.Bottom + 5; this.grid.Width = 560; this.grid.Height = 220;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("FILE_NAME", "파일명");
        this.grid.Columns.Add("FILE_BYTE", "크기");

        this.bottom.Left = 20; this.bottom.Top = this.grid.Bottom + 10; this.bottom.Width = 560; this.bottom.Height = 35;
        this.btnSave.Left = 340; this.btnSave.Top = 0; this.btnSave.Width = 100;
        this.btnClose.Left = 450; this.btnClose.Top = 0; this.btnClose.Width = 100;
        this.bottom.Controls.AddRange(new Control[] { this.btnSave, this.btnClose });

        this.Controls.AddRange(new Control[] { this.gridButtons, this.grid, this.bottom });
        this.ClientSize = new Size(600, this.bottom.Bottom + 15);

        this.btnUpload.Click += (_, _) => PickFiles();
        this.btnSave.Click += (_, _) => Save();
        this.btnClose.Click += (_, _) => Close();

        this.KeyDown += new KeyEventHandler(this.VersionUploadForm_KeyDown);
        this.Shown += (_, _) => this.edtVersionId.Focus();
        this.ResumeLayout(false);
    }
}
