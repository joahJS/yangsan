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
    private Label lblTdate;
    private Label lblNo;
    private Label lblCvcod;
    private Label lblCvnam;
    private Label lblTelno;
    private Label lblLncod;
    private Label lblLnnam;
    private Label lblLnadr;
    private Label lblPlncd;
    private Label lblSsamt;
    private Label lblMbigo;

    private void InitializeComponent()
    {
        this.eTdate = new TextBox();
        this.eNo = new TextBox();
        this.eNo.ReadOnly = true;
        this.chkAuto = new CheckBox();
        this.chkAuto.Text = "자동채번";
        this.chkAuto.Checked = true;
        this.eCvcod = new TextBox();
        this.lCvnam = new TextBox();
        this.lCvnam.ReadOnly = true;
        this.lTelno = new TextBox();
        this.lTelno.ReadOnly = true;
        this.eLncod = new TextBox();
        this.lLnnam = new TextBox();
        this.lLnnam.ReadOnly = true;
        this.lLnadr = new TextBox();
        this.lLnadr.ReadOnly = true;
        this.ePlncd = new TextBox();
        this.eMbigo = new TextBox();
        this.eMbigo.Multiline = true;
        this.eMbigo.Height = 50;
        this.eSsamt = new NumericUpDown();
        this.eSsamt.Maximum = 999999999;
        this.eSsamt.DecimalPlaces = 0;
        this.rgListS = new FastDataGridView();
        this.dTamt = new NumericUpDown();
        this.dTamt.ReadOnly = true;
        this.dTamt.Maximum = 999999999999;
        this.dTamt.DecimalPlaces = 0;
        this.dJamt = new NumericUpDown();
        this.dJamt.ReadOnly = true;
        this.dJamt.Maximum = 999999999999;
        this.dJamt.DecimalPlaces = 0;
        this.btnAddRow = new Button();
        this.btnAddRow.Text = "행추가";
        this.btnDelRow = new Button();
        this.btnDelRow.Text = "행삭제(F4)";
        this.ckPrint = new CheckBox();
        this.ckPrint.Text = "저장 후 인쇄";
        this.btnPrintNow = new Button();
        this.btnPrintNow.Text = "인쇄";
        this.bAdd = new Button();
        this.bAdd.Text = "연속저장(F2)";
        this.bOne = new Button();
        this.bOne.Text = "저장(F3)";
        this.btnClose = new Button();
        this.btnClose.Text = "닫기(Esc)";
        this._tip = new ToolTip();
        this.panelTop = new Panel();
        this.panelGrid = new Panel();
        this.panelGridButtons = new Panel();
        this.panelBottom = new Panel();
        this.lblTamt = new Label();
        this.lblJamt = new Label();
        this.lblTdate = new Label();
        this.lblNo = new Label();
        this.lblCvcod = new Label();
        this.lblCvnam = new Label();
        this.lblTelno = new Label();
        this.lblLncod = new Label();
        this.lblLnnam = new Label();
        this.lblLnadr = new Label();
        this.lblPlncd = new Label();
        this.lblSsamt = new Label();
        this.lblMbigo = new Label();
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
        // panelTop 내부(원본 GridLayout(panelTop, 10, 5, slotWidth:300, labelWidth:85,
        // rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영)
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 165;

        this.lblTdate.Text = "출고일자";
        this.lblTdate.Left = 10; this.lblTdate.Top = 8; this.lblTdate.AutoSize = true;
        this.eTdate.Left = 95; this.eTdate.Top = 5; this.eTdate.Width = 203;

        this.lblNo.Text = "전표번호";
        this.lblNo.Left = 310; this.lblNo.Top = 8; this.lblNo.AutoSize = true;
        this.eNo.Left = 395; this.eNo.Top = 5; this.eNo.Width = 203;

        this.chkAuto.Left = 610; this.chkAuto.Top = 7; this.chkAuto.Width = 100;

        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 10; this.lblCvcod.Top = 38; this.lblCvcod.AutoSize = true;
        this.eCvcod.Left = 95; this.eCvcod.Top = 35; this.eCvcod.Width = 203;
        this._tip.SetToolTip(this.eCvcod, "Enter 키를 누르면 거래처를 검색합니다.");
        this.eCvcod.KeyDown += new KeyEventHandler(this.ECvcod_KeyDown);

        this.lblCvnam.Text = "거래처명";
        this.lblCvnam.Left = 310; this.lblCvnam.Top = 38; this.lblCvnam.AutoSize = true;
        this.lCvnam.Left = 395; this.lCvnam.Top = 35; this.lCvnam.Width = 203;

        this.lblTelno.Text = "전화번호";
        this.lblTelno.Left = 610; this.lblTelno.Top = 38; this.lblTelno.AutoSize = true;
        this.lTelno.Left = 695; this.lTelno.Top = 35; this.lTelno.Width = 203;

        this.lblLncod.Text = "착지처코드";
        this.lblLncod.Left = 10; this.lblLncod.Top = 68; this.lblLncod.AutoSize = true;
        this.eLncod.Left = 95; this.eLncod.Top = 65; this.eLncod.Width = 203;
        this.eLncod.KeyDown += new KeyEventHandler(this.ELncod_KeyDown);

        this.lblLnnam.Text = "착지처명";
        this.lblLnnam.Left = 310; this.lblLnnam.Top = 68; this.lblLnnam.AutoSize = true;
        this.lLnnam.Left = 395; this.lLnnam.Top = 65; this.lLnnam.Width = 203;

        this.lblLnadr.Text = "착지처주소";
        this.lblLnadr.Left = 610; this.lblLnadr.Top = 68; this.lblLnadr.AutoSize = true;
        this.lLnadr.Left = 695; this.lLnadr.Top = 65; this.lLnadr.Width = 203;

        this.lblPlncd.Text = "담당자";
        this.lblPlncd.Left = 10; this.lblPlncd.Top = 98; this.lblPlncd.AutoSize = true;
        this.ePlncd.Left = 95; this.ePlncd.Top = 95; this.ePlncd.Width = 203;

        this.lblSsamt.Text = "운송비";
        this.lblSsamt.Left = 310; this.lblSsamt.Top = 98; this.lblSsamt.AutoSize = true;
        this.eSsamt.Left = 395; this.eSsamt.Top = 95; this.eSsamt.Width = 203;

        this.lblMbigo.Text = "비고";
        this.lblMbigo.Left = 10; this.lblMbigo.Top = 128; this.lblMbigo.AutoSize = true;
        this.eMbigo.Left = 95; this.eMbigo.Top = 125; this.eMbigo.Width = 803;

        this.panelTop.Controls.AddRange(new Control[]
        {
            this.lblTdate, this.eTdate,
            this.lblNo, this.eNo,
            this.chkAuto,
            this.lblCvcod, this.eCvcod,
            this.lblCvnam, this.lCvnam,
            this.lblTelno, this.lTelno,
            this.lblLncod, this.eLncod,
            this.lblLnnam, this.lLnnam,
            this.lblLnadr, this.lLnadr,
            this.lblPlncd, this.ePlncd,
            this.lblSsamt, this.eSsamt,
            this.lblMbigo, this.eMbigo
        });

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
        this.rgListS.Columns[0].ReadOnly = true; // ITCOD
        this.rgListS.Columns[1].ReadOnly = true; // ITNAM
        this.rgListS.Columns[2].ReadOnly = true; // ISPEC
        this.rgListS.Columns[3].ReadOnly = true; // DANWI
        this.rgListS.Columns[4].ReadOnly = true; // HOUSE
        this.rgListS.CellDoubleClick += new DataGridViewCellEventHandler(this.RgListS_CellDoubleClick);
        this.rgListS.CellEndEdit += new DataGridViewCellEventHandler(this.RgListS_CellEndEdit);

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
        this.btnAddRow.Click += new EventHandler(this.BtnAddRow_Click);
        this.btnDelRow.Click += new EventHandler(this.BtnDelRow_Click);

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

        this.bAdd.Click += new EventHandler(this.BAdd_Click);
        this.bOne.Click += new EventHandler(this.BOne_Click);
        this.btnPrintNow.Click += new EventHandler(this.BtnPrintNow_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.KeyDown += new KeyEventHandler(this.SS020F01Form_KeyDown);
        this.ResumeLayout(false);
    }
}
