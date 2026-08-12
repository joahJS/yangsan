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
        this.btnSave = new Button() { Text = "저장(F2)" };
        this.btnClose = new Button() { Text = "닫기(Esc)" };
        this.editPanel = new Panel();
        this.btnPost = new Button();
        this.SuspendLayout();
        //
        // BA00Form (원본 생성자에서 BuildLayout() 호출 전에 설정하던 폼 속성)
        //
        this.Text = "사업장 마스터";
        this.Width = 660;
        this.KeyPreview = true;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        //
        // 원본 BuildLayout()
        //
        this.edtBigo.Multiline = true;
        this.edtBigo.Height = 60;

        this.editPanel.Dock = DockStyle.Fill;
        var grid = new GridLayout(this.editPanel, 10, 10, slotWidth: 290, labelWidth: 80, rowHeight: 30, slotsPerRow: 2);

        grid.Add("상호", this.edtSang);
        grid.Add("대표자", this.edtName);

        grid.Add("사업자번호", this.edtSa1, editWidth: 50);
        this.edtSa1.MaxLength = 3;
        this.edtSa2.Left = this.edtSa1.Right + 5; this.edtSa2.Top = this.edtSa1.Top; this.edtSa2.Width = 40; this.edtSa2.MaxLength = 2;
        this.edtSa3.Left = this.edtSa2.Right + 5; this.edtSa3.Top = this.edtSa1.Top; this.edtSa3.Width = 60; this.edtSa3.MaxLength = 5;
        this.editPanel.Controls.AddRange(new Control[] { this.edtSa2, this.edtSa3 });

        grid.Add("법인번호", this.edtNo1, editWidth: 80);
        this.edtNo1.MaxLength = 6;
        this.edtNo2.Left = this.edtNo1.Right + 5; this.edtNo2.Top = this.edtNo1.Top; this.edtNo2.Width = 80; this.edtNo2.MaxLength = 7;
        this.editPanel.Controls.Add(this.edtNo2);

        grid.Add("업태", this.edtUptae);
        grid.Add("종목", this.edtJong);

        grid.Add("우편번호", this.edtPost1, editWidth: 50);
        this.edtPost1.MaxLength = 3;
        this.edtPost2.Left = this.edtPost1.Right + 5; this.edtPost2.Top = this.edtPost1.Top; this.edtPost2.Width = 50; this.edtPost2.MaxLength = 3;
        this.btnPost.Text = "검색";
        this.btnPost.Left = this.edtPost2.Right + 5;
        this.btnPost.Top = this.edtPost1.Top - 2;
        this.btnPost.Width = 60;
        this.btnPost.Click += (_, _) => LookupPostalCode();
        this.editPanel.Controls.AddRange(new Control[] { this.edtPost2, this.btnPost });

        grid.Add("지역번호", this.edtDDD);

        grid.Add("전화번호", this.edtTel);
        grid.Add("팩스번호", this.edtFax);

        grid.NewRow();
        grid.Add("주소", this.edtAddr, span: 2);
        this.edtAddr.DoubleClick += (_, _) => LookupPostalCode();

        grid.NewRow();
        grid.Add("비고", this.edtBigo, span: 2);

        // 비고가 Multiline(높이 60)이라 grid.Bottom()이 계산하는 표준 행 높이(rowHeight=30)보다
        // 실제로 더 아래까지 차지한다. 그 값 그대로 버튼 Y좌표를 잡으면 버튼이 비고 입력란에
        // 가려지므로, 비고의 실제 Bottom을 기준으로 버튼 위치를 잡는다.
        int y = this.edtBigo.Bottom + 20;
        this.btnSave.Left = 220; this.btnSave.Top = y; this.btnSave.Width = 100; this.btnSave.Height = 30;
        this.btnClose.Left = 330; this.btnClose.Top = y; this.btnClose.Width = 100; this.btnClose.Height = 30;
        this.editPanel.Controls.AddRange(new Control[] { this.btnSave, this.btnClose });

        this.Controls.Add(this.editPanel);

        // 버튼 아래로 남는 여백 없이 딱 맞게 창 높이를 잡는다(가로폭은 기존 그대로 유지).
        this.ClientSize = new Size(this.ClientSize.Width, y + this.btnSave.Height + 20);

        this.btnSave.Click += (_, _) => Save();
        this.btnClose.Click += (_, _) => Close();

        this.Load += (_, _) => LoadCompanyInfo();
        this.KeyDown += new KeyEventHandler(this.BA00Form_KeyDown);
        this.ResumeLayout(false);
    }
}
