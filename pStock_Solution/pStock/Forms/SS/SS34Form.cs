using System.Data;
using pStock.Common;
using pStock.Data;

namespace pStock.Forms.SS;

/// <summary>
/// 원본 SS34.pas / SS34.dfm (TfrmSS34) 이식 — 미수금 조회(MISUF 월별 누계).
/// 원본 DFM 확인 SQL: SELECT A.*,CVNAM,OWNAM,TELNO FROM MISUF A
///                     LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD WHERE MYEAR=:YEAR
/// </summary>
public class SS34Form : Form
{
    private const string BaseSql =
        "SELECT A.*,CVNAM,OWNAM,TELNO FROM MISUF A" +
        "  LEFT OUTER JOIN CVMAST B ON A.CVCOD=B.CVCOD" +
        " WHERE MYEAR=@YEAR";

    private readonly FastDataGridView grid = new();
    private readonly TextBox edtMonth = new();
    private readonly TextBox edtCvcod = new();
    private readonly TextBox dspName = new() { ReadOnly = true };
    private readonly Label lblAmt = new() { AutoSize = true };

    private readonly Button btnNew = new() { Text = "초기화(F1)" };
    private readonly Button btnSearch = new() { Text = "조회(F5)" };
    private readonly Button btnExcel = new() { Text = "엑셀저장" };
    private readonly Button btnPrint = new() { Text = "인쇄" };
    private readonly Button btnClose = new() { Text = "닫기(Esc)" };
    private readonly Button btnMonthDown = new() { Text = "◀" };
    private readonly Button btnMonthUp = new() { Text = "▶" };

    public SS34Form()
    {
        Text = "미수금 조회";
        Width = 1100;
        Height = 600;
        KeyPreview = true;

        BuildLayout();

        Load += (_, _) => { edtMonth.Text = DateTime.Now.ToString("yyyy-MM"); Search(); };
        KeyDown += SS34Form_KeyDown;
    }

    private void BuildLayout()
    {
        var top = new Panel { Dock = DockStyle.Top, Height = 40 };
        top.Controls.AddRange(new Control[] { btnNew, btnSearch, btnExcel, btnPrint, btnClose });
        int bx = 5;
        foreach (Control c in new Control[] { btnNew, btnSearch, btnExcel, btnPrint, btnClose })
        { c.Left = bx; c.Top = 8; c.Width = 100; bx += 105; }
        btnExcel.Click += (_, _) => pStock.Common.ExcelExporter.Export(grid, "미수금조회");
        btnPrint.Click += (_, _) => pStock.Common.GridPrinter.Print(grid, "미수금조회");

        var editPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
        var lblMonth = new Label { Text = "조회월(YYYY-MM)", Left = 10, Top = 12, AutoSize = true };
        edtMonth.Left = 140; edtMonth.Top = 8; edtMonth.Width = 80;
        edtMonth.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) Search(); };
        btnMonthDown.Left = 225; btnMonthDown.Top = 8; btnMonthDown.Width = 30;
        btnMonthUp.Left = 258; btnMonthUp.Top = 8; btnMonthUp.Width = 30;
        btnMonthDown.Click += (_, _) => { ShiftMonth(-1); Search(); };
        btnMonthUp.Click += (_, _) => { ShiftMonth(1); Search(); };

        var lblCvcod = new Label { Text = "거래처코드", Left = 320, Top = 12, AutoSize = true };
        edtCvcod.Left = 400; edtCvcod.Top = 8; edtCvcod.Width = 80;
        edtCvcod.KeyDown += EdtCvcod_KeyDown;
        dspName.Left = 490; dspName.Top = 8; dspName.Width = 200;

        var lblAmtCap = new Label { Text = "미수금합계:", Left = 720, Top = 12, AutoSize = true };
        lblAmt.Left = 800; lblAmt.Top = 12;

        editPanel.Controls.AddRange(new Control[]
        {
            lblMonth, edtMonth, btnMonthDown, btnMonthUp, lblCvcod, edtCvcod, dspName, lblAmtCap, lblAmt
        });

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;

        Controls.Add(grid);
        Controls.Add(editPanel);
        Controls.Add(top);

        btnNew.Click += (_, _) => { edtCvcod.Clear(); dspName.Clear(); edtCvcod.Focus(); };
        btnSearch.Click += (_, _) => Search();
        btnClose.Click += (_, _) => Close();
    }

    private void SS34Form_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.F1: btnNew.PerformClick(); break;
            case Keys.F5: btnSearch.PerformClick(); break;
            case Keys.Escape: btnClose.PerformClick(); break;
        }
    }

    private void ShiftMonth(int delta)
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var d))
            d = DateTime.Now;
        edtMonth.Text = d.AddMonths(delta).ToString("yyyy-MM");
    }

    /// <summary>원본 edtCvcodKeyDown.</summary>
    private void EdtCvcod_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;

        if (string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            using var dlg = new Common.CvcodLookupForm(cvguFilter: "2");
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                edtCvcod.Text = dlg.SelectedCode;
                dspName.Text = dlg.SelectedName;
            }
            else
            {
                dspName.Clear();
            }
            return;
        }

        using var q = new DbQuery();
        q.Add("SELECT * FROM CVMAST WHERE CVCOD=@CVCOD AND CVGU IN('2','3')");
        q.ParamByName("CVCOD").AsString = edtCvcod.Text.Trim();
        q.Open();
        if (!q.IsEmpty)
        {
            edtCvcod.Text = q.FieldByName("CVCOD").AsString;
            dspName.Text = q.FieldByName("CVNAM").AsString;
        }
        Search();
    }

    /// <summary>원본 prcListDisplay.</summary>
    private void Search()
    {
        if (!DateTime.TryParseExact(edtMonth.Text + "-01", "yyyy-MM-dd",
                null, System.Globalization.DateTimeStyles.None, out var monthDate))
        {
            MessageBox.Show("조회월 형식이 올바르지 않습니다 (YYYY-MM).", "확인",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        int month = monthDate.Month;
        var monthStr = month.ToString("00");

        var terms = new List<string>();
        for (int i = 1; i < month; i++)
        {
            var m = i.ToString("00");
            terms.Add($"+OAMT{m}+OVAT{m}-SAMT{m}");
        }
        var extraWhere = $"AND ((BAMT{string.Concat(terms)})<>0 OR " +
            $"(OAMT{monthStr}<>0 OR OVAT{monthStr}<>0 OR SAMT{monthStr}<>0))";

        using var q = new DbQuery();
        q.Add(BaseSql);
        q.Add(extraWhere);
        q.ParamByName("YEAR").AsString = monthDate.Year.ToString();
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            q.Add("AND CVCOD = @FCVCOD");
            q.ParamByName("FCVCOD").AsString = edtCvcod.Text.Trim();
        }
        q.Add("ORDER BY CVNAM");
        q.Open();

        var table = q.Table ?? new DataTable();
        AddCalculatedColumns(table, month);
        grid.DataSource = table;
        ApplyGridHeaders();

        // 원본: 조회월까지 누계 미수잔액 합계 표시
        var sumTerms = new List<string> { "BAMT" };
        for (int i = 1; i <= month; i++)
        {
            var m = i.ToString("00");
            sumTerms.Add($"+OAMT{m}+OVAT{m}-SAMT{m}");
        }
        using var q2 = new DbQuery();
        q2.Add($"SELECT SUM({string.Concat(sumTerms)}) AS TOTAL FROM MISUF WHERE MYEAR=@YEAR");
        q2.ParamByName("YEAR").AsString = monthDate.Year.ToString();
        if (!string.IsNullOrWhiteSpace(edtCvcod.Text))
        {
            q2.Add("AND CVCOD = @FCVCOD");
            q2.ParamByName("FCVCOD").AsString = edtCvcod.Text.Trim();
        }
        q2.Open();
        lblAmt.Text = q2.IsEmpty || q2.FieldByName("TOTAL").IsNull
            ? "0" : PublicLib.MoneyToStr((long)q2.FieldByName("TOTAL").AsFloat);
    }

    /// <summary>원본 qryListCalcFields.</summary>
    private static void AddCalculatedColumns(DataTable table, int month)
    {
        table.Columns.Add("BBAMT", typeof(double));
        table.Columns.Add("IAMT", typeof(double));
        table.Columns.Add("OAMT_", typeof(double));
        table.Columns.Add("JAMT_", typeof(double));

        foreach (DataRow row in table.Rows)
        {
            double amt = ToDbl(row["BAMT"]);
            for (int i = 1; i < month; i++)
            {
                var m = i.ToString("00");
                amt += ToDbl(row[$"OAMT{m}"]) + ToDbl(row[$"OVAT{m}"]) - ToDbl(row[$"SAMT{m}"]);
            }
            var mm = month.ToString("00");
            double iamt = ToDbl(row[$"OAMT{mm}"]) + ToDbl(row[$"OVAT{mm}"]);
            double oamt = ToDbl(row[$"SAMT{mm}"]);

            row["BBAMT"] = amt;
            row["IAMT"] = iamt;
            row["OAMT_"] = oamt;
            row["JAMT_"] = amt + iamt - oamt;
        }
    }

    private static double ToDbl(object v) => v == null || v == DBNull.Value ? 0 : Convert.ToDouble(v);

    private void ApplyGridHeaders()
    {
        var map = new Dictionary<string, string>
        {
            ["CVCOD"] = "거래처코드", ["CVNAM"] = "거래처명", ["OWNAM"] = "대표자", ["TELNO"] = "전화번호",
            ["BBAMT"] = "이월잔액", ["IAMT"] = "당월발생", ["OAMT_"] = "당월수금", ["JAMT_"] = "미수잔액",
        };
        foreach (var (field, caption) in map)
            if (grid.Columns[field] != null) grid.Columns[field]!.HeaderText = caption;

        foreach (DataGridViewColumn col in grid.Columns)
        {
            if (col.Name.StartsWith("OAMT0") || col.Name.StartsWith("OAMT1") ||
                col.Name.StartsWith("OVAT") || col.Name.StartsWith("SAMT") ||
                col.Name is "BAMT" or "MYEAR" or "MDATE")
                col.Visible = false;
        }
    }
}
