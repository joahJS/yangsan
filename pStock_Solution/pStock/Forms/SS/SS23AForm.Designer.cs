using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

partial class SS23AForm
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
    private Label lblNo;
    private TextBox dspCvcod;
    private TextBox dspCvnam;
    private TextBox edtItnbr;
    private TextBox dspItdsc;
    private TextBox dspDanwi;
    private NumericUpDown edtOqty;
    private NumericUpDown edtOcost;
    private NumericUpDown edtOamt;
    private NumericUpDown edtJamt1;
    private NumericUpDown edtJamt2;
    private NumericUpDown edtJamt3;
    private NumericUpDown dspTamt;
    private TextBox edtBigo;
    private Label pnlJob;

    private Button btnAdd;
    private Button btnOne;
    private Button btnClose;

    private ToolTip _tip;

    private void InitializeComponent()
    {
        this.dtpDate = new DateTimePicker();
        this.edtNo = new TextBox { ReadOnly = true };
        this.lblNo = new Label { AutoSize = true };
        this.dspCvcod = new TextBox { ReadOnly = true };
        this.dspCvnam = new TextBox { ReadOnly = true };
        this.edtItnbr = new TextBox();
        this.dspItdsc = new TextBox { ReadOnly = true };
        this.dspDanwi = new TextBox { ReadOnly = true };
        this.edtOqty = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtOcost = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtOamt = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        this.edtJamt1 = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtJamt2 = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtJamt3 = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.dspTamt = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0, ReadOnly = true };
        this.edtBigo = new TextBox();
        this.pnlJob = new Label { AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) };
        this.btnAdd = new Button { Text = "연속저장(F2)" };
        this.btnOne = new Button { Text = "저장(F3)" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this._tip = new ToolTip();
        this.SuspendLayout();
        //
        // SS23AForm (원본 생성자 프롤로그 — ClientSize 계산이 Width에 의존하므로
        // 반드시 아래 BuildLayout 이식 코드보다 먼저 설정해야 한다)
        //
        this.Text = "보관료 등록";
        this.Width = 720;
        this.Height = 480;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        PublicLib.MakeTypingFriendly(this.edtOqty);
        PublicLib.MakeTypingFriendly(this.edtOcost);
        PublicLib.MakeTypingFriendly(this.edtOamt);
        PublicLib.MakeTypingFriendly(this.edtJamt1);
        PublicLib.MakeTypingFriendly(this.edtJamt2);
        PublicLib.MakeTypingFriendly(this.edtJamt3);

        this.pnlJob.Left = 20; this.pnlJob.Top = 10;
        this.Controls.Add(this.pnlJob);

        var layout = new GridLayout(this, 20, 45, slotWidth: 220, labelWidth: 75, rowHeight: 30, slotsPerRow: 3);

        layout.Add("산정일자", this.dtpDate);
        this.dtpDate.Format = DateTimePickerFormat.Short;
        this.dtpDate.ValueChanged += (_, _) => this.lblNo.Text = TermLabel(this.dtpDate.Value);
        layout.AddRaw(this.lblNo);
        layout.Add("전표번호", this.edtNo);

        layout.Add("거래처코드", this.dspCvcod);
        this._tip.SetToolTip(this.dspCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.dspCvcod.KeyDown += new KeyEventHandler(this.DspCvcod_KeyDown);
        layout.Add("거래처명", this.dspCvnam, span: 2);

        layout.Add("품번", this.edtItnbr);
        this._tip.SetToolTip(this.edtItnbr, "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItnbr.KeyDown += new KeyEventHandler(this.EdtItnbr_KeyDown);
        layout.Add("품명", this.dspItdsc, span: 2);

        layout.Add("단위", this.dspDanwi);
        layout.Add("보관수량", this.edtOqty);
        this.edtOqty.ValueChanged += (_, _) => RecalcAmt();
        layout.Add("보관단가", this.edtOcost);
        this.edtOcost.ValueChanged += (_, _) => RecalcAmt();

        layout.Add("보관금액", this.edtOamt);
        layout.Add("부가세1", this.edtJamt1);
        this.edtJamt1.ValueChanged += (_, _) => RecalcTotal();
        layout.Add("부가세2", this.edtJamt2);
        this.edtJamt2.ValueChanged += (_, _) => RecalcTotal();

        layout.Add("부가세3", this.edtJamt3);
        this.edtJamt3.ValueChanged += (_, _) => RecalcTotal();
        layout.Add("합계금액", this.dspTamt);

        layout.NewRow();
        layout.Add("비고", this.edtBigo, span: 3);

        int y = layout.Bottom(20);
        this.btnAdd.Left = 150; this.btnAdd.Top = y; this.btnAdd.Width = 120;
        this.btnOne.Left = 280; this.btnOne.Top = y; this.btnOne.Width = 100;
        this.btnClose.Left = 390; this.btnClose.Top = y; this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.btnAdd, this.btnOne, this.btnClose });

        this.btnAdd.Click += (_, _) => { if (SaveEntry()) { Saved = true; ClearForm(); this.edtItnbr.Focus(); } };
        this.btnOne.Click += (_, _) => { if (SaveEntry()) { Saved = true; Close(); } };
        this.btnClose.Click += (_, _) => Close();

        this.ClientSize = new Size(this.ClientSize.Width, y + 45);

        this.KeyDown += new KeyEventHandler(this.SS23AForm_KeyDown);
        this.ResumeLayout(false);
    }
}
