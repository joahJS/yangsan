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
public class BA00EForm : Form
{
    private readonly FastDataGridView grid = new();
    private readonly TextBox edtCode = new();  // USRID
    private readonly TextBox edtName = new();  // PNAME
    private readonly TextBox edtPass = new();  // PPASS
    private readonly TextBox edtDate1 = new(); // PSDAT 사용시작일
    private readonly TextBox edtDate2 = new(); // PEDAT 사용종료일

    private readonly Button btnNew = new() { Text = "신규(F1)" };
    private readonly Button btnSave = new() { Text = "저장(F2)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    private DataTable? _passTable;

    public BA00EForm()
    {
        Text = "패스워드 변경";
        Width = 700;
        Height = 500;
        KeyPreview = true;
        StartPosition = FormStartPosition.CenterParent;

        BuildLayout();

        Load += (_, _) => ReloadList();
        KeyDown += BA00EForm_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnSave, btnClose });
        int bx = 5;
        foreach (Control c in top.Controls) { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 100 };
        var layout = new GridLayout(editPanel, 10, 5, slotWidth: 250, labelWidth: 90, rowHeight: 30, slotsPerRow: 2);
        layout.Add("사용자 ID", edtCode);
        layout.Add("성명", edtName);
        layout.Add("비밀번호", edtPass);
        layout.Add("사용시작일", edtDate1);
        layout.Add("사용종료일", edtDate2);
        editPanel.Height = layout.Bottom();

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.CellClick += (_, _) => SyncEditFromGrid();
        grid.KeyDown += Grid_KeyDown;

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => { ClearEdit(); edtCode.Focus(); };
        btnSave.Click += (_, _) => Save();
        btnClose.Click += (_, _) => Close();
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
