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
    private Guna2Button btnNavMain;
    private Guna2Button btnNavStock;
    private Guna2Button btnNavFlow;
    private Guna2Button btnNavSales;
    private Guna2Button btnNavClose;
    private Guna2Button btnNavBase;
    private Guna2Button btnNavSys;
    private Guna2Panel headerPanel;
    private Guna2Panel headerIcon;
    private Label headerTitle;
    private StatusStrip statusBar;
    private ToolStripStatusLabel statusUser;
    private ToolStripStatusLabel statusServer;
    private ToolStripStatusLabel statusCompany;
    private ToolStripStatusLabel statusTime;
    private System.Windows.Forms.Timer clockTimer;

    /// <summary>
    /// 사이드바 버튼 각각에 붙는 하위 화면 목록(ContextMenuStrip)은 FormRegistry.Entries를
    /// 순회하며 그룹별로 가변 개수 생성되므로(디자이너가 표현할 수 없는 동적 구성)
    /// BuildNavMenus()/BuildStatusBar()에 남겨두고, 생성자에서 InitializeComponent()
    /// 호출 직후에 호출한다. 팝업 메뉴는 클릭 전까지 화면에 그려지지 않는 요소라
    /// 디자인 서페이스 렌더링에는 영향이 없다.
    /// </summary>
    private void InitializeComponent()
    {
        this.sidebarPanel = new Guna2Panel();
        this.logoPanel = new Guna2Panel();
        this.logoBadge = new Guna2Panel();
        this.logoText = new Label();
        this.btnNavMain = new Guna2Button();
        this.btnNavStock = new Guna2Button();
        this.btnNavFlow = new Guna2Button();
        this.btnNavSales = new Guna2Button();
        this.btnNavClose = new Guna2Button();
        this.btnNavBase = new Guna2Button();
        this.btnNavSys = new Guna2Button();
        this.headerPanel = new Guna2Panel();
        this.headerIcon = new Guna2Panel();
        this.headerTitle = new Label();
        this.statusBar = new StatusStrip();
        this.statusUser = new ToolStripStatusLabel();
        this.statusServer = new ToolStripStatusLabel();
        this.statusCompany = new ToolStripStatusLabel();
        this.statusTime = new ToolStripStatusLabel();
        this.clockTimer = new System.Windows.Forms.Timer();
        this.sidebarPanel.SuspendLayout();
        this.logoPanel.SuspendLayout();
        this.headerPanel.SuspendLayout();
        this.SuspendLayout();
        //
        // logoBadge / logoText (사이드바 상단 로고 영역)
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
        // 네비게이션 버튼 공통 스타일 + 개별 배치
        //
        this.btnNavMain.Text = "메인";
        this.btnNavStock.Text = "재고관리";
        this.btnNavFlow.Text = "재고수불";
        this.btnNavSales.Text = "매출관리";
        this.btnNavClose.Text = "마감관리";
        this.btnNavBase.Text = "기초관리";
        this.btnNavSys.Text = "시스템관리";

        this.btnNavMain.Left = 10; this.btnNavMain.Top = 82; this.btnNavMain.Width = 200; this.btnNavMain.Height = 44;
        this.btnNavStock.Left = 10; this.btnNavStock.Top = 132; this.btnNavStock.Width = 200; this.btnNavStock.Height = 44;
        this.btnNavFlow.Left = 10; this.btnNavFlow.Top = 182; this.btnNavFlow.Width = 200; this.btnNavFlow.Height = 44;
        this.btnNavSales.Left = 10; this.btnNavSales.Top = 232; this.btnNavSales.Width = 200; this.btnNavSales.Height = 44;
        this.btnNavClose.Left = 10; this.btnNavClose.Top = 282; this.btnNavClose.Width = 200; this.btnNavClose.Height = 44;
        this.btnNavBase.Left = 10; this.btnNavBase.Top = 332; this.btnNavBase.Width = 200; this.btnNavBase.Height = 44;
        this.btnNavSys.Left = 10; this.btnNavSys.Top = 382; this.btnNavSys.Width = 200; this.btnNavSys.Height = 44;

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

        this.btnNavMain.TextOffset = new Point(16, 0);
        this.btnNavStock.TextOffset = new Point(16, 0);
        this.btnNavFlow.TextOffset = new Point(16, 0);
        this.btnNavSales.TextOffset = new Point(16, 0);
        this.btnNavClose.TextOffset = new Point(16, 0);
        this.btnNavBase.TextOffset = new Point(16, 0);
        this.btnNavSys.TextOffset = new Point(16, 0);

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
        // sidebarPanel
        //
        this.sidebarPanel.Dock = DockStyle.Left;
        this.sidebarPanel.Width = 220;
        this.sidebarPanel.FillColor = Color.White;
        this.sidebarPanel.Controls.Add(this.btnNavSys);
        this.sidebarPanel.Controls.Add(this.btnNavBase);
        this.sidebarPanel.Controls.Add(this.btnNavClose);
        this.sidebarPanel.Controls.Add(this.btnNavSales);
        this.sidebarPanel.Controls.Add(this.btnNavFlow);
        this.sidebarPanel.Controls.Add(this.btnNavStock);
        this.sidebarPanel.Controls.Add(this.btnNavMain);
        this.sidebarPanel.Controls.Add(this.logoPanel);
        //
        // headerIcon / headerTitle
        //
        this.headerIcon.FillColor = Color.FromArgb(47, 111, 237);
        this.headerIcon.BorderRadius = 8;
        this.headerIcon.Left = 24; this.headerIcon.Top = 14; this.headerIcon.Width = 32; this.headerIcon.Height = 32;
        this.headerTitle.Text = "재고관리 시스템_개발서버";
        this.headerTitle.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
        this.headerTitle.ForeColor = Color.FromArgb(31, 41, 55);
        this.headerTitle.AutoSize = true;
        this.headerTitle.Left = 68; this.headerTitle.Top = 20;
        //
        // headerPanel
        //
        this.headerPanel.Dock = DockStyle.Top;
        this.headerPanel.Height = 60;
        this.headerPanel.FillColor = Color.White;
        this.headerPanel.BorderThickness = 0;
        this.headerPanel.Controls.Add(this.headerIcon);
        this.headerPanel.Controls.Add(this.headerTitle);
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
        this.Controls.Add(this.headerPanel);
        this.Controls.Add(this.sidebarPanel);
        this.FormClosing += this.MainForm_FormClosing;
        this.KeyDown += this.MainForm_KeyDown;
        this.KeyPreview = true;
        this.sidebarPanel.ResumeLayout(false);
        this.logoPanel.ResumeLayout(false);
        this.logoPanel.PerformLayout();
        this.headerPanel.ResumeLayout(false);
        this.headerPanel.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
