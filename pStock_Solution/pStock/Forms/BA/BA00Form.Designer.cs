using pStock.Forms.Common;

namespace pStock.Forms.BA;

partial class BA00Form
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

    private TextBox edtSang;   // 상호
    private TextBox edtName;   // 대표자명
    private TextBox edtSa1;    // 사업자번호 앞3
    private TextBox edtSa2;    // 사업자번호 중2
    private TextBox edtSa3;    // 사업자번호 뒤5
    private TextBox edtNo1;    // 법인번호 앞6
    private TextBox edtNo2;    // 법인번호 뒤7
    private TextBox edtUptae;  // 업태
    private TextBox edtJong;   // 종목
    private TextBox edtPost1;  // 우편번호 앞3
    private TextBox edtPost2;  // 우편번호 뒤3
    private TextBox edtAddr;   // 주소
    private TextBox edtDDD;    // 지역번호
    private TextBox edtTel;
    private TextBox edtFax;
    private TextBox edtBigo;
    private Button btnSave;
    private Button btnClose;
    private Panel editPanel;
    private Button btnPost;
    private Label lblSang;
    private Label lblRep;
    private Label lblSano;
    private Label lblBnno;
    private Label lblUptae;
    private Label lblJong;
    private Label lblPost;
    private Label lblDDD;
    private Label lblTel;
    private Label lblFax;
    private Label lblAddr;
    private Label lblBigo;

    private void InitializeComponent()
    {
        this.edtSang = new TextBox();
        this.edtName = new TextBox();
        this.edtSa1 = new TextBox();
        this.edtSa2 = new TextBox();
        this.edtSa3 = new TextBox();
        this.edtNo1 = new TextBox();
        this.edtNo2 = new TextBox();
        this.edtUptae = new TextBox();
        this.edtJong = new TextBox();
        this.edtPost1 = new TextBox();
        this.edtPost2 = new TextBox();
        this.edtAddr = new TextBox();
        this.edtDDD = new TextBox();
        this.edtTel = new TextBox();
        this.edtFax = new TextBox();
        this.edtBigo = new TextBox();
        this.btnSave = new Button();
        this.btnClose = new Button();
        this.editPanel = new Panel();
        this.btnPost = new Button();
        this.lblSang = new Label();
        this.lblRep = new Label();
        this.lblSano = new Label();
        this.lblBnno = new Label();
        this.lblUptae = new Label();
        this.lblJong = new Label();
        this.lblPost = new Label();
        this.lblDDD = new Label();
        this.lblTel = new Label();
        this.lblFax = new Label();
        this.lblAddr = new Label();
        this.lblBigo = new Label();
        this.editPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // btnSave / btnClose
        //
        this.btnSave.Text = "저장(F2)";
        this.btnClose.Text = "닫기(Esc)";
        //
        // editPanel (원본 GridLayout(editPanel, 10, 10, slotWidth:290, labelWidth:80, rowHeight:30, slotsPerRow:2) 계산 결과를 리터럴로 반영)
        //
        this.editPanel.Dock = DockStyle.Fill;
        this.edtBigo.Multiline = true;
        this.edtBigo.Height = 60;

        this.lblSang.Text = "상호";
        this.lblSang.Left = 10; this.lblSang.Top = 13; this.lblSang.AutoSize = true;
        this.edtSang.Left = 90; this.edtSang.Top = 10; this.edtSang.Width = 198;

        this.lblRep.Text = "대표자";
        this.lblRep.Left = 300; this.lblRep.Top = 13; this.lblRep.AutoSize = true;
        this.edtName.Left = 380; this.edtName.Top = 10; this.edtName.Width = 198;

        this.lblSano.Text = "사업자번호";
        this.lblSano.Left = 10; this.lblSano.Top = 43; this.lblSano.AutoSize = true;
        this.edtSa1.Left = 90; this.edtSa1.Top = 40; this.edtSa1.Width = 50; this.edtSa1.MaxLength = 3;
        this.edtSa2.Left = 145; this.edtSa2.Top = 40; this.edtSa2.Width = 40; this.edtSa2.MaxLength = 2;
        this.edtSa3.Left = 190; this.edtSa3.Top = 40; this.edtSa3.Width = 60; this.edtSa3.MaxLength = 5;

        this.lblBnno.Text = "법인번호";
        this.lblBnno.Left = 300; this.lblBnno.Top = 43; this.lblBnno.AutoSize = true;
        this.edtNo1.Left = 380; this.edtNo1.Top = 40; this.edtNo1.Width = 80; this.edtNo1.MaxLength = 6;
        this.edtNo2.Left = 465; this.edtNo2.Top = 40; this.edtNo2.Width = 80; this.edtNo2.MaxLength = 7;

        this.lblUptae.Text = "업태";
        this.lblUptae.Left = 10; this.lblUptae.Top = 73; this.lblUptae.AutoSize = true;
        this.edtUptae.Left = 90; this.edtUptae.Top = 70; this.edtUptae.Width = 198;

        this.lblJong.Text = "종목";
        this.lblJong.Left = 300; this.lblJong.Top = 73; this.lblJong.AutoSize = true;
        this.edtJong.Left = 380; this.edtJong.Top = 70; this.edtJong.Width = 198;

        this.lblPost.Text = "우편번호";
        this.lblPost.Left = 10; this.lblPost.Top = 103; this.lblPost.AutoSize = true;
        this.edtPost1.Left = 90; this.edtPost1.Top = 100; this.edtPost1.Width = 50; this.edtPost1.MaxLength = 3;
        this.edtPost2.Left = 145; this.edtPost2.Top = 100; this.edtPost2.Width = 50; this.edtPost2.MaxLength = 3;
        this.btnPost.Text = "검색";
        this.btnPost.Left = 200; this.btnPost.Top = 98; this.btnPost.Width = 60;
        this.btnPost.Click += new EventHandler(this.BtnPost_Click);

        this.lblDDD.Text = "지역번호";
        this.lblDDD.Left = 300; this.lblDDD.Top = 103; this.lblDDD.AutoSize = true;
        this.edtDDD.Left = 380; this.edtDDD.Top = 100; this.edtDDD.Width = 198;

        this.lblTel.Text = "전화번호";
        this.lblTel.Left = 10; this.lblTel.Top = 133; this.lblTel.AutoSize = true;
        this.edtTel.Left = 90; this.edtTel.Top = 130; this.edtTel.Width = 198;

        this.lblFax.Text = "팩스번호";
        this.lblFax.Left = 300; this.lblFax.Top = 133; this.lblFax.AutoSize = true;
        this.edtFax.Left = 380; this.edtFax.Top = 130; this.edtFax.Width = 198;

        this.lblAddr.Text = "주소";
        this.lblAddr.Left = 10; this.lblAddr.Top = 163; this.lblAddr.AutoSize = true;
        this.edtAddr.Left = 90; this.edtAddr.Top = 160; this.edtAddr.Width = 488;
        this.edtAddr.DoubleClick += new EventHandler(this.EdtAddr_DoubleClick);

        this.lblBigo.Text = "비고";
        this.lblBigo.Left = 10; this.lblBigo.Top = 193; this.lblBigo.AutoSize = true;
        this.edtBigo.Left = 90; this.edtBigo.Top = 190; this.edtBigo.Width = 488;

        // 비고가 Multiline(높이 60)이라 grid.Bottom()이 계산하는 표준 행 높이(rowHeight=30)보다
        // 실제로 더 아래까지 차지한다. 원본은 비고의 실제 Bottom(190+60=250) 기준으로
        // 버튼 Y좌표를 잡았다(250+20=270).
        this.btnSave.Left = 220; this.btnSave.Top = 270; this.btnSave.Width = 100; this.btnSave.Height = 30;
        this.btnClose.Left = 330; this.btnClose.Top = 270; this.btnClose.Width = 100; this.btnClose.Height = 30;
        this.btnSave.Click += new EventHandler(this.BtnSave_Click);
        this.btnClose.Click += new EventHandler(this.BtnClose_Click);

        this.editPanel.Controls.AddRange(new Control[]
        {
            this.lblSang, this.edtSang,
            this.lblRep, this.edtName,
            this.lblSano, this.edtSa1, this.edtSa2, this.edtSa3,
            this.lblBnno, this.edtNo1, this.edtNo2,
            this.lblUptae, this.edtUptae,
            this.lblJong, this.edtJong,
            this.lblPost, this.edtPost1, this.edtPost2, this.btnPost,
            this.lblDDD, this.edtDDD,
            this.lblTel, this.edtTel,
            this.lblFax, this.edtFax,
            this.lblAddr, this.edtAddr,
            this.lblBigo, this.edtBigo,
            this.btnSave, this.btnClose
        });
        //
        // BA00Form
        //
        this.Text = "사업장 마스터";
        this.Width = 660;
        this.KeyPreview = true;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Controls.Add(this.editPanel);
        // 버튼 아래로 남는 여백 없이 딱 맞게 창 높이를 잡는다(가로폭은 기존 그대로 유지).
        this.ClientSize = new Size(this.ClientSize.Width, 320);
        this.Load += new EventHandler(this.BA00Form_Load);
        this.KeyDown += new KeyEventHandler(this.BA00Form_KeyDown);
        this.editPanel.ResumeLayout(false);
        this.editPanel.PerformLayout();
        this.ResumeLayout(false);
    }
}
