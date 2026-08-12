using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

partial class SS32AForm
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
    private TextBox edtNo;
    private TextBox edtCode;
    private TextBox dspName;
    private ComboBox cboGu;
    private NumericUpDown edtAmt;
    private TextBox edtBigo;
    private Label lblMisu;
    private Label lblJob;
    private Button btnAdd;
    private Button btnOne;
    private Button btnClose;
    private ToolTip _tip;
    private Label lblMisuCap;
    private Label lblDate;
    private Label lblNo;
    private Label lblCode;
    private Label lblName;
    private Label lblGu;
    private Label lblAmt;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.dtpDate = new DateTimePicker();
        this.edtNo = new TextBox();
        this.edtCode = new TextBox();
        this.dspName = new TextBox();
        this.cboGu = new ComboBox();
        this.edtAmt = new NumericUpDown();
        this.edtBigo = new TextBox();
        this.lblMisu = new Label();
        this.lblJob = new Label();
        this.btnAdd = new Button();
        this.btnOne = new Button();
        this.btnClose = new Button();
        this._tip = new ToolTip();
        this.lblMisuCap = new Label();
        this.lblDate = new Label();
        this.lblNo = new Label();
        this.lblCode = new Label();
        this.lblName = new Label();
        this.lblGu = new Label();
        this.lblAmt = new Label();
        this.lblBigo = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.edtAmt)).BeginInit();
        this.SuspendLayout();
        //
        // 원본 필드 초기값
        //
        this.edtNo.ReadOnly = true;
        this.dspName.ReadOnly = true;
        this.cboGu.DropDownStyle = ComboBoxStyle.DropDownList;
        this.edtAmt.Maximum = 999999999999;
        this.edtAmt.DecimalPlaces = 0;
        this.lblMisu.AutoSize = true;
        this.lblJob.AutoSize = true;
        this.lblJob.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
        this.btnAdd.Text = "연속저장(F2)";
        this.btnOne.Text = "저장(F3)";
        this.btnClose.Text = "닫기(Esc)";
        //
        // lblJob
        //
        this.lblJob.Left = 20;
        this.lblJob.Top = 10;
        //
        // 원본 GridLayout(this, 20, 45, slotWidth:220, labelWidth:85, rowHeight:30, slotsPerRow:2) 계산 결과를 리터럴로 반영
        //
        this.lblDate.Text = "수금일자";
        this.lblDate.Left = 20; this.lblDate.Top = 48; this.lblDate.AutoSize = true;
        this.dtpDate.Left = 105; this.dtpDate.Top = 45; this.dtpDate.Width = 123;
        this.dtpDate.Format = DateTimePickerFormat.Short;

        this.lblNo.Text = "전표번호";
        this.lblNo.Left = 240; this.lblNo.Top = 48; this.lblNo.AutoSize = true;
        this.edtNo.Left = 325; this.edtNo.Top = 45; this.edtNo.Width = 123;

        this.lblCode.Text = "거래처코드";
        this.lblCode.Left = 20; this.lblCode.Top = 78; this.lblCode.AutoSize = true;
        this.edtCode.Left = 105; this.edtCode.Top = 75; this.edtCode.Width = 123;
        this._tip.SetToolTip(this.edtCode, "Enter 키를 누르면 거래처를 검색합니다.");
        this.edtCode.KeyDown += new KeyEventHandler(this.EdtCode_KeyDown);

        this.lblName.Text = "거래처명";
        this.lblName.Left = 240; this.lblName.Top = 78; this.lblName.AutoSize = true;
        this.dspName.Left = 325; this.dspName.Top = 75; this.dspName.Width = 123;

        this.lblGu.Text = "수금구분";
        this.lblGu.Left = 20; this.lblGu.Top = 108; this.lblGu.AutoSize = true;
        this.cboGu.Left = 105; this.cboGu.Top = 105; this.cboGu.Width = 123;

        this.lblAmt.Text = "수금액";
        this.lblAmt.Left = 240; this.lblAmt.Top = 108; this.lblAmt.AutoSize = true;
        this.edtAmt.Left = 325; this.edtAmt.Top = 105; this.edtAmt.Width = 123;

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 20; this.lblBigo.Top = 138; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 105; this.edtBigo.Top = 135; this.edtBigo.Width = 343;
        //
        // lblMisuCap / lblMisu (원본 y = grid.Bottom(10) = 175 기준)
        //
        this.lblMisuCap.Text = "현재 미수잔액:";
        this.lblMisuCap.Left = 20; this.lblMisuCap.Top = 178; this.lblMisuCap.AutoSize = true;
        this.lblMisu.Left = 130; this.lblMisu.Top = 178;
        //
        // btnAdd / btnOne / btnClose (원본 y += 30 = 205 기준)
        //
        this.btnAdd.Left = 100; this.btnAdd.Top = 205; this.btnAdd.Width = 120;
        this.btnOne.Left = 230; this.btnOne.Top = 205; this.btnOne.Width = 100;
        this.btnClose.Left = 340; this.btnClose.Top = 205; this.btnClose.Width = 100;
        this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
        this.btnOne.Click += new EventHandler(this.BtnOne_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Controls.AddRange(new Control[]
        {
            this.lblJob,
            this.lblDate, this.dtpDate,
            this.lblNo, this.edtNo,
            this.lblCode, this.edtCode,
            this.lblName, this.dspName,
            this.lblGu, this.cboGu,
            this.lblAmt, this.edtAmt,
            this.lblBigo, this.edtBigo,
            this.lblMisuCap, this.lblMisu,
            this.btnAdd, this.btnOne, this.btnClose
        });
        //
        // SS32AForm
        //
        this.Text = "수금 등록";
        this.Width = 500;
        this.Height = 350;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.ClientSize = new Size(this.ClientSize.Width, 250);

        this.Load += new EventHandler(this.SS32AForm_Load);
        this.KeyDown += new KeyEventHandler(this.SS32AForm_KeyDown);

        ((System.ComponentModel.ISupportInitialize)(this.edtAmt)).EndInit();
        this.ResumeLayout(false);
    }
}
