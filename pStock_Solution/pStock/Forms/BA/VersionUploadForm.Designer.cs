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
    private Label lblVersionId;
    private Label lblRmk;

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
        this.lblVersionId = new Label();
        this.lblRmk = new Label();
        this.gridButtons.SuspendLayout();
        this.bottom.SuspendLayout();
        this.SuspendLayout();
        //
        // btnUpload / btnSave / btnClose
        //
        this.btnUpload.Text = "파일선택";
        this.btnSave.Text = "저장(F3)";
        this.btnClose.Text = "닫기(Esc)";
        //
        // 원본 GridLayout(this, 20, 15, 280, 90, 30, 2) 계산 결과를 리터럴로 반영
        //
        this.lblVersionId.Text = "VersionID";
        this.lblVersionId.Left = 20; this.lblVersionId.Top = 18; this.lblVersionId.AutoSize = true;
        this.edtVersionId.Left = 110; this.edtVersionId.Top = 15; this.edtVersionId.Width = 150;

        this.lblRmk.Text = "비고";
        this.lblRmk.Left = 20; this.lblRmk.Top = 48; this.lblRmk.AutoSize = true;
        this.edtRmk.Left = 110; this.edtRmk.Top = 45; this.edtRmk.Width = 470;
        //
        // gridButtons
        //
        this.gridButtons.Left = 20; this.gridButtons.Top = 80; this.gridButtons.Width = 560; this.gridButtons.Height = 30;
        this.btnUpload.Left = 0; this.btnUpload.Top = 0; this.btnUpload.Width = 100;
        this.btnUpload.Click += new EventHandler(this.BtnUpload_Click);
        this.lblHint.Text = "※ debug 폴더내의 pStock.zip 파일 업로드";
        this.lblHint.ForeColor = Color.Red;
        this.lblHint.AutoSize = true;
        this.lblHint.Left = 125; this.lblHint.Top = 4;
        this.gridButtons.Controls.Add(this.btnUpload);
        this.gridButtons.Controls.Add(this.lblHint);
        //
        // grid
        //
        this.grid.Left = 20; this.grid.Top = 115; this.grid.Width = 560; this.grid.Height = 220;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("FILE_NAME", "파일명");
        this.grid.Columns.Add("FILE_BYTE", "크기");
        //
        // bottom
        //
        this.bottom.Left = 20; this.bottom.Top = 345; this.bottom.Width = 560; this.bottom.Height = 35;
        this.btnSave.Left = 340; this.btnSave.Top = 0; this.btnSave.Width = 100;
        this.btnClose.Left = 450; this.btnClose.Top = 0; this.btnClose.Width = 100;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        this.bottom.Controls.AddRange(new Control[] { this.btnSave, this.btnClose });
        //
        // VersionUploadForm
        //
        this.Text = "버전 등록";
        this.Width = 620;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.Controls.Add(this.lblVersionId);
        this.Controls.Add(this.edtVersionId);
        this.Controls.Add(this.lblRmk);
        this.Controls.Add(this.edtRmk);
        this.Controls.AddRange(new Control[] { this.gridButtons, this.grid, this.bottom });
        this.ClientSize = new Size(600, 395);
        this.KeyDown += new KeyEventHandler(this.VersionUploadForm_KeyDown);
        this.Shown += new EventHandler(this.VersionUploadForm_Shown);
        this.gridButtons.ResumeLayout(false);
        this.gridButtons.PerformLayout();
        this.bottom.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
