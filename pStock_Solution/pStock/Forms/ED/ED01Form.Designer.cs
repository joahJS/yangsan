namespace pStock.Forms.ED;

partial class ED01Form
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
    private ComboBox cboMonth;
    private CheckBox chk1;
    private CheckBox chk2;
    private CheckBox chk3;
    private CheckBox chk4;
    private CheckBox chk5;
    private ProgressBar progress;
    private Button btnOk;
    private Button btnCancel;
    private Label lblYear;
    private Label lblMonth;

    private void InitializeComponent()
    {
        this.edtYear = new TextBox();
        this.cboMonth = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        this.chk1 = new CheckBox { Text = "입고 마감" };
        this.chk2 = new CheckBox { Text = "보관 마감" };
        this.chk3 = new CheckBox { Text = "출고 마감" };
        this.chk4 = new CheckBox { Text = "수금 마감" };
        this.chk5 = new CheckBox { Text = "임시자료 정리" };
        this.progress = new ProgressBar();
        this.btnOk = new Button { Text = "마감실행(F2)" };
        this.btnCancel = new Button { Text = "취소(Esc)" };
        this.SuspendLayout();
        //
        // ED01Form
        //
        this.Text = "월마감 작업";
        this.Width = 450;
        this.Height = 400;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        this.cboMonth.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });

        this.lblYear = new Label { Text = "년도", Left = 20, Top = 20, AutoSize = true };
        this.edtYear.Left = 70; this.edtYear.Top = 16; this.edtYear.Width = 60;
        this.lblMonth = new Label { Text = "월", Left = 150, Top = 20, AutoSize = true };
        this.cboMonth.Left = 180; this.cboMonth.Top = 16; this.cboMonth.Width = 60;

        this.chk1.Left = 20;
        this.chk1.Top = 60;
        this.chk1.AutoSize = true;
        this.chk2.Left = 20;
        this.chk2.Top = 86;
        this.chk2.AutoSize = true;
        this.chk3.Left = 20;
        this.chk3.Top = 112;
        this.chk3.AutoSize = true;
        this.chk4.Left = 20;
        this.chk4.Top = 138;
        this.chk4.AutoSize = true;
        this.chk5.Left = 20;
        this.chk5.Top = 164;
        this.chk5.AutoSize = true;
        this.chk1.Checked = true;
        this.chk2.Checked = true;
        this.chk3.Checked = true;
        this.chk4.Checked = true;
        this.chk5.Checked = true;

        this.progress.Left = 20;
        this.progress.Top = 204;
        this.progress.Width = 390;

        this.btnOk.Left = 100;
        this.btnOk.Top = 244;
        this.btnOk.Width = 100;
        this.btnCancel.Left = 220;
        this.btnCancel.Top = 244;
        this.btnCancel.Width = 100;

        this.Controls.AddRange(new Control[]
        {
            this.lblYear, this.edtYear, this.lblMonth, this.cboMonth,
            this.chk1, this.chk2, this.chk3, this.chk4, this.chk5, this.progress, this.btnOk, this.btnCancel
        });

        this.btnOk.Click += (_, _) => RunClose();
        this.btnCancel.Click += (_, _) => Close();
        //
        // Load / KeyDown
        //
        this.Load += (_, _) =>
        {
            this.edtYear.Text = DateTime.Now.Year.ToString();
            this.cboMonth.SelectedItem = DateTime.Now.Month.ToString();
        };
        this.KeyDown += new KeyEventHandler(this.ED01Form_KeyDown);
        this.ResumeLayout(false);
    }
}
