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

    private Label lblDate;
    private Label lblNo;
    private Label lblCvcod;
    private Label lblCvnam;
    private Label lblItnbr;
    private Label lblItdsc;
    private Label lblDanwi;
    private Label lblBcost;
    private Label lblOcost;
    private Label lblIqty;
    private Label lblIcost;
    private Label lblIamt;
    private Label lblJamt1;
    private Label lblJamt2;
    private Label lblJamt3;
    private Label lblHouse;
    private Label lblYdate;
    private Label lblOqty;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.dtpDate = new DateTimePicker();
        this.edtNo = new TextBox();
        this.edtNo.ReadOnly = true;
        this.dspCvcod = new TextBox();
        this.dspCvcod.ReadOnly = true;
        this.dspCvnam = new TextBox();
        this.dspCvnam.ReadOnly = true;
        this.chkCancel = new CheckBox();
        this.chkCancel.Text = "취소분(반품)";
        this.edtItnbr = new TextBox();
        this.dspItdsc = new TextBox();
        this.dspItdsc.ReadOnly = true;
        this.dspDanwi = new TextBox();
        this.dspDanwi.ReadOnly = true;
        this.edtIqty = new NumericUpDown();
        this.edtIqty.Maximum = 999999999;
        this.edtIqty.DecimalPlaces = 0;
        this.edtIcost = new NumericUpDown();
        this.edtIcost.Maximum = 999999999;
        this.edtIcost.DecimalPlaces = 0;
        this.edtIamt = new NumericUpDown();
        this.edtIamt.Maximum = 999999999999;
        this.edtIamt.DecimalPlaces = 0;
        this.dspBcost = new NumericUpDown();
        this.dspBcost.Maximum = 999999999;
        this.dspBcost.DecimalPlaces = 0;
        this.dspBcost.ReadOnly = true;
        this.dspOcost = new NumericUpDown();
        this.dspOcost.Maximum = 999999999;
        this.dspOcost.DecimalPlaces = 0;
        this.dspOcost.ReadOnly = true;
        this.edtJamt1 = new NumericUpDown();
        this.edtJamt1.Maximum = 999999999;
        this.edtJamt1.DecimalPlaces = 0;
        this.edtJamt2 = new NumericUpDown();
        this.edtJamt2.Maximum = 999999999;
        this.edtJamt2.DecimalPlaces = 0;
        this.edtJamt3 = new NumericUpDown();
        this.edtJamt3.Maximum = 999999999;
        this.edtJamt3.DecimalPlaces = 0;
        this.cboHouse = new ComboBox();
        this.cboHouse.DropDownStyle = ComboBoxStyle.DropDownList;
        this.mskYdate = new TextBox();
        this.edtBigo = new TextBox();
        this.dspOqty = new TextBox();
        this.dspOqty.ReadOnly = true;
        this.dspOqty.Text = "0";
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
        this.lblNo = new Label();
        this.lblCvcod = new Label();
        this.lblCvnam = new Label();
        this.lblItnbr = new Label();
        this.lblItdsc = new Label();
        this.lblDanwi = new Label();
        this.lblBcost = new Label();
        this.lblOcost = new Label();
        this.lblIqty = new Label();
        this.lblIcost = new Label();
        this.lblIamt = new Label();
        this.lblJamt1 = new Label();
        this.lblJamt2 = new Label();
        this.lblJamt3 = new Label();
        this.lblHouse = new Label();
        this.lblYdate = new Label();
        this.lblOqty = new Label();
        this.lblBigo = new Label();
        this.SuspendLayout();
        //
        // pnlJob
        //
        this.pnlJob.Left = 20; this.pnlJob.Top = 10;
        //
        // 원본 GridLayout(this, 20, 45, slotWidth:220, labelWidth:75, rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영
        //
        this.lblDate.Text = "입고일자";
        this.lblDate.Left = 20; this.lblDate.Top = 48; this.lblDate.AutoSize = true;
        this.dtpDate.Left = 95; this.dtpDate.Top = 45; this.dtpDate.Width = 133;
        this.dtpDate.Format = DateTimePickerFormat.Short;

        this.lblNo.Text = "전표번호";
        this.lblNo.Left = 240; this.lblNo.Top = 48; this.lblNo.AutoSize = true;
        this.edtNo.Left = 315; this.edtNo.Top = 45; this.edtNo.Width = 133;

        this.chkCancel.Left = 460; this.chkCancel.Top = 47; this.chkCancel.Width = 100;

        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 20; this.lblCvcod.Top = 78; this.lblCvcod.AutoSize = true;
        this.dspCvcod.Left = 95; this.dspCvcod.Top = 75; this.dspCvcod.Width = 133;

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

        this.lblBcost.Text = "기준단가";
        this.lblBcost.Left = 240; this.lblBcost.Top = 138; this.lblBcost.AutoSize = true;
        this.dspBcost.Left = 315; this.dspBcost.Top = 135; this.dspBcost.Width = 133;

        this.lblOcost.Text = "출고단가";
        this.lblOcost.Left = 460; this.lblOcost.Top = 138; this.lblOcost.AutoSize = true;
        this.dspOcost.Left = 535; this.dspOcost.Top = 135; this.dspOcost.Width = 133;

        this.lblIqty.Text = "입고수량";
        this.lblIqty.Left = 20; this.lblIqty.Top = 168; this.lblIqty.AutoSize = true;
        this.edtIqty.Left = 95; this.edtIqty.Top = 165; this.edtIqty.Width = 133;
        this.edtIqty.ValueChanged += new EventHandler(this.EdtIqty_ValueChanged);

        this.lblIcost.Text = "입고단가";
        this.lblIcost.Left = 240; this.lblIcost.Top = 168; this.lblIcost.AutoSize = true;
        this.edtIcost.Left = 315; this.edtIcost.Top = 165; this.edtIcost.Width = 133;
        this.edtIcost.ValueChanged += new EventHandler(this.EdtIcost_ValueChanged);

        this.lblIamt.Text = "입고금액";
        this.lblIamt.Left = 460; this.lblIamt.Top = 168; this.lblIamt.AutoSize = true;
        this.edtIamt.Left = 535; this.edtIamt.Top = 165; this.edtIamt.Width = 133;

        this.lblJamt1.Text = "부가세1";
        this.lblJamt1.Left = 20; this.lblJamt1.Top = 198; this.lblJamt1.AutoSize = true;
        this.edtJamt1.Left = 95; this.edtJamt1.Top = 195; this.edtJamt1.Width = 133;

        this.lblJamt2.Text = "부가세2";
        this.lblJamt2.Left = 240; this.lblJamt2.Top = 198; this.lblJamt2.AutoSize = true;
        this.edtJamt2.Left = 315; this.edtJamt2.Top = 195; this.edtJamt2.Width = 133;

        this.lblJamt3.Text = "부가세3";
        this.lblJamt3.Left = 460; this.lblJamt3.Top = 198; this.lblJamt3.AutoSize = true;
        this.edtJamt3.Left = 535; this.edtJamt3.Top = 195; this.edtJamt3.Width = 133;

        this.lblHouse.Text = "저장위치";
        this.lblHouse.Left = 20; this.lblHouse.Top = 228; this.lblHouse.AutoSize = true;
        this.cboHouse.Left = 95; this.cboHouse.Top = 225; this.cboHouse.Width = 133;

        this.lblYdate.Text = "만기일";
        this.lblYdate.Left = 240; this.lblYdate.Top = 228; this.lblYdate.AutoSize = true;
        this.mskYdate.Left = 315; this.mskYdate.Top = 225; this.mskYdate.Width = 133;

        this.lblOqty.Text = "출고중수량";
        this.lblOqty.Left = 460; this.lblOqty.Top = 228; this.lblOqty.AutoSize = true;
        this.dspOqty.Left = 535; this.dspOqty.Top = 225; this.dspOqty.Width = 133;

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 20; this.lblBigo.Top = 258; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 95; this.edtBigo.Top = 255; this.edtBigo.Width = 573;
        //
        // btnAdd / btnOne / btnClose (원본 y = layout.Bottom(20) = 305 기준)
        //
        this.btnAdd.Left = 150; this.btnAdd.Top = 305; this.btnAdd.Width = 120;
        this.btnOne.Left = 280; this.btnOne.Top = 305; this.btnOne.Width = 100;
        this.btnClose.Left = 390; this.btnClose.Top = 305; this.btnClose.Width = 100;
        this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
        this.btnOne.Click += new EventHandler(this.BtnOne_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Controls.AddRange(new Control[]
        {
            this.pnlJob,
            this.lblDate, this.dtpDate,
            this.lblNo, this.edtNo,
            this.chkCancel,
            this.lblCvcod, this.dspCvcod,
            this.lblCvnam, this.dspCvnam,
            this.lblItnbr, this.edtItnbr,
            this.lblItdsc, this.dspItdsc,
            this.lblDanwi, this.dspDanwi,
            this.lblBcost, this.dspBcost,
            this.lblOcost, this.dspOcost,
            this.lblIqty, this.edtIqty,
            this.lblIcost, this.edtIcost,
            this.lblIamt, this.edtIamt,
            this.lblJamt1, this.edtJamt1,
            this.lblJamt2, this.edtJamt2,
            this.lblJamt3, this.edtJamt3,
            this.lblHouse, this.cboHouse,
            this.lblYdate, this.mskYdate,
            this.lblOqty, this.dspOqty,
            this.lblBigo, this.edtBigo,
            this.btnAdd, this.btnOne, this.btnClose
        });
        //
        // SS21AForm
        //
        this.Text = "입고 등록";
        this.Width = 720;
        this.Height = 560;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.ClientSize = new Size(this.ClientSize.Width, 350);

        this.Load += new EventHandler(this.SS21AForm_Load);
        this.KeyDown += new KeyEventHandler(this.SS21AForm_KeyDown);
        this.ResumeLayout(false);
    }
}
