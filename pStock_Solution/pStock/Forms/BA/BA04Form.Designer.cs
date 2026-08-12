using pStock.Common;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA04Form
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
    private TextBox edtCode;
    private CheckBox chkAuto;
    private TextBox edtName;
    private TextBox edtPost;
    private TextBox edtAddr1;
    private TextBox edtAddr2;
    private TextBox edtTel;
    private TextBox edtBigo;
    private TextBox edtWord;

    private Button btnNew;
    private Button btnAdd;
    private Button btnOne;
    private Button btnDel;
    private Button btnClose;
    private Label lblDbCnt;
    private Panel panelTop;
    private Panel searchPanel;
    private Label lblWord;
    private Panel editPanel;
    private Label lblCode;
    private Label lblName;
    private Label lblPost;
    private Label lblTel;
    private Label lblAddr1;
    private Label lblAddr2;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.grid = new FastDataGridView();
        this.edtCode = new TextBox();
        this.chkAuto = new CheckBox();
        this.edtName = new TextBox();
        this.edtPost = new TextBox();
        this.edtAddr1 = new TextBox();
        this.edtAddr2 = new TextBox();
        this.edtTel = new TextBox();
        this.edtBigo = new TextBox();
        this.edtWord = new TextBox();
        this.btnNew = new Button();
        this.btnAdd = new Button();
        this.btnOne = new Button();
        this.btnDel = new Button();
        this.btnClose = new Button();
        this.lblDbCnt = new Label();
        this.panelTop = new Panel();
        this.searchPanel = new Panel();
        this.lblWord = new Label();
        this.editPanel = new Panel();
        this.lblCode = new Label();
        this.lblName = new Label();
        this.lblPost = new Label();
        this.lblTel = new Label();
        this.lblAddr1 = new Label();
        this.lblAddr2 = new Label();
        this.lblBigo = new Label();
        this.panelTop.SuspendLayout();
        this.searchPanel.SuspendLayout();
        this.editPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // chkAuto / btnNew / btnAdd / btnOne / btnDel / btnClose / lblDbCnt
        //
        this.chkAuto.Text = "자동채번";
        this.btnNew.Text = "신규(F1)";
        this.btnAdd.Text = "저장(F2)";
        this.btnOne.Text = "수정(F3)";
        this.btnDel.Text = "삭제(F4)";
        this.btnClose.Text = "닫기(Esc)";
        this.lblDbCnt.AutoSize = true;
        //
        // panelTop
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Left = 5; this.btnNew.Top = 8; this.btnNew.Width = 90;
        this.btnAdd.Left = 100; this.btnAdd.Top = 8; this.btnAdd.Width = 90;
        this.btnOne.Left = 195; this.btnOne.Top = 8; this.btnOne.Width = 90;
        this.btnDel.Left = 290; this.btnDel.Top = 8; this.btnDel.Width = 90;
        this.btnClose.Left = 385; this.btnClose.Top = 8; this.btnClose.Width = 90;
        this.lblDbCnt.Left = 500; this.lblDbCnt.Top = 14;
        this.panelTop.Controls.AddRange(new Control[]
        {
            this.btnNew, this.btnAdd, this.btnOne, this.btnDel, this.btnClose, this.lblDbCnt
        });
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
        // editPanel (원본 GridLayout(editPanel, 5, 5, slotWidth:250, labelWidth:80, rowHeight:30, slotsPerRow:3) 계산 결과를 리터럴로 반영)
        //
        this.editPanel.Dock = DockStyle.Top;
        this.editPanel.Height = 170;
        this.editPanel.AutoScroll = true;

        this.lblCode.Text = "코드";
        this.lblCode.Left = 5; this.lblCode.Top = 8; this.lblCode.AutoSize = true;
        this.edtCode.Left = 85; this.edtCode.Top = 5; this.edtCode.Width = 158;

        this.chkAuto.Left = 255; this.chkAuto.Top = 7; this.chkAuto.Width = 100;
        this.chkAuto.Click += new EventHandler(this.ChkAuto_Click);

        this.lblName.Text = "착지처명";
        this.lblName.Left = 5; this.lblName.Top = 38; this.lblName.AutoSize = true;
        this.edtName.Left = 85; this.edtName.Top = 35; this.edtName.Width = 158;

        this.lblPost.Text = "우편번호";
        this.lblPost.Left = 255; this.lblPost.Top = 38; this.lblPost.AutoSize = true;
        this.edtPost.Left = 335; this.edtPost.Top = 35; this.edtPost.Width = 158;

        this.lblTel.Text = "전화번호";
        this.lblTel.Left = 505; this.lblTel.Top = 38; this.lblTel.AutoSize = true;
        this.edtTel.Left = 585; this.edtTel.Top = 35; this.edtTel.Width = 158;

        this.lblAddr1.Text = "주소1";
        this.lblAddr1.Left = 5; this.lblAddr1.Top = 68; this.lblAddr1.AutoSize = true;
        this.edtAddr1.Left = 85; this.edtAddr1.Top = 65; this.edtAddr1.Width = 408;
        this.edtAddr1.DoubleClick += new EventHandler(this.EdtAddr1_DoubleClick);

        this.lblAddr2.Text = "주소2";
        this.lblAddr2.Left = 5; this.lblAddr2.Top = 98; this.lblAddr2.AutoSize = true;
        this.edtAddr2.Left = 85; this.edtAddr2.Top = 95; this.edtAddr2.Width = 408;

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 5; this.lblBigo.Top = 128; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 85; this.edtBigo.Top = 125; this.edtBigo.Width = 408;

        this.editPanel.Controls.AddRange(new Control[]
        {
            this.lblCode, this.edtCode,
            this.chkAuto,
            this.lblName, this.edtName,
            this.lblPost, this.edtPost,
            this.lblTel, this.edtTel,
            this.lblAddr1, this.edtAddr1,
            this.lblAddr2, this.edtAddr2,
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
        // BA04Form
        //
        this.Text = "착지처 마스터";
        this.Width = 950;
        this.Height = 600;
        this.KeyPreview = true;
        this.Controls.Add(this.grid);
        this.Controls.Add(this.editPanel);
        this.Controls.Add(this.searchPanel);
        this.Controls.Add(this.panelTop);
        this.Load += new EventHandler(this.BA04Form_Load);
        this.KeyDown += new KeyEventHandler(this.BA04Form_KeyDown);
        this.panelTop.ResumeLayout(false);
        this.searchPanel.ResumeLayout(false);
        this.searchPanel.PerformLayout();
        this.editPanel.ResumeLayout(false);
        this.editPanel.PerformLayout();
        this.ResumeLayout(false);
    }
}
