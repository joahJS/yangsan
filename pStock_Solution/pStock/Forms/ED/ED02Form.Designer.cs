using pStock.Common;

namespace pStock.Forms.ED;

partial class ED02Form
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

    private TextBox edtYear;
    private Button btnDown;
    private Button btnUp;
    private Button btnOk;
    private Button btnCancel;
    private Label lblYear;
    private Label lblHint;

    private void InitializeComponent()
    {
        this.edtYear = new TextBox();
        this.btnDown = new Button();
        this.btnUp = new Button();
        this.btnOk = new Button();
        this.btnCancel = new Button();
        this.lblYear = new Label();
        this.lblHint = new Label();
        this.SuspendLayout();
        //
        // btnDown / btnUp / btnOk / btnCancel
        //
        this.btnDown.Text = "◀";
        this.btnUp.Text = "▶";
        this.btnOk.Text = "마감실행(F2)";
        this.btnCancel.Text = "취소(Esc)";
        //
        // ED02Form
        //
        this.Font = new Font("맑은 고딕", 9F);
        this.Text = "년마감 작업";
        this.Width = 400;
        this.Height = 220;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        this.lblYear.Text = "마감년도";
        this.lblYear.Left = 20;
        this.lblYear.Top = 25;
        this.lblYear.AutoSize = true;
        this.edtYear.Left = 100; this.edtYear.Top = 20; this.edtYear.Width = 60;
        this.btnDown.Left = 165; this.btnDown.Top = 19; this.btnDown.Width = 30;
        this.btnUp.Left = 200; this.btnUp.Top = 19; this.btnUp.Width = 30;

        this.lblHint.Text = "선택한 년도의 재고/미수금을 다음 해로 이월합니다.";
        this.lblHint.Left = 20;
        this.lblHint.Top = 60;
        this.lblHint.AutoSize = true;

        this.btnOk.Left = 100; this.btnOk.Top = 130; this.btnOk.Width = 100;
        this.btnCancel.Left = 210; this.btnCancel.Top = 130; this.btnCancel.Width = 100;

        this.Controls.AddRange(new Control[] { this.lblYear, this.edtYear, this.btnDown, this.btnUp, this.lblHint, this.btnOk, this.btnCancel });

        this.btnDown.Click += new EventHandler(this.BtnDown_Click);
        this.btnUp.Click += new EventHandler(this.BtnUp_Click);
        this.btnOk.Click += new EventHandler(this.BtnOk_Click);
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);
        //
        // Load / KeyDown
        //
        this.Load += new EventHandler(this.ED02Form_Load);
        this.KeyDown += new KeyEventHandler(this.ED02Form_KeyDown);
        this.ResumeLayout(false);
    }
}
