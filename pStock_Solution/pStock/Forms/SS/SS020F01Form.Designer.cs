using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.SS;

partial class SS020F01Form
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

    private TextBox eTdate;
    private TextBox eNo;
    private CheckBox chkAuto;
    private TextBox eCvcod;
    private TextBox lCvnam;
    private TextBox lTelno;
    private TextBox eLncod;
    private TextBox lLnnam;
    private TextBox lLnadr;
    private TextBox ePlncd;
    private TextBox eMbigo;
    private NumericUpDown eSsamt;
    private FastDataGridView rgListS;
    private NumericUpDown dTamt;
    private NumericUpDown dJamt;

    private Button btnAddRow;
    private Button btnDelRow;
    private CheckBox ckPrint;
    private Button btnPrintNow;
    private Button bAdd;
    private Button bOne;
    private Button btnClose;

    private ToolTip _tip;

    private Panel panelTop;
    private Panel panelGrid;
    private Panel panelGridButtons;
    private Panel panelBottom;
    private Label lblTamt;
    private Label lblJamt;

    private void InitializeComponent()
    {
        this.eTdate = new TextBox();
        this.eNo = new TextBox { ReadOnly = true };
        this.chkAuto = new CheckBox { Text = "자동채번", Checked = true };
        this.eCvcod = new TextBox();
        this.lCvnam = new TextBox { ReadOnly = true };
        this.lTelno = new TextBox { ReadOnly = true };
        this.eLncod = new TextBox();
        this.lLnnam = new TextBox { ReadOnly = true };
        this.lLnadr = new TextBox { ReadOnly = true };
        this.ePlncd = new TextBox();
        this.eMbigo = new TextBox { Multiline = true, Height = 50 };
        this.eSsamt = new NumericUpDown { Maximum = 999999999, DecimalPlaces = 0 };
        this.rgListS = new FastDataGridView();
        this.dTamt = new NumericUpDown { ReadOnly = true, Maximum = 999999999999, DecimalPlaces = 0 };
        this.dJamt = new NumericUpDown { ReadOnly = true, Maximum = 999999999999, DecimalPlaces = 0 };
        this.btnAddRow = new Button { Text = "행추가" };
        this.btnDelRow = new Button { Text = "행삭제(F4)" };
        this.ckPrint = new CheckBox { Text = "저장 후 인쇄" };
        this.btnPrintNow = new Button { Text = "인쇄" };
        this.bAdd = new Button { Text = "연속저장(F2)" };
        this.bOne = new Button { Text = "저장(F3)" };
        this.btnClose = new Button { Text = "닫기(Esc)" };
        this._tip = new ToolTip();
        this.panelTop = new Panel();
        this.panelGrid = new Panel();
        this.panelGridButtons = new Panel();
        this.panelBottom = new Panel();
        this.lblTamt = new Label();
        this.lblJamt = new Label();
        this.SuspendLayout();
        //
        // SS020F01Form (원본 생성자 프롤로그)
        //
        this.Text = "출고 등록";
        this.Width = 950;
        this.Height = 700;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.KeyPreview = true;
        // panelTop 내부는 GridLayout 헬퍼(지역변수 기반 동적 좌표 계산)로 배치하기 때문에
        // WinForms 디자이너가 InitializeComponent() 안에서 처리하지 못한다. 그 부분만
        // BuildDynamicLayout()(SS020F01Form.cs)으로 분리해 생성자에서 InitializeComponent()
        // 호출 직후 실행한다. 나머지(panelGrid/panelGridButtons/panelBottom 등, 전부 리터럴
        // 좌표)는 그대로 여기 둔다.
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 180;

        this.rgListS.Dock = DockStyle.Fill;
        this.rgListS.AllowUserToAddRows = false;
        this.rgListS.Columns.Add("ITCOD", "품번");
        this.rgListS.Columns.Add("ITNAM", "품명");
        this.rgListS.Columns.Add("ISPEC", "규격");
        this.rgListS.Columns.Add("DANWI", "단위");
        this.rgListS.Columns.Add("HOUSE", "저장위치");
        this.rgListS.Columns.Add("TRQTY", "수량");
        this.rgListS.Columns.Add("UCOST", "단가");
        this.rgListS.Columns.Add("TRAMT", "금액");
        this.rgListS.Columns.Add("JAMT1", "부가세1");
        this.rgListS.Columns.Add("JAMT2", "부가세2");
        this.rgListS.Columns.Add("JAMT3", "부가세3");
        this.rgListS.Columns.Add("DBIGO", "비고");
        this.rgListS.Columns["ITCOD"]!.ReadOnly = true;
        this.rgListS.Columns["ITNAM"]!.ReadOnly = true;
        this.rgListS.Columns["ISPEC"]!.ReadOnly = true;
        this.rgListS.Columns["DANWI"]!.ReadOnly = true;
        this.rgListS.Columns["HOUSE"]!.ReadOnly = true;
        this.rgListS.CellDoubleClick += new DataGridViewCellEventHandler(this.RgListS_CellDoubleClick);
        this.rgListS.CellEndEdit += (_, _) => RecalcRowAndTotal();

        // 원본 BuildLayout()에서 gridPanel은 rgListS를 담아뒀지만 정작 Controls에는 추가되지
        // 않고(Controls.Add(rgListS)가 폼에 직접 호출됨) 버려지는 패널이었다. rgListS의 최종
        // 부모는 gridPanel이 아니라 폼이 되므로, 그 결과를 그대로 재현하기 위해 여기서도
        // panelGrid는 Controls.Add하지 않는다.
        this.panelGrid.Dock = DockStyle.Top;
        this.panelGrid.Height = 300;
        this.panelGrid.Controls.Add(this.rgListS);
        this.panelGridButtons.Dock = DockStyle.Top;
        this.panelGridButtons.Height = 35;
        this.btnAddRow.Left = 5; this.btnAddRow.Top = 5; this.btnAddRow.Width = 90;
        this.btnDelRow.Left = 100; this.btnDelRow.Top = 5; this.btnDelRow.Width = 100;
        this.panelGridButtons.Controls.AddRange(new Control[] { this.btnAddRow, this.btnDelRow });
        this.btnAddRow.Click += (_, _) => { this.rgListS.Rows.Add("", "", "", "", "", 0, 0, 0, 0, 0, 0, ""); };
        this.btnDelRow.Click += (_, _) => { if (this.rgListS.CurrentRow != null) { this.rgListS.Rows.Remove(this.rgListS.CurrentRow); RecalcTotal(); } };

        this.panelBottom.Dock = DockStyle.Bottom;
        this.panelBottom.Height = 80;
        this.lblTamt.Text = "출고금액:"; this.lblTamt.Left = 10; this.lblTamt.Top = 10; this.lblTamt.AutoSize = true;
        this.dTamt.Left = 90; this.dTamt.Top = 6; this.dTamt.Width = 100;
        this.lblJamt.Text = "부가세:"; this.lblJamt.Left = 200; this.lblJamt.Top = 10; this.lblJamt.AutoSize = true;
        this.dJamt.Left = 250; this.dJamt.Top = 6; this.dJamt.Width = 100;
        this.ckPrint.Left = 365; this.ckPrint.Top = 10; this.ckPrint.AutoSize = true;
        this.btnPrintNow.Left = 480; this.btnPrintNow.Top = 6; this.btnPrintNow.Width = 70;
        this.bAdd.Left = 560; this.bAdd.Top = 6; this.bAdd.Width = 110;
        this.bOne.Left = 680; this.bOne.Top = 6; this.bOne.Width = 90;
        this.btnClose.Left = 780; this.btnClose.Top = 6; this.btnClose.Width = 90;
        this.panelBottom.Controls.AddRange(new Control[]
        {
            this.lblTamt, this.dTamt, this.lblJamt, this.dJamt, this.ckPrint, this.btnPrintNow, this.bAdd, this.bOne, this.btnClose
        });

        this.Controls.Add(this.rgListS);
        this.Controls.Add(this.panelGridButtons);
        this.Controls.Add(this.panelBottom);
        this.Controls.Add(this.panelTop);

        this.bAdd.Click += (_, _) =>
        {
            if (SaveEntry(out var savedNo))
            {
                Saved = true;
                if (this.ckPrint.Checked) pStock.Common.DeliverySlipPrinter.Print(savedNo);
                InitScreen();
                this.eCvcod.Focus();
            }
        };
        this.bOne.Click += (_, _) =>
        {
            if (SaveEntry(out var savedNo))
            {
                Saved = true;
                if (this.ckPrint.Checked) pStock.Common.DeliverySlipPrinter.Print(savedNo);
                Close();
            }
        };
        this.btnPrintNow.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(this.eNo.Text))
            {
                MessageBox.Show("저장된 출고번호가 없습니다. 먼저 저장하세요.", "확인",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            pStock.Common.DeliverySlipPrinter.Print(this.eNo.Text.Trim());
        };
        this.btnClose.Click += (_, _) => Close();

        this.KeyDown += new KeyEventHandler(this.SS020F01Form_KeyDown);
        this.ResumeLayout(false);
    }
}
