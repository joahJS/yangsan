using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA04.pas / BA04.dfm (TfrmBA04) 이식 — 착지처 마스터(REACH 테이블).
/// 원본 cnREACH_I/U (cmSQL_0.pas) SQL을 그대로 이식.
/// </summary>
public class BA04Form : Form
{
    private readonly FastDataGridView grid = new();
    private readonly TextBox edtCode = new();
    private readonly CheckBox chkAuto = new() { Text = "자동채번" };
    private readonly TextBox edtName = new();
    private readonly TextBox edtPost = new();
    private readonly TextBox edtAddr1 = new();
    private readonly TextBox edtAddr2 = new();
    private readonly TextBox edtTel = new();
    private readonly TextBox edtBigo = new();
    private readonly TextBox edtWord = new();

    private readonly Button btnNew = new() { Text = "신규(F1)" };
    private readonly Button btnAdd = new() { Text = "저장(F2)" };
    private readonly Button btnOne = new() { Text = "수정(F3)" };
    private readonly Button btnDel = new() { Text = "삭제(F4)" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };
    private readonly Label lblDbCnt = new() { AutoSize = true };

    private string _vSort = "Code";
    private DataTable? _listTable;

    public BA04Form()
    {
        Text = "착지처 마스터";
        Width = 950;
        Height = 600;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { _vSort = "Code"; ReloadList(); };
        KeyDown += BA04Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnAdd, btnOne, btnDel, btnClose, lblDbCnt });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnAdd, btnOne, btnDel, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }
        lblDbCnt.Left = bx + 20; lblDbCnt.Top = 14;

        var searchPanel = new Panel { Dock = DockStyle.Top, Height = 35 };
        var lblWord = new Label { Text = "검색어", Left = 5, Top = 10, AutoSize = true };
        edtWord.Left = 60; edtWord.Top = 6; edtWord.Width = 200;
        edtWord.KeyUp += (_, _) => LocateInGrid(edtWord.Text);
        searchPanel.Controls.AddRange(new Control[] { lblWord, edtWord });

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 190, AutoScroll = true };
        var layout = new GridLayout(editPanel, 5, 5, slotWidth: 250, labelWidth: 80, rowHeight: 30, slotsPerRow: 3);

        layout.Add("코드", edtCode);
        layout.AddRaw(chkAuto, width: 100);
        layout.NewRow();

        layout.Add("착지처명", edtName);
        layout.Add("우편번호", edtPost);
        layout.Add("전화번호", edtTel);

        layout.Add("주소1", edtAddr1, span: 2);
        layout.NewRow();
        layout.Add("주소2", edtAddr2, span: 2);

        layout.NewRow();
        layout.Add("비고", edtBigo, span: 2);

        editPanel.Height = layout.Bottom(15);

        edtAddr1.DoubleClick += (_, _) => LookupPostalCode();
        chkAuto.Click += (_, _) => CodeToggle(!chkAuto.Checked);

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.CellDoubleClick += (_, _) => SyncEditFromGrid();
        grid.KeyDown += (_, e) => { if (e.KeyCode == Keys.Delete) btnDel.PerformClick(); };

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(searchPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => { ClearEdit(); edtName.Focus(); };
        btnAdd.Click += (_, _) => Save(isInsert: true);
        btnOne.Click += (_, _) => Save(isInsert: false);
        btnDel.Click += (_, _) => Delete();
        btnClose.Click += (_, _) => Close();
    }

    private void BA04Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F2: btnAdd.PerformClick(); break;
            case Keys.F3: btnOne.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 prcDBopen.</summary>
    private void ReloadList()
    {
        using var q = new DbQuery();
        q.Add("SELECT A1.*, (ADDR1+' '+ADDR2) ADDR FROM REACH A1");
        q.Add(_vSort == "Name" ? "ORDER BY LNNAM" : "ORDER BY LNCOD");
        q.Open();
        _listTable = q.Table;
        grid.DataSource = _listTable;
        ApplyGridHeaders();
        lblDbCnt.Text = "착지처 총 " + PublicLib.MoneyToStr(_listTable?.Rows.Count ?? 0) + " 건";
    }

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["LNCOD"] = "코드", ["LNNAM"] = "착지처명", ["POSNO"] = "우편번호",
            ["ADDR"] = "주소", ["LNTEL"] = "전화번호", ["MBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
            if (col.Name is "ADDR1" or "ADDR2" or "USRID" or "MDATE") col.Visible = false;

        // 주소 컬럼이 잘려 보여서 요청대로 폭을 3배로 넓힘.
        if (grid.Columns["ADDR"] is { } addrCol) addrCol.Width *= 3;
    }

    private void LocateInGrid(string word)
    {
        var field = _vSort == "Name" ? "LNNAM" : "LNCOD";
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.Cells[field].Value?.ToString()?.StartsWith(word, StringComparison.OrdinalIgnoreCase) == true)
            {
                grid.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    private void SyncEditFromGrid()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView row) { ClearEdit(); return; }

        edtCode.Text = row["LNCOD"].ToString();
        edtName.Text = row["LNNAM"].ToString();
        edtAddr1.Text = row["ADDR1"].ToString();
        edtAddr2.Text = row["ADDR2"].ToString();
        edtPost.Text = row["POSNO"].ToString();
        edtTel.Text = row["LNTEL"].ToString();
        edtBigo.Text = row["MBIGO"].ToString();

        chkAuto.Enabled = false;
        CodeToggle(false);
        btnDel.Enabled = true;
    }

    private void ClearEdit()
    {
        edtCode.Clear(); edtName.Clear();
        edtPost.Clear(); edtAddr1.Clear(); edtAddr2.Clear();
        edtTel.Clear(); edtBigo.Clear();
        chkAuto.Enabled = true;
        CodeToggle(!chkAuto.Checked);
        btnDel.Enabled = false;
    }

    private void CodeToggle(bool enabled) => PublicLib.CodeToggle(edtCode, enabled);

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck(bool isInsert)
    {
        if (!chkAuto.Checked && string.IsNullOrWhiteSpace(edtCode.Text))
        {
            MessageBox.Show("자동채번이 아닌경우는 코드를 반드시 입력하세요.", "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtCode.Focus();
            return false;
        }

        // 중복코드 검사는 신규 등록시에만 수행한다. 수정(저장) 시에는 자기 자신의
        // 코드가 이미 존재하는게 당연하므로 여기서 검사하면 항상 "이미존재" 오류가 발생한다.
        if (isInsert && !chkAuto.Checked && !string.IsNullOrWhiteSpace(edtCode.Text))
        {
            using var q = new DbQuery();
            q.Add($"SELECT * FROM REACH WHERE LNCOD='{edtCode.Text.Trim()}'");
            q.Open();
            if (!q.IsEmpty)
            {
                MessageBox.Show($"코드: {edtCode.Text.Trim()}는 이미존재하는 코드입니다.\r\n" +
                    "자동채번을 하거나, 중복되지 않는 번호를 입력하세요.", "경고",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                edtCode.Focus();
                return false;
            }
        }

        if (string.IsNullOrWhiteSpace(edtName.Text))
        {
            MessageBox.Show("착지처명을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtName.Focus();
            return false;
        }

        return true;
    }

    /// <summary>원본 fncAutoNo: MAX(LNCOD)+1, 4자리.</summary>
    private string AutoNo()
    {
        using var q = new DbQuery();
        q.Add("SELECT ISNULL(MAX(LNCOD), 0) AS MAXCODE FROM REACH");
        q.Open();
        var max = q.IsEmpty ? "0" : q.FieldByName("MAXCODE").AsString;
        return (PublicLib.StrToMoney(max) + 1).ToString("0000");
    }

    /// <summary>원본 prcAddJob.</summary>
    private void Save(bool isInsert)
    {
        if (!ErrCheck(isInsert)) return;

        var no = isInsert && chkAuto.Checked ? AutoNo() : edtCode.Text.Trim();
        edtCode.Text = no;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            if (isInsert)
            {
                q.Add("INSERT INTO REACH (LNCOD,LNNAM,ADDR1,ADDR2,POSNO,LNTEL,MBIGO,USRID,MDATE)");
                q.Add("VALUES(@LNCOD,@LNNAM,@ADDR1,@ADDR2,@POSNO,@LNTEL,@MBIGO,@USRID,GETDATE())");
            }
            else
            {
                q.Add("UPDATE REACH SET LNNAM=@LNNAM,ADDR1=@ADDR1,ADDR2=@ADDR2,POSNO=@POSNO,");
                q.Add("  LNTEL=@LNTEL,MBIGO=@MBIGO,USRID=@USRID,MDATE=GETDATE()");
                q.Add(" WHERE LNCOD=@LNCOD");
            }
            q.ParamByName("LNCOD").AsString = no;
            q.ParamByName("LNNAM").AsString = edtName.Text.Trim();
            q.ParamByName("ADDR1").AsString = edtAddr1.Text.Trim();
            q.ParamByName("ADDR2").AsString = edtAddr2.Text.Trim();
            q.ParamByName("POSNO").AsString = PublicLib.DelChar(edtPost.Text, "-");
            q.ParamByName("LNTEL").AsString = edtTel.Text.Trim();
            q.ParamByName("MBIGO").AsString = edtBigo.Text.Trim();
            q.ParamByName("USRID").AsString = UserContext.Current.Code;
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("착지처 자료 보수시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReloadList();
    }

    /// <summary>원본 btnDelClick.</summary>
    private void Delete()
    {
        if (!PublicLib.ConfirmDelete("착지처코드: " + edtCode.Text)) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM REACH WHERE LNCOD = @LNCOD");
            q.ParamByName("LNCOD").AsString = edtCode.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReloadList();
    }

    /// <summary>원본 up_InfoPOSTF (우편번호 검색 팝업).</summary>
    private void LookupPostalCode()
    {
        using var dlg = new Common.PostalLookupForm();
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        edtPost.Text = dlg.SelectedZip;
        edtAddr1.Text = dlg.SelectedAddr;
        edtAddr2.Focus();
    }
}
