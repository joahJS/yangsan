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
public class BA03Form : Form
{
    private readonly FastDataGridView grid = new();
    private readonly TextBox edtCvcod = new();  // 거래처코드(품번 앞 4자리)
    private readonly TextBox edtCvnam = new();  // 거래처명(참조표시, 읽기전용)
    private readonly TextBox edtCode = new();   // 품번 전체
    private readonly CheckBox chkAuto = new() { Text = "자동채번" };
    private readonly TextBox edtItdsc = new();  // 품명
    private readonly TextBox edtSpec = new();   // 규격
    private readonly ComboBox cboDanwi = new() { DropDownStyle = ComboBoxStyle.DropDownList }; // 단위
    private readonly NumericUpDown eItwgt = new() { DecimalPlaces = 2, Maximum = 999999 };     // 중량
    private readonly NumericUpDown edtIcost = new() { DecimalPlaces = 0, Maximum = 999999999 }; // 입고단가
    private readonly NumericUpDown edtBcost = new() { DecimalPlaces = 0, Maximum = 999999999 }; // 기준단가
    private readonly NumericUpDown edtOcost = new() { DecimalPlaces = 0, Maximum = 999999999 }; // 출고단가
    private readonly ComboBox cboSavLoc = new() { DropDownStyle = ComboBoxStyle.DropDownList }; // 저장위치
    private readonly TextBox edtBigo = new();
    private readonly TextBox edtWord = new();

    private readonly Button btnNew = new() { Text = "신규(F1)" };
    private readonly Button btnAdd = new() { Text = "연속저장(F2)" };
    private readonly Button btnOne = new() { Text = "저장(F3)" };
    private readonly Button btnDel = new() { Text = "삭제(F4)" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };
    private readonly Label lblDbCnt = new() { AutoSize = true };

    private string _vSort = "Code";
    private DataTable? _listTable;

    public BA03Form()
    {
        Text = "품목,단가 마스터";
        Width = 1100;
        Height = 650;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { ResetDanwiList(); ResetSavLocList(); _vSort = "Code"; ReloadList(); };
        KeyDown += BA03Form_KeyDown;
    }

    private void BuildLayout()
    {
        foreach (var n in new[] { eItwgt, edtIcost, edtBcost, edtOcost })
            PublicLib.MakeTypingFriendly(n);

        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnAdd, btnOne, btnDel, btnExcel, btnPrint, btnClose, lblDbCnt });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnAdd, btnOne, btnDel, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        lblDbCnt.Left = bx + 20; lblDbCnt.Top = 14;
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(grid, "품목단가마스터");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(grid, "품목단가마스터");

        var searchPanel = new Panel { Dock = DockStyle.Top, Height = 35 };
        var lblWord = new Label { Text = "검색어", Left = 5, Top = 10, AutoSize = true };
        edtWord.Left = 60; edtWord.Top = 6; edtWord.Width = 200;
        edtWord.KeyUp += (_, _) => LocateInGrid(edtWord.Text);
        searchPanel.Controls.AddRange(new Control[] { lblWord, edtWord });

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 250, AutoScroll = true };
        var layout = new GridLayout(editPanel, 5, 5, slotWidth: 250, labelWidth: 85, rowHeight: 30, slotsPerRow: 3);

        layout.Add("거래처코드", edtCvcod);
        edtCvnam.ReadOnly = true;
        layout.Add("거래처명", edtCvnam, span: 2);

        layout.Add("품번", edtCode);
        layout.AddRaw(chkAuto, width: 100);

        layout.NewRow();
        layout.Add("품명", edtItdsc, span: 2);

        layout.Add("규격", edtSpec);
        layout.Add("단위", cboDanwi);

        layout.Add("중량", eItwgt);
        layout.Add("저장위치", cboSavLoc);

        layout.NewRow();
        layout.Add("입고단가", edtIcost);
        layout.Add("기준단가", edtBcost);
        layout.Add("출고단가", edtOcost);

        layout.NewRow();
        layout.Add("비고", edtBigo, span: 3);

        editPanel.Height = layout.Bottom(15);

        edtCvcod.KeyDown += EdtCvcod_KeyDown;
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

        btnNew.Click += (_, _) => { ClearEdit(); edtCvcod.Focus(); };
        btnAdd.Click += (_, _) => { if (Save()) { ClearEdit(); edtCvcod.Focus(); } };
        btnOne.Click += (_, _) => Save();
        btnDel.Click += (_, _) => Delete();
        btnClose.Click += (_, _) => Close();
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
