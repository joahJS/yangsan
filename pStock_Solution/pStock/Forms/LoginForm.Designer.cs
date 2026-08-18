namespace pStock.Forms;

partial class LoginForm
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

    private TextBox edtCode;
    private TextBox edtNo;
    private Button btnOk;
    private Button btnCancel;
    private Label lblCodeCaption;
    private Label lblPassCaption;
    private CheckBox chkRemember;

    private void InitializeComponent()
    {
        this.edtCode = new TextBox();
        this.edtNo = new TextBox();
        this.btnOk = new Button();
        this.btnCancel = new Button();
        this.lblCodeCaption = new Label();
        this.lblPassCaption = new Label();
        this.chkRemember = new CheckBox();
        this.SuspendLayout();
        //
        // LoginForm
        //
        this.Text = "로그인_개발서버";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ClientSize = new Size(320, 195);
        this.KeyPreview = true;
        //
        // lblCodeCaption
        //
        this.lblCodeCaption.Text = "사용자 코드";
        this.lblCodeCaption.Location = new Point(20, 25);
        this.lblCodeCaption.AutoSize = true;
        //
        // edtCode
        //
        this.edtCode.Location = new Point(120, 22);
        this.edtCode.Width = 160;
        this.edtCode.Leave += this.EdtCode_Leave;
        //
        // lblPassCaption
        //
        this.lblPassCaption.Text = "비밀번호";
        this.lblPassCaption.Location = new Point(20, 60);
        this.lblPassCaption.AutoSize = true;
        //
        // edtNo
        //
        this.edtNo.Location = new Point(120, 57);
        this.edtNo.Width = 160;
        this.edtNo.PasswordChar = '*';
        //
        // chkRemember
        //
        this.chkRemember.Text = "접속정보 기억하기";
        this.chkRemember.Location = new Point(120, 88);
        this.chkRemember.AutoSize = true;
        //
        // btnOk
        //
        this.btnOk.Text = "확인";
        this.btnOk.Location = new Point(120, 130);
        this.btnOk.Width = 75;
        this.btnOk.Click += this.BtnOk_Click;
        //
        // btnCancel
        //
        this.btnCancel.Text = "취소";
        this.btnCancel.Location = new Point(205, 130);
        this.btnCancel.Width = 75;
        this.btnCancel.Click += this.BtnCancel_Click;

        this.Controls.AddRange(new Control[]
        {
            this.lblCodeCaption, this.edtCode, this.lblPassCaption, this.edtNo,
            this.chkRemember, this.btnOk, this.btnCancel
        });

        this.AcceptButton = this.btnOk;
        this.CancelButton = this.btnCancel;

        this.Load += this.LoginForm_Load;
        this.FormClosing += this.LoginForm_FormClosing;
        this.KeyDown += this.LoginForm_KeyDown;
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
