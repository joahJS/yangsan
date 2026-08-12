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

    private void InitializeComponent()
    {
        this.edtTDate = new TextBox();
        this.edtNo = new TextBox { ReadOnly = true };
        this.edtCvcod = new TextBox();
        this.dspCvnam = new TextBox { ReadOnly = true };
        this.rdo1 = new RadioButton { Text = "영수" };
        this.rdo2 = new RadioButton { Text = "청구", Checked = true };
        this.edtBigo = new TextBox();
        this.lblAmt = new Label { AutoSize = true };
        this.lblVat = new Label { AutoSize = true };

        this.edtMmdd = new TextBox[4];
        this.edtItnbr = new TextBox[4];
        this.edtItdsc = new TextBox[4];
        this.edtQty = new NumericUpDown[4];
        this.edtCost = new NumericUpDown[4];
        this.edtAmt = new NumericUpDown[4];

        this.btnAdd = new Button { Text = "연속저장(F2)" };
        this.btnOne = new Button { Text = "저장(F3)" };
        this.btnClose = new Button { Text = "닫기(Esc)" };

        this._tip = new ToolTip();

        this.SuspendLayout();
        //
        // SS33BForm
        //
        this.Text = "계산서 등록";
        this.Width = 750;
        this.Height = 560;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;

        //
        // 라인아이템 4건 컨트롤 생성 (원본 생성자에서 BuildLayout() 호출 전에 있던 루프)
        //
        this.edtMmdd[0] = new TextBox();
        this.edtItnbr[0] = new TextBox();
        this.edtItdsc[0] = new TextBox { ReadOnly = true };
        this.edtQty[0] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[0] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[0] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[0]);
        PublicLib.MakeTypingFriendly(this.edtCost[0]);
        PublicLib.MakeTypingFriendly(this.edtAmt[0]);
        this.edtMmdd[1] = new TextBox();
        this.edtItnbr[1] = new TextBox();
        this.edtItdsc[1] = new TextBox { ReadOnly = true };
        this.edtQty[1] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[1] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[1] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[1]);
        PublicLib.MakeTypingFriendly(this.edtCost[1]);
        PublicLib.MakeTypingFriendly(this.edtAmt[1]);
        this.edtMmdd[2] = new TextBox();
        this.edtItnbr[2] = new TextBox();
        this.edtItdsc[2] = new TextBox { ReadOnly = true };
        this.edtQty[2] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[2] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[2] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[2]);
        PublicLib.MakeTypingFriendly(this.edtCost[2]);
        PublicLib.MakeTypingFriendly(this.edtAmt[2]);
        this.edtMmdd[3] = new TextBox();
        this.edtItnbr[3] = new TextBox();
        this.edtItdsc[3] = new TextBox { ReadOnly = true };
        this.edtQty[3] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtCost[3] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.edtAmt[3] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
        PublicLib.MakeTypingFriendly(this.edtQty[3]);
        PublicLib.MakeTypingFriendly(this.edtCost[3]);
        PublicLib.MakeTypingFriendly(this.edtAmt[3]);

        //
        // 원본 BuildLayout() 그대로 이동 (GridLayout 헬퍼로 상단 4개 입력란 배치)
        //
        int col1 = 20;
        var grid = new GridLayout(this, col1, 15, slotWidth: 260, labelWidth: 85, rowHeight: 30, slotsPerRow: 3);
        grid.Add("발행일자", this.edtTDate);
        grid.Add("전표번호", this.edtNo);
        grid.NewRow();
        grid.Add("거래처코드", this.edtCvcod);
        this._tip.SetToolTip(this.edtCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.edtCvcod.KeyDown += this.EdtCvcod_KeyDown;
        grid.Add("거래처명", this.dspCvnam);
        grid.AddRaw(this.rdo1, width: 60);
        grid.AddRaw(this.rdo2, width: 60);
        int y = grid.Bottom();

        var headerY = y;
        this.lblHdr = new Label[6];
        this.lblHdr[0] = new Label { Text = "일자", Left = col1, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[0]);
        this.lblHdr[1] = new Label { Text = "품번", Left = col1 + 70, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[1]);
        this.lblHdr[2] = new Label { Text = "품명", Left = col1 + 200, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[2]);
        this.lblHdr[3] = new Label { Text = "수량", Left = col1 + 380, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[3]);
        this.lblHdr[4] = new Label { Text = "단가", Left = col1 + 460, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[4]);
        this.lblHdr[5] = new Label { Text = "금액", Left = col1 + 550, Top = headerY, AutoSize = true };
        this.Controls.Add(this.lblHdr[5]);
        y += 20;

        // 행 0
        this.edtMmdd[0].Left = col1; this.edtMmdd[0].Top = y; this.edtMmdd[0].Width = 60;
        this.edtItnbr[0].Left = col1 + 70; this.edtItnbr[0].Top = y; this.edtItnbr[0].Width = 120;
        this.edtItnbr[0].KeyDown += (_, e) => this.EdtItnbr_KeyDown(0, e);
        this._tip.SetToolTip(this.edtItnbr[0], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[0].Left = col1 + 200; this.edtItdsc[0].Top = y; this.edtItdsc[0].Width = 170;
        this.edtQty[0].Left = col1 + 380; this.edtQty[0].Top = y; this.edtQty[0].Width = 70;
        this.edtQty[0].ValueChanged += (_, _) => this.RecalcRow(0);
        this.edtCost[0].Left = col1 + 460; this.edtCost[0].Top = y; this.edtCost[0].Width = 80;
        this.edtCost[0].ValueChanged += (_, _) => this.RecalcRow(0);
        this.edtAmt[0].Left = col1 + 550; this.edtAmt[0].Top = y; this.edtAmt[0].Width = 100;
        this.edtAmt[0].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[0], this.edtItnbr[0], this.edtItdsc[0], this.edtQty[0], this.edtCost[0], this.edtAmt[0] });
        y += 30;

        // 행 1
        this.edtMmdd[1].Left = col1; this.edtMmdd[1].Top = y; this.edtMmdd[1].Width = 60;
        this.edtItnbr[1].Left = col1 + 70; this.edtItnbr[1].Top = y; this.edtItnbr[1].Width = 120;
        this.edtItnbr[1].KeyDown += (_, e) => this.EdtItnbr_KeyDown(1, e);
        this._tip.SetToolTip(this.edtItnbr[1], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[1].Left = col1 + 200; this.edtItdsc[1].Top = y; this.edtItdsc[1].Width = 170;
        this.edtQty[1].Left = col1 + 380; this.edtQty[1].Top = y; this.edtQty[1].Width = 70;
        this.edtQty[1].ValueChanged += (_, _) => this.RecalcRow(1);
        this.edtCost[1].Left = col1 + 460; this.edtCost[1].Top = y; this.edtCost[1].Width = 80;
        this.edtCost[1].ValueChanged += (_, _) => this.RecalcRow(1);
        this.edtAmt[1].Left = col1 + 550; this.edtAmt[1].Top = y; this.edtAmt[1].Width = 100;
        this.edtAmt[1].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[1], this.edtItnbr[1], this.edtItdsc[1], this.edtQty[1], this.edtCost[1], this.edtAmt[1] });
        y += 30;

        // 행 2
        this.edtMmdd[2].Left = col1; this.edtMmdd[2].Top = y; this.edtMmdd[2].Width = 60;
        this.edtItnbr[2].Left = col1 + 70; this.edtItnbr[2].Top = y; this.edtItnbr[2].Width = 120;
        this.edtItnbr[2].KeyDown += (_, e) => this.EdtItnbr_KeyDown(2, e);
        this._tip.SetToolTip(this.edtItnbr[2], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[2].Left = col1 + 200; this.edtItdsc[2].Top = y; this.edtItdsc[2].Width = 170;
        this.edtQty[2].Left = col1 + 380; this.edtQty[2].Top = y; this.edtQty[2].Width = 70;
        this.edtQty[2].ValueChanged += (_, _) => this.RecalcRow(2);
        this.edtCost[2].Left = col1 + 460; this.edtCost[2].Top = y; this.edtCost[2].Width = 80;
        this.edtCost[2].ValueChanged += (_, _) => this.RecalcRow(2);
        this.edtAmt[2].Left = col1 + 550; this.edtAmt[2].Top = y; this.edtAmt[2].Width = 100;
        this.edtAmt[2].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[2], this.edtItnbr[2], this.edtItdsc[2], this.edtQty[2], this.edtCost[2], this.edtAmt[2] });
        y += 30;

        // 행 3
        this.edtMmdd[3].Left = col1; this.edtMmdd[3].Top = y; this.edtMmdd[3].Width = 60;
        this.edtItnbr[3].Left = col1 + 70; this.edtItnbr[3].Top = y; this.edtItnbr[3].Width = 120;
        this.edtItnbr[3].KeyDown += (_, e) => this.EdtItnbr_KeyDown(3, e);
        this._tip.SetToolTip(this.edtItnbr[3], "Enter 키를 누르면 품목을 검색합니다.");
        this.edtItdsc[3].Left = col1 + 200; this.edtItdsc[3].Top = y; this.edtItdsc[3].Width = 170;
        this.edtQty[3].Left = col1 + 380; this.edtQty[3].Top = y; this.edtQty[3].Width = 70;
        this.edtQty[3].ValueChanged += (_, _) => this.RecalcRow(3);
        this.edtCost[3].Left = col1 + 460; this.edtCost[3].Top = y; this.edtCost[3].Width = 80;
        this.edtCost[3].ValueChanged += (_, _) => this.RecalcRow(3);
        this.edtAmt[3].Left = col1 + 550; this.edtAmt[3].Top = y; this.edtAmt[3].Width = 100;
        this.edtAmt[3].ValueChanged += (_, _) => this.RecalcTotal();
        this.Controls.AddRange(new Control[] { this.edtMmdd[3], this.edtItnbr[3], this.edtItdsc[3], this.edtQty[3], this.edtCost[3], this.edtAmt[3] });
        y += 30;

        this.AddRow(this, "비고", this.edtBigo, col1, ref y, 400, labelWidth: 85);

        this.lblAmtCap = new Label { Text = "합계금액:", Left = col1, Top = y + 5, AutoSize = true };
        this.lblAmt.Left = col1 + 90; this.lblAmt.Top = y + 5;
        this.lblVatCap = new Label { Text = "부가세:", Left = col1 + 250, Top = y + 5, AutoSize = true };
        this.lblVat.Left = col1 + 330; this.lblVat.Top = y + 5;
        this.Controls.AddRange(new Control[] { this.lblAmtCap, this.lblAmt, this.lblVatCap, this.lblVat });
        y += 35;

        this.btnAdd.Left = 150; this.btnAdd.Top = y; this.btnAdd.Width = 130;
        this.btnOne.Left = 290; this.btnOne.Top = y; this.btnOne.Width = 100;
        this.btnClose.Left = 400; this.btnClose.Top = y; this.btnClose.Width = 100;
        this.Controls.AddRange(new Control[] { this.btnAdd, this.btnOne, this.btnClose });

        this.btnAdd.Click += (_, _) => { if (this.SaveEntry()) { this.Saved = true; this.ClearForm(); this.edtCvcod.Focus(); } };
        this.btnOne.Click += (_, _) => { if (this.SaveEntry()) { this.Saved = true; this.Close(); } };
        this.btnClose.Click += (_, _) => this.Close();

        this.ClientSize = new Size(this.ClientSize.Width, y + 45);

        this.KeyDown += new KeyEventHandler(this.SS33BForm_KeyDown);
        this.ResumeLayout(false);
    }
}
