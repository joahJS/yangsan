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

    private MenuStrip menu;
    private StatusStrip statusBar;
    private ToolStripStatusLabel statusLeft;
    private ToolStripStatusLabel statusRight;

    /// <summary>
    /// 정적 레이아웃만 구성한다. "윈도우"/"종료" 메뉴와 그룹별(BA/JA/SS/ED) 하위 메뉴는
    /// FormRegistry.Entries를 순회하며 개수가 가변적으로 생성되므로(디자이너가 표현할 수 없는
    /// 동적 구성) BuildMenu()/BuildStatusBar()에 그대로 남겨두고, 생성자에서 InitializeComponent()
    /// 호출 직후에 호출한다.
    /// </summary>
    private void InitializeComponent()
    {
        this.menu = new MenuStrip();
        this.statusBar = new StatusStrip();
        this.statusLeft = new ToolStripStatusLabel();
        this.statusRight = new ToolStripStatusLabel();
        this.SuspendLayout();
        //
        // MainForm
        //
        this.Text = "재고관리 시스템_개발서버";
        this.IsMdiContainer = true;
        this.WindowState = FormWindowState.Maximized;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MainMenuStrip = this.menu;
        this.Controls.Add(this.menu);
        this.Controls.Add(this.statusBar);
        this.FormClosing += this.MainForm_FormClosing;
        this.KeyDown += this.MainForm_KeyDown;
        this.MdiChildActivate += this.MainForm_MdiChildActivate;
        this.KeyPreview = true;
        this.ResumeLayout(false);
    }
}
