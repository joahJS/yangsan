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
        this.btnDown = new Button { Text = "◀" };
        this.btnUp = new Button { Text = "▶" };
        this.btnOk = new Button { Text = "마감실행(F2)" };
        this.btnCancel = new Button { Text = "취소(Esc)" };
        this.SuspendLayout();
        //
        // ED02Form
        //
        this.Text = "년마감 작업";
        this.Width = 400;
        this.Height = 220;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        this.lblYear = new Label { Text = "마감년도", Left = 20, Top = 25, AutoSize = true };
        this.edtYear.Left = 100; this.edtYear.Top = 20; this.edtYear.Width = 60;
        this.btnDown.Left = 165; this.btnDown.Top = 19; this.btnDown.Width = 30;
        this.btnUp.Left = 200; this.btnUp.Top = 19; this.btnUp.Width = 30;

        this.lblHint = new Label
        {
            Text = "선택한 년도의 재고/미수금을 다음 해로 이월합니다.",
            Left = 20, Top = 60, AutoSize = true
        };

        this.btnOk.Left = 100; this.btnOk.Top = 130; this.btnOk.Width = 100;
        this.btnCancel.Left = 210; this.btnCancel.Top = 130; this.btnCancel.Width = 100;

        this.Controls.AddRange(new Control[] { this.lblYear, this.edtYear, this.btnDown, this.btnUp, this.lblHint, this.btnOk, this.btnCancel });

        this.btnDown.Click += (_, _) => { edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) - 1).ToString(); };
        this.btnUp.Click += (_, _) => { edtYear.Text = (PublicLib.StrToIntSafe(edtYear.Text) + 1).ToString(); };
        this.btnOk.Click += (_, _) => RunClose();
        this.btnCancel.Click += (_, _) => Close();
        //
        // Load / KeyDown
        //
        this.Load += (_, _) => { this.edtYear.Text = DateTime.Now.Year.ToString(); };
        this.KeyDown += new KeyEventHandler(this.ED02Form_KeyDown);
        this.ResumeLayout(false);
    }
}
