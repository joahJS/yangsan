using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA00E.pas / BA00E.dfm (TfrmBA00E) 이식 — 사용자/패스워드 관리(PASSWD 테이블).
/// 로그인 화면(LoginForm)이 조회하는 PASSWD 테이블(USRID/PNAME/PPASS/PSDAT/PEDAT)을
/// 등록/수정/삭제하는 관리자용 화면이다. 원본은 모달(ShowModal)로 열린다.
/// </summary>
public partial class BA00EForm : Form
{
    private DataTable? _passTable;

    public BA00EForm()
    {
        InitializeComponent();
        BuildDynamicLayout();
    }

    /// <summary>
    /// 원본 BuildLayout() — GridLayout 헬퍼로 좌표를 동적으로 계산하기 때문에(지역변수 사용)
    /// WinForms 디자이너가 InitializeComponent() 안에서는 처리하지 못해 이 메서드로 분리했다.
    /// InitializeComponent() 호출 직후 생성자에서 실행되므로 동작은 이전과 동일하다.
    /// </summary>
    private void BuildDynamicLayout()
    {
        //
        // panelTop (원본 BuildLayout()의 top 패널)
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
        this.btnClose.Left = 195;
        this.btnClose.Top = 8;
        this.btnClose.Width = 90;
        this.panelTop.Controls.Add(this.btnClose);
        //
        // panelEdit (원본 BuildLayout()의 editPanel: GridLayout 헬퍼로 라벨/입력란 배치)
        //
        this.panelEdit.Dock = DockStyle.Top;
        this.panelEdit.Height = 100;
        var layout = new GridLayout(this.panelEdit, 10, 5, slotWidth: 250, labelWidth: 90, rowHeight: 30, slotsPerRow: 2);
        layout.Add("사용자 ID", this.edtCode);
        layout.Add("성명", this.edtName);
        layout.Add("비밀번호", this.edtPass);
        layout.Add("사용시작일", this.edtDate1);
        layout.Add("사용종료일", this.edtDate2);
        this.panelEdit.Height = layout.Bottom();
        //
        // grid
        //
        this.grid.Dock = DockStyle.Fill;
        this.grid.ReadOnly = true;
        this.grid.AllowUserToAddRows = false;
        this.grid.CellClick += (_, _) => SyncEditFromGrid();
        this.grid.KeyDown += new KeyEventHandler(this.Grid_KeyDown);
        //
        // Controls.Add 순서 (원본 BuildLayout())
        //
        this.Controls.Add(this.grid);
        this.Controls.Add(this.panelEdit);
        this.Controls.Add(this.panelTop);
        //
        // 이벤트 배선
        //
        this.btnNew.Click += (_, _) => { ClearEdit(); edtCode.Focus(); };
        this.btnSave.Click += (_, _) => Save();
        this.btnClose.Click += (_, _) => Close();
    }

    private void BA00EForm_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F2: btnSave.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void ReloadList()
    {
        using var q = new DbQuery();
        q.Add("SELECT * FROM PASSWD ORDER BY USRID");
        q.Open();
        _passTable = q.Table;
        grid.DataSource = _passTable;
        if (grid.Columns["USRID"] != null) grid.Columns["USRID"]!.HeaderText = "사용자ID";
        if (grid.Columns["PNAME"] != null) grid.Columns["PNAME"]!.HeaderText = "성명";
        if (grid.Columns["PPASS"] != null) grid.Columns["PPASS"]!.HeaderText = "비밀번호";
        if (grid.Columns["PSDAT"] != null) grid.Columns["PSDAT"]!.HeaderText = "시작일";
        if (grid.Columns["PEDAT"] != null) grid.Columns["PEDAT"]!.HeaderText = "종료일";
        if (grid.Columns["MDATE"] != null) grid.Columns["MDATE"]!.Visible = false;
    }

    private void SyncEditFromGrid()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) { ClearEdit(); return; }
        edtCode.Text = row["USRID"].ToString();
        edtName.Text = row["PNAME"].ToString();
        edtPass.Text = row["PPASS"].ToString();
        edtDate1.Text = row["PSDAT"].ToString();
        edtDate2.Text = row["PEDAT"].ToString();
    }

    private void ClearEdit()
    {
        edtCode.Clear(); edtName.Clear(); edtPass.Clear();
        edtDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        edtDate2.Text = "9999-12-31";
    }

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtCode.Text) || string.IsNullOrWhiteSpace(edtPass.Text))
        {
            MessageBox.Show("입력항목이 누락되었습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    /// <summary>원본 btnSaveClick: PASSWD에 USRID가 있으면 UPDATE, 없으면 INSERT.</summary>
    private void Save()
    {
        if (!ErrCheck()) return;

        using var check = new DbQuery();
        check.Add("SELECT * FROM PASSWD WHERE USRID = @CODE");
        check.ParamByName("CODE").AsString = edtCode.Text.Trim();
        check.Open();
        bool isInsert = check.RecordCount < 1;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            if (isInsert)
            {
                q.Add("INSERT INTO PASSWD (USRID,PNAME,PPASS,PSDAT,PEDAT,MDATE)");
                q.Add("       VALUES(@CODE,@PNAME,@PPASS,@PSDAT,@PEDAT,@MDATE)");
            }
            else
            {
                q.Add("UPDATE PASSWD SET PNAME=@PNAME,PPASS=@PPASS,PSDAT=@PSDAT,");
                q.Add("                  PEDAT=@PEDAT,MDATE=@MDATE");
                q.Add(" WHERE USRID=@CODE");
            }
            q.ParamByName("CODE").AsString = edtCode.Text.Trim();
            q.ParamByName("PNAME").AsString = edtName.Text.Trim();
            q.ParamByName("PPASS").AsString = edtPass.Text.Trim();
            q.ParamByName("PSDAT").AsString = edtDate1.Text.Trim();
            q.ParamByName("PEDAT").AsString = edtDate2.Text.Trim();
            q.ParamByName("MDATE").AsString = DateTime.Now.ToString("yyyy-MM-dd");
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 입/수정시 에러발생..\r\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ReloadList();
        edtCode.Focus();
    }

    /// <summary>원본 DBGrid1KeyDown: Delete 키로 삭제.</summary>
    private void Grid_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete) return;
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) return;

        var userId = row["USRID"].ToString() ?? string.Empty;
        if (!PublicLib.ConfirmDelete("사용자 ID: " + userId)) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM PASSWD WHERE USRID = @CODE");
            q.ParamByName("CODE").AsString = userId;
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ReloadList();
    }
}
