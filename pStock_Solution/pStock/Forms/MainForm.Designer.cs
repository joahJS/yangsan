using Guna.UI2.WinForms;

namespace pStock.Forms;

partial class MainForm
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

    private Guna2Panel sidebarPanel;
    private Guna2Panel logoPanel;
    private Guna2Panel logoBadge;
    private Label logoText;
    private FlowLayoutPanel navFlow;
    private Guna2Button btnNavMain;
    private Guna2Button btnNavStock;
    private FlowLayoutPanel subStock;
    private Guna2Button btnNavFlow;
    private FlowLayoutPanel subFlow;
    private Guna2Button btnNavSales;
    private FlowLayoutPanel subSales;
    private Guna2Button btnNavClose;
    private FlowLayoutPanel subClose;
    private Guna2Button btnNavBase;
    private FlowLayoutPanel subBase;
    private Guna2Button btnNavSys;
    private FlowLayoutPanel subSys;
    private StatusStrip statusBar;
    private ToolStripStatusLabel statusUser;
    private ToolStripStatusLabel statusServer;
    private ToolStripStatusLabel statusCompany;
    private ToolStripStatusLabel statusTime;
    private System.Windows.Forms.Timer clockTimer;

    /// <summary>
    /// 각 카테고리(subXxx) 안의 실제 화면 목록은 FormRegistry.Entries를 순회하며
    /// 그룹별로 가변 개수 생성되므로(디자이너가 표현할 수 없는 동적 구성)
    /// BuildNavMenus()/BuildStatusBar()에 남겨두고, 생성자에서 InitializeComponent()
    /// 호출 직후에 호출한다. subXxx 패널은 처음에 접혀 있어(Visible=false) 디자인
    /// 서페이스에도 빈 채로 나타나는 게 정상 모습이라 문제 없다.
    /// </summary>
    private void InitializeComponent()
    {
        this.sidebarPanel = new Guna2Panel();
        this.logoPanel = new Guna2Panel();
        this.logoBadge = new Guna2Panel();
        this.logoText = new Label();
        this.navFlow = new FlowLayoutPanel();
        this.btnNavMain = new Guna2Button();
        this.btnNavStock = new Guna2Button();
        this.subStock = new FlowLayoutPanel();
        this.btnNavFlow = new Guna2Button();
        this.subFlow = new FlowLayoutPanel();
        this.btnNavSales = new Guna2Button();
        this.subSales = new FlowLayoutPanel();
        this.btnNavClose = new Guna2Button();
        this.subClose = new FlowLayoutPanel();
        this.btnNavBase = new Guna2Button();
        this.subBase = new FlowLayoutPanel();
        this.btnNavSys = new Guna2Button();
        this.subSys = new FlowLayoutPanel();
        this.statusBar = new StatusStrip();
        this.statusUser = new ToolStripStatusLabel();
        this.statusServer = new ToolStripStatusLabel();
        this.statusCompany = new ToolStripStatusLabel();
        this.statusTime = new ToolStripStatusLabel();
        this.clockTimer = new System.Windows.Forms.Timer();
        this.sidebarPanel.SuspendLayout();
        this.logoPanel.SuspendLayout();
        this.navFlow.SuspendLayout();
        this.SuspendLayout();
        //
        // logoBadge / logoText (사이드바 상단 로고 영역 — 헤더는 이거 하나만 쓴다)
        //
        this.logoBadge.FillColor = Color.FromArgb(47, 111, 237);
        this.logoBadge.BorderRadius = 10;
        this.logoBadge.Left = 20; this.logoBadge.Top = 15; this.logoBadge.Width = 36; this.logoBadge.Height = 36;
        this.logoText.Text = "재고관리 시스템";
        this.logoText.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        this.logoText.ForeColor = Color.FromArgb(31, 41, 55);
        this.logoText.AutoSize = true;
        this.logoText.Left = 66; this.logoText.Top = 25;
        //
        // logoPanel
        //
        this.logoPanel.Dock = DockStyle.Top;
        this.logoPanel.Height = 70;
        this.logoPanel.FillColor = Color.White;
        this.logoPanel.Controls.Add(this.logoBadge);
        this.logoPanel.Controls.Add(this.logoText);
        //
        // 카테고리 버튼 공통 스타일
        //
        this.btnNavMain.Text = "메인";
        this.btnNavStock.Text = "재고관리";
        this.btnNavFlow.Text = "재고수불";
        this.btnNavSales.Text = "매출관리";
        this.btnNavClose.Text = "마감관리";
        this.btnNavBase.Text = "기초관리";
        this.btnNavSys.Text = "시스템관리";

        this.btnNavMain.Width = 200; this.btnNavMain.Height = 44;
        this.btnNavStock.Width = 200; this.btnNavStock.Height = 44;
        this.btnNavFlow.Width = 200; this.btnNavFlow.Height = 44;
        this.btnNavSales.Width = 200; this.btnNavSales.Height = 44;
        this.btnNavClose.Width = 200; this.btnNavClose.Height = 44;
        this.btnNavBase.Width = 200; this.btnNavBase.Height = 44;
        this.btnNavSys.Width = 200; this.btnNavSys.Height = 44;

        this.btnNavMain.Margin = new Padding(10, 6, 10, 0);
        this.btnNavStock.Margin = new Padding(10, 6, 10, 0);
        this.btnNavFlow.Margin = new Padding(10, 6, 10, 0);
        this.btnNavSales.Margin = new Padding(10, 6, 10, 0);
        this.btnNavClose.Margin = new Padding(10, 6, 10, 0);
        this.btnNavBase.Margin = new Padding(10, 6, 10, 0);
        this.btnNavSys.Margin = new Padding(10, 6, 10, 0);

        this.btnNavMain.BorderRadius = 8;
        this.btnNavStock.BorderRadius = 8;
        this.btnNavFlow.BorderRadius = 8;
        this.btnNavSales.BorderRadius = 8;
        this.btnNavClose.BorderRadius = 8;
        this.btnNavBase.BorderRadius = 8;
        this.btnNavSys.BorderRadius = 8;

        this.btnNavMain.FillColor = Color.White;
        this.btnNavStock.FillColor = Color.White;
        this.btnNavFlow.FillColor = Color.White;
        this.btnNavSales.FillColor = Color.White;
        this.btnNavClose.FillColor = Color.White;
        this.btnNavBase.FillColor = Color.White;
        this.btnNavSys.FillColor = Color.White;

        this.btnNavMain.ForeColor = Color.FromArgb(55, 65, 81);
        this.btnNavStock.ForeColor = Color.FromArgb(55, 65, 81);
        this.btnNavFlow.ForeColor = Color.FromArgb(55, 65, 81);
        this.btnNavSales.ForeColor = Color.FromArgb(55, 65, 81);
        this.btnNavClose.ForeColor = Color.FromArgb(55, 65, 81);
        this.btnNavBase.ForeColor = Color.FromArgb(55, 65, 81);
        this.btnNavSys.ForeColor = Color.FromArgb(55, 65, 81);

        this.btnNavMain.Font = new Font("맑은 고딕", 10F);
        this.btnNavStock.Font = new Font("맑은 고딕", 10F);
        this.btnNavFlow.Font = new Font("맑은 고딕", 10F);
        this.btnNavSales.Font = new Font("맑은 고딕", 10F);
        this.btnNavClose.Font = new Font("맑은 고딕", 10F);
        this.btnNavBase.Font = new Font("맑은 고딕", 10F);
        this.btnNavSys.Font = new Font("맑은 고딕", 10F);

        this.btnNavMain.TextAlign = HorizontalAlignment.Left;
        this.btnNavStock.TextAlign = HorizontalAlignment.Left;
        this.btnNavFlow.TextAlign = HorizontalAlignment.Left;
        this.btnNavSales.TextAlign = HorizontalAlignment.Left;
        this.btnNavClose.TextAlign = HorizontalAlignment.Left;
        this.btnNavBase.TextAlign = HorizontalAlignment.Left;
        this.btnNavSys.TextAlign = HorizontalAlignment.Left;

        this.btnNavMain.TextOffset = new Point(44, 0);
        this.btnNavStock.TextOffset = new Point(44, 0);
        this.btnNavFlow.TextOffset = new Point(44, 0);
        this.btnNavSales.TextOffset = new Point(44, 0);
        this.btnNavClose.TextOffset = new Point(44, 0);
        this.btnNavBase.TextOffset = new Point(44, 0);
        this.btnNavSys.TextOffset = new Point(44, 0);

        this.btnNavMain.ImageAlign = HorizontalAlignment.Left;
        this.btnNavStock.ImageAlign = HorizontalAlignment.Left;
        this.btnNavFlow.ImageAlign = HorizontalAlignment.Left;
        this.btnNavSales.ImageAlign = HorizontalAlignment.Left;
        this.btnNavClose.ImageAlign = HorizontalAlignment.Left;
        this.btnNavBase.ImageAlign = HorizontalAlignment.Left;
        this.btnNavSys.ImageAlign = HorizontalAlignment.Left;

        this.btnNavMain.ImageOffset = new Point(16, 0);
        this.btnNavStock.ImageOffset = new Point(16, 0);
        this.btnNavFlow.ImageOffset = new Point(16, 0);
        this.btnNavSales.ImageOffset = new Point(16, 0);
        this.btnNavClose.ImageOffset = new Point(16, 0);
        this.btnNavBase.ImageOffset = new Point(16, 0);
        this.btnNavSys.ImageOffset = new Point(16, 0);

        this.btnNavMain.HoverState.FillColor = Color.FromArgb(232, 240, 254);
        this.btnNavStock.HoverState.FillColor = Color.FromArgb(232, 240, 254);
        this.btnNavFlow.HoverState.FillColor = Color.FromArgb(232, 240, 254);
        this.btnNavSales.HoverState.FillColor = Color.FromArgb(232, 240, 254);
        this.btnNavClose.HoverState.FillColor = Color.FromArgb(232, 240, 254);
        this.btnNavBase.HoverState.FillColor = Color.FromArgb(232, 240, 254);
        this.btnNavSys.HoverState.FillColor = Color.FromArgb(232, 240, 254);

        this.btnNavMain.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        this.btnNavStock.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        this.btnNavFlow.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        this.btnNavSales.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        this.btnNavClose.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        this.btnNavBase.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        this.btnNavSys.HoverState.ForeColor = Color.FromArgb(47, 111, 237);
        //
        // 카테고리별 하위 화면 목록을 담을 서브패널(초기엔 접힌 상태) 공통 스타일
        //
        this.subStock.Width = 220; this.subFlow.Width = 220; this.subSales.Width = 220;
        this.subClose.Width = 220; this.subBase.Width = 220; this.subSys.Width = 220;

        this.subStock.FlowDirection = FlowDirection.TopDown;
        this.subFlow.FlowDirection = FlowDirection.TopDown;
        this.subSales.FlowDirection = FlowDirection.TopDown;
        this.subClose.FlowDirection = FlowDirection.TopDown;
        this.subBase.FlowDirection = FlowDirection.TopDown;
        this.subSys.FlowDirection = FlowDirection.TopDown;

        this.subStock.WrapContents = false;
        this.subFlow.WrapContents = false;
        this.subSales.WrapContents = false;
        this.subClose.WrapContents = false;
        this.subBase.WrapContents = false;
        this.subSys.WrapContents = false;

        this.subStock.AutoSize = true;
        this.subFlow.AutoSize = true;
        this.subSales.AutoSize = true;
        this.subClose.AutoSize = true;
        this.subBase.AutoSize = true;
        this.subSys.AutoSize = true;

        this.subStock.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.subFlow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.subSales.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.subClose.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.subBase.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.subSys.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        this.subStock.Margin = new Padding(0);
        this.subFlow.Margin = new Padding(0);
        this.subSales.Margin = new Padding(0);
        this.subClose.Margin = new Padding(0);
        this.subBase.Margin = new Padding(0);
        this.subSys.Margin = new Padding(0);

        this.subStock.Visible = false;
        this.subFlow.Visible = false;
        this.subSales.Visible = false;
        this.subClose.Visible = false;
        this.subBase.Visible = false;
        this.subSys.Visible = false;
        //
        // navFlow (사이드바 본문 — 카테고리 버튼 + 서브패널이 세로로 쌓이는 아코디언)
        //
        this.navFlow.Dock = DockStyle.Fill;
        this.navFlow.FlowDirection = FlowDirection.TopDown;
        this.navFlow.WrapContents = false;
        this.navFlow.AutoScroll = true;
        this.navFlow.Controls.AddRange(new Control[]
        {
            this.btnNavMain,
            this.btnNavStock, this.subStock,
            this.btnNavFlow, this.subFlow,
            this.btnNavSales, this.subSales,
            this.btnNavClose, this.subClose,
            this.btnNavBase, this.subBase,
            this.btnNavSys, this.subSys
        });
        //
        // sidebarPanel
        //
        this.sidebarPanel.Dock = DockStyle.Left;
        this.sidebarPanel.Width = 220;
        this.sidebarPanel.FillColor = Color.White;
        this.sidebarPanel.Controls.Add(this.navFlow);
        this.sidebarPanel.Controls.Add(this.logoPanel);
        //
        // statusBar
        //
        this.statusBar.Dock = DockStyle.Bottom;
        this.statusBar.BackColor = Color.White;
        this.statusBar.SizingGrip = false;
        this.statusUser.ForeColor = Color.FromArgb(107, 114, 128);
        this.statusServer.ForeColor = Color.FromArgb(107, 114, 128);
        this.statusCompany.ForeColor = Color.FromArgb(107, 114, 128);
        this.statusTime.ForeColor = Color.FromArgb(107, 114, 128);
        this.statusBar.Items.AddRange(new ToolStripItem[]
        {
            this.statusUser, this.statusServer, this.statusCompany, this.statusTime
        });
        //
        // clockTimer
        //
        this.clockTimer.Interval = 1000;
        this.clockTimer.Tick += new EventHandler(this.ClockTimer_Tick);
        this.clockTimer.Enabled = true;
        //
        // MainForm
        //
        this.Text = "재고관리 시스템_개발서버";
        this.IsMdiContainer = true;
        this.WindowState = FormWindowState.Maximized;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(243, 244, 247);
        this.Controls.Add(this.statusBar);
        this.Controls.Add(this.sidebarPanel);
        this.FormClosing += this.MainForm_FormClosing;
        this.KeyDown += this.MainForm_KeyDown;
        this.KeyPreview = true;
        this.navFlow.ResumeLayout(false);
        this.sidebarPanel.ResumeLayout(false);
        this.logoPanel.ResumeLayout(false);
        this.logoPanel.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
