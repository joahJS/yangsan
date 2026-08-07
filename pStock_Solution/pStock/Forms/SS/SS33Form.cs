using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS33.pas / SS33.dfm (TfrmSS33) 이식 — 계산서 관리(TAXF 테이블) 목록/검색/삭제.
/// 신규/수정 입력창(SS33B)과 자동발행(SS33A)은 이후 단계에서 연결 예정.
/// </summary>
public class SS33Form : Form
{
    private readonly TabControl tabs = new();
    private readonly TabPage tab1 = new("일자별");
    private readonly TabPage tab2 = new("거래처별");

    private readonly FastDataGridView gridList = new();
    private readonly FastDataGridView gridSum = new();
    private readonly DateTimePicker dtpDate1 = new();
    private readonly DateTimePicker dtpDate2 = new();
    private readonly DateTimePicker dtpDate3 = new();
    private readonly DateTimePicker dtpDate4 = new();
    private readonly TextBox edtCvcd1 = new();
    private readonly TextBox edtCvcd2 = new();

    private readonly Button btnNew = new() { Text = "신규(F1)" };
    private readonly Button btnDel = new() { Text = "삭제" };
    private readonly Button btnSum = new() { Text = "합계표" };
    private readonly Button btnAuto = new() { Text = "자동발행" };
    private readonly Button btnSearch = new() { Text = "조회" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };

    public SS33Form()
    {
        Text = "계산서 관리";
        Width = 1300;
        Height = 700;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) =>
        {
            var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDate1.Value = monthStart; dtpDate2.Value = DateTime.Now;
            dtpDate3.Value = monthStart; dtpDate4.Value = DateTime.Now;
            Search();
        };
        KeyDown += SS33Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnDel, btnSum, btnAuto, btnSearch, btnExcel, btnPrint, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnDel, btnSum, btnAuto, btnSearch, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 90; bx += 95; }
        btnAuto.Click += (_, _) =>
        {
            using var dlg = new SS33AForm();
            dlg.ShowDialog(this);
            if (dlg.Executed) Search();
        };
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(gridList, "계산서관리");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(gridList, "계산서관리");

        var panel1 = new Panel { Dock = DockStyle.Top, Height = 35 };
        var lbl1a = new Label { Text = "발행기간", Left = 10, Top = 10, AutoSize = true };
        dtpDate1.Left = 90; dtpDate1.Top = 6; dtpDate1.Width = 110; dtpDate1.Format = DateTimePickerFormat.Short;
        var lbl1b = new Label { Text = "~", Left = 205, Top = 10, AutoSize = true };
        dtpDate2.Left = 220; dtpDate2.Top = 6; dtpDate2.Width = 110; dtpDate2.Format = DateTimePickerFormat.Short;
        panel1.Controls.AddRange(new Control[] { lbl1a, dtpDate1, lbl1b, dtpDate2 });

        var split1 = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 900 };
        gridList.Dock = DockStyle.Fill; gridList.ReadOnly = true; gridList.AllowUserToAddRows = false;
        gridList.CellDoubleClick += (_, _) => OpenEntry(isNew: false);
        gridSum.Dock = DockStyle.Fill; gridSum.ReadOnly = true; gridSum.AllowUserToAddRows = false;
        gridSum.Columns.Add("KEY", "구분");
        gridSum.Columns.Add("CNT", "건수");
        gridSum.Columns.Add("SAMT", "금액");
        split1.Panel1.Controls.Add(gridList);
        split1.Panel2.Controls.Add(gridSum);

        tab1.Controls.Add(split1);
        tab1.Controls.Add(panel1);

        var panel2 = new Panel { Dock = DockStyle.Top, Height = 35 };
        var lbl2a = new Label { Text = "발행기간", Left = 10, Top = 10, AutoSize = true };
        dtpDate3.Left = 90; dtpDate3.Top = 6; dtpDate3.Width = 110; dtpDate3.Format = DateTimePickerFormat.Short;
        var lbl2b = new Label { Text = "~", Left = 205, Top = 10, AutoSize = true };
        dtpDate4.Left = 220; dtpDate4.Top = 6; dtpDate4.Width = 110; dtpDate4.Format = DateTimePickerFormat.Short;
        var lbl2c = new Label { Text = "거래처", Left = 350, Top = 10, AutoSize = true };
        edtCvcd1.Left = 400; edtCvcd1.Top = 6; edtCvcd1.Width = 70;
        var lbl2d = new Label { Text = "~", Left = 475, Top = 10, AutoSize = true };
        edtCvcd2.Left = 490; edtCvcd2.Top = 6; edtCvcd2.Width = 70;
        panel2.Controls.AddRange(new Control[] { lbl2a, dtpDate3, lbl2b, dtpDate4, lbl2c, edtCvcd1, lbl2d, edtCvcd2 });

        tab2.Controls.Add(panel2);

        tabs.Dock = DockStyle.Fill;
        tabs.TabPages.AddRange(new[] { tab1, tab2 });
        tabs.SelectedIndexChanged += (_, _) => Search();

        Controls.Add(tabs);
        Controls.Add(top);

        btnNew.Click += (_, _) => OpenEntry(isNew: true);
        btnDel.Click += (_, _) => Delete();
        btnSum.Click += (_, _) => pStock.Common.GridPrinter.Print(gridList, "매출계산서 합계표");
        btnSearch.Click += (_, _) => Search();
        btnClose.Click += (_, _) => Close();
    }

    private void SS33Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    /// <summary>원본 prcDBopen + qryListAfterOpen(qrySum/qrySum1).</summary>
    private void Search()
    {
        bool isTab1 = tabs.SelectedTab == tab1;

        using var q = new DbQuery();
        q.Add("SELECT A.*,CVNAM FROM TAXF A");
        q.Add("  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD");
        q.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
        if (isTab1)
        {
            q.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
            q.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
            q.Add("ORDER BY TDATE,SEQNO");
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
            {
                q.Add(" AND A.CVCOD BETWEEN @FCVCOD AND @TCVCOD");
                q.ParamByName("FCVCOD").AsString = edtCvcd1.Text.Trim();
                q.ParamByName("TCVCOD").AsString = edtCvcd2.Text.Trim();
            }
            q.ParamByName("DATE1").AsString = dtpDate3.Value.ToString("yyyy-MM-dd");
            q.ParamByName("DATE2").AsString = dtpDate4.Value.ToString("yyyy-MM-dd");
            q.Add("ORDER BY A.CVCOD,TDATE,SEQNO");
        }
        q.Open();
        gridList.DataSource = q.Table;
        ApplyListHeaders();

        gridSum.Rows.Clear();
        if (isTab1)
        {
            using var qs = new DbQuery();
            qs.Add("SELECT TDATE,COUNT(*) CNT,SUM(TAMT) SAMT FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            qs.Add(" GROUP BY TDATE");
            qs.Add(" UNION");
            qs.Add("SELECT '합      계',COUNT(*),SUM(TAMT) FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            qs.ParamByName("DATE1").AsString = dtpDate1.Value.ToString("yyyy-MM-dd");
            qs.ParamByName("DATE2").AsString = dtpDate2.Value.ToString("yyyy-MM-dd");
            qs.Open();
            FillSum(qs, isDateKey: true);
        }
        else
        {
            using var qs = new DbQuery();
            qs.Add("SELECT CVCOD,COUNT(*) CNT,SUM(TAMT) SAMT FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
                qs.Add(" AND CVCOD BETWEEN @FCVCOD AND @TCVCOD");
            qs.Add(" GROUP BY CVCOD");
            qs.Add(" UNION");
            qs.Add("SELECT '합계',COUNT(*),SUM(TAMT) FROM TAXF");
            qs.Add(" WHERE TDATE BETWEEN @DATE1 AND @DATE2");
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
                qs.Add(" AND CVCOD BETWEEN @FCVCOD AND @TCVCOD");
            qs.ParamByName("DATE1").AsString = dtpDate3.Value.ToString("yyyy-MM-dd");
            qs.ParamByName("DATE2").AsString = dtpDate4.Value.ToString("yyyy-MM-dd");
            if (!string.IsNullOrWhiteSpace(edtCvcd1.Text + edtCvcd2.Text))
            {
                qs.ParamByName("FCVCOD").AsString = edtCvcd1.Text.Trim();
                qs.ParamByName("TCVCOD").AsString = edtCvcd2.Text.Trim();
            }
            qs.Open();
            FillSum(qs, isDateKey: false);
        }
    }

    private void FillSum(DbQuery qs, bool isDateKey)
    {
        if (qs.IsEmpty) return;
        qs.First();
        while (!qs.Eof)
        {
            var key = isDateKey ? qs.FieldByName("TDATE").AsString : qs.FieldByName("CVCOD").AsString;
            gridSum.Rows.Add(key, qs.FieldByName("CNT").AsString, PublicLib.MoneyToStr((long)qs.FieldByName("SAMT").AsFloat));
            qs.Next();
        }
    }

    private void ApplyListHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["TDATE"] = "발행일자", ["SEQNO"] = "순번", ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명",
            ["TAMT"] = "공급가액", ["TVAT"] = "부가세", ["TGUBN"] = "구분", ["TBIGO"] = "비고",
        };
        foreach (var (field, caption) in map)
            if (gridList.Columns[field] != null) gridList.Columns[field]!.HeaderText = caption;

        // TAXF는 라인아이템 상세 컬럼(MMDD1~4/ITNBR1~4 등)이 매우 많아 요약 목록에서는
        // 위에서 매핑한 주요 컬럼만 보여주고 나머지는 숨긴다.
        foreach (DataGridViewColumn col in gridList.Columns)
            if (!map.ContainsKey(col.Name)) col.Visible = false;
    }

    private void OpenEntry(bool isNew)
    {
        using var dlg = new SS33BForm();
        if (!isNew)
        {
            if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
            dlg.Text = "계산서 수정";
            dlg.LoadForEdit(row);
        }
        dlg.ShowDialog(this);
        if (dlg.Saved) Search();
    }

    /// <summary>원본 prcSubDel.</summary>
    private void Delete()
    {
        if (gridList.CurrentRow?.DataBoundItem is not DataRowView row) return;
        var tdate = row["TDATE"].ToString() ?? "";
        var seqno = Convert.ToInt32(row["SEQNO"]);

        if (!PublicLib.ConfirmDelete($"발행일자: {tdate}\r\n순번: {seqno}")) return;

        using var q = new DbQuery();
        try
        {
            AppDb.BeginTransaction();
            q.Add("DELETE FROM TAXF WHERE TDATE=@TDATE AND SEQNO=@SEQNO");
            q.ParamByName("TDATE").AsString = tdate;
            q.ParamByName("SEQNO").AsInteger = seqno;
            q.ExecSQL();
            AppDb.Commit();
        }
        catch (Exception ex)
        {
            AppDb.Rollback();
            MessageBox.Show("자료 삭제시 에러발생..\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Search();
    }
}
