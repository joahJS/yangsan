using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

partial class SS21AForm
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
    private TextBox dspCvcod;
    private TextBox dspCvnam;
    private CheckBox chkCancel;
    private TextBox edtItnbr;
    private TextBox dspItdsc;
    private TextBox dspDanwi;
    private NumericUpDown edtIqty;
    private NumericUpDown edtIcost;
    private NumericUpDown edtIamt;
    private NumericUpDown dspBcost;
    private NumericUpDown dspOcost;
    private NumericUpDown edtJamt1;
    private NumericUpDown edtJamt2;
    private NumericUpDown edtJamt3;
    private ComboBox cboHouse;
    private TextBox mskYdate;
    private TextBox edtBigo;
    private TextBox dspOqty;
    private Label pnlJob;

    private Button btnAdd;
    private Button btnOne;
    private Button btnClose;

    private ToolTip _tip;

    private void InitializeComponent()
    {
        this.dtpDate = new DateTimePicker();
        this.edtNo = new TextBox { ReadOnly = true };
        this.dspCvcod = new TextBox { ReadOnly = true };
        this.dspCvnam = new TextBox { ReadOnly = true };
        this.chkCancel = new CheckBox { Text = "취소분(반품)" };
        this.edtItnbr = new TextBox();
        this.dspItdsc = new TextBox { ReadOnly = true };
        this.dspDanwi = new TextBox { ReadOnly = true };
        this.edtIqty = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtIcost = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtIamt = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        this.dspBcost = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0, ReadOnly = true };
        this.dspOcost = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0, ReadOnly = true };
        this.edtJamt1 = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtJamt2 = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtJamt3 = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.cboHouse = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        this.mskYdate = new TextBox();
        this.edtBigo = new TextBox();
        this.dspOqty = new TextBox { ReadOnly = true, Text = "0" };
        this.pnlJob = new Label { AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) };
        this.btnAdd = new Button { Text = "연속저장(F2)" };
        this.btnOne = new Button { Text = "저장(F3)" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this._tip = new ToolTip();
        this.SuspendLayout();
        //
        // SS21AForm (원본 생성자 프롤로그 — ClientSize 계산이 Width에 의존하므로
        // 반드시 아래 BuildLayout 이식 코드보다 먼저 설정해야 한다)
        //
        this.Text = "입고 등록";
        this.Width = 720;
        this.Height = 560;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        //
        // BuildLayout (원본 그대로 이동)
        //
        foreach (var n in new[] { this.edtIqty, this.edtIcost, this.edtIamt, this.edtJamt1, this.edtJamt2, this.edtJamt3 })
            PublicLib.MakeTypingFriendly(n);

        this.pnlJob.Left = 20; this.pnlJob.Top = 10;
        this.Controls.Add(this.pnlJob);

        var layout = new GridLayout(this, 20, 45, slotWidth: 220, labelWidth: 75, rowHeight: 30, slotsPerRow: 3);

        layout.Add("입고일자", this.dtpDate);
        this.dtpDate.Format = DateTimePickerFormat.Short;
        layout.Add("전표번호", this.edtNo);
        layout.AddRaw(this.chkCancel, width: 100);

        layout.Add("거래처코드", this.dspCvcod);
        layout.Add("거래처명", this.dspCvnam, span: 2);

        layout.Add("품번", this.edtItnbr);
        this._tip.SetToolTip(this.edtItnbr, "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItnbr.KeyDown += new KeyEventHandler(this.EdtItnbr_KeyDown);
        layout.Add("품명", this.dspItdsc, span: 2);

        layout.Add("단위", this.dspDanwi);
        layout.Add("기준단가", this.dspBcost);
        layout.Add("출고단가", this.dspOcost);

        layout.Add("입고수량", this.edtIqty);
        this.edtIqty.ValueChanged += (_, _) => RecalcAmt();
        layout.Add("입고단가", this.edtIcost);
        this.edtIcost.ValueChanged += (_, _) => RecalcAmt();
        layout.Add("입고금액", this.edtIamt);

        layout.Add("부가세1", this.edtJamt1);
        layout.Add("부가세2", this.edtJamt2);
        layout.Add("부가세3", this.edtJamt3);

        layout.Add("저장위치", this.cboHouse);
        layout.Add("만기일", this.mskYdate);
        layout.Add("출고중수량", this.dspOqty);

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
        ClearForm();

        this.Load += (_, _) => { if (this.cboHouse.Items.Count == 0) ResetHouseList(); };
        this.KeyDown += new KeyEventHandler(this.SS21AForm_KeyDown);
        this.ResumeLayout(false);
    }
}
