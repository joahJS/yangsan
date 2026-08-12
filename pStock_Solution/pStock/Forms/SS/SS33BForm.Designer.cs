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

        this.edtMmdd = new TextBox[4];
        this.edtItnbr = new TextBox[4];
        this.edtItdsc = new TextBox[4];
        this.edtQty = new NumericUpDown[4];
        this.edtCost = new NumericUpDown[4];
        this.edtAmt = new NumericUpDown[4];

        this.btnAdd = new Button();
        this.btnAdd.Text = "연속저장(F2)";
        this.btnOne = new Button();
        this.btnOne.Text = "저장(F3)";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";

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

        // 라인아이템 컨트롤 생성 및 GridLayout 기반 배치는 지역변수(col1/y/grid 등)를 써서
        // WinForms 디자이너가 InitializeComponent() 안에서 처리하지 못한다. 전부
        // BuildDynamicLayout()(SS33BForm.cs)으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후 실행한다.

        this.KeyDown += new KeyEventHandler(this.SS33BForm_KeyDown);
        this.ResumeLayout(false);
    }
}
