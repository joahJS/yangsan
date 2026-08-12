using pStock.Common;

namespace pStock.Forms.BA;

partial class BA02Form
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

    private FastDataGridView gridGroup;
    private FastDataGridView gridCode;
    private SplitContainer splitContainer;
    private RadioButton radModeGroup;
    private RadioButton radModeCode;
    private TextBox edtRcdtp;
    private TextBox edtRetxf;
    private Label lblRcdtp;
    private Label lblRetxf;
    private Label lblCode;
    private TextBox edtCode;
    private Label lblRetxs;
    private TextBox edtRetxs;
    private Button btnNew;
    private Button btnSave;
    private Button btnUpd;
    private Button btnDel;
    private Button btnSearch;
    private Button btnClose;
    private Panel panelTop;
    private Panel panelEdit;

    private void InitializeComponent()
    {
        this.gridGroup = new FastDataGridView();
        this.gridCode = new FastDataGridView();
        this.splitContainer = new SplitContainer();
        this.radModeGroup = new RadioButton();
        this.radModeCode = new RadioButton();
        this.lblRcdtp = new Label();
        this.edtRcdtp = new TextBox();
        this.lblRetxf = new Label();
        this.edtRetxf = new TextBox();
        this.lblCode = new Label();
        this.edtCode = new TextBox();
        this.lblRetxs = new Label();
        this.edtRetxs = new TextBox();
        this.btnNew = new Button();
        this.btnSave = new Button();
        this.btnUpd = new Button();
        this.btnDel = new Button();
        this.btnSearch = new Button();
        this.btnClose = new Button();
        this.panelTop = new Panel();
        this.panelEdit = new Panel();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
        this.splitContainer.Panel1.SuspendLayout();
        this.splitContainer.Panel2.SuspendLayout();
        this.splitContainer.SuspendLayout();
        this.panelTop.SuspendLayout();
        this.panelEdit.SuspendLayout();
        this.SuspendLayout();
        //
        // radModeGroup / radModeCode
        //
        this.radModeGroup.Text = "구분코드";
        this.radModeGroup.Checked = true;
        this.radModeGroup.AutoSize = true;
        this.radModeCode.Text = "코드";
        this.radModeCode.AutoSize = true;
        //
        // lblCode / lblRetxs (원본에서 필드로 선언돼 SyncEditFromGroup 등에서 참조되지는 않지만
        // ApplyMode()에서 Visible 토글 대상이라 그대로 필드 유지)
        //
        this.lblCode.Text = "코드";
        this.lblCode.AutoSize = true;
        this.lblRetxs.Text = "약칭(S)";
        this.lblRetxs.AutoSize = true;
        //
        // 상단 버튼 6개
        //
        this.btnNew.Text = "신규(F1)";
        this.btnSave.Text = "저장(F2)";
        this.btnUpd.Text = "수정(F3)";
        this.btnDel.Text = "삭제(F4)";
        this.btnSearch.Text = "조회";
        this.btnClose.Text = "닫기(Esc)";
        //
        // panelTop (원본 BuildLayout()의 top 패널: 버튼 6개를 95px 간격으로 가로 배치)
        //
        this.panelTop.Dock = DockStyle.Top;
        this.panelTop.Height = 40;
        this.btnNew.Left = 5;
        this.btnNew.Top = 8;
        this.btnNew.Width = 90;
        this.panelTop.Controls.Add(this.btnNew);
        this.btnSave.Left = 100;
        this.btnSave.Top = 8;
        this.btnSave.Width = 90;
        this.panelTop.Controls.Add(this.btnSave);
        this.btnUpd.Left = 195;
        this.btnUpd.Top = 8;
        this.btnUpd.Width = 90;
        this.panelTop.Controls.Add(this.btnUpd);
        this.btnDel.Left = 290;
        this.btnDel.Top = 8;
        this.btnDel.Width = 90;
        this.panelTop.Controls.Add(this.btnDel);
        this.btnSearch.Left = 385;
        this.btnSearch.Top = 8;
        this.btnSearch.Width = 90;
        this.panelTop.Controls.Add(this.btnSearch);
        this.btnClose.Left = 480;
        this.btnClose.Top = 8;
        this.btnClose.Width = 90;
        this.panelTop.Controls.Add(this.btnClose);
        //
        // panelEdit (원본 BuildLayout()의 editPanel: 라디오버튼 2개 + 라벨/입력란 4쌍을
        // 이전 컨트롤의 오른쪽 끝(Right) 기준으로 이어붙이던 좌표 계산을 그대로 유지)
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 40;
        this.radModeGroup.Left = 5;
        this.radModeGroup.Top = 12;
        this.panelEdit.Controls.Add(this.radModeGroup);
        this.radModeCode.Left = this.radModeGroup.Right + 10;
        this.radModeCode.Top = 12;
        this.panelEdit.Controls.Add(this.radModeCode);
        this.lblRcdtp.Text = "구분";
        this.lblRcdtp.AutoSize = true;
        this.lblRcdtp.Left = this.radModeCode.Right + 20;
        this.lblRcdtp.Top = 12;
        this.panelEdit.Controls.Add(this.lblRcdtp);
        this.edtRcdtp.Left = this.lblRcdtp.Right + 5;
        this.edtRcdtp.Top = 8;
        this.edtRcdtp.Width = 120;
        this.panelEdit.Controls.Add(this.edtRcdtp);
        this.lblRetxf.Text = "전체명";
        this.lblRetxf.AutoSize = true;
        this.lblRetxf.Left = this.edtRcdtp.Right + 20;
        this.lblRetxf.Top = 12;
        this.panelEdit.Controls.Add(this.lblRetxf);
        this.edtRetxf.Left = this.lblRetxf.Right + 5;
        this.edtRetxf.Top = 8;
        this.edtRetxf.Width = 120;
        this.panelEdit.Controls.Add(this.edtRetxf);
        this.lblCode.Left = this.edtRetxf.Right + 20;
        this.lblCode.Top = 12;
        this.panelEdit.Controls.Add(this.lblCode);
        this.edtCode.Left = this.lblCode.Right + 5;
        this.edtCode.Top = 8;
        this.edtCode.Width = 120;
        this.panelEdit.Controls.Add(this.edtCode);
        this.lblRetxs.Left = this.edtCode.Right + 20;
        this.lblRetxs.Top = 12;
        this.panelEdit.Controls.Add(this.lblRetxs);
        this.edtRetxs.Left = this.lblRetxs.Right + 5;
        this.edtRetxs.Top = 8;
        this.edtRetxs.Width = 120;
        this.panelEdit.Controls.Add(this.edtRetxs);
        //
        // gridGroup / gridCode / splitContainer
        //
        this.gridGroup.Dock = DockStyle.Fill;
        this.gridGroup.ReadOnly = true;
        this.gridGroup.AllowUserToAddRows = false;
        this.gridCode.Dock = DockStyle.Fill;
        this.gridCode.ReadOnly = true;
        this.gridCode.AllowUserToAddRows = false;
        this.splitContainer.Dock = DockStyle.Fill;
        this.splitContainer.FixedPanel = FixedPanel.Panel1;
        this.splitContainer.Panel1.Controls.Add(this.gridGroup);
        this.splitContainer.Panel2.Controls.Add(this.gridCode);
        //
        // 이벤트 배선 (원본 BuildLayout()에서 그대로 이동)
        //
        this.radModeGroup.CheckedChanged += (_, _) => ApplyMode();
        this.radModeCode.CheckedChanged += (_, _) => ApplyMode();
        this.gridGroup.SelectionChanged += new System.EventHandler(this.GridGroup_SelectionChanged);
        this.gridGroup.CellDoubleClick += (_, _) => SyncEditFromGroup();
        this.gridCode.CellDoubleClick += (_, _) => SyncEditFromCode();
        this.btnNew.Click += (_, _) => { ClearEdit(); (radModeCode.Checked ? edtCode : edtRcdtp).Focus(); };
        this.btnSave.Click += (_, _) => Save(isInsert: true);
        this.btnUpd.Click += (_, _) => Save(isInsert: false);
        this.btnDel.Click += (_, _) => Delete();
        this.btnSearch.Click += (_, _) => Search();
        this.btnClose.Click += (_, _) => Close();
        //
        // BA02Form
        //
        this.Text = "공통코드 마스터";
        this.Width = 1200;
        this.Height = 800;
        this.KeyPreview = true;
        this.Controls.Add(this.splitContainer);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        this.Load += (_, _) =>
        {
            LoadGroup();
            // 생성자 시점에는 splitContainer가 실제 화면 크기를 아직 갖지 못해 SplitterDistance를
            // 픽셀값으로 바로 지정하면 이후 폼이 리사이즈될 때 비율이 깨지면서(작은 초기값 기준으로
            // 재계산되어) 오른쪽에 큰 빈 여백이 생기는 문제가 있었다. 폼이 실제 크기를 가진 뒤인
            // Load 시점에 왼쪽(그룹) 그리드 컬럼들이 다 보일 정도로만 폭을 잡아준다.
            this.splitContainer.SplitterDistance = 560;
        };
        this.KeyDown += new KeyEventHandler(this.BA02Form_KeyDown);
        this.splitContainer.Panel1.ResumeLayout(false);
        this.splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
        this.splitContainer.ResumeLayout(false);
        this.panelTop.ResumeLayout(false);
        this.panelEdit.ResumeLayout(false);
        this.panelEdit.PerformLayout();
        this.ResumeLayout(false);
    }
}
