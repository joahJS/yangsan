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
    public MainForm()
    {
        InitializeComponent();

        BuildNavMenus();
        BuildStatusBar();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyMdiClientStyle();
    }

    /// <summary>MDI 클라이언트 영역(업무 화면들이 떠 있는 배경)을 사이드바/헤더와 어울리는
    /// 연회색으로 바꾼다. MdiClient는 폼이 실제로 표시된 뒤에야 생성되는 내부 컨트롤이라
    /// 디자인 타임에는 존재하지 않으므로 런타임 전용으로 처리한다.</summary>
    private void ApplyMdiClientStyle()
    {
        foreach (Control c in Controls)
        {
            if (c is MdiClient mdi)
            {
                mdi.BackColor = Color.FromArgb(243, 244, 247);
                return;
            }
        }
    }

    private void BuildNavMenus()
    {
        btnNavMain.Click += (_, _) => { foreach (Form child in MdiChildren.ToArray()) child.Close(); };

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

        button.Click += (_, _) => subPanel.Visible = !subPanel.Visible;
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
