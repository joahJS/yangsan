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
        // SS32AForm
        //
        this.Text = "수금 등록";
        this.Width = 500;
        this.Height = 350;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout()
        //
        PublicLib.MakeTypingFriendly(this.edtAmt);

        this.lblJob.Left = 20;
        this.lblJob.Top = 10;
        this.Controls.Add(this.lblJob);

        var grid = new GridLayout(this, 20, 45, slotWidth: 220, labelWidth: 85, rowHeight: 30, slotsPerRow: 2);

        grid.Add("수금일자", this.dtpDate);
        this.dtpDate.Format = DateTimePickerFormat.Short;
        grid.Add("전표번호", this.edtNo);

        grid.Add("거래처코드", this.edtCode);
        this._tip.SetToolTip(this.edtCode, "Enter 키를 누르면 거래처를 검색합니다.");
        this.edtCode.KeyDown += new KeyEventHandler(this.EdtCode_KeyDown);
        grid.Add("거래처명", this.dspName);

        grid.Add("수금구분", this.cboGu);
        grid.Add("수금액", this.edtAmt);

        grid.NewRow();
        grid.Add("비고", this.edtBigo, span: 2);

        int y = grid.Bottom(10);
        this.lblMisuCap.Text = "현재 미수잔액:";
        this.lblMisuCap.Left = 20;
        this.lblMisuCap.Top = y + 3;
        this.lblMisuCap.AutoSize = true;
        this.lblMisu.Left = 130;
        this.lblMisu.Top = y + 3;
        this.Controls.AddRange(new Control[] { this.lblMisuCap, this.lblMisu });
        y += 30;

        this.btnAdd.Left = 100; this.btnAdd.Top = y; this.btnAdd.Width = 120;
        this.btnOne.Left = 230; this.btnOne.Top = y; this.btnOne.Width = 100;
        this.btnClose.Left = 340; this.btnClose.Top = y; this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.btnAdd, this.btnOne, this.btnClose });

        this.btnAdd.Click += (_, _) => { if (SaveEntry()) { Saved = true; ClearForm(); this.edtCode.Focus(); } };
        this.btnOne.Click += (_, _) => { if (SaveEntry()) { Saved = true; Close(); } };
        this.btnClose.Click += (_, _) => Close();

        this.ClientSize = new Size(this.ClientSize.Width, y + 45);
        ClearForm();

        this.Load += (_, _) => { if (this.cboGu.Items.Count == 0) ResetGuList(); };
        this.KeyDown += new KeyEventHandler(this.SS32AForm_KeyDown);

        ((System.ComponentModel.ISupportInitialize)(this.edtAmt)).EndInit();
        this.ResumeLayout(false);
    }
}
