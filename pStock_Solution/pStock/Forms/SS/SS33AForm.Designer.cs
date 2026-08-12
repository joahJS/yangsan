namespace pStock.Forms.SS;

partial class SS33AForm
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

    private DateTimePicker dtpDate1;
    private DateTimePicker dtpDate2;
    private DateTimePicker dtpDate;
    private TextBox edtCd1;
    private TextBox edtCd2;
    private RadioButton rdo1;
    private RadioButton rdo2;
    private Button btnYes;
    private Button btnClose;
    private Label lblDate1;
    private Label lblTilde1;
    private Label lblDate;
    private Label lblCd;
    private Label lblTilde2;
    private Button btnLookup;
    private Label lblGu;
    private Label lblHint;

    private void InitializeComponent()
    {
        this.dtpDate1 = new DateTimePicker();
        this.dtpDate2 = new DateTimePicker();
        this.dtpDate = new DateTimePicker();
        this.edtCd1 = new TextBox();
        this.edtCd2 = new TextBox();
        this.rdo1 = new RadioButton();
        this.rdo2 = new RadioButton();
        this.btnYes = new Button();
        this.btnClose = new Button();
        this.lblDate1 = new Label();
        this.lblTilde1 = new Label();
        this.lblDate = new Label();
        this.lblCd = new Label();
        this.lblTilde2 = new Label();
        this.btnLookup = new Button();
        this.lblGu = new Label();
        this.lblHint = new Label();
        this.SuspendLayout();
        //
        // 원본 필드 초기값
        //
        this.rdo1.Text = "영수";
        this.rdo2.Text = "청구";
        this.rdo2.Checked = true;
        this.btnYes.Text = "실행(F2)";
        this.btnClose.Text = "닫기(Esc)";
        //
        // SS33AForm
        //
        this.Text = "계산서 자동발행";
        this.Width = 480;
        this.Height = 320;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout() (원본 AddRow 헬퍼 호출 2회 포함 — y=20에서 시작해 30,30,30,40,50씩
        // 증가하던 지역변수 누적 계산을 손으로 검산한 리터럴 값으로 치환)
        //

        // AddRow("집계기간", dtpDate1, col1=20, ref y=20, 130)
        this.lblDate1.Text = "집계기간";
        this.lblDate1.Left = 20;
        this.lblDate1.Top = 23;
        this.lblDate1.AutoSize = true;
        this.dtpDate1.Left = 170;
        this.dtpDate1.Top = 20;
        this.dtpDate1.Width = 130;
        this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.Controls.Add(this.lblDate1);
        this.Controls.Add(this.dtpDate1);
        // y = 50

        this.lblTilde1.Text = "~";
        this.lblTilde1.Left = 320;
        this.lblTilde1.Top = 27;
        this.lblTilde1.AutoSize = true;
        this.dtpDate2.Left = 340;
        this.dtpDate2.Top = 24;
        this.dtpDate2.Width = 130;
        this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.Controls.AddRange(new Control[] { this.lblTilde1, this.dtpDate2 });

        // AddRow("발행일자", dtpDate, col1=20, ref y=50, 130)
        this.lblDate.Text = "발행일자";
        this.lblDate.Left = 20;
        this.lblDate.Top = 53;
        this.lblDate.AutoSize = true;
        this.dtpDate.Left = 170;
        this.dtpDate.Top = 50;
        this.dtpDate.Width = 130;
        this.dtpDate.Format = DateTimePickerFormat.Short;
        this.Controls.Add(this.lblDate);
        this.Controls.Add(this.dtpDate);
        // y = 80

        this.lblCd.Text = "거래처코드 범위";
        this.lblCd.Left = 20;
        this.lblCd.Top = 83;
        this.lblCd.AutoSize = true;
        this.edtCd1.Left = 170;
        this.edtCd1.Top = 80;
        this.edtCd1.Width = 80;
        this.lblTilde2.Text = "~";
        this.lblTilde2.Left = 255;
        this.lblTilde2.Top = 83;
        this.lblTilde2.AutoSize = true;
        this.edtCd2.Left = 275;
        this.edtCd2.Top = 80;
        this.edtCd2.Width = 80;
        this.btnLookup.Text = "검색";
        this.btnLookup.Left = 360;
        this.btnLookup.Top = 78;
        this.btnLookup.Width = 60;
        this.btnLookup.Click += new EventHandler(this.BtnLookup_Click);
        this.Controls.AddRange(new Control[] { this.lblCd, this.edtCd1, this.edtCd2, this.lblTilde2, this.btnLookup });
        // y = 110

        this.rdo1.Left = 170;
        this.rdo1.Top = 110;
        this.rdo1.AutoSize = true;
        this.rdo2.Left = 250;
        this.rdo2.Top = 110;
        this.rdo2.AutoSize = true;
        this.lblGu.Text = "발행구분";
        this.lblGu.Left = 20;
        this.lblGu.Top = 113;
        this.lblGu.AutoSize = true;
        this.Controls.AddRange(new Control[] { this.lblGu, this.rdo1, this.rdo2 });
        // y = 150

        this.lblHint.Text = "선택한 발행일자에 이미 자동생성된(TNO1='A') 계산서가 있으면\r\n" +
                             "해당 거래처코드 범위 내에서 삭제 후 다시 생성합니다.";
        this.lblHint.Left = 20;
        this.lblHint.Top = 150;
        this.lblHint.AutoSize = true;
        // y = 200

        this.btnYes.Left = 130;
        this.btnYes.Top = 200;
        this.btnYes.Width = 100;
        this.btnClose.Left = 250;
        this.btnClose.Top = 200;
        this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.lblHint, this.btnYes, this.btnClose });

        this.btnYes.Click += new EventHandler(this.BtnYes_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Load += new EventHandler(this.SS33AForm_Load);
        this.KeyDown += new KeyEventHandler(this.SS33AForm_KeyDown);

        this.ResumeLayout(false);
    }
}
