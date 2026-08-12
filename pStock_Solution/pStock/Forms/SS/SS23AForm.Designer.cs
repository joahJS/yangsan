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

    private Label lblDate;
    private Label lblNoCap;
    private Label lblCvcod;
    private Label lblCvnam;
    private Label lblItnbr;
    private Label lblItdsc;
    private Label lblDanwi;
    private Label lblOqty;
    private Label lblOcost;
    private Label lblOamt;
    private Label lblJamt1;
    private Label lblJamt2;
    private Label lblJamt3;
    private Label lblTamt;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.dtpDate = new DateTimePicker();
        this.edtNo = new TextBox();
        this.edtNo.ReadOnly = true;
        this.lblNo = new Label();
        this.lblNo.AutoSize = true;
        this.dspCvcod = new TextBox();
        this.dspCvcod.ReadOnly = true;
        this.dspCvnam = new TextBox();
        this.dspCvnam.ReadOnly = true;
        this.edtItnbr = new TextBox();
        this.dspItdsc = new TextBox();
        this.dspItdsc.ReadOnly = true;
        this.dspDanwi = new TextBox();
        this.dspDanwi.ReadOnly = true;
        this.edtOqty = new NumericUpDown();
        this.edtOqty.Maximum = 999999999;
        this.edtOqty.DecimalPlaces = 0;
        this.edtOcost = new NumericUpDown();
        this.edtOcost.Maximum = 999999999;
        this.edtOcost.DecimalPlaces = 0;
        this.edtOamt = new NumericUpDown();
        this.edtOamt.Maximum = 999999999999;
        this.edtOamt.DecimalPlaces = 0;
        this.edtJamt1 = new NumericUpDown();
        this.edtJamt1.Maximum = 999999999;
        this.edtJamt1.DecimalPlaces = 0;
        this.edtJamt2 = new NumericUpDown();
        this.edtJamt2.Maximum = 999999999;
        this.edtJamt2.DecimalPlaces = 0;
        this.edtJamt3 = new NumericUpDown();
        this.edtJamt3.Maximum = 999999999;
        this.edtJamt3.DecimalPlaces = 0;
        this.dspTamt = new NumericUpDown();
        this.dspTamt.Maximum = 999999999999;
        this.dspTamt.DecimalPlaces = 0;
        this.dspTamt.ReadOnly = true;
        this.edtBigo = new TextBox();
        this.pnlJob = new Label();
        this.pnlJob.AutoSize = true;
        this.pnlJob.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
        this.btnAdd = new Button();
        this.btnAdd.Text = "연속저장(F2)";
        this.btnOne = new Button();
        this.btnOne.Text = "저장(F3)";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";
        this._tip = new ToolTip();
        this.lblDate = new Label();
        this.lblNoCap = new Label();
        this.lblCvcod = new Label();
        this.lblCvnam = new Label();
        this.lblItnbr = new Label();
        this.lblItdsc = new Label();
        this.lblDanwi = new Label();
        this.lblOqty = new Label();
        this.lblOcost = new Label();
        this.lblOamt = new Label();
        this.lblJamt1 = new Label();
        this.lblJamt2 = new Label();
        this.lblJamt3 = new Label();
        this.lblTamt = new Label();
        this.lblBigo = new Label();
        this.SuspendLayout();
        //
        // pnlJob
        //
        this.pnlJob.Left = 20; this.pnlJob.Top = 10;
        //
        // 원본 GridLayout(this, 20, 45, slotWidth:220, labelWidth:75, rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영
        //
        this.lblDate.Text = "산정일자";
        this.lblDate.Left = 20; this.lblDate.Top = 48; this.lblDate.AutoSize = true;
        this.dtpDate.Left = 95; this.dtpDate.Top = 45; this.dtpDate.Width = 133;
        this.dtpDate.Format = DateTimePickerFormat.Short;
        this.dtpDate.ValueChanged += new EventHandler(this.DtpDate_ValueChanged);

        this.lblNo.Left = 240; this.lblNo.Top = 47;

        this.lblNoCap.Text = "전표번호";
        this.lblNoCap.Left = 460; this.lblNoCap.Top = 48; this.lblNoCap.AutoSize = true;
        this.edtNo.Left = 535; this.edtNo.Top = 45; this.edtNo.Width = 133;

        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 20; this.lblCvcod.Top = 78; this.lblCvcod.AutoSize = true;
        this.dspCvcod.Left = 95; this.dspCvcod.Top = 75; this.dspCvcod.Width = 133;
        this._tip.SetToolTip(this.dspCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.dspCvcod.KeyDown += new KeyEventHandler(this.DspCvcod_KeyDown);

        this.lblCvnam.Text = "거래처명";
        this.lblCvnam.Left = 240; this.lblCvnam.Top = 78; this.lblCvnam.AutoSize = true;
        this.dspCvnam.Left = 315; this.dspCvnam.Top = 75; this.dspCvnam.Width = 353;

        this.lblItnbr.Text = "품번";
        this.lblItnbr.Left = 20; this.lblItnbr.Top = 108; this.lblItnbr.AutoSize = true;
        this.edtItnbr.Left = 95; this.edtItnbr.Top = 105; this.edtItnbr.Width = 133;
        this._tip.SetToolTip(this.edtItnbr, "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItnbr.KeyDown += new KeyEventHandler(this.EdtItnbr_KeyDown);

        this.lblItdsc.Text = "품명";
        this.lblItdsc.Left = 240; this.lblItdsc.Top = 108; this.lblItdsc.AutoSize = true;
        this.dspItdsc.Left = 315; this.dspItdsc.Top = 105; this.dspItdsc.Width = 353;

        this.lblDanwi.Text = "단위";
        this.lblDanwi.Left = 20; this.lblDanwi.Top = 138; this.lblDanwi.AutoSize = true;
        this.dspDanwi.Left = 95; this.dspDanwi.Top = 135; this.dspDanwi.Width = 133;

        this.lblOqty.Text = "보관수량";
        this.lblOqty.Left = 240; this.lblOqty.Top = 138; this.lblOqty.AutoSize = true;
        this.edtOqty.Left = 315; this.edtOqty.Top = 135; this.edtOqty.Width = 133;
        this.edtOqty.ValueChanged += new EventHandler(this.EdtOqty_ValueChanged);

        this.lblOcost.Text = "보관단가";
        this.lblOcost.Left = 460; this.lblOcost.Top = 138; this.lblOcost.AutoSize = true;
        this.edtOcost.Left = 535; this.edtOcost.Top = 135; this.edtOcost.Width = 133;
        this.edtOcost.ValueChanged += new EventHandler(this.EdtOcost_ValueChanged);

        this.lblOamt.Text = "보관금액";
        this.lblOamt.Left = 20; this.lblOamt.Top = 168; this.lblOamt.AutoSize = true;
        this.edtOamt.Left = 95; this.edtOamt.Top = 165; this.edtOamt.Width = 133;

        this.lblJamt1.Text = "부가세1";
        this.lblJamt1.Left = 240; this.lblJamt1.Top = 168; this.lblJamt1.AutoSize = true;
        this.edtJamt1.Left = 315; this.edtJamt1.Top = 165; this.edtJamt1.Width = 133;
        this.edtJamt1.ValueChanged += new EventHandler(this.EdtJamt1_ValueChanged);

        this.lblJamt2.Text = "부가세2";
        this.lblJamt2.Left = 460; this.lblJamt2.Top = 168; this.lblJamt2.AutoSize = true;
        this.edtJamt2.Left = 535; this.edtJamt2.Top = 165; this.edtJamt2.Width = 133;
        this.edtJamt2.ValueChanged += new EventHandler(this.EdtJamt2_ValueChanged);

        this.lblJamt3.Text = "부가세3";
        this.lblJamt3.Left = 20; this.lblJamt3.Top = 198; this.lblJamt3.AutoSize = true;
        this.edtJamt3.Left = 95; this.edtJamt3.Top = 195; this.edtJamt3.Width = 133;
        this.edtJamt3.ValueChanged += new EventHandler(this.EdtJamt3_ValueChanged);

        this.lblTamt.Text = "합계금액";
        this.lblTamt.Left = 240; this.lblTamt.Top = 198; this.lblTamt.AutoSize = true;
        this.dspTamt.Left = 315; this.dspTamt.Top = 195; this.dspTamt.Width = 133;

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 20; this.lblBigo.Top = 228; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 95; this.edtBigo.Top = 225; this.edtBigo.Width = 573;
        //
        // btnAdd / btnOne / btnClose (원본 y = layout.Bottom(20) = 275 기준)
        //
        this.btnAdd.Left = 150; this.btnAdd.Top = 275; this.btnAdd.Width = 120;
        this.btnOne.Left = 280; this.btnOne.Top = 275; this.btnOne.Width = 100;
        this.btnClose.Left = 390; this.btnClose.Top = 275; this.btnClose.Width = 100;
        this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
        this.btnOne.Click += new EventHandler(this.BtnOne_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Controls.AddRange(new Control[]
        {
            this.pnlJob,
            this.lblDate, this.dtpDate,
            this.lblNo,
            this.lblNoCap, this.edtNo,
            this.lblCvcod, this.dspCvcod,
            this.lblCvnam, this.dspCvnam,
            this.lblItnbr, this.edtItnbr,
            this.lblItdsc, this.dspItdsc,
            this.lblDanwi, this.dspDanwi,
            this.lblOqty, this.edtOqty,
            this.lblOcost, this.edtOcost,
            this.lblOamt, this.edtOamt,
            this.lblJamt1, this.edtJamt1,
            this.lblJamt2, this.edtJamt2,
            this.lblJamt3, this.edtJamt3,
            this.lblTamt, this.dspTamt,
            this.lblBigo, this.edtBigo,
            this.btnAdd, this.btnOne, this.btnClose
        });
        //
        // SS23AForm
        //
        this.Text = "보관료 등록";
        this.Width = 720;
        this.Height = 480;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.ClientSize = new Size(this.ClientSize.Width, 320);

        this.KeyDown += new KeyEventHandler(this.SS23AForm_KeyDown);
        this.ResumeLayout(false);
    }
}
