using pStock.Common;
using pStock.Data;

namespace pStock.Forms;

/// <summary>
/// 원본 Main.pas / Main.dfm (Tfrm_Main, MDI 부모 폼) 이식.
/// 상단 메뉴 대신 좌측 사이드바 네비게이션(카테고리별 팝업 메뉴) 구조로 재구성했다.
/// 카테고리 목록은 FormRegistry에 정의된 항목으로부터 자동 생성한다.
/// </summary>
public partial class MainForm : Form
{
    private static readonly Color NavColorNormal = Color.FromArgb(107, 114, 128);
    private static readonly Color NavColorSelected = Color.White;

    /// <summary>카테고리 버튼별 아이콘(선택 안 됨/선택됨 버전)을 미리 만들어 보관한다.</summary>
    private readonly Dictionary<Guna.UI2.WinForms.Guna2Button, (Image Normal, Image Selected)> _navIcons = new();
    private Guna.UI2.WinForms.Guna2Button? _selectedNav;

    /// <summary>브라우저 탭처럼 열려 있는 화면 목록. Tag(화면 번호) → (호스팅 중인 Form, 탭 버튼).
    /// 실제 MDI 대신 formHostPanel 위에 TopLevel=false로 올려 붙이고, 한 번에 하나만 보인다.</summary>
    private readonly Dictionary<int, (Form Form, TabStripItem Tab)> _openScreens = new();
    private int? _activeTag;

    public MainForm()
    {
        InitializeComponent();

        logoIcon.Image = CreateIcon(DrawBoxesIcon, Color.White);
        BuildNavIcons();
        BuildNavMenus();
        BuildStatusBar();
    }

    private void BuildNavMenus()
    {
        btnNavMain.Click += (_, _) =>
        {
            foreach (var kv in _openScreens.ToArray()) kv.Value.Form.Close();
            SelectNav(btnNavMain);
        };

        AttachGroupMenu(btnNavStock, subStock, "STOCK");
        AttachGroupMenu(btnNavFlow, subFlow, "FLOW");
        AttachGroupMenu(btnNavSales, subSales, "SALES");
        AttachGroupMenu(btnNavClose, subClose, "CLOSE");
        AttachGroupMenu(btnNavBase, subBase, "BASE");
        AttachGroupMenu(btnNavSys, subSys, "SYS");
    }

    /// <summary>카테고리 버튼 아래 서브패널(subPanel)에 그 그룹(Group)에 속한 화면 목록을
    /// 채워 넣고, 버튼 클릭 시 서브패널을 펼치거나 접는다. 사이드바 전체가
    /// FlowLayoutPanel(navFlow) 안에 있어서, 펼침/접힘에 따라 그 아래 다른 카테고리들이
    /// 자동으로 밀리거나 당겨진다(계단식 아코디언).</summary>
    private void AttachGroupMenu(Guna.UI2.WinForms.Guna2Button button, FlowLayoutPanel subPanel, string group)
    {
        foreach (var entry in FormRegistry.Entries.Where(e => e.Group == group))
        {
            var item = new Label
            {
                Text = entry.Caption,
                Tag = entry.Tag,
                AutoSize = false,
                Width = 220,
                Height = 34,
                Padding = new Padding(40, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("맑은 고딕", 9.5F),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(243, 244, 247),
                Margin = new Padding(0),
            };
            var tag = entry.Tag;
            item.Click += (_, _) => ShowScreen(tag);
            item.MouseEnter += (_, _) => item.BackColor = Color.FromArgb(232, 240, 254);
            item.MouseLeave += (_, _) => item.BackColor = Color.FromArgb(243, 244, 247);
            subPanel.Controls.Add(item);
        }

        button.Click += (_, _) =>
        {
            var willOpen = !subPanel.Visible;
            subPanel.Visible = willOpen;
            if (willOpen) SelectNav(button);
            else if (_selectedNav == button) SelectNav(null);
        };
    }

    /// <summary>선택된 카테고리 버튼을 파란 배경 + 흰 텍스트/아이콘으로 강조하고,
    /// 이전에 선택돼 있던 버튼은 원래 스타일로 되돌린다.</summary>
    private void SelectNav(Guna.UI2.WinForms.Guna2Button? button)
    {
        if (_selectedNav != null) SetNavSelected(_selectedNav, false);
        if (button != null) SetNavSelected(button, true);
        _selectedNav = button;
    }

    private void SetNavSelected(Guna.UI2.WinForms.Guna2Button button, bool selected)
    {
        button.FillColor = selected ? Color.FromArgb(47, 111, 237) : Color.White;
        button.ForeColor = selected ? Color.White : Color.FromArgb(55, 65, 81);
        if (_navIcons.TryGetValue(button, out var icons))
            button.Image = selected ? icons.Selected : icons.Normal;
    }

    /// <summary>대메뉴 좌측에 붙일 작은 아이콘을 GDI+로 직접 그려서 만든다. 이미지 파일이나
    /// 외부 아이콘 폰트 없이도 어떤 환경에서나 동일하게 렌더링되는 간단한 벡터 아이콘이다.
    /// 버튼마다 기본색/선택됐을 때(흰색) 두 가지 버전을 만들어 두고 SetNavSelected에서 교체한다.</summary>
    private void BuildNavIcons()
    {
        RegisterNavIcon(btnNavMain, DrawHomeIcon);
        RegisterNavIcon(btnNavStock, DrawBoxesIcon);
        RegisterNavIcon(btnNavFlow, DrawFlowIcon);
        RegisterNavIcon(btnNavSales, DrawChartIcon);
        RegisterNavIcon(btnNavClose, DrawCalendarIcon);
        RegisterNavIcon(btnNavBase, DrawDocIcon);
        RegisterNavIcon(btnNavSys, DrawGearIcon);
    }

    private void RegisterNavIcon(Guna.UI2.WinForms.Guna2Button button, Action<Graphics, Color> draw)
    {
        var normal = CreateIcon(draw, NavColorNormal);
        var selected = CreateIcon(draw, NavColorSelected);
        _navIcons[button] = (normal, selected);
        button.Image = normal;
    }

    private static Bitmap CreateIcon(Action<Graphics, Color> draw, Color color)
    {
        var bmp = new Bitmap(20, 20);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        draw(g, color);
        return bmp;
    }

    private static void DrawHomeIcon(Graphics g, Color c)
    {
        using var brush = new SolidBrush(c);
        g.FillPolygon(brush, new[] { new Point(10, 2), new Point(18, 9), new Point(2, 9) });
        g.FillRectangle(brush, 5, 9, 10, 9);
    }

    private static void DrawBoxesIcon(Graphics g, Color c)
    {
        using var brush = new SolidBrush(c);
        g.FillRectangle(brush, 2, 2, 7, 7);
        g.FillRectangle(brush, 11, 2, 7, 7);
        g.FillRectangle(brush, 2, 11, 7, 7);
        g.FillRectangle(brush, 11, 11, 7, 7);
    }

    private static void DrawFlowIcon(Graphics g, Color c)
    {
        using var brush = new SolidBrush(c);
        g.FillPolygon(brush, new[] { new Point(6, 1), new Point(11, 8), new Point(1, 8) });
        g.FillRectangle(brush, 5, 8, 2, 5);
        g.FillPolygon(brush, new[] { new Point(14, 19), new Point(9, 12), new Point(19, 12) });
        g.FillRectangle(brush, 13, 6, 2, 5);
    }

    private static void DrawChartIcon(Graphics g, Color c)
    {
        using var brush = new SolidBrush(c);
        g.FillRectangle(brush, 2, 12, 4, 6);
        g.FillRectangle(brush, 8, 7, 4, 11);
        g.FillRectangle(brush, 14, 2, 4, 16);
    }

    private static void DrawCalendarIcon(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.6f);
        g.DrawRectangle(pen, 2, 4, 16, 14);
        g.DrawLine(pen, 2, 8, 18, 8);
        g.DrawLine(pen, 6, 2, 6, 6);
        g.DrawLine(pen, 14, 2, 14, 6);
    }

    private static void DrawDocIcon(Graphics g, Color c)
    {
        using var pen = new Pen(c, 1.6f);
        g.DrawRectangle(pen, 4, 2, 12, 16);
        g.DrawLine(pen, 7, 7, 15, 7);
        g.DrawLine(pen, 7, 11, 15, 11);
        g.DrawLine(pen, 7, 15, 12, 15);
    }

    private static void DrawGearIcon(Graphics g, Color c)
    {
        using var brush = new SolidBrush(c);
        g.FillRectangle(brush, 8, 0, 4, 4);
        g.FillRectangle(brush, 8, 16, 4, 4);
        g.FillRectangle(brush, 0, 8, 4, 4);
        g.FillRectangle(brush, 16, 8, 4, 4);
        g.FillEllipse(brush, 4, 4, 12, 12);
    }

    private void BuildStatusBar()
    {
        var userName = string.IsNullOrWhiteSpace(UserContext.Current.Name) ? "관리자" : UserContext.Current.Name;
        statusUser.Text = "사용자 : " + userName;
        statusServer.Text = "접속서버 : " + AppDb.Server;
        statusCompany.Text = "회사명 : " + LoadCompanyName();
        UpdateClock();
    }

    /// <summary>사업장 마스터(SAUPJANGF)에서 회사명을 조회한다. DB 접속 문제 등으로
    /// 실패해도 메인 화면 자체는 뜨는 게 우선이므로 실패 시 조용히 빈 값으로 둔다.</summary>
    private static string LoadCompanyName()
    {
        try
        {
            using var q = new DbQuery();
            q.Add("SELECT SSANG FROM SAUPJANGF WHERE SCODE = @SCODE");
            q.ParamByName("SCODE").AsString = "100001";
            q.Open();
            return q.IsEmpty ? "-" : q.FieldByName("SSANG").AsString;
        }
        catch
        {
            return "-";
        }
    }

    private void ClockTimer_Tick(object? sender, EventArgs e) => UpdateClock();

    private void UpdateClock()
    {
        statusTime.Text = DateTime.Now.ToString("yyyy-MM-dd (ddd) HH:mm:ss",
            new System.Globalization.CultureInfo("ko-KR"));
    }

    /// <summary>원본 prcFormShow(Inx). 이미 열려있는 동일 Tag 화면은 그 탭으로 전환하고,
    /// 없으면 FormRegistry에 등록된 factory로 새로 생성해 탭으로 연다.</summary>
    public void ShowScreen(int tag)
    {
        if (_openScreens.ContainsKey(tag))
        {
            ActivateTab(tag);
            return;
        }

        var entry = FormRegistry.Find(tag);
        if (entry == null) return;

        var form = entry.Factory();
        form.Tag = tag;

        if (entry.Modal)
        {
            form.ShowDialog(this);
            return;
        }

        OpenTab(tag, entry.Caption, form);
    }

    /// <summary>실제 MDI 대신 폼을 TopLevel=false로 formHostPanel에 올려 붙이고,
    /// 그 위에 대응하는 탭 버튼을 만들어 tabStripPanel에 추가한다.</summary>
    private void OpenTab(int tag, string caption, Form form)
    {
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        form.Visible = false;
        formHostPanel.Controls.Add(form);
        form.Show();

        var tab = new TabStripItem(caption);
        tab.Activate += (_, _) => ActivateTab(tag);
        tab.CloseRequested += (_, _) => form.Close();
        tabStripPanel.Controls.Add(tab.Panel);

        _openScreens[tag] = (form, tab);
        form.FormClosed += (_, _) => CloseTab(tag);

        ActivateTab(tag);
    }

    /// <summary>tag에 해당하는 탭/폼을 활성화하고 나머지는 숨긴다.</summary>
    private void ActivateTab(int tag)
    {
        if (!_openScreens.TryGetValue(tag, out var target)) return;

        foreach (var kv in _openScreens)
        {
            var isActive = kv.Key == tag;
            kv.Value.Form.Visible = isActive;
            kv.Value.Tab.SetActive(isActive);
        }

        target.Form.BringToFront();
        target.Form.Select();
        _activeTag = tag;
    }

    /// <summary>폼이 닫힐 때(닫기 버튼/Esc/탭의 × 버튼 어느 경로든) 탭 UI와 추적 정보를 정리하고,
    /// 남은 탭이 있으면 마지막 탭으로, 없으면 빈 화면으로 되돌아간다.</summary>
    private void CloseTab(int tag)
    {
        if (!_openScreens.TryGetValue(tag, out var closed)) return;

        _openScreens.Remove(tag);
        tabStripPanel.Controls.Remove(closed.Tab.Panel);
        closed.Tab.Panel.Dispose();

        if (_activeTag == tag)
        {
            _activeTag = null;
            if (_openScreens.Count > 0) ActivateTab(_openScreens.Keys.First());
            else SelectNav(null);
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        foreach (var kv in _openScreens.ToArray())
        {
            kv.Value.Form.Close();
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
