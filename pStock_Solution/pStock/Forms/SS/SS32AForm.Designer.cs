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
        // BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다(지역변수/누적계산
        // 불가). 그래서 BuildDynamicLayout()으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후에 실행한다 — 동작은 100% 동일하고 디자이너가 이 메서드 밖의 정적인
        // 부분만 인식하면 되므로 로드가 가능해진다.

        this.Load += (_, _) => { if (this.cboGu.Items.Count == 0) ResetGuList(); };
        this.KeyDown += new KeyEventHandler(this.SS32AForm_KeyDown);

        ((System.ComponentModel.ISupportInitialize)(this.edtAmt)).EndInit();
        this.ResumeLayout(false);
    }
}
