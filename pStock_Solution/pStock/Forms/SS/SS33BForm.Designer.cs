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
        for (int i = 0; i < 4; i++)
        {
            this.edtMmdd[i] = new TextBox();
            this.edtItnbr[i] = new TextBox();
            this.edtItdsc[i] = new TextBox { ReadOnly = true };
            this.edtQty[i] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
            this.edtCost[i] = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
            this.edtAmt[i] = new NumericUpDown { Maximum = 999999999999, DecimalPlaces = 0 };
            PublicLib.MakeTypingFriendly(this.edtQty[i]);
            PublicLib.MakeTypingFriendly(this.edtCost[i]);
            PublicLib.MakeTypingFriendly(this.edtAmt[i]);
        }

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
        var headers = new[] { "일자", "품번", "품명", "수량", "단가", "금액" };
        var xs = new[] { col1, col1 + 70, col1 + 200, col1 + 380, col1 + 460, col1 + 550 };
        this.lblHdr = new Label[headers.Length];
        for (int c = 0; c < headers.Length; c++)
        {
            this.lblHdr[c] = new Label { Text = headers[c], Left = xs[c], Top = headerY, AutoSize = true };
            this.Controls.Add(this.lblHdr[c]);
        }
        y += 20;

        for (int i = 0; i < 4; i++)
        {
            this.edtMmdd[i].Left = xs[0]; this.edtMmdd[i].Top = y; this.edtMmdd[i].Width = 60;
            this.edtItnbr[i].Left = xs[1]; this.edtItnbr[i].Top = y; this.edtItnbr[i].Width = 120;
            var idx = i;
            this.edtItnbr[i].KeyDown += (_, e) => this.EdtItnbr_KeyDown(idx, e);
            this._tip.SetToolTip(this.edtItnbr[i], "Enter 키를 누르면 품목을 검색합니다.");
            this.edtItdsc[i].Left = xs[2]; this.edtItdsc[i].Top = y; this.edtItdsc[i].Width = 170;
            this.edtQty[i].Left = xs[3]; this.edtQty[i].Top = y; this.edtQty[i].Width = 70;
            this.edtQty[i].ValueChanged += (_, _) => this.RecalcRow(idx);
            this.edtCost[i].Left = xs[4]; this.edtCost[i].Top = y; this.edtCost[i].Width = 80;
            this.edtCost[i].ValueChanged += (_, _) => this.RecalcRow(idx);
            this.edtAmt[i].Left = xs[5]; this.edtAmt[i].Top = y; this.edtAmt[i].Width = 100;
            this.edtAmt[i].ValueChanged += (_, _) => this.RecalcTotal();

            this.Controls.AddRange(new Control[] { this.edtMmdd[i], this.edtItnbr[i], this.edtItdsc[i], this.edtQty[i], this.edtCost[i], this.edtAmt[i] });
            y += 30;
        }

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
