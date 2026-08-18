using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA02.pas / BA02.dfm (Tfrm_BA02) 이식 — 공통코드 마스터(REFFPF 테이블).
/// 좌측 그리드: 그룹(RCDTP, REFNO='') / 우측 그리드: 선택한 그룹에 속한 코드(REFNO&lt;&gt;'').
///
/// 원본 DFM에서 확인한 SQL:
///   qryGroup: SELECT * FROM REFFPF WHERE REFNO = ''  ORDER BY RCDTP
///   qryCode : SELECT * FROM REFFPF WHERE REFNO <> '' AND RCDTP = :RCDTP ORDER BY RCDTP
///             (원본은 DataSource=srcGroup 마스터-디테일 자동 연동. 여기서는 그룹 그리드
///              선택이 바뀔 때마다 qryCode를 다시 조회하는 방식으로 재현한다.)
/// </summary>
public partial class BA02Form : Form
{
    private DataTable? _groupTable;
    private DataTable? _codeTable;

    public BA02Form()
    {
        InitializeComponent();

        headerIconImg.Image = IconFactory.Create(IconFactory.Document, Color.White, 18);
        btnNew.Image = IconFactory.Create(IconFactory.New, Color.White, 16);
        btnSave.Image = IconFactory.Create(IconFactory.Save, Color.FromArgb(47, 111, 237), 16);
        btnUpd.Image = IconFactory.Create(IconFactory.Edit, Color.FromArgb(47, 111, 237), 16);
        btnDel.Image = IconFactory.Create(IconFactory.Delete, Color.FromArgb(220, 53, 69), 16);
        btnSearch.Image = IconFactory.Create(IconFactory.Search, Color.FromArgb(47, 111, 237), 16);
        btnClose.Image = IconFactory.Create(IconFactory.Close, Color.FromArgb(75, 85, 99), 16);
        btnNew.ImageAlign = HorizontalAlignment.Left;
        btnSave.ImageAlign = HorizontalAlignment.Left;
        btnUpd.ImageAlign = HorizontalAlignment.Left;
        btnDel.ImageAlign = HorizontalAlignment.Left;
        btnSearch.ImageAlign = HorizontalAlignment.Left;
        btnClose.ImageAlign = HorizontalAlignment.Left;
        btnNew.ImageOffset = new Point(12, 0);
        btnSave.ImageOffset = new Point(12, 0);
        btnUpd.ImageOffset = new Point(12, 0);
        btnDel.ImageOffset = new Point(12, 0);
        btnSearch.ImageOffset = new Point(12, 0);
        btnClose.ImageOffset = new Point(12, 0);
        btnNew.TextOffset = new Point(10, 0);
        btnSave.TextOffset = new Point(10, 0);
        btnUpd.TextOffset = new Point(10, 0);
        btnDel.TextOffset = new Point(10, 0);
        btnSearch.TextOffset = new Point(10, 0);
        btnClose.TextOffset = new Point(10, 0);

        ApplyMode();
    }

    /// <summary>
    /// 라디오버튼(구분코드/코드) 선택에 따라 입력란 구성을 전환한다.
    /// - 구분코드: 코드/약칭 입력란을 숨겨 구분(RCDTP)+전체명(RETXF)만으로 그룹을 등록하게 한다.
    /// - 코드: 코드/약칭 입력란을 다시 보이고, 구분/전체명은 계속 입력 가능(Enabled) 상태를 유지하되
    ///   왼쪽 그룹 그리드에서 현재 선택된 행의 구분코드/전체명 값으로 채워 넣어 편의를 준다
    ///   (자유 입력 자체는 막지 않음 — 기존에 그룹 강제 바인딩을 없앤 정책과 동일하게 유지).
    /// </summary>
    private void ApplyMode()
    {
        bool codeMode = radModeCode.Checked;

        lblCode.Visible = codeMode;
        edtCode.Visible = codeMode;
        lblRetxs.Visible = codeMode;
        edtRetxs.Visible = codeMode;

        edtRcdtp.Enabled = true;
        edtRetxf.Enabled = true;

        if (codeMode)
        {
            if (gridGroup.CurrentRow?.DataBoundItem is DataRowView row)
            {
                edtRcdtp.Text = row["RCDTP"].ToString();
                edtRetxf.Text = row["RETXF"].ToString();
            }
        }
        else
        {
            // 구분코드 모드로 전환하면 코드/약칭은 이 모드에서 쓰지 않으므로 비워서
            // 이전 값이 실수로 같이 저장되는 일이 없게 한다.
            edtCode.Clear();
            edtRetxs.Clear();
        }
    }

    /// <summary>저장/수정 후 왼쪽 그룹 그리드에서 해당 구분(RCDTP) 행을 찾아 선택(포커스 표시)한다.
    /// 오른쪽 코드 그리드 갱신은 이 메서드가 아니라 호출부(RefreshAndFocusSaved)에서 LoadCode를
    /// 직접 호출해 처리한다(SelectionChanged 이벤트 발생 여부에 의존하지 않기 위함).</summary>
    private void SelectGroupRow(string rcdtp)
    {
        foreach (DataGridViewRow gridRow in gridGroup.Rows)
        {
            if (gridRow.DataBoundItem is DataRowView row && (row["RCDTP"].ToString() ?? string.Empty) == rcdtp)
            {
                gridGroup.ClearSelection();
                gridRow.Selected = true;
                gridGroup.CurrentCell = gridRow.Cells[0];
                return;
            }
        }
    }

    /// <summary>오른쪽 코드 그리드에서 해당 코드(REFNO) 행을 찾아 선택하고 포커스를 준다.</summary>
    private void SelectCodeRow(string refno)
    {
        foreach (DataGridViewRow gridRow in gridCode.Rows)
        {
            if (gridRow.DataBoundItem is DataRowView row && (row["REFNO"].ToString() ?? string.Empty) == refno)
            {
                gridCode.ClearSelection();
                gridRow.Selected = true;
                gridCode.CurrentCell = gridRow.Cells[0];
                gridCode.Focus();
                return;
            }
        }
    }

    /// <summary>저장/수정 완료 후 양쪽 그리드를 새로 조회하고, 방금 등록/수정된 행에 포커스를 맞춘다.
    /// 그룹 재조회(LoadGroup) 직후 자동 선택되는 첫 행이 방금 저장한 행과 우연히 같으면
    /// SelectionChanged 이벤트가 발생하지 않아 오른쪽 코드 그리드가 안 바뀌는 문제가 있었으므로,
    /// 이벤트 발생 여부와 상관없이 LoadCode(rcdtp)를 직접 호출해 항상 최신 상태로 맞춘다.</summary>
    private void RefreshAndFocusSaved(string rcdtp, string refno)
    {
        LoadGroup();
        SelectGroupRow(rcdtp);
        LoadCode(rcdtp);
        if (!string.IsNullOrEmpty(refno))
        {
            SelectCodeRow(refno);
        }
        else
        {
            gridGroup.Focus();
        }
    }

    private void BA02Form_Load(object? sender, EventArgs e)
    {
        LoadGroup();
    }

    private void RadModeGroup_CheckedChanged(object? sender, EventArgs e)
    {
        ApplyMode();
    }

    private void RadModeCode_CheckedChanged(object? sender, EventArgs e)
    {
        ApplyMode();
    }

    private void GridGroup_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        SyncEditFromGroup();
    }

    private void GridCode_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        SyncEditFromCode();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        ClearEdit();
        (radModeCode.Checked ? edtCode : edtRcdtp).Focus();
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        Save(isInsert: true);
    }

    private void BtnUpd_Click(object? sender, EventArgs e)
    {
        Save(isInsert: false);
    }

    private void BtnDel_Click(object? sender, EventArgs e)
    {
        Delete();
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        Search();
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void BA02Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F2: btnSave.PerformClick(); break;
            case Keys.F3: btnUpd.PerformClick(); break;
            case Keys.F4: btnDel.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void LoadGroup()
    {
        using var q = new DbQuery();
        q.Add("SELECT * FROM REFFPF WHERE REFNO = '' ORDER BY RCDTP");
        q.Open();
        _groupTable = q.Table;
        gridGroup.DataSource = _groupTable;
        if (gridGroup.Columns["RCDTP"] != null) gridGroup.Columns["RCDTP"]!.HeaderText = "구분코드";
        if (gridGroup.Columns["RETXF"] != null) gridGroup.Columns["RETXF"]!.HeaderText = "전체명";
        if (gridGroup.Columns["RETXS"] != null) gridGroup.Columns["RETXS"]!.HeaderText = "약칭(S)";
        if (gridGroup.Columns["RPRINT"] != null) gridGroup.Columns["RPRINT"]!.HeaderText = "출력여부";
        if (gridGroup.Columns["REFNO"] != null) gridGroup.Columns["REFNO"]!.Visible = false;
        groupBadgeLabel.Text = (_groupTable?.Rows.Count ?? 0) + "건";
        LoadCode(string.Empty);
    }

    private void LoadCode(string rcdtp)
    {
        using var q = new DbQuery();
        if (string.IsNullOrEmpty(rcdtp))
        {
            q.Add("SELECT * FROM REFFPF WHERE REFNO <> '' AND 1 = 0");
        }
        else
        {
            q.Add("SELECT * FROM REFFPF WHERE REFNO <> '' AND RCDTP = @RCDTP ORDER BY RCDTP");
            q.ParamByName("RCDTP").AsString = rcdtp;
        }
        q.Open();
        _codeTable = q.Table;
        gridCode.DataSource = _codeTable;
        if (gridCode.Columns["RCDTP"] != null) gridCode.Columns["RCDTP"]!.HeaderText = "구분코드";
        if (gridCode.Columns["REFNO"] != null) gridCode.Columns["REFNO"]!.HeaderText = "코드";
        if (gridCode.Columns["RETXF"] != null) gridCode.Columns["RETXF"]!.HeaderText = "전체명";
        if (gridCode.Columns["RETXS"] != null) gridCode.Columns["RETXS"]!.HeaderText = "약칭(S)";
        if (gridCode.Columns["RPRINT"] != null) gridCode.Columns["RPRINT"]!.HeaderText = "출력여부";
        codeBadgeLabel.Text = (_codeTable?.Rows.Count ?? 0) + "건";
    }

    private void GridGroup_SelectionChanged(object? sender, EventArgs e)
    {
        if (gridGroup.CurrentRow?.DataBoundItem is DataRowView row)
        {
            var rcdtp = row["RCDTP"].ToString() ?? string.Empty;
            LoadCode(rcdtp);

            // "코드" 모드에서는 왼쪽 그룹 그리드 선택이 바뀔 때마다 구분/전체명을 그 그룹 값으로
            // 갱신해준다(코드 입력 전 소속 그룹을 편하게 잡아주는 용도, 자유 수정은 계속 가능).
            if (radModeCode.Checked)
            {
                edtRcdtp.Text = rcdtp;
                edtRetxf.Text = row["RETXF"].ToString();
            }
        }
    }

    private void SyncEditFromGroup()
    {
        if (gridGroup.CurrentRow?.DataBoundItem is not DataRowView row) { ClearEdit(); return; }
        radModeGroup.Checked = true;
        edtRcdtp.Text = row["RCDTP"].ToString();
        edtCode.Text = row["REFNO"].ToString();
        edtRetxf.Text = row["RETXF"].ToString();
        edtRetxs.Text = row["RETXS"].ToString();
    }

    private void SyncEditFromCode()
    {
        if (gridCode.CurrentRow?.DataBoundItem is not DataRowView row) { ClearEdit(); return; }
        // 코드 행 자체에 RCDTP 컬럼이 있으므로 항상 여기서 채운다(이전에는 이 값이
        // 채워지지 않아 좌측 그룹을 더블클릭한 적이 없으면 빈 값으로 남아있었고,
        // 그 상태로 수정/삭제를 실행하면 WHERE RCDTP=... 조건이 어긋나 다른 행이거나
        // 아무 행도 대상이 되지 않는 문제가 있었음).
        radModeCode.Checked = true;
        edtRcdtp.Text = row["RCDTP"].ToString();
        edtCode.Text = row["REFNO"].ToString();
        edtRetxf.Text = row["RETXF"].ToString();
        edtRetxs.Text = row["RETXS"].ToString();
    }

    private void ClearEdit()
    {
        // "구분코드" 모드의 신규는 그룹 자체를 새로 등록하는 것이므로 구분/전체명도 함께 비운다.
        // "코드" 모드의 신규는 선택된 그룹 밑에 코드를 추가하는 것이므로 구분/전체명은 유지한다.
        if (!radModeCode.Checked)
        {
            edtRcdtp.Clear();
            edtRetxf.Clear();
        }
        edtCode.Clear();
        edtRetxs.Clear();
    }

    /// <summary>원본 fncErrCheck.</summary>
    private bool ErrCheck()
    {
        if (string.IsNullOrWhiteSpace(edtRcdtp.Text) || string.IsNullOrWhiteSpace(edtRetxf.Text))
        {
            MessageBox.Show("입력항목이 누락되었습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    /// <summary>RCDTP+REFNO 조합의 행이 이미 있는지 확인 (INSERT 전 중복 방지 / UPDATE 전 대상 존재 확인용).</summary>
    private bool ExistsKey(string rcdtp, string refno)
    {
        using var q = new DbQuery();
        q.Add("SELECT COUNT(*) AS CNT FROM REFFPF WHERE RCDTP = @RCDTP AND REFNO = @REFNO");
        q.ParamByName("RCDTP").AsString = rcdtp;
        q.ParamByName("REFNO").AsString = refno;
        q.Open();
        return q.RecordCount > 0 && q.FieldByName("CNT").AsInteger > 0;
    }

    /// <summary>구분(RCDTP)이 마스터(그룹)로 이미 등록되어 있는지 확인(REFNO='').</summary>
    private bool GroupExists(string rcdtp)
    {
        using var q = new DbQuery();
        q.Add("SELECT COUNT(*) AS CNT FROM REFFPF WHERE RCDTP = @RCDTP AND REFNO = ''");
        q.ParamByName("RCDTP").AsString = rcdtp;
        q.Open();
        return q.RecordCount > 0 && q.FieldByName("CNT").AsInteger > 0;
    }

    /// <summary>
    /// 원본 btnSaveClick (btnSave: insert, btnUpd: update — Tag로 구분하던 것을 파라미터로 대체).
    /// 구분(RCDTP)/코드(REFNO) 모두 입력란 값을 그대로 사용한다 — 왼쪽 마스터(그룹)만 등록하려면
    /// 코드란을 비워둔 채 구분/전체명만 입력하고, 오른쪽 디테일(코드)을 등록하려면 구분(소속 그룹)과
    /// 코드를 함께 입력하면 된다. 둘 다 자유롭게 입력해서 신규등록/수정이 가능해야 하므로 그리드
    /// 선택 여부로 값을 강제로 덮어쓰지 않는다.
    /// </summary>
    private void Save(bool isInsert)
    {
        if (!ErrCheck()) return;

        var rcdtp = edtRcdtp.Text.Trim();
        var refno = edtCode.Text.Trim();

        // 코드(REFNO)를 채워서 신규저장하는 것은 "이 구분(그룹) 밑에 코드를 추가"하는 것인데,
        // 정작 그 구분이 마스터(그룹, REFNO='')로 등록되어 있지 않으면 좌측 그룹 그리드에는
        // REFNO=''인 행만 보이므로 방금 저장한 행이 어디에도 보이지 않아 "저장이 안 되는 것처럼"
        // 보인다. 그래서 실제로는 저장이 됐어도 사용자에게는 실패처럼 느껴지므로, 이 경우는
        // 저장을 진행하지 않고 먼저 마스터로 등록해야 함을 명확히 안내한다.
        if (isInsert && !string.IsNullOrEmpty(refno) && !GroupExists(rcdtp))
        {
            MessageBox.Show(
                $"구분(그룹) '{rcdtp}'이(가) 아직 마스터로 등록되어 있지 않습니다.\r\n" +
                "코드는 이미 등록된 그룹 밑에만 등록할 수 있습니다.\r\n\r\n" +
                "① 코드란을 비우고 저장하여 이 구분을 그룹(마스터)으로 먼저 등록한 뒤\r\n" +
                "② 다시 코드를 입력해 하위 코드를 등록하세요.",
                "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var exists = ExistsKey(rcdtp, refno);

        // 신규(F2)인데 같은 그룹+코드가 이미 존재하면 중복 행을 만드는 대신 안내하고 중단한다.
        // (기존에는 여기서 그냥 INSERT가 되어, 이후 삭제시 키가 같은 두 행이 한꺼번에 지워지는
        //  원인이 되었음)
        if (isInsert && exists)
        {
            MessageBox.Show("이미 등록된 그룹/코드입니다. 수정(F3)을 사용하세요.", "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 수정(F3)인데 대상 행이 없으면(그리드에서 제대로 선택되지 않은 상태 등) 조용히 아무 일도
        // 없었던 것처럼 넘어가지 않고 명확히 알린다.
        if (!isInsert && !exists)
        {
            MessageBox.Show("수정할 대상을 찾을 수 없습니다. 구분/코드 값을 확인한 뒤 다시 시도하세요.",
                "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            if (isInsert)
            {
                q.Add("INSERT INTO REFFPF (RCDTP,REFNO,RETXF,RETXS)");
                q.Add("  VALUES(@RCDTP,@REFNO,@RETXF,@RETXS)");
            }
            else
            {
                q.Add("UPDATE REFFPF SET RETXF=@RETXF,RETXS=@RETXS");
                q.Add(" WHERE RCDTP = @RCDTP AND REFNO = @REFNO");
            }
            q.ParamByName("RCDTP").AsString = rcdtp;
            q.ParamByName("REFNO").AsString = refno;
            q.ParamByName("RETXF").AsString = edtRetxf.Text.Trim();
            q.ParamByName("RETXS").AsString = edtRetxs.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
            ReffpfCache.Invalidate();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            var msg = ex.Message.Contains("잘립니다") || ex.Message.Contains("truncat")
                ? "입력한 값이 항목의 최대 길이를 초과했습니다. 값을 줄여서 다시 시도하세요.\r\n\r\n" + ex.Message
                : ex.Message;
            MessageBox.Show("자료 입/수정시 에러발생..\r\n" + msg, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        RefreshAndFocusSaved(rcdtp, refno);
    }

    /// <summary>원본 btnDelClick.</summary>
    private void Delete()
    {
        if (string.IsNullOrWhiteSpace(edtCode.Text) && (_codeTable?.Rows.Count ?? 0) > 0)
        {
            MessageBox.Show("하위 항목부터 삭제하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!PublicLib.ConfirmDelete($"그룹: {edtRcdtp.Text}\r\n코드: {edtCode.Text}")) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM REFFPF WHERE RCDTP = @RCDTP AND REFNO = @REFNO");
            q.ParamByName("RCDTP").AsString = edtRcdtp.Text.Trim();
            q.ParamByName("REFNO").AsString = edtCode.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
            ReffpfCache.Invalidate();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        LoadGroup();
    }

    /// <summary>원본 btnSearchClick.</summary>
    private void Search()
    {
        using var q = new DbQuery();
        q.Add("SELECT * FROM REFFPF WHERE REFNO = ''");
        if (!string.IsNullOrWhiteSpace(edtRcdtp.Text))
        {
            q.Add("AND RCDTP = @RCDTP");
            q.ParamByName("RCDTP").AsString = edtRcdtp.Text.Trim();
        }
        if (!string.IsNullOrWhiteSpace(edtRetxf.Text))
        {
            q.Add("AND RETXF LIKE @RETXF");
            q.ParamByName("RETXF").AsString = edtRetxf.Text.Trim() + "%";
        }
        q.Open();
        _groupTable = q.Table;
        gridGroup.DataSource = _groupTable;
        groupBadgeLabel.Text = (_groupTable?.Rows.Count ?? 0) + "건";
    }
}
