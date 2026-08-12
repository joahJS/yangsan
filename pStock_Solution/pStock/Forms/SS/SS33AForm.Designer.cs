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
        // BuildLayout() (원본 AddRow 헬퍼 호출 2회 포함 인라인)
        //
        int y = 20, col1 = 20;

        // AddRow("집계기간", dtpDate1, col1, ref y, 130)
        this.lblDate1.Text = "집계기간";
        this.lblDate1.Left = col1;
        this.lblDate1.Top = y + 3;
        this.lblDate1.AutoSize = true;
        this.dtpDate1.Left = col1 + 150;
        this.dtpDate1.Top = y;
        this.dtpDate1.Width = 130;
        this.dtpDate1.Format = DateTimePickerFormat.Short;
        this.Controls.Add(this.lblDate1);
        this.Controls.Add(this.dtpDate1);
        y += 30;

        this.lblTilde1.Text = "~";
        this.lblTilde1.Left = col1 + 300;
        this.lblTilde1.Top = y - 26 + 3;
        this.lblTilde1.AutoSize = true;
        this.dtpDate2.Left = col1 + 320;
        this.dtpDate2.Top = y - 26;
        this.dtpDate2.Width = 130;
        this.dtpDate2.Format = DateTimePickerFormat.Short;
        this.Controls.AddRange(new Control[] { this.lblTilde1, this.dtpDate2 });

        // AddRow("발행일자", dtpDate, col1, ref y, 130)
        this.lblDate.Text = "발행일자";
        this.lblDate.Left = col1;
        this.lblDate.Top = y + 3;
        this.lblDate.AutoSize = true;
        this.dtpDate.Left = col1 + 150;
        this.dtpDate.Top = y;
        this.dtpDate.Width = 130;
        this.dtpDate.Format = DateTimePickerFormat.Short;
        this.Controls.Add(this.lblDate);
        this.Controls.Add(this.dtpDate);
        y += 30;

        this.lblCd.Text = "거래처코드 범위";
        this.lblCd.Left = col1;
        this.lblCd.Top = y + 3;
        this.lblCd.AutoSize = true;
        this.edtCd1.Left = col1 + 150;
        this.edtCd1.Top = y;
        this.edtCd1.Width = 80;
        this.lblTilde2.Text = "~";
        this.lblTilde2.Left = col1 + 235;
        this.lblTilde2.Top = y + 3;
        this.lblTilde2.AutoSize = true;
        this.edtCd2.Left = col1 + 255;
        this.edtCd2.Top = y;
        this.edtCd2.Width = 80;
        this.btnLookup.Text = "검색";
        this.btnLookup.Left = col1 + 340;
        this.btnLookup.Top = y - 2;
        this.btnLookup.Width = 60;
        this.btnLookup.Click += (_, _) => LookupCvcod();
        this.Controls.AddRange(new Control[] { this.lblCd, this.edtCd1, this.edtCd2, this.lblTilde2, this.btnLookup });
        y += 30;

        this.rdo1.Left = col1 + 150;
        this.rdo1.Top = y;
        this.rdo1.AutoSize = true;
        this.rdo2.Left = col1 + 230;
        this.rdo2.Top = y;
        this.rdo2.AutoSize = true;
        this.lblGu.Text = "발행구분";
        this.lblGu.Left = col1;
        this.lblGu.Top = y + 3;
        this.lblGu.AutoSize = true;
        this.Controls.AddRange(new Control[] { this.lblGu, this.rdo1, this.rdo2 });
        y += 40;

        this.lblHint.Text = "선택한 발행일자에 이미 자동생성된(TNO1='A') 계산서가 있으면\r\n" +
                             "해당 거래처코드 범위 내에서 삭제 후 다시 생성합니다.";
        this.lblHint.Left = col1;
        this.lblHint.Top = y;
        this.lblHint.AutoSize = true;
        y += 50;

        this.btnYes.Left = 130;
        this.btnYes.Top = y;
        this.btnYes.Width = 100;
        this.btnClose.Left = 250;
        this.btnClose.Top = y;
        this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.lblHint, this.btnYes, this.btnClose });

        this.btnYes.Click += (_, _) => RunBatch();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => ClearForm();
        this.KeyDown += new KeyEventHandler(this.SS33AForm_KeyDown);

        this.ResumeLayout(false);
    }
}
