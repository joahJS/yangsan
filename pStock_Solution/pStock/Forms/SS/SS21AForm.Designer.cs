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
        // BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다. BuildDynamicLayout()
        // (SS21AForm.cs)으로 분리해 생성자에서 InitializeComponent() 호출 직후 실행한다.

        this.Load += new EventHandler(this.SS21AForm_Load);
        this.KeyDown += new KeyEventHandler(this.SS21AForm_KeyDown);
        this.ResumeLayout(false);
    }
}
