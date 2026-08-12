namespace pStock.Forms.SS;

partial class SS23BForm
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

    private DateTimePicker dtpDate;
    private Label lblTerm;
    private ProgressBar progress;
    private Label lblStatus;
    private Button btnOk;
    private Button btnCancel;
    private Label lblDate;
    private Label lblHint;

    private void InitializeComponent()
    {
        this.dtpDate = new DateTimePicker();
        this.lblTerm = new Label();
        this.progress = new ProgressBar();
        this.lblStatus = new Label();
        this.btnOk = new Button();
        this.btnCancel = new Button();
        this.lblDate = new Label();
        this.lblHint = new Label();
        this.SuspendLayout();
        //
        // 원본 필드 초기값(lblTerm/lblStatus AutoSize, btnOk/btnCancel Text)
        //
        this.lblTerm.AutoSize = true;
        this.lblStatus.AutoSize = true;
        this.btnOk.Text = "실행(F2)";
        this.btnCancel.Text = "취소(F3)";
        //
        // SS23BForm
        //
        this.Text = "보관료 자동계산";
        this.Width = 480;
        this.Height = 260;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout()
        //
        this.lblDate.Text = "기준일(반월 시작일)";
        this.lblDate.Left = 20;
        this.lblDate.Top = 25;
        this.lblDate.AutoSize = true;
        this.dtpDate.Left = 180;
        this.dtpDate.Top = 20;
        this.dtpDate.Width = 120;
        this.dtpDate.Format = DateTimePickerFormat.Short;
        this.dtpDate.ValueChanged += new EventHandler(this.DtpDate_ValueChanged);
        this.lblTerm.Left = 310;
        this.lblTerm.Top = 25;

        this.lblHint.Text = "선택한 반월 구간의 자동 보관전표를 다시 생성합니다.\r\n" +
                             "(기존 자동전표는 삭제 후 재계산됩니다)";
        this.lblHint.Left = 20;
        this.lblHint.Top = 60;
        this.lblHint.AutoSize = true;

        this.progress.Left = 20;
        this.progress.Top = 110;
        this.progress.Width = 420;
        this.progress.Visible = false;
        this.lblStatus.Left = 20;
        this.lblStatus.Top = 140;

        this.btnOk.Left = 130;
        this.btnOk.Top = 180;
        this.btnOk.Width = 100;
        this.btnCancel.Left = 250;
        this.btnCancel.Top = 180;
        this.btnCancel.Width = 100;

        this.Controls.AddRange(new Control[] { this.lblDate, this.dtpDate, this.lblTerm, this.lblHint, this.progress, this.lblStatus, this.btnOk, this.btnCancel });

        this.btnOk.Click += new EventHandler(this.BtnOk_Click);
        this.btnCancel.Click += new EventHandler(this.BtnCancel_Click);

        this.Load += new EventHandler(this.SS23BForm_Load);
        this.KeyDown += new KeyEventHandler(this.SS23BForm_KeyDown);

        this.ResumeLayout(false);
    }
}
