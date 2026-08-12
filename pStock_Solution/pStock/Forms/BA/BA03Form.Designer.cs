using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA03Form
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

    private FastDataGridView grid;
    private TextBox edtCvcod;  // 거래처코드(품번 앞 4자리)
    private TextBox edtCvnam;  // 거래처명(참조표시, 읽기전용)
    private TextBox edtCode;   // 품번 전체
    private CheckBox chkAuto;
    private TextBox edtItdsc;  // 품명
    private TextBox edtSpec;   // 규격
    private ComboBox cboDanwi; // 단위
    private NumericUpDown eItwgt;  // 중량
    private NumericUpDown edtIcost; // 입고단가
    private NumericUpDown edtBcost; // 기준단가
    private NumericUpDown edtOcost; // 출고단가
    private ComboBox cboSavLoc; // 저장위치
    private TextBox edtBigo;
    private TextBox edtWord;

    private Button btnNew;
    private Button btnAdd;
    private Button btnOne;
    private Button btnDel;
    private Button btnExcel;
    private Button btnPrint;
    private Button btnClose;
    private Label lblDbCnt;
    private Panel panelTop;
    private Panel searchPanel;
    private Label lblWord;
    private Panel editPanel;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtCvcod = new TextBox();
        this.edtCvnam = new TextBox();
        this.edtCode = new TextBox();
        this.chkAuto = new CheckBox() { Text = "자동채번" };
        this.edtItdsc = new TextBox();
        this.edtSpec = new TextBox();
        this.cboDanwi = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
        this.eItwgt = new NumericUpDown() { DecimalPlaces = 2, Maximum = 999999 };
        this.edtIcost = new NumericUpDown() { DecimalPlaces = 0, Maximum = 999999999 };
        this.edtBcost = new NumericUpDown() { DecimalPlaces = 0, Maximum = 999999999 };
        this.edtOcost = new NumericUpDown() { DecimalPlaces = 0, Maximum = 999999999 };
        this.cboSavLoc = new ComboBox() { DropDownStyle = ComboBoxStyle.DropDownList };
        this.edtBigo = new TextBox();
        this.edtWord = new TextBox();
        this.btnNew = new Button() { Text = "신규(F1)" };
        this.btnAdd = new Button() { Text = "연속저장(F2)" };
        this.btnOne = new Button() { Text = "저장(F3)" };
        this.btnDel = new Button() { Text = "삭제(F4)" };
        this.btnExcel = new Button() { Text = "엑셀저장" };
        this.btnPrint = new Button() { Text = "인쇄" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.lblDbCnt = new Label() { AutoSize = true };
        this.panelTop = new Panel();
        this.searchPanel = new Panel();
        this.lblWord = new Label();
        this.editPanel = new Panel();
        this.SuspendLayout();
        //
        // BA03Form (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "품목,단가 마스터";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;
        // 원본 BuildLayout()은 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)를 쓰기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 파싱할 수 없다. 그래서
        // BuildDynamicLayout()(BA03Form.cs)으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후에 실행한다 — 동작은 100% 동일하다.

        this.Load += (_, _) => { ResetDanwiList(); ResetSavLocList(); this._vSort = "Code"; ReloadList(); };
        this.KeyDown += new KeyEventHandler(this.BA03Form_KeyDown);
        this.ResumeLayout(false);
    }
}
