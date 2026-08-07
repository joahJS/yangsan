using pStock.Common;
using pStock.Data;

namespace pStock.Forms;

/// <summary>
/// 원본 Main.pas / Main.dfm (Tfrm_Main, MDI 부모 폼) 이식.
/// 메뉴는 FormRegistry에 정의된 목록으로부터 자동 생성한다.
/// </summary>
public class MainForm : Form
{
    private readonly MenuStrip menu = new();
    private readonly StatusStrip statusBar = new();
    private readonly ToolStripStatusLabel statusLeft = new();
    private readonly ToolStripStatusLabel statusRight = new();

    public MainForm()
    {
        Text = "재고관리 시스템_개발서버";
        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;
        StartPosition = FormStartPosition.CenterScreen;

        BuildMenu();
        BuildStatusBar();

        MainMenuStrip = menu;
        Controls.Add(menu);
        Controls.Add(statusBar);

        FormClosing += MainForm_FormClosing;
        KeyDown += MainForm_KeyDown;
        MdiChildActivate += MainForm_MdiChildActivate;
        KeyPreview = true;
    }

    private void BuildMenu()
    {
        menu.Items.Clear();

        AddGroupMenu("자료관리(&B)", "BA");
        AddGroupMenu("재고관리(&J)", "JA");
        AddGroupMenu("매출관리(&S)", "SS");
        AddGroupMenu("마감작업(&E)", "ED");

        // 원본 Window1 메뉴: Tile1 / Cascade1 / Maximaze1
        var windowMenu = new ToolStripMenuItem("윈도우(&W)");
        var tile = new ToolStripMenuItem("바둑판식 배열(&T)");
        tile.Click += (_, _) => LayoutMdi(MdiLayout.TileVertical);
        var cascade = new ToolStripMenuItem("계단식 배열(&C)");
        cascade.Click += (_, _) => LayoutMdi(MdiLayout.Cascade);
        var maximize = new ToolStripMenuItem("최대화(&M)");
        maximize.Click += (_, _) =>
        {
            if (MdiChildren.Length > 0) MdiChildren[0].WindowState = FormWindowState.Maximized;
        };
        windowMenu.DropDownItems.AddRange(new ToolStripItem[] { tile, cascade, maximize });
        menu.Items.Add(windowMenu);

        var mnuClose = new ToolStripMenuItem("종료(&X)");
        mnuClose.Click += (_, _) => Close();
        menu.Items.Add(mnuClose);
    }

    private void AddGroupMenu(string caption, string group)
    {
        var top = new ToolStripMenuItem(caption);
        foreach (var entry in FormRegistry.Entries.Where(e => e.Group == group))
        {
            var item = new ToolStripMenuItem(entry.Caption) { Tag = entry.Tag };
            item.Click += (_, _) => ShowScreen(entry.Tag);
            top.DropDownItems.Add(item);
        }
        menu.Items.Add(top);
    }

    private void BuildStatusBar()
    {
        statusBar.Items.Add(statusLeft);
        statusBar.Items.Add(statusRight);
        statusBar.Dock = DockStyle.Bottom;
    }

    /// <summary>원본 prcFormShow(Inx). 이미 열려있는 동일 Tag 화면은 앞으로 가져오고,
    /// 없으면 FormRegistry에 등록된 factory로 새로 생성한다.</summary>
    public void ShowScreen(int tag)
    {
        foreach (Form child in MdiChildren)
        {
            if (child.Tag is int t && t == tag)
            {
                child.BringToFront();
                return;
            }
        }

        var entry = FormRegistry.Find(tag);
        if (entry == null) return;

        var form = entry.Factory();
        form.Tag = tag;

        if (entry.Modal)
        {
            form.ShowDialog(this);
        }
        else
        {
            form.MdiParent = this;
            form.Show();
        }
    }

    private void MainForm_MdiChildActivate(object? sender, EventArgs e)
    {
        if (ActiveMdiChild != null)
        {
            statusLeft.Text = ActiveMdiChild.Name;
            statusRight.Text = ActiveMdiChild.Text;
        }
        else
        {
            statusLeft.Text = string.Empty;
            statusRight.Text = string.Empty;
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // 원본 FormClose: 모든 MDI 자식을 닫는다.
        foreach (Form child in MdiChildren.ToArray())
        {
            child.Close();
        }
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) Close();
    }

    /// <summary>원본 AppException: DB 트랜잭션 진행 중이면 롤백.</summary>
    public static void HandleAppException(Exception ex)
    {
        MessageBox.Show(ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (AppDb.InTransaction) AppDb.Rollback();
    }
}
