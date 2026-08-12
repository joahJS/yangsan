using System.Data;
using pStock.Common;
using pStock.Data;
using pStock.Forms.Common;

namespace pStock.Forms.BA;

/// <summary>
/// 원본 BA03.pas / BA03.dfm (TfrmBA03) 이식 — 품목,단가 마스터(ITEMAS 테이블).
/// 품번(ITNBR) = 거래처코드(CVCOD, 4자리) + 순번(3자리) 로 구성된다.
/// 원본 cnITEMAS_I/U (cmSQL_0.pas)의 INSERT/UPDATE SQL을 그대로 이식.
/// </summary>
public partial class BA03Form : Form
{
    private string _vSort = "Code";
    private DataTable? _listTable;

    public BA03Form()
    {
        InitializeComponent();

        PublicLib.MakeTypingFriendly(this.eItwgt);
        PublicLib.MakeTypingFriendly(this.edtIcost);
        PublicLib.MakeTypingFriendly(this.edtBcost);
        PublicLib.MakeTypingFriendly(this.edtOcost);
    }

    private void BtnExcel_Click(object? sender, EventArgs e) =>
        pStock.Common.ExcelExporter.Export(this.grid, "품목단가마스터");

    private void BtnPrint_Click(object? sender, EventArgs e) =>
        pStock.Common.GridPrinter.Print(this.grid, "품목단가마스터");

    private void EdtWord_KeyUp(object? sender, KeyEventArgs e) => LocateInGrid(this.edtWord.Text);

    private void ChkAuto_Click(object? sender, EventArgs e) => CodeToggle(!this.chkAuto.Checked);

    private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e) => SyncEditFromGrid();

    private void Grid_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete) this.btnDel.PerformClick();
    }

    private void BtnNew_Click(object? sender, EventArgs e)
    {
        ClearEdit();
        this.edtCvcod.Focus();
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        if (Save()) { ClearEdit(); this.edtCvcod.Focus(); }
    }

    private void BtnOne_Click(object? sender, EventArgs e) => Save();

    private void BtnDel_Click(object? sender, EventArgs e) => Delete();

    private void BtnClose_Click(object? sender, EventArgs e) => Close();

    private void BA03Form_Load(object? sender, EventArgs e)
    {
        ResetDanwiList();
        ResetSavLocList();
        this._vSort = "Code";
        ReloadList();
    }

    private void BA03Form_KeyDown(object? sender, KeyEventArgs e)
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

    /// <summary>원본 prcDanwiReset: REFFPF(RCDTP='PG')에서 단위 목록 로드.</summary>
    private void ResetDanwiList()
    {
        cboDanwi.Items.Clear();
        cboDanwi.Items.Add("");
        foreach (var (_, retxf) in ReffpfCache.Get("PG")) cboDanwi.Items.Add(retxf);
    }

    /// <summary>원본 prcSavLocReset: REFFPF(RCDTP='CG')에서 저장위치 목록 로드.</summary>
    private void ResetSavLocList()
    {
        cboSavLoc.Items.Clear();
        cboSavLoc.Items.Add("");
        foreach (var (refno, _) in ReffpfCache.Get("CG")) cboSavLoc.Items.Add(refno);
    }

    /// <summary>원본 prcDBopen.</summary>
    private void ReloadList()
    {
        using var q = new DbQuery();
        q.Add("SELECT A.*,CVCOD,B.CVNAM FROM ITEMAS A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON B.CVCOD=SUBSTRING(A.ITNBR,1,4)");
        q.Add(_vSort == "Name" ? "ORDER BY ITDSC" : "ORDER BY ITNBR");
        q.Open();
        _listTable = q.Table;
        grid.DataSource = _listTable;
        ApplyGridHeaders();
        lblDbCnt.Text = "품목 총 " + PublicLib.MoneyToStr(_listTable?.Rows.Count ?? 0) + " 건";
    }

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["ITNBR"] = "품번", ["ITDSC"] = "품명", ["ISPEC"] = "규격", ["DANWI"] = "단위",
            ["ITWGT"] = "중량", ["ICOST"] = "입고단가", ["BCOST"] = "기준단가", ["OCOST"] = "출고단가",
            ["SAVLOC"] = "저장위치", ["IBIGO"] = "비고", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
            if (col.Name is "YESNO" or "MDATE") col.Visible = false;
    }

    private void LocateInGrid(string word)
    {
        var field = _vSort == "Name" ? "ITDSC" : "ITNBR";
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

        edtCode.Text = row["ITNBR"].ToString();
        edtCvcod.Text = (row["ITNBR"].ToString() ?? "").PadRight(4)[..4];
        edtCvnam.Text = row["CVNAM"].ToString();
        edtItdsc.Text = row["ITDSC"].ToString();
        edtSpec.Text = row["ISPEC"].ToString();
        cboDanwi.SelectedIndex = cboDanwi.Items.IndexOf((row["DANWI"].ToString() ?? "").Trim());
        eItwgt.Value = SafeDecimal(row["ITWGT"], eItwgt);
        edtIcost.Value = SafeDecimal(row["ICOST"], edtIcost);
        edtBcost.Value = SafeDecimal(row["BCOST"], edtBcost);
        edtOcost.Value = SafeDecimal(row["OCOST"], edtOcost);
        cboSavLoc.SelectedIndex = cboSavLoc.Items.IndexOf((row["SAVLOC"].ToString() ?? "").Trim());
        edtBigo.Text = row["IBIGO"].ToString();

        edtCvcod.Enabled = false;
        chkAuto.Enabled = false;
        CodeToggle(false);
        btnDel.Enabled = true;
    }

    private static decimal SafeDecimal(object value, NumericUpDown target)
    {
        if (value == null || value == DBNull.Value) return 0;
        var d = Convert.ToDecimal(value);
        return Math.Min(Math.Max(d, target.Minimum), target.Maximum);
    }

    private void ClearEdit()
    {
        edtCode.Clear(); edtItdsc.Clear(); edtSpec.Clear(); edtCvnam.Clear();
        cboDanwi.SelectedIndex = 0; cboSavLoc.SelectedIndex = 0;
        eItwgt.Value = 0; edtIcost.Value = 0; edtBcost.Value = 0; edtOcost.Value = 0;
        edtBigo.Clear();
        edtCvcod.Enabled = true; chkAuto.Enabled = true;
        CodeToggle(!chkAuto.Checked);
        btnDel.Enabled = false;
    }

    private void CodeToggle(bool enabled) => PublicLib.CodeToggle(edtCode, enabled);

    /// <summary>원본 edtcvcodKeyDown: Enter 시 거래처코드로 CVMAST(CVGU='2') 조회, 빈칸이면 검색 팝업.</summary>
    private void EdtCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            using var dlg = new Common.CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCvcod.Text = dlg.SelectedCode;
                edtCvnam.Text = dlg.SelectedName;
            }
            else
            {
                edtCvnam.Clear();
            }
            edtCode.Text = edtCvcod.Text.Trim();
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD AND CVGU='2'");
        q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtCvcod.Text = q.FieldByName("CVCOD").AsString;
            edtCvnam.Text = q.FieldByName("CVNAM").AsString;
        }
        edtCode.Text = edtCvcod.Text.Trim();
    }

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

        if (!chkAuto.Checked && !string.IsNullOrWhiteSpace(edtCode.Text))
        {
            var cv = edtCode.Text.Length >= 4 ? edtCode.Text[..4] : edtCode.Text;
            using (var q = new DbQuery())
            {
                q.Add($"SELECT * FROM CVMAST WHERE CVCOD='{cv}'");
                q.Open();
                if (q.IsEmpty)
                {
                    MessageBox.Show($"코드: {cv}는 존재하지 않는 거래처코드입니다.\r\n" +
                        "품번의 앞4자리는 반드시 거래처코드여야합니다.\r\n" +
                        "자동채번을 하거나, 거래처코드를 다시확인하여 주십시오.", "경고",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    edtCode.Focus();
                    return false;
                }
            }
            // 품번 중복검사는 신규 등록시에만 수행한다(수정시 자기 품번이 이미 존재하는건 정상).
            if (isInsert)
            {
                using var q = new DbQuery();
                q.Add($"SELECT * FROM ITEMAS WHERE ITNBR='{edtCode.Text.Trim()}'");
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
        }

        if (string.IsNullOrWhiteSpace(edtItdsc.Text))
        {
            MessageBox.Show("품명을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtItdsc.Focus();
            return false;
        }

        if (edtIcost.Value + edtOcost.Value + edtBcost.Value == 0)
        {
            MessageBox.Show("단가을 입력하세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            edtIcost.Focus();
            return false;
        }

        return true;
    }

    /// <summary>원본 fncAutoNo.</summary>
    private string AutoNo()
    {
        using var q = new DbQuery();
        q.Add("SELECT MAX(ITNBR) AS MAXNBR FROM ITEMAS WHERE SUBSTRING(ITNBR,1,4)=@CVCOD");
        q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (q.IsEmpty || q.FieldByName("MAXNBR").IsNull)
            return edtCvcod.Text.Trim() + "001";

        var max = q.FieldByName("MAXNBR").AsString;
        var seq = PublicLib.StrToMoney(max.Length >= 8 ? max[4..7] : "0") + 1;
        return edtCvcod.Text.Trim() + seq.ToString("000");
    }

    /// <summary>원본 fncAddJob (btnAdd=연속저장, btnOne=저장 모두 이 로직 사용).</summary>
    private bool Save()
    {
        bool isInsert = !btnDel.Enabled;
        if (!ErrCheck(isInsert)) return false;

        var no = isInsert && chkAuto.Checked ? AutoNo() : edtCode.Text.Trim();
        edtCode.Text = no;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            if (isInsert)
            {
                q.Add("INSERT INTO ITEMAS (ITNBR,ITDSC,ISPEC,DANWI,ITWGT,ICOST,OCOST,BCOST,YESNO,SAVLOC,IBIGO,MDATE)");
                q.Add("VALUES(@ITNBR,@ITDSC,@ISPEC,@DANWI,@ITWGT,@ICOST,@OCOST,@BCOST,'Y',@SAVLOC,@IBIGO,GETDATE())");
            }
            else
            {
                q.Add("UPDATE ITEMAS SET ITDSC=@ITDSC,ISPEC=@ISPEC,DANWI=@DANWI,ITWGT=@ITWGT,");
                q.Add("  ICOST=@ICOST,OCOST=@OCOST,BCOST=@BCOST,SAVLOC=@SAVLOC,IBIGO=@IBIGO,MDATE=GETDATE()");
                q.Add(" WHERE ITNBR=@ITNBR");
            }
            q.ParamByName("ITNBR").AsString = no;
            q.ParamByName("ITDSC").AsString = edtItdsc.Text.Trim();
            q.ParamByName("ISPEC").AsString = edtSpec.Text.Trim();
            q.ParamByName("DANWI").AsString = cboDanwi.Text.Trim();
            q.ParamByName("ITWGT").AsCurrency = eItwgt.Value;
            q.ParamByName("ICOST").AsCurrency = edtIcost.Value;
            q.ParamByName("OCOST").AsCurrency = edtOcost.Value;
            q.ParamByName("BCOST").AsCurrency = edtBcost.Value;
            q.ParamByName("SAVLOC").AsString = cboSavLoc.Text.Trim();
            q.ParamByName("IBIGO").AsString = edtBigo.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("품번 저장시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        ReloadList();
        return true;
    }

    /// <summary>원본 btnDelClick.</summary>
    private void Delete()
    {
        if (!PublicLib.ConfirmDelete("품목코드: " + edtCode.Text)) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM ITEMAS WHERE ITNBR = @ITNBR");
            q.ParamByName("ITNBR").AsString = edtCode.Text.Trim();
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("품번 삭제시 에러발생..\r\n" + ex.Message, "경고",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ReloadList();
    }
}
