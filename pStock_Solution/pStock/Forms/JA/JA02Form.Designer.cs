using pStock.Common;

namespace pStock.Forms.JA;

partial class JA02Form
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

    private TextBox edtCode;
    private TextBox dspName;
    private TextBox edtYear;
    private ComboBox cboHouse;
    private FastDataGridView grid;
    private Button btnNew;
    private Button btnSearch;
    private Button btnClose;
    private Button btnYearDown;
    private Button btnYearUp;
    private Panel panelTop;
    private Panel panelEdit;
    private Label lblCode;
    private Label lblYear;
    private Label lblHouse;

    private void InitializeComponent()
    {
        this.edtCode = new TextBox();
        this.dspName = new TextBox();
        this.edtYear = new TextBox();
        this.cboHouse = new ComboBox();
        this.grid = new FastDataGridView();
        this.btnNew = new Button();
        this.btnSearch = new Button();
        this.btnClose = new Button();
        this.btnYearDown = new Button();
        this.btnYearUp = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        this.lblCode = new Label();
        this.lblYear = new Label();
        this.lblHouse = new Label();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // dspName / cboHouse
        //
        this.dspName.ReadOnly = true;
        this.cboHouse.DropDownStyle = ComboBoxStyle.DropDownList;
        //
        // 버튼 텍스트
        //
        this.btnNew.Text = "초기화(F1)";
        this.btnSearch.Text = "조회(F5)";
        this.btnClose.Text = "닫기(Esc)";
        this.btnYearDown.Text = "◀";
        this.btnYearUp.Text = "▶";
        //
        // panelTop
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.panelTop.Controls.AddRange(new Control[] { this.btnNew, this.btnSearch, this.btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { this.btnNew, this.btnSearch, this.btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        //
        // panelEdit
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.lblCode.Text = "품번";
        this.lblCode.Left = 5; this.lblCode.Top = 12; this.lblCode.AutoSize = true;
        this.edtCode.Left = 50; this.edtCode.Top = 8; this.edtCode.Width = 100;
        this.edtCode.KeyDown += EdtCode_KeyDown;
        this.dspName.Left = 160; this.dspName.Top = 8; this.dspName.Width = 250;

        this.lblYear.Text = "년도";
        this.lblYear.Left = 420; this.lblYear.Top = 12; this.lblYear.AutoSize = true;
        this.edtYear.Left = 460; this.edtYear.Top = 8; this.edtYear.Width = 60;
        this.edtYear.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) LoadYear(); };
        this.btnYearDown.Left = 525; this.btnYearDown.Top = 8; this.btnYearDown.Width = 30;
        this.btnYearUp.Left = 558; this.btnYearUp.Top = 8; this.btnYearUp.Width = 30;
        this.btnYearDown.Click += (_, _) => { this.edtYear.Text = (PublicLib.StrToIntSafe(this.edtYear.Text) - 1).ToString(); LoadYear(); };
        this.btnYearUp.Click += (_, _) => { this.edtYear.Text = (PublicLib.StrToIntSafe(this.edtYear.Text) + 1).ToString(); LoadYear(); };

        this.lblHouse.Text = "저장위치";
        this.lblHouse.Left = 600; this.lblHouse.Top = 12; this.lblHouse.AutoSize = true;
        this.cboHouse.Left = 660; this.cboHouse.Top = 8; this.cboHouse.Width = 100;

        this.panelEdit.Controls.AddRange(new Control[]
        {
            this.lblCode, this.edtCode, this.dspName, this.lblYear, this.edtYear, this.btnYearDown, this.btnYearUp, this.lblHouse, this.cboHouse
        });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.Columns.Add("MONTH", "월");
        this.grid.Columns.Add("BQTY", "기초재고");
        this.grid.Columns.Add("IQTY", "입고수량");
        this.grid.Columns.Add("OQTY", "출고수량");
        this.grid.Columns.Add("XQTY", "재고조정");
        this.grid.Columns.Add("JQTY", "재고");
        this.grid.Columns.Add("IAMT", "입고금액");
        this.grid.Columns.Add("SAMT", "출고금액");
        this.grid.Columns.Add("OAMT", "보관금액");
        this.grid.Columns.Add("TAMT", "누계금액");
        //
        // 이벤트 배선
        //
        this.btnNew.Click += (_, _) => ClearEdit();
        this.btnSearch.Click += (_, _) => LoadYear();
        this.btnClose.Click += (_, _) => Close();
        //
        // JA02Form
        //
        this.Text = "재고관리-년간";
        this.Width = 900;
        this.Height = 600;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += (_, _) => { ResetHouseList(); ClearEdit(); };
        this.KeyDown += new KeyEventHandler(this.JA02Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
