using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

partial class SS33BForm
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

    private TextBox edtTDate;
    private TextBox edtNo;
    private TextBox edtCvcod;
    private TextBox dspCvnam;
    private RadioButton rdo1;
    private RadioButton rdo2;
    private TextBox edtBigo;
    private Label lblAmt;
    private Label lblVat;

    private TextBox[] edtMmdd;
    private TextBox[] edtItnbr;
    private TextBox[] edtItdsc;
    private NumericUpDown[] edtQty;
    private NumericUpDown[] edtCost;
    private NumericUpDown[] edtAmt;

    private Button btnAdd;
    private Button btnOne;
    private Button btnClose;

    private ToolTip _tip;
    private Label[] lblHdr;
    private Label lblAmtCap;
    private Label lblVatCap;

    // 원본 배열(edtMmdd[4] 등)은 디자이너가 InitializeComponent() 안에서 인덱서 대입/반복문을
    // 처리하지 못하므로, 아래 4개씩(0~3) 개별 필드로 선언하고 리터럴 값으로 초기화한 뒤
    // 생성자(SS33BForm.cs)에서 배열로 묶는다. 동작은 100% 동일하다.
    private TextBox edtMmdd0, edtMmdd1, edtMmdd2, edtMmdd3;
    private TextBox edtItnbr0, edtItnbr1, edtItnbr2, edtItnbr3;
    private TextBox edtItdsc0, edtItdsc1, edtItdsc2, edtItdsc3;
    private NumericUpDown edtQty0, edtQty1, edtQty2, edtQty3;
    private NumericUpDown edtCost0, edtCost1, edtCost2, edtCost3;
    private NumericUpDown edtAmt0, edtAmt1, edtAmt2, edtAmt3;
    private Label lblHdr0, lblHdr1, lblHdr2, lblHdr3, lblHdr4, lblHdr5;
    private Label lblTDate;
    private Label lblNoCap;
    private Label lblCvcod;
    private Label lblCvnam;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.edtTDate = new TextBox();
        this.edtNo = new TextBox();
        this.edtNo.ReadOnly = true;
        this.edtCvcod = new TextBox();
        this.dspCvnam = new TextBox();
        this.dspCvnam.ReadOnly = true;
        this.rdo1 = new RadioButton();
        this.rdo1.Text = "영수";
        this.rdo2 = new RadioButton();
        this.rdo2.Text = "청구";
        this.rdo2.Checked = true;
        this.edtBigo = new TextBox();
        this.lblAmt = new Label();
        this.lblAmt.AutoSize = true;
        this.lblVat = new Label();
        this.lblVat.AutoSize = true;

        this.edtMmdd0 = new TextBox();
        this.edtItnbr0 = new TextBox();
        this.edtItdsc0 = new TextBox();
        this.edtItdsc0.ReadOnly = true;
        this.edtQty0 = new NumericUpDown();
        this.edtQty0.Maximum = 999999999;
        this.edtQty0.DecimalPlaces = 0;
        this.edtCost0 = new NumericUpDown();
        this.edtCost0.Maximum = 999999999;
        this.edtCost0.DecimalPlaces = 0;
        this.edtAmt0 = new NumericUpDown();
        this.edtAmt0.Maximum = 999999999999;
        this.edtAmt0.DecimalPlaces = 0;

        this.edtMmdd1 = new TextBox();
        this.edtItnbr1 = new TextBox();
        this.edtItdsc1 = new TextBox();
        this.edtItdsc1.ReadOnly = true;
        this.edtQty1 = new NumericUpDown();
        this.edtQty1.Maximum = 999999999;
        this.edtQty1.DecimalPlaces = 0;
        this.edtCost1 = new NumericUpDown();
        this.edtCost1.Maximum = 999999999;
        this.edtCost1.DecimalPlaces = 0;
        this.edtAmt1 = new NumericUpDown();
        this.edtAmt1.Maximum = 999999999999;
        this.edtAmt1.DecimalPlaces = 0;

        this.edtMmdd2 = new TextBox();
        this.edtItnbr2 = new TextBox();
        this.edtItdsc2 = new TextBox();
        this.edtItdsc2.ReadOnly = true;
        this.edtQty2 = new NumericUpDown();
        this.edtQty2.Maximum = 999999999;
        this.edtQty2.DecimalPlaces = 0;
        this.edtCost2 = new NumericUpDown();
        this.edtCost2.Maximum = 999999999;
        this.edtCost2.DecimalPlaces = 0;
        this.edtAmt2 = new NumericUpDown();
        this.edtAmt2.Maximum = 999999999999;
        this.edtAmt2.DecimalPlaces = 0;

        this.edtMmdd3 = new TextBox();
        this.edtItnbr3 = new TextBox();
        this.edtItdsc3 = new TextBox();
        this.edtItdsc3.ReadOnly = true;
        this.edtQty3 = new NumericUpDown();
        this.edtQty3.Maximum = 999999999;
        this.edtQty3.DecimalPlaces = 0;
        this.edtCost3 = new NumericUpDown();
        this.edtCost3.Maximum = 999999999;
        this.edtCost3.DecimalPlaces = 0;
        this.edtAmt3 = new NumericUpDown();
        this.edtAmt3.Maximum = 999999999999;
        this.edtAmt3.DecimalPlaces = 0;

        this.lblHdr0 = new Label();
        this.lblHdr1 = new Label();
        this.lblHdr2 = new Label();
        this.lblHdr3 = new Label();
        this.lblHdr4 = new Label();
        this.lblHdr5 = new Label();
        this.lblAmtCap = new Label();
        this.lblVatCap = new Label();
        this.lblTDate = new Label();
        this.lblNoCap = new Label();
        this.lblCvcod = new Label();
        this.lblCvnam = new Label();
        this.lblBigo = new Label();

        this.btnAdd = new Button();
        this.btnAdd.Text = "연속저장(F2)";
        this.btnOne = new Button();
        this.btnOne.Text = "저장(F3)";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";

        this._tip = new ToolTip();

        this.SuspendLayout();
        //
        // 원본 GridLayout(this, 20, 15, slotWidth:260, labelWidth:85, rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영
        //
        this.lblTDate.Text = "발행일자";
        this.lblTDate.Left = 20; this.lblTDate.Top = 18; this.lblTDate.AutoSize = true;
        this.edtTDate.Left = 105; this.edtTDate.Top = 15; this.edtTDate.Width = 163;

        this.lblNoCap.Text = "전표번호";
        this.lblNoCap.Left = 280; this.lblNoCap.Top = 18; this.lblNoCap.AutoSize = true;
        this.edtNo.Left = 365; this.edtNo.Top = 15; this.edtNo.Width = 163;

        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 20; this.lblCvcod.Top = 48; this.lblCvcod.AutoSize = true;
        this.edtCvcod.Left = 105; this.edtCvcod.Top = 45; this.edtCvcod.Width = 163;
        this._tip.SetToolTip(this.edtCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.edtCvcod.KeyDown += new KeyEventHandler(this.EdtCvcod_KeyDown);

        this.lblCvnam.Text = "거래처명";
        this.lblCvnam.Left = 280; this.lblCvnam.Top = 48; this.lblCvnam.AutoSize = true;
        this.dspCvnam.Left = 365; this.dspCvnam.Top = 45; this.dspCvnam.Width = 163;

        this.rdo1.Left = 540; this.rdo1.Top = 47; this.rdo1.Width = 60;
        this.rdo2.Left = 20; this.rdo2.Top = 77; this.rdo2.Width = 60;
        //
        // 헤더 라벨(원본 headerY = grid.Bottom() = 115)
        //
        this.lblHdr0.Text = "일자"; this.lblHdr0.Left = 20; this.lblHdr0.Top = 115; this.lblHdr0.AutoSize = true;
        this.lblHdr1.Text = "품번"; this.lblHdr1.Left = 90; this.lblHdr1.Top = 115; this.lblHdr1.AutoSize = true;
        this.lblHdr2.Text = "품명"; this.lblHdr2.Left = 220; this.lblHdr2.Top = 115; this.lblHdr2.AutoSize = true;
        this.lblHdr3.Text = "수량"; this.lblHdr3.Left = 400; this.lblHdr3.Top = 115; this.lblHdr3.AutoSize = true;
        this.lblHdr4.Text = "단가"; this.lblHdr4.Left = 480; this.lblHdr4.Top = 115; this.lblHdr4.AutoSize = true;
        this.lblHdr5.Text = "금액"; this.lblHdr5.Left = 570; this.lblHdr5.Top = 115; this.lblHdr5.AutoSize = true;
        //
        // 라인아이템 행 0~3 (원본 y = 135, 165, 195, 225)
        //
        this.edtMmdd0.Left = 20; this.edtMmdd0.Top = 135; this.edtMmdd0.Width = 60;
        this.edtItnbr0.Left = 90; this.edtItnbr0.Top = 135; this.edtItnbr0.Width = 120;
        this.edtItdsc0.Left = 220; this.edtItdsc0.Top = 135; this.edtItdsc0.Width = 170;
        this.edtQty0.Left = 400; this.edtQty0.Top = 135; this.edtQty0.Width = 70;
        this.edtCost0.Left = 480; this.edtCost0.Top = 135; this.edtCost0.Width = 80;
        this.edtAmt0.Left = 570; this.edtAmt0.Top = 135; this.edtAmt0.Width = 100;

        this.edtMmdd1.Left = 20; this.edtMmdd1.Top = 165; this.edtMmdd1.Width = 60;
        this.edtItnbr1.Left = 90; this.edtItnbr1.Top = 165; this.edtItnbr1.Width = 120;
        this.edtItdsc1.Left = 220; this.edtItdsc1.Top = 165; this.edtItdsc1.Width = 170;
        this.edtQty1.Left = 400; this.edtQty1.Top = 165; this.edtQty1.Width = 70;
        this.edtCost1.Left = 480; this.edtCost1.Top = 165; this.edtCost1.Width = 80;
        this.edtAmt1.Left = 570; this.edtAmt1.Top = 165; this.edtAmt1.Width = 100;

        this.edtMmdd2.Left = 20; this.edtMmdd2.Top = 195; this.edtMmdd2.Width = 60;
        this.edtItnbr2.Left = 90; this.edtItnbr2.Top = 195; this.edtItnbr2.Width = 120;
        this.edtItdsc2.Left = 220; this.edtItdsc2.Top = 195; this.edtItdsc2.Width = 170;
        this.edtQty2.Left = 400; this.edtQty2.Top = 195; this.edtQty2.Width = 70;
        this.edtCost2.Left = 480; this.edtCost2.Top = 195; this.edtCost2.Width = 80;
        this.edtAmt2.Left = 570; this.edtAmt2.Top = 195; this.edtAmt2.Width = 100;

        this.edtMmdd3.Left = 20; this.edtMmdd3.Top = 225; this.edtMmdd3.Width = 60;
        this.edtItnbr3.Left = 90; this.edtItnbr3.Top = 225; this.edtItnbr3.Width = 120;
        this.edtItdsc3.Left = 220; this.edtItdsc3.Top = 225; this.edtItdsc3.Width = 170;
        this.edtQty3.Left = 400; this.edtQty3.Top = 225; this.edtQty3.Width = 70;
        this.edtCost3.Left = 480; this.edtCost3.Top = 225; this.edtCost3.Width = 80;
        this.edtAmt3.Left = 570; this.edtAmt3.Top = 225; this.edtAmt3.Width = 100;
        //
        // 비고 (원본 AddRow(this, "비고", edtBigo, 20, ref y=255, 400, labelWidth:85))
        //
        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 20; this.lblBigo.Top = 258; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 105; this.edtBigo.Top = 255; this.edtBigo.Width = 400;
        //
        // 합계금액 / 부가세 (원본 y=283 기준, y+5=288)
        //
        this.lblAmtCap.Text = "합계금액:";
        this.lblAmtCap.Left = 20; this.lblAmtCap.Top = 288; this.lblAmtCap.AutoSize = true;
        this.lblAmt.Left = 110; this.lblAmt.Top = 288;
        this.lblVatCap.Text = "부가세:";
        this.lblVatCap.Left = 270; this.lblVatCap.Top = 288; this.lblVatCap.AutoSize = true;
        this.lblVat.Left = 350; this.lblVat.Top = 288;
        //
        // 버튼 (원본 y=318 기준)
        //
        this.btnAdd.Left = 150; this.btnAdd.Top = 318; this.btnAdd.Width = 130;
        this.btnOne.Left = 290; this.btnOne.Top = 318; this.btnOne.Width = 100;
        this.btnClose.Left = 400; this.btnClose.Top = 318; this.btnClose.Width = 100;
        this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
        this.btnOne.Click += new EventHandler(this.BtnOne_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.Controls.AddRange(new Control[]
        {
            this.lblTDate, this.edtTDate,
            this.lblNoCap, this.edtNo,
            this.lblCvcod, this.edtCvcod,
            this.lblCvnam, this.dspCvnam,
            this.rdo1, this.rdo2,
            this.lblHdr0, this.lblHdr1, this.lblHdr2, this.lblHdr3, this.lblHdr4, this.lblHdr5,
            this.edtMmdd0, this.edtItnbr0, this.edtItdsc0, this.edtQty0, this.edtCost0, this.edtAmt0,
            this.edtMmdd1, this.edtItnbr1, this.edtItdsc1, this.edtQty1, this.edtCost1, this.edtAmt1,
            this.edtMmdd2, this.edtItnbr2, this.edtItdsc2, this.edtQty2, this.edtCost2, this.edtAmt2,
            this.edtMmdd3, this.edtItnbr3, this.edtItdsc3, this.edtQty3, this.edtCost3, this.edtAmt3,
            this.lblBigo, this.edtBigo,
            this.lblAmtCap, this.lblAmt, this.lblVatCap, this.lblVat,
            this.btnAdd, this.btnOne, this.btnClose
        });
        //
        // SS33BForm
        //
        this.Text = "계산서 등록";
        this.Width = 750;
        this.Height = 560;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        this.ClientSize = new Size(this.ClientSize.Width, 363);

        this.KeyDown += new KeyEventHandler(this.SS33BForm_KeyDown);
        this.ResumeLayout(false);
    }
}
