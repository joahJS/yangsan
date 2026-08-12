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
    private Label lblCvcod;
    private Label lblCvnam;
    private Label lblCode;
    private Label lblItdsc;
    private Label lblSpec;
    private Label lblDanwi;
    private Label lblItwgt;
    private Label lblSavLoc;
    private Label lblIcost;
    private Label lblBcost;
    private Label lblOcost;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtCvcod = new TextBox();
        this.edtCvnam = new TextBox();
        this.edtCode = new TextBox();
        this.chkAuto = new CheckBox();
        this.edtItdsc = new TextBox();
        this.edtSpec = new TextBox();
        this.cboDanwi = new ComboBox();
        this.eItwgt = new NumericUpDown();
        this.edtIcost = new NumericUpDown();
        this.edtBcost = new NumericUpDown();
        this.edtOcost = new NumericUpDown();
        this.cboSavLoc = new ComboBox();
        this.edtBigo = new TextBox();
        this.edtWord = new TextBox();
        this.btnNew = new Button();
        this.btnAdd = new Button();
        this.btnOne = new Button();
        this.btnDel = new Button();
        this.btnExcel = new Button();
        this.btnPrint = new Button();
        this.btnClose = new Button();
        this.lblDbCnt = new Label();
        this.panelTop = new Panel();
        this.searchPanel = new Panel();
        this.lblWord = new Label();
        this.editPanel = new Panel();
        this.lblCvcod = new Label();
        this.lblCvnam = new Label();
        this.lblCode = new Label();
        this.lblItdsc = new Label();
        this.lblSpec = new Label();
        this.lblDanwi = new Label();
        this.lblItwgt = new Label();
        this.lblSavLoc = new Label();
        this.lblIcost = new Label();
        this.lblBcost = new Label();
        this.lblOcost = new Label();
        this.lblBigo = new Label();
        this.panelTop.SuspendLayout();
        this.searchPanel.SuspendLayout();
        this.editPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // chkAuto / cboDanwi / eItwgt / edtIcost / edtBcost / edtOcost / cboSavLoc / btnNew / btnAdd / btnOne / btnDel / btnExcel / btnPrint / btnClose / lblDbCnt
        //
        this.chkAuto.Text = "자동채번";
        this.cboDanwi.DropDownStyle = ComboBoxStyle.DropDownList;
        this.eItwgt.DecimalPlaces = 2;
        this.eItwgt.Maximum = 999999;
        this.edtIcost.DecimalPlaces = 0;
        this.edtIcost.Maximum = 999999999;
        this.edtBcost.DecimalPlaces = 0;
        this.edtBcost.Maximum = 999999999;
        this.edtOcost.DecimalPlaces = 0;
        this.edtOcost.Maximum = 999999999;
        this.cboSavLoc.DropDownStyle = ComboBoxStyle.DropDownList;
        this.btnNew.Text = "신규(F1)";
        this.btnAdd.Text = "연속저장(F2)";
        this.btnOne.Text = "저장(F3)";
        this.btnDel.Text = "삭제(F4)";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";
        this.lblDbCnt.AutoSize = true;
        //
        // panelTop
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 100;
        this.btnAdd.Left = 110; this.btnAdd.Top = 8; this.btnAdd.Width = 100;
        this.btnOne.Left = 215; this.btnOne.Top = 8; this.btnOne.Width = 100;
        this.btnDel.Left = 320; this.btnDel.Top = 8; this.btnDel.Width = 100;
        this.btnExcel.Left = 425; this.btnExcel.Top = 8; this.btnExcel.Width = 100;
        this.btnPrint.Left = 530; this.btnPrint.Top = 8; this.btnPrint.Width = 100;
        this.btnClose.Left = 635; this.btnClose.Top = 8; this.btnClose.Width = 100;
        this.lblDbCnt.Left = 760; this.lblDbCnt.Top = 14;
        this.panelTop.Controls.AddRange(new Control[]
        {
            this.btnNew, this.btnAdd, this.btnOne, this.btnDel,
            this.btnExcel, this.btnPrint, this.btnClose, this.lblDbCnt
        });
        this.btnExcel.Click += new EventHandler(this.BtnExcel_Click);
        this.btnPrint.Click += new EventHandler(this.BtnPrint_Click);
        this.btnNew.Click += new EventHandler(this.BtnNew_Click);
        this.btnAdd.Click += new EventHandler(this.BtnAdd_Click);
        this.btnOne.Click += new EventHandler(this.BtnOne_Click);
        this.btnDel.Click += new EventHandler(this.BtnDel_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);
        //
        // searchPanel
        //
        this.searchPanel.Dock = DockStyle.Top;
        this.searchPanel.Height = 35;
        this.lblWord.Text = "검색어";
        this.lblWord.Left = 5; this.lblWord.Top = 10; this.lblWord.AutoSize = true;
        this.edtWord.Left = 60; this.edtWord.Top = 6; this.edtWord.Width = 200;
        this.edtWord.KeyUp += new KeyEventHandler(this.EdtWord_KeyUp);
        this.searchPanel.Controls.AddRange(new Control[] { this.lblWord, this.edtWord });
        //
        // editPanel (원본 GridLayout(editPanel, 5, 5, slotWidth:250, labelWidth:85, rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영)
        //
        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 200;
        this.editPanel.AutoScroll = true;

        this.lblCvcod.Text = "거래처코드";
        this.lblCvcod.Left = 5; this.lblCvcod.Top = 8; this.lblCvcod.AutoSize = true;
        this.edtCvcod.Left = 90; this.edtCvcod.Top = 5; this.edtCvcod.Width = 153;
        this.edtCvcod.KeyDown += new KeyEventHandler(this.EdtCvcod_KeyDown);

        this.lblCvnam.Text = "거래처명";
        this.lblCvnam.Left = 255; this.lblCvnam.Top = 8; this.lblCvnam.AutoSize = true;
        this.edtCvnam.ReadOnly = true;
        this.edtCvnam.Left = 340; this.edtCvnam.Top = 5; this.edtCvnam.Width = 403;

        this.lblCode.Text = "품번";
        this.lblCode.Left = 5; this.lblCode.Top = 38; this.lblCode.AutoSize = true;
        this.edtCode.Left = 90; this.edtCode.Top = 35; this.edtCode.Width = 153;

        this.chkAuto.Left = 255; this.chkAuto.Top = 37; this.chkAuto.Width = 100;
        this.chkAuto.Click += new EventHandler(this.ChkAuto_Click);

        this.lblItdsc.Text = "품명";
        this.lblItdsc.Left = 5; this.lblItdsc.Top = 68; this.lblItdsc.AutoSize = true;
        this.edtItdsc.Left = 90; this.edtItdsc.Top = 65; this.edtItdsc.Width = 403;

        this.lblSpec.Text = "규격";
        this.lblSpec.Left = 505; this.lblSpec.Top = 68; this.lblSpec.AutoSize = true;
        this.edtSpec.Left = 590; this.edtSpec.Top = 65; this.edtSpec.Width = 153;

        this.lblDanwi.Text = "단위";
        this.lblDanwi.Left = 5; this.lblDanwi.Top = 98; this.lblDanwi.AutoSize = true;
        this.cboDanwi.Left = 90; this.cboDanwi.Top = 95; this.cboDanwi.Width = 153;

        this.lblItwgt.Text = "중량";
        this.lblItwgt.Left = 255; this.lblItwgt.Top = 98; this.lblItwgt.AutoSize = true;
        this.eItwgt.Left = 340; this.eItwgt.Top = 95; this.eItwgt.Width = 153;

        this.lblSavLoc.Text = "저장위치";
        this.lblSavLoc.Left = 505; this.lblSavLoc.Top = 98; this.lblSavLoc.AutoSize = true;
        this.cboSavLoc.Left = 590; this.cboSavLoc.Top = 95; this.cboSavLoc.Width = 153;

        this.lblIcost.Text = "입고단가";
        this.lblIcost.Left = 5; this.lblIcost.Top = 128; this.lblIcost.AutoSize = true;
        this.edtIcost.Left = 90; this.edtIcost.Top = 125; this.edtIcost.Width = 153;

        this.lblBcost.Text = "기준단가";
        this.lblBcost.Left = 255; this.lblBcost.Top = 128; this.lblBcost.AutoSize = true;
        this.edtBcost.Left = 340; this.edtBcost.Top = 125; this.edtBcost.Width = 153;

        this.lblOcost.Text = "출고단가";
        this.lblOcost.Left = 505; this.lblOcost.Top = 128; this.lblOcost.AutoSize = true;
        this.edtOcost.Left = 590; this.edtOcost.Top = 125; this.edtOcost.Width = 153;

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 5; this.lblBigo.Top = 158; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 90; this.edtBigo.Top = 155; this.edtBigo.Width = 653;

        this.editPanel.Controls.AddRange(new Control[]
        {
            this.lblCvcod, this.edtCvcod,
            this.lblCvnam, this.edtCvnam,
            this.lblCode, this.edtCode,
            this.chkAuto,
            this.lblItdsc, this.edtItdsc,
            this.lblSpec, this.edtSpec,
            this.lblDanwi, this.cboDanwi,
            this.lblItwgt, this.eItwgt,
            this.lblSavLoc, this.cboSavLoc,
            this.lblIcost, this.edtIcost,
            this.lblBcost, this.edtBcost,
            this.lblOcost, this.edtOcost,
            this.lblBigo, this.edtBigo
        });
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += new DataGridViewCellEventHandler(this.Grid_CellDoubleClick);
        this.grid.KeyDown += new KeyEventHandler(this.Grid_KeyDown);
        //
        // BA03Form
        //
        this.Text = "품목,단가 마스터";
        this.Width = 1100;
        this.Height = 650;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.searchPanel);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.BA03Form_Load);
        this.KeyDown += new KeyEventHandler(this.BA03Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.searchPanel.ResumeLayout(false);
        this.searchPanel.PerformLayout();
        this.editPanel.ResumeLayout(false);
        this.editPanel.PerformLayout();
        this.ResumeLayout(false);
    }
}
