using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA01Form
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
    private ComboBox cboGu;
    private CheckBox chkAuto;
    private TextBox edtCode;
    private TextBox edtName;
    private TextBox edtOwnam;
    private TextBox edtSano;   // 사업자번호
    private TextBox edtCvno;   // 법인번호
    private TextBox edtUptae;  // 업태
    private TextBox edtJongk;  // 종목
    private TextBox edtPost;   // 우편번호
    private TextBox edtDDD;    // 지역번호
    private TextBox edtTel;
    private TextBox edtFax;
    private TextBox edtAddr;
    private TextBox edtSdate;  // 시작일
    private TextBox edtEdate;  // 종료일
    private TextBox edtBigo;   // 비고
    private TextBox edtWord;   // 검색어

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
    private Label lblGu;
    private Label lblCode;
    private Label lblName;
    private Label lblOwnam;
    private Label lblSano;
    private Label lblCvno;
    private Label lblUptae;
    private Label lblJongk;
    private Label lblPost;
    private Label lblAddr;
    private Label lblDDD;
    private Label lblTel;
    private Label lblFax;
    private Label lblSdate;
    private Label lblEdate;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.cboGu = new ComboBox();
        this.chkAuto = new CheckBox();
        this.edtCode = new TextBox();
        this.edtName = new TextBox();
        this.edtOwnam = new TextBox();
        this.edtSano = new TextBox();
        this.edtCvno = new TextBox();
        this.edtUptae = new TextBox();
        this.edtJongk = new TextBox();
        this.edtPost = new TextBox();
        this.edtDDD = new TextBox();
        this.edtTel = new TextBox();
        this.edtFax = new TextBox();
        this.edtAddr = new TextBox();
        this.edtSdate = new TextBox();
        this.edtEdate = new TextBox();
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
        this.lblGu = new Label();
        this.lblCode = new Label();
        this.lblName = new Label();
        this.lblOwnam = new Label();
        this.lblSano = new Label();
        this.lblCvno = new Label();
        this.lblUptae = new Label();
        this.lblJongk = new Label();
        this.lblPost = new Label();
        this.lblAddr = new Label();
        this.lblDDD = new Label();
        this.lblTel = new Label();
        this.lblFax = new Label();
        this.lblSdate = new Label();
        this.lblEdate = new Label();
        this.lblBigo = new Label();
        this.panelTop.SuspendLayout();
        this.searchPanel.SuspendLayout();
        this.editPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // cboGu / chkAuto / btnNew / btnAdd / btnOne / btnDel / btnExcel / btnPrint / btnClose / lblDbCnt
        //
        this.cboGu.DropDownStyle = ComboBoxStyle.DropDownList;
        this.cboGu.Items.AddRange(new object[] { "1. 매입처", "2. 매출처", "3. 공통" });
        this.chkAuto.Text = "자동채번";
        this.btnNew.Text = "신규(F1)";
        this.btnAdd.Text = "저장(F2)";
        this.btnOne.Text = "수정(F3)";
        this.btnDel.Text = "삭제(F4)";
        this.btnExcel.Text = "엑셀저장";
        this.btnPrint.Text = "인쇄";
        this.btnClose.Text = "닫기(Esc)";
        this.lblDbCnt.AutoSize = true;
        //
        // panelTop (원본 GridLayout 없이 고정 좌표로 배치된 영역)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnAdd.Left = 100; this.btnAdd.Top = 8; this.btnAdd.Width = 90;
        this.btnOne.Left = 195; this.btnOne.Top = 8; this.btnOne.Width = 90;
        this.btnDel.Left = 290; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnExcel.Left = 385; this.btnExcel.Top = 8; this.btnExcel.Width = 90;
        this.btnPrint.Left = 480; this.btnPrint.Top = 8; this.btnPrint.Width = 90;
        this.btnClose.Left = 575; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.lblDbCnt.Left = 690; this.lblDbCnt.Top = 14;
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
        this.searchPanel.Controls.AddRange(new Control[] { this.lblWord, this.edtWord });
        this.edtWord.KeyUp += new KeyEventHandler(this.EdtWord_KeyUp);
        //
        // editPanel (원본 GridLayout(editPanel, 5, 5, slotWidth:250, labelWidth:85, rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영)
        //
        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 230;
        this.editPanel.AutoScroll = true;

        this.lblGu.Text = "구분";
        this.lblGu.Left = 5; this.lblGu.Top = 8; this.lblGu.AutoSize = true;
        this.cboGu.Left = 90; this.cboGu.Top = 5; this.cboGu.Width = 153;

        this.lblCode.Text = "코드";
        this.lblCode.Left = 255; this.lblCode.Top = 8; this.lblCode.AutoSize = true;
        this.edtCode.Left = 340; this.edtCode.Top = 5; this.edtCode.Width = 153;

        this.chkAuto.Left = 505; this.chkAuto.Top = 7; this.chkAuto.Width = 100;

        this.lblName.Text = "거래처명";
        this.lblName.Left = 5; this.lblName.Top = 38; this.lblName.AutoSize = true;
        this.edtName.Left = 90; this.edtName.Top = 35; this.edtName.Width = 153;

        this.lblOwnam.Text = "대표자";
        this.lblOwnam.Left = 255; this.lblOwnam.Top = 38; this.lblOwnam.AutoSize = true;
        this.edtOwnam.Left = 340; this.edtOwnam.Top = 35; this.edtOwnam.Width = 153;

        this.lblSano.Text = "사업자번호";
        this.lblSano.Left = 505; this.lblSano.Top = 38; this.lblSano.AutoSize = true;
        this.edtSano.Left = 590; this.edtSano.Top = 35; this.edtSano.Width = 153;

        this.lblCvno.Text = "법인번호";
        this.lblCvno.Left = 5; this.lblCvno.Top = 68; this.lblCvno.AutoSize = true;
        this.edtCvno.Left = 90; this.edtCvno.Top = 65; this.edtCvno.Width = 153;

        this.lblUptae.Text = "업태";
        this.lblUptae.Left = 255; this.lblUptae.Top = 68; this.lblUptae.AutoSize = true;
        this.edtUptae.Left = 340; this.edtUptae.Top = 65; this.edtUptae.Width = 153;

        this.lblJongk.Text = "종목";
        this.lblJongk.Left = 505; this.lblJongk.Top = 68; this.lblJongk.AutoSize = true;
        this.edtJongk.Left = 590; this.edtJongk.Top = 65; this.edtJongk.Width = 153;

        this.lblPost.Text = "우편번호";
        this.lblPost.Left = 5; this.lblPost.Top = 98; this.lblPost.AutoSize = true;
        this.edtPost.Left = 90; this.edtPost.Top = 95; this.edtPost.Width = 153;

        this.lblAddr.Text = "주소";
        this.lblAddr.Left = 255; this.lblAddr.Top = 98; this.lblAddr.AutoSize = true;
        this.edtAddr.Left = 340; this.edtAddr.Top = 95; this.edtAddr.Width = 403;

        this.lblDDD.Text = "지역번호";
        this.lblDDD.Left = 5; this.lblDDD.Top = 128; this.lblDDD.AutoSize = true;
        this.edtDDD.Left = 90; this.edtDDD.Top = 125; this.edtDDD.Width = 153;

        this.lblTel.Text = "전화번호";
        this.lblTel.Left = 255; this.lblTel.Top = 128; this.lblTel.AutoSize = true;
        this.edtTel.Left = 340; this.edtTel.Top = 125; this.edtTel.Width = 153;

        this.lblFax.Text = "팩스번호";
        this.lblFax.Left = 505; this.lblFax.Top = 128; this.lblFax.AutoSize = true;
        this.edtFax.Left = 590; this.edtFax.Top = 125; this.edtFax.Width = 153;

        this.lblSdate.Text = "시작일";
        this.lblSdate.Left = 5; this.lblSdate.Top = 158; this.lblSdate.AutoSize = true;
        this.edtSdate.Left = 90; this.edtSdate.Top = 155; this.edtSdate.Width = 153;

        this.lblEdate.Text = "종료일";
        this.lblEdate.Left = 255; this.lblEdate.Top = 158; this.lblEdate.AutoSize = true;
        this.edtEdate.Left = 340; this.edtEdate.Top = 155; this.edtEdate.Width = 153;

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 5; this.lblBigo.Top = 188; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 90; this.edtBigo.Top = 185; this.edtBigo.Width = 653;

        this.editPanel.Controls.AddRange(new Control[]
        {
            this.lblGu, this.cboGu,
            this.lblCode, this.edtCode,
            this.chkAuto,
            this.lblName, this.edtName,
            this.lblOwnam, this.edtOwnam,
            this.lblSano, this.edtSano,
            this.lblCvno, this.edtCvno,
            this.lblUptae, this.edtUptae,
            this.lblJongk, this.edtJongk,
            this.lblPost, this.edtPost,
            this.lblAddr, this.edtAddr,
            this.lblDDD, this.edtDDD,
            this.lblTel, this.edtTel,
            this.lblFax, this.edtFax,
            this.lblSdate, this.edtSdate,
            this.lblEdate, this.edtEdate,
            this.lblBigo, this.edtBigo
        });
        this.edtPost.DoubleClick += new EventHandler(this.EdtPost_DoubleClick);
        this.edtAddr.DoubleClick += new EventHandler(this.EdtAddr_DoubleClick);
        this.chkAuto.Click += new EventHandler(this.ChkAuto_Click);
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellDoubleClick += new DataGridViewCellEventHandler(this.Grid_CellDoubleClick);
        this.grid.KeyDown += new KeyEventHandler(this.Grid_KeyDown);
        //
        // BA01Form
        //
        this.Text = "거래처 마스터";
        this.Width = 1100;
        this.Height = 700;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.searchPanel);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.BA01Form_Load);
        this.KeyDown += new KeyEventHandler(this.BA01Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.searchPanel.ResumeLayout(false);
        this.searchPanel.PerformLayout();
        this.editPanel.ResumeLayout(false);
        this.editPanel.PerformLayout();
        this.ResumeLayout(false);
    }
}
